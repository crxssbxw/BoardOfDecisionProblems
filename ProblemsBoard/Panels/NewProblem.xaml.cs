using ProblemsBoard.Windows;
using ProblemsBoardLib;
using ProblemsBoardLib.Models;
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
    /// Логика взаимодействия для NewProblem.xaml
    /// </summary>
    public partial class NewProblem : UserControl
    {
        private NewProblemPanelViewModel viewModel;
        public NewProblemPanelViewModel ViewModel 
        { 
            get => viewModel; 
            set => viewModel = value; 
        }

        public NewProblem()
        {
            Animation.Completed += Animation_Completed;
            InitializeComponent();
            DataContext = ViewModel;
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
<<<<<<< HEAD
<<<<<<< HEAD
            ProblemsBoardLib.Models.Problem? problem = DataContext as ProblemsBoardLib.Models.Problem;
<<<<<<< HEAD
            MainWindow.ProblemsViewModel.Add(problem);
=======
            ViewModel.Problems.Add(problem);
>>>>>>> 081a081 (Added Startup Window)
=======
            ProblemsBoardLib.Models.Problem problem = DataContext as ProblemsBoardLib.Models.Problem;
            ViewModel.Problems.Add(problem);
            try
            {
                var dep = ViewModel.dbContext.Departments.Find(problem.Department.DepartmentId);
                if (dep.Problems == null)
                    dep.Problems = [];
                dep.Problems.Add(problem);
                ViewModel.dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
>>>>>>> e8a7a46 (Now problems added to DB)
=======
>>>>>>> ce1a19b (Added themes view in menu, adding themes to db, model changes)
            this.BeginAnimation(OpacityProperty, Animation);
        }
    }
}
