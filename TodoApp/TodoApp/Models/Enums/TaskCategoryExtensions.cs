namespace TodoApp.Models.Enums;

public static class TaskCategoryExtensions
{
    public static string GetDisplayLabel(this TaskCategory category)
        => category switch
        {
            TaskCategory.Work => "Work",
            TaskCategory.Personal => "Personal",
            TaskCategory.Shopping => "Shopping",
            TaskCategory.Health => "Health",
            _ => "Other"
        };
}
