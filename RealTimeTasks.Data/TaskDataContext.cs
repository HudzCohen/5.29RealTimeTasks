using Microsoft.EntityFrameworkCore;

namespace RealTimeTasks.Data;

public class TaskDataContext : DbContext
{
    private readonly string _connectionString;

    public TaskDataContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connectionString);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }

    public DbSet<TaskItem> TaskItems { get; set; }
    public DbSet<User> Users { get; set; }
}