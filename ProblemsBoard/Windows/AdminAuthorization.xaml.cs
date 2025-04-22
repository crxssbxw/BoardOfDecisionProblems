using ProblemsBoardLib.Models;
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

namespace ProblemsBoard.Windows
{
    /// <summary>
    /// Логика взаимодействия для AdminAuthorization.xaml
    /// </summary>
    public partial class AdminAuthorization : Window
    {
        public Department Department { get; set; }
        public AdminAuthorization(Department department)
        {
            InitializeComponent();
            Department = department;
        }

        private void AcceptBT_Click(object sender, RoutedEventArgs e)
        {
            if (Department?.Admin?.Login == LoginTB.Text)
            {
                if (Department?.Admin?.Password == PasswordPB.Password)
                {
                    DialogResult = true;
                }
                else
                    MessageBox.Show("Неверный пароль!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
                MessageBox.Show("Неверный логин!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void CancelBT_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
