using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public string Color { get; set; } = "#808080"; // Default gray

        // Foreign key to the user who owns this category
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        // Navigation property to tasks
        public ICollection<TaskItem> Tasks { get; set; }
    }
}