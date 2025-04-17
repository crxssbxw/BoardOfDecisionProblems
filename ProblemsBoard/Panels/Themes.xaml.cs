using ProblemsBoardLib.ViewModel;
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
    /// Логика взаимодействия для Themes.xaml
    /// </summary>
    public partial class Themes : UserControl
    {
        public Themes()
        {
            Animation.Completed += Animation_Completed;
            InitializeComponent();
        }
        static DoubleAnimation Animation = new DoubleAnimation() { From = 1.0, To = 0, Duration = TimeSpan.FromSeconds(0.25) };

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.BeginAnimation(OpacityProperty, Animation);
        }
        private void Animation_Completed(object? sender, EventArgs e)
        {
            Visibility = Visibility.Collapsed;
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            AddEditTheme.Visibility ^= Visibility.Collapsed;
        }

        private void ThemeAdd_Click(object sender, RoutedEventArgs e)
        {
            AddEditTheme.Visibility = Visibility.Visible;
        }

        private void AddEditCloseBT_Click(object sender, RoutedEventArgs e)
        {
            AddEditTheme.Visibility ^= Visibility.Collapsed;
        }

        private void ThemesLB_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ThemesViewModel themesViewModel = DataContext as ThemesViewModel;
            themesViewModel.AddEditThemeViewModel = new()
            {
                Theme = themesViewModel.SelectedTheme,
                Title = "Изменить тему",
                Department = themesViewModel.Department,
                IsNoEdit = false
            };
            AddEditTheme.Visibility = Visibility.Visible;
        }
    }
}
