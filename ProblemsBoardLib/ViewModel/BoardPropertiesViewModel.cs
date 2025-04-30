using Microsoft.EntityFrameworkCore;
using ProblemsBoardLib.Commands;
using ProblemsBoardLib.DialogWindows;
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
		public Department Temp { get; set; } = new();
		public BoardPropertiesViewModel(Department department) 
		{
			dbContext.Themes.Load();
			Temp = department;
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
			foreach (var responsible in dbContext.Responsibles.Include(x => x.Worker).Where(a => a.Department.DepartmentId == Department.DepartmentId).AsNoTrackingWithIdentityResolution())
			{
				Responsibles.Add(responsible);
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

		private ObservableCollection<Responsible> responsibles = new();
		public ObservableCollection<Responsible> Responsibles
		{
			get => responsibles;
			set
			{
				responsibles = value;
				OnPropertyChanged(nameof(Responsibles));
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
						dbContext.Departments.Update(Department);
						dbContext.Admins.Update(Department.Admin);
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

		private RelayCommand setNewResponsible;
		public RelayCommand SetNewResponsible
		{
			get
			{
				return setNewResponsible ?? (setNewResponsible = new(obj =>
				{
					Worker worker = new();
					NewResponsibleDialog newResponsibleDialog = new(Department, worker); 

					if (newResponsibleDialog.ShowDialog() == true)
					{
						Responsible responsible = new();
						responsible.Worker = worker;
						responsible.Department = Department;
						responsible.IsCurrent = true;

						if (Responsibles.Any(a => a.Worker.WorkerId == worker.WorkerId))
                        {
							MessageBox.Show("Ответственный уже назначен", "Внимание", MessageBoxButton.OK, MessageBoxImage.Stop);
							return;
                        }

						PasswordGenerator passwordGenerator = new();
						if (passwordGenerator.ShowDialog() == true)
						{
							responsible.Password = Helper.EncryptString(passwordGenerator.Password);
							responsible.Login = passwordGenerator.Login;
						}

                        dbContext.Add(responsible);
						dbContext.Update(worker);
						Responsibles.Add(responsible);
					}
				},
				obj => true));
			}
		}
	}
}
