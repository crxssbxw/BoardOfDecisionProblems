using BoardOfDecisionProblems.Commands;
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
    /// Представление модели Работников
    /// </summary>
    public class WorkersViewModel : BaseViewModel
    {
        private ObservableCollection<Worker> workers = new();
        /// <summary>
        /// Коллекция работников
        /// </summary>
        public ObservableCollection<Worker> Workers
        {
            get => workers;
            set
            {
                workers = value;
                OnPropertyChanged(nameof(Workers));
            }
        }

        private Worker selectedWorker;
        /// <summary>
        /// Объект выбранного работника
        /// </summary>
        public Worker SelectedWorker
        {
            get => selectedWorker;
            set
            {
                selectedWorker = value;
                OnPropertyChanged(nameof(SelectedWorker));
            }
        }

        public WorkersViewModel()
        {
            dbContext.Departments.Load();
            dbContext.Responsibles.Load();
            dbContext.Problems.Load();

            foreach (var worker in dbContext.Workers)
            {
                Workers.Add(worker);
            }
        }

        #region Commands

        private RelayCommand add;
        /// <summary>
        /// Команда добавления работника
        /// </summary>
        public RelayCommand Add
        {
            get
            {
                return add ?? (add = new(obj =>
                {
                    Worker worker = new();

                    NewWorker workerNew = new NewWorker();
                    workerNew.DataContext = worker;
                    workerNew.DepartmentBox.ItemsSource = new DepartmentsViewModel().Departments;

                    if(workerNew.ShowDialog() == true)
                    {
                        dbContext.Workers.Add(worker);
                        dbContext.SaveChanges();
                        Workers.Add(worker);
                    }
                },
                obj => true));
            }
        }

        private RelayCommand delete;
        /// <summary>
        /// Команда удаления работника
        /// </summary>
        public RelayCommand Delete
        {
            get
            {
                return delete ?? (delete = new(obj =>
                {
                    Worker worker = new();
                    worker = SelectedWorker;

                    var result = MessageBox.Show($"Вы уверены, что хотите удалить данные выбранного работника?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        dbContext.Workers.Remove(SelectedWorker);
                        dbContext.SaveChanges();
                        Workers.Remove(worker);
                    }
                }, 
                obj => SelectedWorker != null));
            }
        }

        #endregion
    }
}
