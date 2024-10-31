using BoardOfDecisionProblems.Commands;
using BoardOfDecisionProblems.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BoardOfDecisionProblems.ViewModel
{
    /// <summary>
    /// Представление модели пользователей
    /// </summary>
    public class UserViewModel : BaseViewModel
    {
        /// <summary>
        /// Список доступных ролей пользователей
        /// </summary>
        public List<string> Roles => new() { "Работник", "Ответственный" };
        /// <summary>
        /// Свойство для вызова окна авторизации
        /// </summary>
        public BoardOfDecisionProblems.Windows.Authorization Authorization { get; set; }
        /// <summary>
        /// Свойство для вызова окна регистрации
        /// </summary>
        public BoardOfDecisionProblems.Windows.Registration RegistrationWindow { get; set; }

        private ObservableCollection<User> users = new();
        /// <summary>
        /// Коллекция Пользователей
        /// </summary>
        public ObservableCollection<User> Users
        {
            get => users;
            set
            {
                users = value;
                OnPropertyChanged(nameof(Users));
            }
        }

        /// <summary>
        /// Представление ответственных
        /// </summary>
        public ResponsiblesViewModel responsibles { get; set; } = new();
        /// <summary>
        /// Представление отделов
        /// </summary>
        public DepartmentsViewModel departments { get; set; } = new();
        /// <summary>
        /// Представление работников
        /// </summary>
        public WorkersViewModel workers { get; set; } = new();

        /// <summary>
        /// Изменение выпадающего списка при регистрации в зависимости от выбора роли
        /// </summary>
        public IList Collection
        {
            get
            {
                if (SelectedRole == null) return null;
                if (SelectedRole.ToLower() == "Работник") return departments.Departments.ToList();
                if (SelectedRole.ToLower() == "Ответственный") return responsibles.Responsibles.ToList();
                else return null;
            }
        }


        private string loginField;
        /// <summary>
        /// Поле Логина
        /// </summary>
        public string LoginField
        {
            get => loginField;
            set
            {
                loginField = value;
                OnPropertyChanged(nameof(LoginField));
            }
        }

        private string passwordField;
        /// <summary>
        /// Поле Пароля
        /// </summary>
        public string PasswordField
        {
            private get => passwordField;
            set
            {
                passwordField = value;
                OnPropertyChanged(nameof(PasswordField));
            }
        }

        private string repeatPasswordField;
        /// <summary>
        /// Поле повтора пароля
        /// </summary>
        public string RepeatPasswordField
        {
            private get => repeatPasswordField;
            set
            {
                repeatPasswordField = value;
                OnPropertyChanged(nameof(RepeatPasswordField));
            }
        }

        private string selectedRole;
        /// <summary>
        /// Объект выбранной роли
        /// </summary>
        public string SelectedRole
        {
            get => selectedRole;
            set
            {
                selectedRole = value;
                OnPropertyChanged(nameof(SelectedRole));
                OnPropertyChanged(nameof(Collection));
            }
        }

        private object? selectedObject;
        /// <summary>
        /// Объект выбранной роли. 
        /// Используется для дальнейшей регистрации
        /// </summary>
        public object? SelectedObject
        {
            get => selectedObject;
            set
            {
                selectedObject = value;
                OnPropertyChanged(nameof(SelectedObject));
            }
        }

        public UserViewModel()
        {
            if (dbContext.Users.Count() == 0)
            {
                User user = new User()
                {
                    Login = "admin",
                    Password = "admin",
                    Role = "admin"
                };

                User user1 = new User()
                {
                    Login = "worker",
                    Password = "password",
                    Role = "worker"
                };

                User user2 = new User()
                {
                    Login = "responsible",
                    Password = "password",
                    Role = "responsible"
                };

                dbContext.Users.Add(user);
                dbContext.Users.Add(user1);
                dbContext.Users.Add(user2);
                dbContext.SaveChanges();
            }

            dbContext.Workers.Load();
            dbContext.Departments.Load();
            foreach (var user in dbContext.Users)
            {
                users.Add(user);
            }
        }

        /// <summary>
        /// Валидация логина
        /// </summary>
        /// <param name="login">Строка логина</param>
        /// <returns>True если логин прошел валидацию</returns>
        public bool LoginValidation(string login)
        {
            if (Users.Any(a => a.Login == login))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Валидация пароля
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        /// <returns>True если пароль прошел валидацию, иначе False</returns>
        public bool PasswordValidation(string login, string password)
        {
            if (Users.Any(a => a.Login == login && a.Password == password)){
                return true;
            }
            return false;
        }

        /// <summary>
        /// Валидация регистрации
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        /// <returns>True, если регистрация прошла валидацию, иначе False</returns>
        public bool RegistrationValidation(string login, string password)
        {
            if (string.IsNullOrEmpty(password)) return false;
            if (password != RepeatPasswordField) return false;
            if (SelectedRole == null) return false;
            if (SelectedObject == null) return false;
            return true;
        }

        private RelayCommand login;
        /// <summary>
        /// Команда авторизации
        /// </summary>
        public RelayCommand Login
        {
            get
            {
                return login ?? (login = new(obj =>
                {
                    if(LoginValidation(loginField) && PasswordValidation(LoginField, PasswordField))
                    {
                        App.CurrentUser = Users.Where(a => a.Login == LoginField).FirstOrDefault();
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        Authorization.Close();
                    }
                    else
                    {
                        MessageBox.Show("Некорректные данные!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                },
                obj => !String.IsNullOrEmpty(LoginField) 
                && !String.IsNullOrEmpty(PasswordField)));
            }
        }

        private RelayCommand registration;
        /// <summary>
        /// Команда регистрации
        /// </summary>
        public RelayCommand Registration
        {
            get
            {
                return registration ?? (registration = new(obj =>
                {

                    if (LoginValidation(LoginField))
                    {
                        MessageBox.Show("Пользователь с данным логином существует");
                        return;
                    }
                    User newuser = new();

                    newuser.Login = LoginField;
                    newuser.Password = PasswordField;
                    if (SelectedRole == "Работник") 
                    { 
                        newuser.Role = "worker";
                        newuser.Department = SelectedObject as Department;
                    }
                    if (SelectedRole == "Ответственный") 
                    { 
                        newuser.Role = "responsible";
                        var responsible = SelectedObject as Responsible;
                        newuser.Worker = responsible.Worker;
                    }
                    
                    dbContext.Users.Add(newuser);
                    dbContext.SaveChanges();
                    Users.Add(newuser);
                    RegistrationWindow.Close();
                }, 
                obj => RegistrationValidation(LoginField, PasswordField)));
            }
        }
    }
}
