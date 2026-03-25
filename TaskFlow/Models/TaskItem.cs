using System;
using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime? DueDate { get; set; }

        // Priority: 0 = Low, 1 = Medium, 2 = High (or use an enum)
        public int Priority { get; set; } = 1;

        public bool IsCompleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Order index for drag-and-drop reordering
        public int OrderIndex { get; set; }

        // Foreign keys
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int? CategoryId { get; set; }
        public Category Category { get; set; }

        // Navigation properties
        public ICollection<Subtask> Subtasks { get; set; }
        public RecurringTask RecurringTask { get; set; }
    }
}