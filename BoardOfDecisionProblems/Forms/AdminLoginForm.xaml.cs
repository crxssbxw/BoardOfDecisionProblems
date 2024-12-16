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

namespace BoardOfDecisionProblems.Forms
{
    /// <summary>
    /// Логика взаимодействия для AdminLoginForm.xaml
    /// </summary>
    public partial class AdminLoginForm : Window, INotifyPropertyChanged
    {
        private string _login;
        public string Login
        {
            get => _login;
            set 
            { 
                _login = value;
                OnPropertyChanged(nameof(Login));
            }
        }

        private string _password;
        public string Password
        {
            get => PasswordField.Password;
        }

        private string warningMessage = "";
        public string WarningMessage 
        { 
            get => warningMessage; 
            set 
            { 
                warningMessage = value;
                OnPropertyChanged(nameof(WarningMessage));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AdminLoginForm()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            if (Login == App.Admin.Login)
            {
                if (Password == App.Admin.Password)
                {
                    DialogResult = true;
                    return;
                }
                else WarningMessage = "Неверный пароль";
            }
            else WarningMessage = "Неверный логин";
        }
    }
}
