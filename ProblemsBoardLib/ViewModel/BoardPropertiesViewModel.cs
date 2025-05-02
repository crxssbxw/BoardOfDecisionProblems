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
        private Department department = new();
        public Department Department 
		{ 
			get => department; 
			set 
			{
				department = value; 
				OnPropertyChanged(nameof(Department));
			}
		}

		public BoardPropertiesViewModel(Department department) 
		{
			dbContext.Themes.Load();
			dbContext.Responsibles.Load();
			dbContext.Workers.Load();
			dbContext.Admins.Load();
			Department = dbContext.Departments.Find(department.DepartmentId);

			if (Department.Themes != null)
			{
                foreach (var theme in Department.Themes)
                {
                    Themes.Add(theme);
                }
            }

			foreach (var theme in dbContext.Themes.Where(a => a.Department == null))
			{
				Themes.Add(theme);
			}

			if (Department.Responsibles != null)
			{
                foreach (var responsible in Department.Responsibles)
                {
                    Responsibles.Add(responsible);
                }
            }

            if (Department.Admin == null)
			{
				Login = "admin";
				Password = "admin";
			}
			else
			{
				Login = Department.Admin.Login;
			}
		}

		private void RefreshResponsibles()
		{
			Responsibles.Clear();
            foreach (var responsible in Department.Responsibles)
            {
                Responsibles.Add(responsible);
            }
        }

		private string login;
		public string Login
		{
			get => login;
			set
			{
				login = value;
				OnPropertyChanged(nameof(Login));
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

		private Responsible selectedResponsible;
		public Responsible SelectedResponsible
		{
			get => selectedResponsible;
			set
			{
				selectedResponsible = value;
				OnPropertyChanged(nameof(SelectedResponsible));
			}
		}

		private RelayCommand accept;
		public RelayCommand Accept
		{
			get
			{
				return accept ?? (accept = new(obj =>
				{
					if (Department.Admin == null)
						Department.Admin = new();

					if (Login != Department.Admin.Login)
						Department.Admin.Login = Login;
					if (!string.IsNullOrEmpty(Password) && Helper.EncryptString(Password) != Department.Admin.Password)
                        Department.Admin.Password = Helper.EncryptString(Password);

					dbContext.SaveChanges();
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
					Responsible newResp = new();
					Worker worker = new Worker();
					Responsible prevResp = new();

					Department temp = new();
					Helper.CopyTo(department, temp);
					NewResponsibleDialog newResponsibleDialog;
					newResponsibleDialog = new(temp, worker);

					if (newResponsibleDialog.ShowDialog() == true)
					{
                        newResp.Worker = dbContext.Workers.Find(worker.WorkerId);

						PasswordGenerator passwordGenerator = new PasswordGenerator();
						if (passwordGenerator.ShowDialog() == true)
						{
                            if (Department.Responsibles != null)
                            {
                                prevResp = Department.Responsibles.Where(a => a.IsCurrent).FirstOrDefault();
                                prevResp.IsCurrent = false;
                            }

                            newResp.Login = passwordGenerator.Login;
							newResp.Password = Helper.EncryptString(passwordGenerator.Password);
							newResp.IsCurrent = true;
						}
						else
						{
							MessageBox.Show("Логин и пароль не установлены, операция отменена!");
							return;
						}

                        if (Department.Responsibles == null)
						{
							Department.Responsibles = [];
						}
                        Department.Responsibles.Add(newResp);
                        RefreshResponsibles();
					}
				},
				obj => true));
			}
		}

		private RelayCommand setSelectedResponsible;

        public RelayCommand SetSelectedResponsible
		{
			get
			{
				return setSelectedResponsible ?? (setSelectedResponsible = new(obj =>
				{
					PasswordGenerator passwordGenerator = new PasswordGenerator();
					if (passwordGenerator.ShowDialog() == true)
					{
                        Responsible prevResp = new();
                        prevResp = Department.Responsibles.Where(a => a.IsCurrent).FirstOrDefault();
                        prevResp.IsCurrent = false;

                        SelectedResponsible.Login = passwordGenerator.Login;
						SelectedResponsible.Password = Helper.EncryptString(passwordGenerator.Password);
						SelectedResponsible.IsCurrent = true;
					}
					else
					{
						MessageBox.Show("Логин и пароль не установлены, операция отменена!", "Отмена", MessageBoxButton.OK, MessageBoxImage.Information);
						return;
					}

					RefreshResponsibles();
				},
				obj => SelectedResponsible != null && SelectedResponsible.IsCurrent == false)) ;
			}
		}
	}
}
