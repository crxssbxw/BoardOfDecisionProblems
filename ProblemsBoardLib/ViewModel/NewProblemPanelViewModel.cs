using Microsoft.EntityFrameworkCore;
using ProblemsBoardLib.Commands;
using ProblemsBoardLib.Models;
using ProblemsBoardLib.Tools;
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
        public NewProblemPanelViewModel(ProblemsViewModel vm, Department department)
        {
            dbContext.Themes.Load();
            dbContext.Problems.Load();
            dbContext.Workers.Load();
            dbContext.DepartmentThemes.Load();
            NewProblem = new()
            {
                DateOccurance = Today,
                Department = dbContext.Departments.Find(department.DepartmentId),
                Description = "Описание",
                Status = "Решается"
            };
            VM = vm;
            NewProblem.Responsible = NewProblem.Department.Workers.FirstOrDefault(a => a.IsResponsible == true, null);
            NewProblem.Header = NewProblem.Department.Workers.FirstOrDefault(a => a.IsHeader == true, null);

            foreach (var theme in dbContext.Themes.Where(a => a.Departments.Count() == 0))
            {
                Themes.Add(theme);
            }

            foreach (var theme in NewProblem.Department.Themes)
            {
                Themes.Add(theme);
            }
        }
        public ProblemsViewModel VM { get; set; }

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
                        LoggingTool.NewProblemAdded(NewProblem);
                        VM.ProblemsReload();
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
