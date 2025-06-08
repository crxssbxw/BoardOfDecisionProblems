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
			dbContext.Departments.Load();
			dbContext.Workers.Load();
			dbContext.DepartmentThemes.Load();
			Department = dbContext.Departments.Find(department.DepartmentId);

			// var themesload = dbContext.Themes.Include(a => a.Departments).ToList();

            foreach (var theme in Department.Themes)
            {
                Themes.Add(theme);
            }

            foreach (var theme in dbContext.Themes.Where(a => a.Departments.Count() == 0))
			{
				Themes.Add(theme);
			}

			//if (Department.Responsibles != null)
			//{
			//             foreach (var responsible in Department.Responsibles)
			//             {
			//                 Responsibles.Add(responsible);
			//             }
			//         }

			if (Department.Workers != null)
			{
				foreach (var worker in Department.Workers)
				{
					Workers.Add(worker);
				}
			}

			//         if (Department.Admin == null)
			//{
			//	Login = "admin";
			//	Password = "admin";
			//}
			//else
			//{
			//Login = Department.Admin.Login;
			//}
		}

		private void RefreshWorkers()
		{
			Workers.Clear();
			foreach (var worker in Department.Workers)
			{
				Workers.Add(worker);
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
                    PasswordGenerator passwordGenerator = new();
                    if (passwordGenerator.ShowDialog() == true)
                    {
                        Worker? prevResp = Workers.FirstOrDefault(a => a.IsResponsible == true, null);
                        if (prevResp != null)
                        {
                            prevResp.IsResponsible = false;
                        }

                        SelectedWorker.IsResponsible = true;
                        User user = new()
                        {
                            Login = passwordGenerator.Login,
                            Password = Helper.EncryptString(passwordGenerator.Password),
                            Role = "Ответственный по решению проблем"
                        };

                        dbContext.Users.Add(user);

                        if (passwordGenerator.SendEmail)
                        {
                            try
                            {
                                MailSender sender = new();
                                sender.RecepientEmail = passwordGenerator.Email;
                                sender.SendAuthorizationData(passwordGenerator.Login, passwordGenerator.Password, SelectedWorker, "ответственным по решению проблем");
                                MessageBox.Show("Письмо успешно отправлено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            catch
                            {
                                MessageBox.Show("Ошибка при отправке письма!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }

                        LoggingTool.ResponsibleReset(Department, prevResp, SelectedWorker);
                    }
                    else
                    {
                        MessageBox.Show("Логин и пароль не установлены, операция отменена!", "Отмена", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    OnPropertyChanged(nameof(Workers));
                    OnPropertyChanged(nameof(SelectedWorker));
                    RefreshWorkers();
                },
				obj => SelectedWorker != null));
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
							prevHeader.IsHeader = false;
						}

						SelectedWorker.IsHeader = true;
						User user = new()
						{
							Login = passwordGenerator.Login,
							Password = Helper.EncryptString(passwordGenerator.Password),
							Role = "Главный по сбору проблем"
                        };

						dbContext.Users.Add(user);

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

                    OnPropertyChanged(nameof(Workers));
					OnPropertyChanged(nameof(SelectedWorker));
                    RefreshWorkers();
				},
				obj => SelectedWorker != null));
			}
		}
	}
}
