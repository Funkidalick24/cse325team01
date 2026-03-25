using System;

namespace TaskFlow.Models
{
    public class RecurringTask
    {
        public int Id { get; set; }

        // Frequency: Daily, Weekly, Monthly, etc. (you can use an enum)
        public string Frequency { get; set; } // e.g., "Daily", "Weekly", "Monthly"

        public int? Interval { get; set; } = 1; // e.g., every 2 weeks

        public DateTime? NextOccurrence { get; set; }

        public DateTime? EndDate { get; set; }

        // Foreign key – one-to-one with TaskItem
        public int TaskItemId { get; set; }
        public TaskItem TaskItem { get; set; }
    }
}