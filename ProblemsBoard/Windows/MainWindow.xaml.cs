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
<<<<<<< HEAD
<<<<<<< HEAD
    private static ProblemsViewModel problemsViewModel = new();
    public static ProblemsViewModel ProblemsViewModel
    {
        get => problemsViewModel;
        set => problemsViewModel = value;
    }
=======
    public ProblemsViewModel ViewModel { get; set; } = new();
>>>>>>> 081a081 (Added Startup Window)
    public MainWindow()
=======
    public static ProblemsViewModel ViewModel { get; set; }
    public MainWindow(Department department)
>>>>>>> e8a7a46 (Now problems added to DB)
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
        Problem newProblem = new()
        {
            Department = ViewModel.Department,
            DateOccurance = DateTime.Now,
            Description = "Описание",
            Status = "Решается"
        };
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
        ViewModel.Problems.Add(problem);
    }

    private void ProblemView_Click(object sender, RoutedEventArgs e)
    {
        ProblemPanel.Visibility = Visibility.Visible;
        ProblemPanel.BeginAnimation(OpacityProperty, OpacityAnimation);
        var DataSender = sender as Button;
        ViewModel.SelectedProblem = DataSender.DataContext as Problem;
        ProblemPanel.DataContext = ViewModel.SelectedProblem;
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
}