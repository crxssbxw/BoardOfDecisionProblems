using BoardOfDecisionProblems.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace BoardOfDecisionProblems.Windows
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        public Authorization()
        {
            InitializeComponent();
            DataContext = new UserViewModel();
            ((dynamic)this.DataContext).Authorization = this;
        }

        private void PasswordField_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ((dynamic)this.DataContext).PasswordField = PasswordField.Password;
        }

        private void RegistrationBtn_Click(object sender, RoutedEventArgs e)
        {
            Registration registration = new Registration();
            ((dynamic)this.DataContext).RegistrationWindow = registration;
            registration.DataContext = this.DataContext;
            registration.Show();
        }
    }
}
