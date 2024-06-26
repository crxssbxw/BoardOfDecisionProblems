using BoardOfDecisionProblems.ViewModel;
using BoardOfDecisionProblems.Windows;
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
    /// Логика взаимодействия для NewProblem.xaml
    /// </summary>
    public partial class NewProblem : Window
    {
        public NewProblem()
        {
            InitializeComponent();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            if(OccuranceDateBox.SelectedDate > DateTime.Now)
            {
                MessageBox.Show("Проблема не может быть создана позднее сегодняшнего дня");
            }
            DialogResult = true;
        }

        private void ThemesBtn_Click(object sender, RoutedEventArgs e)
        {
            ThemesView themesView = new ThemesView();
            themesView.DataContext = ProblemViewModel.ThemesViewModel;
            themesView.ShowDialog();
        }
    }
}
