using ProblemsBoardLib;
using ProblemsBoardLib.Models;
using System;
using System.Collections.Generic;
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

namespace ProblemsBoard.Windows
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window, INotifyPropertyChanged
    {
        private Roles requiredRole;
        private Roles outRole;
        private string login;
        private List<User> users = new();

        private DatabaseContext _context { get; set; } = new();
        public Roles RequiredRole 
        { 
            get => requiredRole; 
            set
            {
                requiredRole = value;
                OnPropertyChanged(nameof(RequiredRole));
            } 
        }

        public string Login 
        { 
            get => login;
            set
            {
                login = value;
                OnPropertyChanged(nameof(Login));
            }
        }

        private string Password
        {
            get => PasswordPB.Password;
        }
        public Roles OutRole 
        { 
            get => outRole; 
            set
            {
                outRole = value;
                OnPropertyChanged(nameof(OutRole));
            } 
        }

        public Authorization(Roles required = Roles.None)
        {
            InitializeComponent();
            RequiredRole = required;
            users = _context.Users.ToList();
            DataContext = this;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool ValidateAdmin()
        {
            if (users.Where(a => a.EnumRole == Roles.RAdmin).Any(a => a.Login == Login && a.Password == Helper.EncryptString(Password)))
            {
                OutRole = Roles.RAdmin;
                return true;
            }
            return false;
        }
        public bool ValidateHeader()
        {
            if (users.Where(a => a.EnumRole == Roles.RHeaderWorker).Any(a => a.Login == Login && a.Password == Helper.EncryptString(Password)))
            {
                OutRole = Roles.RHeaderWorker;
                return true;
            }
            return false;
        }
        public bool ValidateResponsible()
        {
            if (users.Where(a => a.EnumRole == Roles.RResponsible).Any(a => a.Login == Login && a.Password == Helper.EncryptString(Password)))
            {
                OutRole = Roles.RResponsible;
                return true;
            }
            return false;
        }

        public bool Validate()
        {
            switch (RequiredRole)
            {
                case Roles.RAdmin:
                    if (ValidateAdmin()) return true;
                    else
                    {
                        MessageBox.Show("Неверный логин или пароль!", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                case Roles.RResponsible:
                    if (ValidateResponsible()) return true;
                    else
                    {
                        MessageBox.Show("Неверный логин или пароль!", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                case Roles.RHeaderWorker:
                    if (ValidateHeader()) return true;
                    else
                    {
                        MessageBox.Show("Неверный логин или пароль!", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                case Roles.None:
                    if (ValidateResponsible() || ValidateHeader() || ValidateAdmin()) return true;
                    else
                    {
                        MessageBox.Show("Неверный логин или пароль!", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                default:
                    MessageBox.Show("Неверный логин или пароль!", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
            }
        }

        private void OkBT_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                DialogResult = true;
            }
        }

        private void CancelBT_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DockPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
