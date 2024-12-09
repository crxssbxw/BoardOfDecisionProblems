using ProblemsBoard.Windows;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProblemsBoard.Panels
{
    /// <summary>
    /// Логика взаимодействия для NewProblem.xaml
    /// </summary>
    public partial class NewProblem : UserControl
    {
        public NewProblem()
        {
            Animation.Completed += Animation_Completed;
            InitializeComponent();
        }

        static DoubleAnimation Animation = new DoubleAnimation() { From = 0.9, To = 0, Duration = TimeSpan.FromSeconds(0.5) };
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.BeginAnimation(OpacityProperty, Animation);
        }
        private void Animation_Completed(object? sender, EventArgs e)
        {
            DataContext = null;
            Visibility = Visibility.Collapsed;
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            ProblemsBoardLib.Models.Problem? problem = DataContext as ProblemsBoardLib.Models.Problem;
            MainWindow.problemsViewModel.Add(problem);
            this.BeginAnimation(OpacityProperty, Animation);
        }
    }
}
