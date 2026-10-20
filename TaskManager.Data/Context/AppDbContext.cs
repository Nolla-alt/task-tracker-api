using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Data.Entities;
using Microsoft.EntityFrameworkCore;



namespace TaskManager.Data.Context
{
    // This class is EF Core's gateway to the database.
    // Each DbSet below becomes one real table.
    public class AppDbContext : DbContext
    {
        // The connection string gets passed in here from Program.cs,
        // it's never written directly inside this file.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }
    }
}