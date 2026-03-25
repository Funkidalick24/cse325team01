using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Models
{
    public class Subtask
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        public bool IsCompleted { get; set; } = false;

        // Foreign key
        public int TaskItemId { get; set; }
        public TaskItem TaskItem { get; set; }
    }
}