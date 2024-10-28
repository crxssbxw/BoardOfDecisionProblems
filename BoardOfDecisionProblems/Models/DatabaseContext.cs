using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardOfDecisionProblems.Models
{
    /// <summary>
    /// Логика взаимодействия с Базой Данных
    /// </summary>
    public class DatabaseContext : DbContext
    {
        /// <summary>
        /// Таблица Users
        /// </summary>
        public DbSet<User> Users { get; set; }
        /// <summary>
        /// Таблица Работников
        /// </summary>
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

        /// <summary>
        /// Создание БД, если она не создана
        /// </summary>
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
            modelBuilder.Entity<Department>()
                .HasIndex(department => department.ViewerNumber)
                .IsUnique();
        }
    }
}
