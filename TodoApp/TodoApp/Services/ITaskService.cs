using TodoApp.Models;

namespace TodoApp.Services;

public interface ITaskService
{
    Task<List<TaskItem>> GetTasksAsync(string userId);
    Task<TaskItem?> GetTaskAsync(int id, string userId);
    Task<TaskItem> CreateTaskAsync(TaskItem task, string userId);
    Task<TaskItem> UpdateTaskAsync(TaskItem task, string userId);
    Task DeleteTaskAsync(int id, string userId);
    Task ToggleTaskCompletionAsync(int id, string userId);
}
