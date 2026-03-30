using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Models;

namespace TodoApp.Services;

public class TaskService : ITaskService
{
    private readonly ApplicationDbContext _context;

    public TaskService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<TaskItem>> GetTasksAsync(string userId)
    {
        return await _context.Tasks
            .Where(t => t.UserId == userId)
            .OrderBy(t => t.IsCompleted)
            .ThenBy(t => t.DueDate ?? DateTime.MaxValue)
            .ThenByDescending(t => t.Priority)
            .ToListAsync();
    }

    public async Task<TaskItem?> GetTaskAsync(int id, string userId)
    {
        return await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
    }

    public async Task<TaskItem> CreateTaskAsync(TaskItem task, string userId)
    {
        task.UserId = userId;
        task.CreatedAt = DateTime.UtcNow;

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return task;
    }

    public async Task<TaskItem> UpdateTaskAsync(TaskItem task, string userId)
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

    public async Task DeleteTaskAsync(int id, string userId)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

        if (task != null)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }
    }

    public async Task ToggleTaskCompletionAsync(int id, string userId)
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

    public async Task<int> DeleteCompletedTasksAsync(string userId)
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

    public async Task<int> DeleteAllTasksAsync(string userId)
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
}
