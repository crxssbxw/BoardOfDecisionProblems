using ProblemsBoardLib.Commands;
using ProblemsBoardLib.Forms;
using ProblemsBoardLib.Models;
using Microsoft.EntityFrameworkCore;
using ProblemsBoardLib.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProblemsBoardLib.ViewModel
{
    public class DepartmentsViewModel : BaseViewModel
    {
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
                        Departments.Add(department);
                    }
                },
                obj => true));
            }
        }

        private RelayCommand delete;
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
                        Departments.Remove(SelectedDepartment);
                    }
                },
                obj => SelectedDepartment != null));
            }
        }

        #endregion
    }
}
