using BoardOfDecisionProblems.Forms;
using BoardOfDecisionProblems.ViewModel;
using BoardOfDecisionProblems.Windows;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BoardOfDecisionProblems
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Главное объектное представление программы
        /// </summary>
        public static ProblemViewModel ProblemViewModel { get; set; } = new();
        /// <summary>
        /// Инициализация главного окна
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DataContext = ProblemViewModel;
        }

        private void DepartmentsButton_Click(object sender, RoutedEventArgs e)
        {
            DepartmentsView departmentsView = new DepartmentsView();
            departmentsView.DataContext = ProblemViewModel.DepartmentsViewModel;
            departmentsView.Show();
        }

        private void DepartmentBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ThemeBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void DecisionTimeBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        /// <summary>
        /// Обработка события нажатия кнопки "Рабочие"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkersButton_Click(object sender, RoutedEventArgs e)
        {
            WorkersView workersView = new WorkersView();
            workersView.DataContext = ProblemViewModel.WorkersViewModel;
            workersView.Show();
        }

        private void ResponsiblesButton_Click(object sender, RoutedEventArgs e)
        {
            ResponsibleView responsibleView = new ResponsibleView();
            responsibleView.DataContext = ProblemViewModel.ResponsiblesViewModel;
            responsibleView.Show();
        }

        private void ThemesButton_Click(object sender, RoutedEventArgs e)
        {
            ThemesView themesView = new ThemesView();
            themesView.DataContext = ProblemViewModel.ThemesViewModel;
            themesView.Show();
        }

        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProblemViewModel.IsAdmin == true)
            {
                MessageBox.Show("Вы уже авторизованы как администратор", "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            AdminLoginForm adminLoginForm = new AdminLoginForm();
            if(adminLoginForm.ShowDialog() == true)
            {
                ProblemViewModel.IsAdmin = true;
            }
        }
    }
}