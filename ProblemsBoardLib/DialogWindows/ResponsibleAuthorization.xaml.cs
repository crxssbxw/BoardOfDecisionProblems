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
    /// Логика взаимодействия для ResponsibleAuthorization.xaml
    /// </summary>
    public partial class ResponsibleAuthorization : Window, INotifyPropertyChanged
    {
        DatabaseContext DatabaseContext { get; set; } = new();
        //public ResponsibleAuthorization(Responsible responsible)
        //{
        //    Responsible = DatabaseContext.Responsibles.Find(responsible.ResponsibleId);
        //    InitializeComponent();
        //    DataContext = this;
        //}

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
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

        private string Password
        {
            get => PasswordPB.Password;
        }

        private bool Validation()
        {
            //if (Login == Responsible.Login)
            //{
            //    if (Helper.EncryptString(Password) == Responsible.Password)
            //        return true;
            //    else
            //        MessageBox.Show("Неверный пароль", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            //}
            //else MessageBox.Show("Неверный логин", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void OkBT_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
                DialogResult = true;
        }

        private void CancelBT_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void DockPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
