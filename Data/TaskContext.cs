using Microsoft.EntityFrameworkCore;
using TaskApplication.Models;

namespace TaskApplication.Data
{
    public class TaskContext : DbContext
    {
        public TaskContext(DbContextOptions<TaskContext> options)
            : base(options) { }

        public DbSet<TaskItem> Tasks { get; set; }
    }
}
