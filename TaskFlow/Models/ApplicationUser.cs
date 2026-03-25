using Microsoft.AspNetCore.Identity;

namespace TaskFlow.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Additional properties can be added here if needed
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ICollection<TaskItem> TaskItems { get; set; }
        public ICollection<Category> Categories { get; set; }
    }
}