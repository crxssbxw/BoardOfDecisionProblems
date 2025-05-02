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
    /// Логика взаимодействия для DecideProblem.xaml
    /// </summary>
    public partial class DecideProblem : UserControl
    {
        public DecideProblem()
        {
            Animation.Completed += Animation_Completed;
            InitializeComponent();
        }

        private DoubleAnimation Animation = new DoubleAnimation() { From = 1.0, To = 0, Duration = TimeSpan.FromSeconds(0.25) };

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.BeginAnimation(OpacityProperty, Animation);
        }
        private void Animation_Completed(object? sender, EventArgs e)
        {
            Visibility = Visibility.Collapsed;
        }

        private void AcceptDecision_Click(object sender, RoutedEventArgs e)
        {
            this.BeginAnimation(OpacityProperty, Animation);
        }
    }
}
