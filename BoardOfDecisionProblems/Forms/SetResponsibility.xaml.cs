using BoardOfDecisionProblems.Models;
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

namespace BoardOfDecisionProblems.Forms
{
    /// <summary>
    /// Логика взаимодействия для SetResponsibility.xaml
    /// </summary>
    public partial class SetResponsibility : Window
    {
        public SetResponsibility()
        {
            InitializeComponent();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            var worker = WorkersBox.SelectedItem as Worker;
            var department = DepartmentsBox.SelectedItem as Department;
            if(department == null)
            {
                MessageBox.Show("Выберите участок / подразделение!");
                return;
            }
            if(worker == null)
            {
                MessageBox.Show("Выберите работника!");
                return;
            }
            if (worker.Department != department)
            {
                MessageBox.Show("Данный работник не относится к этому участку");
                return;
            }
            DialogResult = true;
        }
    }
}
