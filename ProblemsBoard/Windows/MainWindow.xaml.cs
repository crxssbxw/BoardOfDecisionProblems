using ProblemsBoardLib.ViewModel;
using ProblemsBoardLib.Models;
using System.Text;
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

namespace ProblemsBoard.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public static ProblemsViewModel ViewModel { get; set; }
    public MainWindow(Department department)
    {
        ViewModel = new(department);
        InitializeComponent();
        DataContext = ViewModel;
    }
    private DoubleAnimation OpacityAnimation = new DoubleAnimation() { From = 0, To = 1.0, Duration = TimeSpan.FromSeconds(0.25) };
    private void Statistic_Click(object sender, RoutedEventArgs e)
    {
        StatisticPanel.Visibility = Visibility.Visible;
        StatisticPanel.BeginAnimation(OpacityProperty, OpacityAnimation);
    }

    private void NewProblem_Click(object sender, RoutedEventArgs e)
    {
        NewProblemPanel.Visibility = Visibility.Visible;
        NewProblemPanel.BeginAnimation(OpacityProperty, OpacityAnimation);
    }

    private int TestPlusCount = 1;
    private void TestPlus_Click(object sender, RoutedEventArgs e)
    {
        string[] Statuses = { "Решено", "Решается" };
        Problem problem = new()
        {
            ProblemId = TestPlusCount++,
            Description = "Test",
            Status = Statuses[new Random().Next(0, 2)]
        };
        ViewModel.Problems.Add(problem);
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        var result = MessageBox.Show("Хотите сменить отдел / цех?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            Startup startup = new();
            startup.Show();
        }
        else Application.Current.Shutdown();
    }

    private void Themes_Click(object sender, RoutedEventArgs e)
    {
        ThemesPanel.Visibility = Visibility.Visible;
        ThemesPanel.BeginAnimation(OpacityProperty, OpacityAnimation);
    }

    private void Responsibles_Click(object sender, RoutedEventArgs e)
    {

    }

    private void ProblemView_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        ProblemPanel.Visibility = Visibility.Visible;
        ProblemPanel.BeginAnimation(OpacityProperty, OpacityAnimation);
    }

    private void DecideView_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        DecisionPanel.Visibility = Visibility.Visible;
        DecisionPanel.BeginAnimation(OpacityProperty, OpacityAnimation);
    }

    private void ReportsMenu_Click(object sender, RoutedEventArgs e)
    {
        ReportsWindow reportsWindow = new ReportsWindow();
        reportsWindow.ShowDialog();
    }

    private void LoggerMenu_Click(object sender, RoutedEventArgs e)
    {
        LoggerWindow loggerWindow = new LoggerWindow();
        loggerWindow.ShowDialog();
    }
}