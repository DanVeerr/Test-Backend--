using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Db;

public class TaskDbContext : DbContext
{
    public DbSet<TaskItem> Tasks => Set<TaskItem>();
    public TaskDbContext(DbContextOptions<TaskDbContext> o) : base(o) { }

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<TaskItem>(e =>
        {
            e.HasKey(t => t.Id);
            e.Property(t => t.Status).HasConversion<string>();
        });
    }
}