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
    public static ProblemsViewModel problemsViewModel = new();
    public MainWindow()
    {
        InitializeComponent();
        DataContext = problemsViewModel;
    }
    private DoubleAnimation OpacityAnimation = new DoubleAnimation() { From = 0, To = 0.9, Duration = TimeSpan.FromSeconds(0.5) };
    private void Statistic_Click(object sender, RoutedEventArgs e)
    {
        StatisticPanel.Visibility = Visibility.Visible;
        StatisticPanel.BeginAnimation(OpacityProperty, OpacityAnimation);
    }

    private void NewProblem_Click(object sender, RoutedEventArgs e)
    {
        NewProblemPanel.Visibility = Visibility.Visible;
        NewProblemPanel.BeginAnimation(OpacityProperty, OpacityAnimation);
        Problem newProblem = new();
        newProblem.Status = "Решается";
        NewProblemPanel.DataContext = newProblem;
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
        problemsViewModel.Add(problem);
    }

    private void ProblemView_Click(object sender, RoutedEventArgs e)
    {
        ProblemPanel.Visibility = Visibility.Visible;
        ProblemPanel.BeginAnimation(OpacityProperty, OpacityAnimation);
        var DataSender = sender as Button;
        ProblemPanel.DataContext = DataSender.DataContext;
    }
}