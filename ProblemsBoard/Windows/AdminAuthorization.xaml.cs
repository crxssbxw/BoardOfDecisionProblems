using Microsoft.EntityFrameworkCore;
using ProblemsBoardLib;
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
        private Admin Super { get; set; } = new();
        private List<Responsible> Responsibles { get; set; }
        private DatabaseContext context { get; set; } = new();
        public Roles OutRole { get; set; }
        public AdminAuthorization(Department department = null)
        {
            InitializeComponent();
            context.Responsibles.Load();
            context.Admins.Load();
            context.Workers.Load();
            Super = context.Admins.Find(1);
			Department = context.Departments.Find(department.DepartmentId);
            if (Department.Responsibles != null || Department.Responsibles.Count > 0)
                Responsibles = Department.Responsibles.ToList();
		}

        private void AcceptBT_Click(object sender, RoutedEventArgs e)
        {
            if (Responsibles != null)
            {
                if (Responsibles.Any(a => LoginTB.Text == a.Login))
                {
                    if (Responsibles.Any(a => a.Login == LoginTB.Text && a.Password == Helper.EncryptString(PasswordPB.Password)))
                    {
                        OutRole = Roles.RResponsible;
                        DialogResult = true;
                        return;
                    }
                    else
                        MessageBox.Show("Неверный логин или пароль!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            if (Department.Workers.Any(a => a.IsHeader))
            {
                var worker = Department.Workers.FirstOrDefault(a => a.IsHeader);
                if (worker.HeaderLogin == LoginTB.Text)
                {
                    if (Helper.EncryptString(PasswordPB.Password) == worker.HeaderPassword)
                    {
                        OutRole = Roles.RHeaderWorker;
                        DialogResult = true;
                        return;
                    }
                    else
                        MessageBox.Show("Неверный логин или пароль!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            if (Department?.Admin?.Login == LoginTB.Text)
            {
                if (Department?.Admin?.Password == Helper.EncryptString(PasswordPB.Password))
                {
                    OutRole = Roles.RAdmin;
                    DialogResult = true;
                }
                else
                    MessageBox.Show("Неверный пароль!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (Super.Login == LoginTB.Text)
            {
                if (Super.Password == Helper.EncryptString(PasswordPB.Password))
                {
                    OutRole = Roles.RAdmin;
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
