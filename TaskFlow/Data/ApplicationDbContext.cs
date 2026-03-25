using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Models;

namespace TaskFlow.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet properties for each model
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Subtask> Subtasks { get; set; }
        public DbSet<RecurringTask> RecurringTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure the one-to-one relationship between TaskItem and RecurringTask
            builder.Entity<TaskItem>()
                .HasOne(t => t.RecurringTask)
                .WithOne(r => r.TaskItem)
                .HasForeignKey<RecurringTask>(r => r.TaskItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // Optional: Configure cascading delete for Subtasks when a Task is deleted
            builder.Entity<Subtask>()
                .HasOne(s => s.TaskItem)
                .WithMany(t => t.Subtasks)
                .HasForeignKey(s => s.TaskItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // Optional: Configure cascading delete for Tasks when a Category is deleted
            builder.Entity<TaskItem>()
                .HasOne(t => t.Category)
                .WithMany(c => c.Tasks)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}