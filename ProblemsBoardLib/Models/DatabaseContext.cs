using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemsBoardLib.Models
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Problem> Problems { get; set; }
        public DbSet<Responsible> Responsibles { get; set; }
        public DbSet<Theme> Themes { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<LogEvent> LogEvents { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Admin> Admins { get; set; }

        public DatabaseContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=board.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Department>()
                .HasOne(e => e.Admin)
                .WithOne(e => e.Department)
                .HasForeignKey<Department>(e => e.AdminId);
        }
    }
}
