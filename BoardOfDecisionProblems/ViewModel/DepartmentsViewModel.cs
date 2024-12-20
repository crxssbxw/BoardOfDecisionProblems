﻿using BoardOfDecisionProblems.Commands;
using BoardOfDecisionProblems.Forms;
using BoardOfDecisionProblems.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BoardOfDecisionProblems.ViewModel
{
    /// <summary>
    /// Представление модели Отделов
    /// </summary>
    public class DepartmentsViewModel : BaseViewModel
    {
        /// <summary>
        /// Объект выбранного отдела в таблице
        /// </summary>
        private Department selectedDepartment;
        public Department SelectedDepartment
        {
            get => selectedDepartment;
            set
            {
                selectedDepartment = value;
                OnPropertyChanged(nameof(SelectedDepartment));
            }
        }
        /// <summary>
        /// Коллекция отделов
        /// </summary>
        private ObservableCollection<Department> departments = new();
        public ObservableCollection<Department> Departments
        {
            get => departments;
            set
            {
                departments = value;
                OnPropertyChanged(nameof(Departments));
            }
        }

        public DepartmentsViewModel()
        {
            dbContext.Responsibles.Load();
            dbContext.Workers.Load();
            dbContext.Problems.Load();
            foreach (var department in dbContext.Departments)
            {
                Departments.Add(department);
            }
        }

        #region Commands

        private RelayCommand add;
        /// <summary>
        /// Команда добавления отдела
        /// </summary>
        public RelayCommand Add
        {
            get
            {
                return add ?? (add = new(obj =>
                {
                    Department department = new();

                    NewDepartment newDepartment = new NewDepartment();
                    newDepartment.DataContext = department;

                    if(newDepartment.ShowDialog() == true)
                    {
                        dbContext.Add(department);
                        dbContext.SaveChanges();

                        LogEvent logEvent = new LogEvent()
                        {
                            Date = DateOnly.FromDateTime(DateTime.Now),
                            Time = TimeOnly.FromDateTime(DateTime.Now),
                            Title = $"Создание отдела Id:{department.DepartmentId}",
                            User = "Admin"
                        };
                        logEvent.Object = $"Отдел Id:{department.DepartmentId}";
                        dbContext.Add(logEvent);
                        ProblemViewModel.LogsViewModel.LogEvents.Add(logEvent);

                        Departments.Add(department);
                        dbContext.SaveChanges();
                    }
                },
                obj => true));
            }
        }

        private RelayCommand delete;
        /// <summary>
        /// Команда удаления отдела
        /// </summary>
        public RelayCommand Delete
        {
            get
            {
                return delete ?? (delete = new(obj =>
                {
                    var result = MessageBox.Show($"Вы уверены, что хотите удалить данный участок?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        dbContext.Remove(SelectedDepartment);
                        dbContext.SaveChanges();

                        LogEvent logEvent = new LogEvent()
                        {
                            Date = DateOnly.FromDateTime(DateTime.Now),
                            Time = TimeOnly.FromDateTime(DateTime.Now),
                            Title = $"Удаление отдела №{SelectedDepartment.DepartmentId}",
                            User = "Admin"
                        };
                        logEvent.Object = $"Отдел №{SelectedDepartment.DepartmentId}";
                        dbContext.Add(logEvent);
                        ProblemViewModel.LogsViewModel.LogEvents.Add(logEvent);

                        dbContext.SaveChanges();
                        Departments.Remove(SelectedDepartment);
                    }
                },
                obj => SelectedDepartment != null));
            }
        }

        #endregion
    }
}
