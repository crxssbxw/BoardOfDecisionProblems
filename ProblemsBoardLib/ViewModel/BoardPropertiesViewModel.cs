using Microsoft.EntityFrameworkCore;
using ProblemsBoardLib.Commands;
using ProblemsBoardLib.DialogWindows;
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

			if (Department.Workers != null)
			{
				foreach (var worker in Department.Workers)
				{
					Workers.Add(worker);
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

		private void RefreshWorkers()
		{
			Workers.Clear();
			foreach (var worker in Department.Workers)
			{
				Workers.Add(worker);
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

		private ObservableCollection<Worker> workers = new();
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
		public Worker SelectedWorker
		{
			get => selectedWorker;
			set
			{
				selectedWorker = value;
				OnPropertyChanged(nameof(SelectedWorker));
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
					{
						Department.Admin = new();
						LoggingTool.NewAdminAuthorizationData(Department);
					}

					if (Login != Department.Admin.Login)
					{
						Department.Admin.Login = Login;
						LoggingTool.NewAdminAuthorizationData(Department, login);
					}
					if (!string.IsNullOrEmpty(Password) && Helper.EncryptString(Password) != Department.Admin.Password)    
					{ 
						Department.Admin.Password = Helper.EncryptString(Password);
						LoggingTool.NewAdminAuthorizationData(Department, password: Password);
					}

					dbContext.SaveChanges();

					if (Department.Admin != null && Department.Responsibles.Any(a => a.IsCurrent))
					{
						LoggingTool.BoardSet(Department);
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
                            if (Department.Responsibles != null && Department.Responsibles.Count > 0)
                            {
                                prevResp = Department.Responsibles.Where(a => a.IsCurrent).FirstOrDefault();
                                prevResp.IsCurrent = false;
                            }

                            newResp.Login = passwordGenerator.Login;
							newResp.Password = Helper.EncryptString(passwordGenerator.Password);
							newResp.IsCurrent = true;

							if (passwordGenerator.SendEmail)
							{
								try
								{
									MailSender sender = new();
									sender.RecepientEmail = passwordGenerator.Email;
									sender.SendAuthorizationData(passwordGenerator.Login, passwordGenerator.Password, newResp.Worker, "ответственным по решению проблем");
									MessageBox.Show("Письмо успешно отправлено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
								}
								catch
								{
									MessageBox.Show("Ошибка при отправке письма!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
								}
							}
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
						LoggingTool.NewResponsibleSet(Department, newResp);
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

						if (passwordGenerator.SendEmail)
						{
							try
							{
								MailSender sender = new();
								sender.RecepientEmail = passwordGenerator.Email;
								sender.SendAuthorizationData(passwordGenerator.Login, passwordGenerator.Password, SelectedResponsible.Worker, "ответственным по решению проблем");
								MessageBox.Show("Письмо успешно отправлено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
							}
							catch
							{
								MessageBox.Show("Ошибка при отправке письма!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
							}
						}

						LoggingTool.ResponsibleReset(Department, prevResp, SelectedResponsible);
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

		private RelayCommand setHeader;
		public RelayCommand SetHeader
		{
			get
			{
				return setHeader ?? (setHeader = new(obj =>
				{
					PasswordGenerator passwordGenerator = new();
					if (passwordGenerator.ShowDialog() == true)
					{
						Worker? prevHeader = Workers.FirstOrDefault(a => a.IsHeader == true, null);
						if (prevHeader != null)
						{
							prevHeader.HeaderLogin = null;
							prevHeader.HeaderPassword = null;
							prevHeader.IsHeader = false;
						}

						SelectedWorker.IsHeader = true;
						SelectedWorker.HeaderLogin = passwordGenerator.Login;
						SelectedWorker.HeaderPassword = Helper.EncryptString(passwordGenerator.Password);

						if (passwordGenerator.SendEmail)
						{
							try
							{
								MailSender sender = new();
								sender.RecepientEmail = passwordGenerator.Email;
								sender.SendAuthorizationData(passwordGenerator.Login, passwordGenerator.Password, SelectedWorker, "ответственным по учету проблем");
								MessageBox.Show("Письмо успешно отправлено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
							}
							catch
							{
								MessageBox.Show("Ошибка при отправке письма!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
							}
						}

						LoggingTool.HeaderReset(Department, prevHeader, SelectedWorker);
					}
					else
					{
						MessageBox.Show("Логин и пароль не установлены, операция отменена!", "Отмена", MessageBoxButton.OK, MessageBoxImage.Information);
						return;
					}

					RefreshWorkers();
				},
				obj => SelectedWorker != null));
			}
		}
	}
}
