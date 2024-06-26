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
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public DepartmentsViewModel departmentsViewModel = new();
        public ResponsiblesViewModel responsiblesViewModel = new();
        public Registration()
        {
            InitializeComponent();
        }

        private void FirstPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ((dynamic)this.DataContext).PasswordField = FirstPassword.Password;
        }

        private void RepeatPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ((dynamic)this.DataContext).RepeatPasswordField = RepeatPassword.Password;
        }

        private void RoleBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(RoleBox.SelectedItem != null)
            {
                if (RoleBox.SelectedItem == "Работник") SelectionBox.ItemsSource = departmentsViewModel.Departments;
                else SelectionBox.ItemsSource = responsiblesViewModel.Responsibles;
            }
        }
    }
}
