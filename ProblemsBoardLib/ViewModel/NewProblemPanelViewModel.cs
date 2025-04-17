using Microsoft.EntityFrameworkCore;
using ProblemsBoardLib.Commands;
using ProblemsBoardLib.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProblemsBoardLib.ViewModel
{
    public class NewProblemPanelViewModel : BaseViewModel
    {
        public NewProblemPanelViewModel(ObservableCollection<Problem> problems, Department department)
        {
            dbContext.Themes.Load();
            dbContext.Departments.Load();
            dbContext.Problems.Load();
            NewProblem = new()
            {
                DateOccurance = DateTime.Now,
                Department = department,
                Description = "Описание",
                Status = "Решается"
            };
            Problems = problems;
            foreach (var theme in dbContext.Themes.Where(a => a.Department.DepartmentId == NewProblem.Department.DepartmentId || a.Department == null))
                Themes.Add(theme);
        }

        private Problem newProblem;
        public Problem NewProblem 
        { 
            get => newProblem; 
            set 
            {
                newProblem = value;
                OnPropertyChanged(nameof(NewProblem));
            }
        }

        public ObservableCollection<Problem> Problems { get; set; }

        private ObservableCollection<Theme> themes = new();
        public ObservableCollection<Theme> Themes
        {
            get => themes;
            set
            {
                themes = value;
                OnPropertyChanged(nameof(Themes));
            }
        }

        private bool IsValid
        {
            get
            {
                return true;
            }
        }

        #region Commands

        private RelayCommand add;
        public RelayCommand Add
        {
            get
            {
                return add ?? (add = new(obj =>
                {
                    try
                    {
                        dbContext.ChangeTracker.Clear();

						var dep = dbContext.Departments.Find(NewProblem.Department.DepartmentId);
                        if (dep.Problems == null)
                            dep.Problems = [];

                        dep.Problems.Add(NewProblem);
                        dbContext.SaveChanges();
                        Problems.Add(NewProblem);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                },
                obj => IsValid));
            }
        }

        #endregion
    }
}
