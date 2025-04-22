using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;
using ProblemsBoardLib;
using ProblemsBoardLib.Models;
using ProblemsBoardLib.ViewModel;

namespace ProblemsBoard.Windows
{
	/// <summary>
	/// Логика взаимодействия для Startup.xaml
	/// </summary>
	public partial class Startup : Window, INotifyPropertyChanged
    {
		public DatabaseContext DatabaseContext { get; set; } = new();
        public Startup()
        {
			DatabaseContext.Problems.Load();
			DatabaseContext.Workers.Load();
			DatabaseContext.Responsibles.Load();
			DatabaseContext.Themes.Load();
			DatabaseContext.Admins.Load();
            DataContext = this;
			Refresh();
            InitializeComponent();
        }

		private void Refresh()
		{
			Departments.Clear();
            foreach (var dep in DatabaseContext.Departments)
            {
				Departments.Add(dep);
            }
			OnPropertyChanged(nameof(Departments));
        }

		private Department selectedDepartment;
		public Department SelectedDepartment
		{
			get => selectedDepartment;
			set
			{
				selectedDepartment = value;
				OnPropertyChanged(nameof(SelectedDepartment));
				OnPropertyChanged(nameof(ContinueAvailable));
			}
		}

		public bool ContinueAvailable
		{
			get
			{
				if (SelectedDepartment == null)
					return false;
				return true;
			}
		}

		public event PropertyChangedEventHandler? PropertyChanged;
		private void OnPropertyChanged([CallerMemberName] string prop = "")
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
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

		private void Continue_Click(object sender, RoutedEventArgs e)
		{
			Department department = new();
			Helper.CopyTo(SelectedDepartment, department);

			if (department.Admin == null)
			{
				MessageBox.Show("Доска для этого участка еще не настроена! Обратитесь к администратору приложения для настройки!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}
			else
			{
				AdminAuthorization adminAuthorization = new(department);
				if (adminAuthorization.ShowDialog() == true)
				{
                    MainWindow mainWindow = new(department);
                    mainWindow.Show();

                    Close();
                }
			}
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
			ImportData importData = new();

			var dialogResult = importData.ShowDialog();

			if (dialogResult == true)
			{
				if (importData.DeltaDepartments.Count == 0 && importData.DeltaWorkers.Count == 0)
					MessageBox.Show("Нечего импортировать", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
				else
				{
					var result = MessageBox.Show($"В базу данных будет добавлено:\n{importData.DeltaDepartments.Count} строк участков" +
						$"\n{importData.DeltaWorkers.Count} строк работников", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Question);

					if (result == MessageBoxResult.OK)
					{
						using (DatabaseContext context = new())
						{
							context.Departments.AddRange(importData.DeltaDepartments);
							context.Workers.AddRange(importData.DeltaWorkers);
							context.SaveChanges();
						}
                        Refresh();
                    }
				}
			}
			else if (dialogResult == false)
				MessageBox.Show("Импорт отменен", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
			else
				MessageBox.Show("Пользователь не воспользовался окном импорта", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
