using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Models;

namespace TodoApp.Services;

public class TaskService : ITaskService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<TaskService> _logger;

    public TaskService(ApplicationDbContext context, ILogger<TaskService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<TaskItem>> GetTasksAsync(string userId)
    {
        EnsureValidUserId(userId);
        try
        {
            return await _context.Tasks
                .Where(t => t.UserId == userId)
                .OrderBy(t => t.IsCompleted)
                .ThenBy(t => t.DueDate ?? DateTime.MaxValue)
                .ThenByDescending(t => t.Priority)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch tasks for user {UserId}", userId);
            throw;
        }
    }

    public async Task<TaskItem?> GetTaskAsync(int id, string userId)
    {
        EnsureValidUserId(userId);
        try
        {
            return await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch task {TaskId} for user {UserId}", id, userId);
            throw;
        }
    }

    public async Task<TaskItem> CreateTaskAsync(TaskItem task, string userId)
    {
        EnsureValidUserId(userId);
        ArgumentNullException.ThrowIfNull(task);

        task.UserId = userId;
        task.CreatedAt = DateTime.UtcNow;

        try
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create task for user {UserId}", userId);
            throw;
        }

        return task;
    }

    public async Task<TaskItem> UpdateTaskAsync(TaskItem task, string userId)
    {
        EnsureValidUserId(userId);
        ArgumentNullException.ThrowIfNull(task);

        try
        {
            var existingTask = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == task.Id && t.UserId == userId);

            if (existingTask == null)
                throw new InvalidOperationException("Task not found or access denied.");

            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.DueDate = task.DueDate;
            existingTask.Priority = task.Priority;
            existingTask.Category = task.Category;
            existingTask.IsCompleted = task.IsCompleted;
            existingTask.CompletedAt = task.IsCompleted ? (existingTask.CompletedAt ?? DateTime.UtcNow) : null;

            await _context.SaveChangesAsync();
            return existingTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update task {TaskId} for user {UserId}", task.Id, userId);
            throw;
        }
    }

    public async Task DeleteTaskAsync(int id, string userId)
    {
        EnsureValidUserId(userId);
        try
        {
            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete task {TaskId} for user {UserId}", id, userId);
            throw;
        }
    }

    public async Task ToggleTaskCompletionAsync(int id, string userId)
    {
        EnsureValidUserId(userId);
        try
        {
            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (task != null)
            {
                task.IsCompleted = !task.IsCompleted;
                task.CompletedAt = task.IsCompleted ? DateTime.UtcNow : null;
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed toggling task {TaskId} completion for user {UserId}", id, userId);
            throw;
        }
    }

    public async Task<int> DeleteCompletedTasksAsync(string userId)
    {
        EnsureValidUserId(userId);
        try
        {
            var completedTasks = await _context.Tasks
                .Where(t => t.UserId == userId && t.IsCompleted)
                .ToListAsync();

            if (completedTasks.Count == 0)
            {
                return 0;
            }

            _context.Tasks.RemoveRange(completedTasks);
            await _context.SaveChangesAsync();
            return completedTasks.Count;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed deleting completed tasks for user {UserId}", userId);
            throw;
        }
    }

    public async Task<int> DeleteAllTasksAsync(string userId)
    {
        EnsureValidUserId(userId);
        try
        {
            var allTasks = await _context.Tasks
                .Where(t => t.UserId == userId)
                .ToListAsync();

            if (allTasks.Count == 0)
            {
                return 0;
            }

            _context.Tasks.RemoveRange(allTasks);
            await _context.SaveChangesAsync();
            return allTasks.Count;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed deleting all tasks for user {UserId}", userId);
            throw;
        }
    }

    private static void EnsureValidUserId(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ArgumentException("A valid authenticated user id is required.", nameof(userId));
        }
    }
}
