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
	public class BoardPropertiesViewModel : BaseViewModel
	{
		public Department Department { get; set; } = new();
		public BoardPropertiesViewModel(Department department) 
		{
			Department = department;
			if (Department.Admin == null)
			{
				Department.Admin = new Admin()
				{
					Login = "admin"
				};
				Password = "admin";
			}
			foreach (var theme in dbContext.Themes.Where(a => a.Department.DepartmentId == Department.DepartmentId || a.Department == null))
			{
				Themes.Add(theme);
			}
		}

		public string Name
		{
			get => Department.Name;
			set
			{
				Department.Name = value;
				OnPropertyChanged(nameof(Name));
			}
		}

		public string ViewerNumber
		{
			get => Department.ViewerNumber;
			set
			{
				Department.ViewerNumber = value;
				OnPropertyChanged(nameof(ViewerNumber));
			}
		}

		public Admin Admin
		{
			get => Department.Admin;
			set
			{
				Department.Admin = value;
				OnPropertyChanged(nameof(Admin));
			}
		}

		private string password;
		public string Password
		{
			get => password;
			set
			{
				password = value;
				OnPropertyChanged(nameof(Password));
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

		private Theme selectedTheme;
		public Theme SelectedTheme
		{
			get => selectedTheme;
			set
			{
				selectedTheme = value;
				OnPropertyChanged(nameof(SelectedTheme));
			}
		}

		private RelayCommand accept;
		public RelayCommand Accept
		{
			get
			{
				return accept ?? (accept = new(obj =>
				{
					if (!string.IsNullOrEmpty(Password))
						Admin.Password = Helper.EncryptString(Password);
					try
					{
						dbContext.Update(Department);
						dbContext.SaveChanges();
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
					}
				},
				obj => true));
			}
		}
	}
}
