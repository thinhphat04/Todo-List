using Microsoft.EntityFrameworkCore;
using Todo_Api.Models;

namespace Todo_Api.Data;

    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<TaskDependency> TaskDependencies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình composite key cho TaskDependency: (TaskId, DependencyId)
            modelBuilder.Entity<TaskDependency>()
                .HasKey(td => new { td.TaskId, td.DependencyId });

            // Cấu hình quan hệ: Một Tasks có nhiều TaskDependencies (các task mà nó phụ thuộc vào)
            modelBuilder.Entity<TaskDependency>()
                .HasOne(td => td.Task)
                .WithMany(t => t.TaskDependencies)
                .HasForeignKey(td => td.TaskId)
                .OnDelete(DeleteBehavior.Restrict);

            // Cấu hình quan hệ: Một Tasks có nhiều DependentOnTasks (các task phụ thuộc vào nó)
            modelBuilder.Entity<TaskDependency>()
                .HasOne(td => td.Dependency)
                .WithMany(t => t.DependentOnTasks)
                .HasForeignKey(td => td.DependencyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
