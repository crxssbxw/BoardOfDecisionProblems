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
    /// Логика взаимодействия для NewTheme.xaml
    /// </summary>
    public partial class NewTheme : Window
    {
        public NewTheme()
        {
            InitializeComponent();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ThemeNameField.Text))
            {
                MessageBox.Show("Заполните название темы!");
                return;
            }
            DialogResult = true;
        }
    }
}
