using ProblemsBoardLib.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace ProblemsBoardLib.DialogWindows
{
    /// <summary>
    /// Логика взаимодействия для NewResponsibleDialog.xaml
    /// </summary>
    public partial class NewResponsibleDialog : Window, INotifyPropertyChanged
    {
        private DatabaseContext DatabaseContext { get; set; } = new();
        public NewResponsibleDialog(Department department, Worker worker)
        {
            InitializeComponent();
            OutWorker = worker;
            foreach(var dbworker in DatabaseContext.Workers.Where(a => 
                a.Responsibles == null || 
                a.Responsibles.Count() == 0 && 
                a.Department.DepartmentId == department.DepartmentId))
            {
                Workers.Add(dbworker);
            }

            DataContext = this;
        }

        private ObservableCollection<Worker> workers = new();
        public ObservableCollection<Worker> Workers
        {
            get => workers;
            set
            {
                workers = value;
                OnPropertyChanged(nameof(Workers));
            }
        }

        private Worker selectedWorker = new();
        public Worker SelectedWorker
        {
            get => selectedWorker;
            set
            {
                selectedWorker = value;
                OnPropertyChanged(nameof(SelectedWorker));
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        private Worker OutWorker { get; set; }

        public bool IsSelected
        {
            get
            {
                if (SelectedWorker.WorkerId != 0)
                    return true;
                return false;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        private void ContinueBT_Click(object sender, RoutedEventArgs e)
        {
            Helper.CopyTo(SelectedWorker, OutWorker);
            DialogResult = true;
        }

        private void CancelBT_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
