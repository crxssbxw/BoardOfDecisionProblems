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
        public NewProblemPanelViewModel(Problem problem, ObservableCollection<Problem> problems)
        {
            dbContext.Problems.Load();
            NewProblem = problem;
            Problems = problems;
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
                        dbContext.Departments.Find(NewProblem.Department.DepartmentId).Problems?.Add(NewProblem);
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
