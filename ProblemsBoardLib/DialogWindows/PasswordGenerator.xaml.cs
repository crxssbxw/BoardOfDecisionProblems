using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
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

namespace ProblemsBoardLib.DialogWindows
{
    /// <summary>
    /// Логика взаимодействия для PasswordGenerator.xaml
    /// </summary>
    public partial class PasswordGenerator : Window, INotifyPropertyChanged
    {
        public PasswordGenerator()
        {
            InitializeComponent();
            DataContext = this;
            Generate();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        private string message;
        public string Message
        {
            get => message;
            set
            {
                message = value;
                OnPropertyChanged(nameof(Message));
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

        private void Generate()
        {
            using (DatabaseContext dbContext = new DatabaseContext())
            {
                do
                {
                    Login = Helper.GenerateLogin();
                }
                while (dbContext.Responsibles.Any(a => Login == a.Login));
            }

            Password = Helper.GeneratePassword(10);
        }

        private void ResetBT_Click(object sender, RoutedEventArgs e)
        {
            Generate();
        }

        private void CopyAllBT_Click(object sender, RoutedEventArgs e)
        {
            Helper.CopyToClipboard($"{Login} {Password}");
            Message = "Логин и пароль скопированы в буфер обмена!";
        }

        private void OkBT_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CopyLoginBT_Click(object sender, RoutedEventArgs e)
        {
            Helper.CopyToClipboard(Login);
            Message = "Логин скопирован в буфер обмена!";
        }

        private void CopyPasswordBT_Click(object sender, RoutedEventArgs e)
        {
            Helper.CopyToClipboard(Password);
            Message = "Пароль скопирован в буфер обмена!";
        }
    }
}
