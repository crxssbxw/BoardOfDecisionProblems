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
using ProblemsBoardLib.Models;
using ProblemsBoardLib.ViewModel;

namespace ProblemsBoard.Windows
{
	/// <summary>
	/// Логика взаимодействия для Startup.xaml
	/// </summary>
	public partial class Startup : Window, INotifyPropertyChanged
    {
        public Startup()
        {
            DataContext = this;
			using (DatabaseContext dbContext = new())
			{
				foreach (var dep in dbContext.Departments)
				{
					Departments.Add(dep);
				}
			}
            InitializeComponent();
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
			MainWindow mainWindow = new();
			mainWindow.ViewModel.Department = SelectedDepartment;
			mainWindow.ViewModel.Department.DepartmentId = SelectedDepartment.DepartmentId;
			mainWindow.Show();
        }
    }
}
