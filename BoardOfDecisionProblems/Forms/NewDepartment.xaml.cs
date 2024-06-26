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
    /// Логика взаимодействия для NewDepartment.xaml
    /// </summary>
    public partial class NewDepartment : Window
    {
        public NewDepartment()
        {
            InitializeComponent();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(DepNumField.Text))
            {
                MessageBox.Show("Заполните номер участка / подразделения!");
                return;
            }
            if(App.dbContext.Departments.Any(a => a.ViewerNumber == DepNumField.Text))
            {
                MessageBox.Show("Участок с таким номером уже существует!");
                return;
            }
            DialogResult = true;
        }
    }
}
