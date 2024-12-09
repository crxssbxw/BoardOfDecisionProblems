using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemsBoardLib.Models
{
    /// <summary>
    /// Логика взаимодействия с Базой Данных
    /// </summary>
    public class DatabaseContext : DbContext
    {
<<<<<<< HEAD
        /// <summary>
        /// Таблица Users
        /// </summary>
=======
>>>>>>> 0e3fd82 (Edited database model and removed Users functions)
        public DbSet<Worker> Workers { get; set; }
        /// <summary>
        /// Таблица Отделов
        /// </summary>
        public DbSet<Department> Departments { get; set; }
        /// <summary>
        /// Таблица Проблем
        /// </summary>
        public DbSet<Problem> Problems { get; set; }
        /// <summary>
        /// Таблица ответственных
        /// </summary>
        public DbSet<Responsible> Responsibles { get; set; }
        /// <summary>
        /// Таблица Тем
        /// </summary>
        public DbSet<Theme> Themes { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<LogEvent> LogEvents { get; set; }
        public DbSet<Report> Reports { get; set; }

        public DbSet<Log> Logs { get; set; }
        public DbSet<LogEvent> LogEvents { get; set; }
        public DbSet<Report> Reports { get; set; }

        /// <summary>
        /// Создание БД, если она не создана
        /// </summary>
        public DatabaseContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=board.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>()
                .HasIndex(department => department.ViewerNumber)
                .IsUnique();
        }
    }
}
