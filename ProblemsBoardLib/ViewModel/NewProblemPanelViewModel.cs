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
            dbContext.Problems.Load();
            dbContext.Responsibles.Load();
            NewProblem = new()
            {
                DateOccurance = Today,
                Department = dbContext.Departments.Find(department.DepartmentId),
                Description = "Описание",
                Status = "Решается"
            };
            NewProblem.Responsible = dbContext.Responsibles.Find(NewProblem.Department.Responsibles.Where(a => a.IsCurrent).FirstOrDefault().ResponsibleId);
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

        public DateTime Today { get => DateTime.Today; }

        private bool IsValid
        {
            get
            {
                if (NewProblem.Theme == null) return false;
                if (string.IsNullOrEmpty(NewProblem.Description)) return false;
                if (NewProblem.DateOccurance == null) return false;
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
                        dbContext.Add(NewProblem);
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
