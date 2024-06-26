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
    /// Логика взаимодействия для NewWorker.xaml
    /// </summary>
    public partial class NewWorker : Window
    {
        public NewWorker()
        {
            InitializeComponent();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(FirstNameField.Text) || string.IsNullOrEmpty(SecondNameField.Text) || string.IsNullOrEmpty(PostField.Text))
            {
                MessageBox.Show("Заполните поля, помеченные '*'!");
                return;
            }
            if(DepartmentBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите участок");
                return;
            }
            DialogResult = true;
        }
    }
}
