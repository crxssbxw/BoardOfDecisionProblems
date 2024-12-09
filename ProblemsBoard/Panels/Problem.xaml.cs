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
    /// Логика взаимодействия для Problem.xaml
    /// </summary>
    public partial class Problem : UserControl
    {
        public Problem()
        {
            InitializeComponent();
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            var Animation = new DoubleAnimation() { From = this.Opacity, To = 0, Duration = TimeSpan.FromSeconds(0.5) };
            Animation.Completed += Animation_Completed;
            this.BeginAnimation(OpacityProperty, Animation);
        }
        private void Animation_Completed(object? sender, EventArgs e)
        {
            Visibility = Visibility.Collapsed;
        }
    }
}
