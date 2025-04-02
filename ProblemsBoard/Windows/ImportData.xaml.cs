using DocumentFormat.OpenXml.Wordprocessing;
using ProblemsBoardLib.ExcelDataReader;
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

namespace ProblemsBoard.Windows
{
    /// <summary>
    /// Логика взаимодействия для ImportData.xaml
    /// </summary>
    public partial class ImportData : Window, INotifyPropertyChanged
    {

        private DatabaseContext DatabaseContext { get; set; } = new();

        private ExcelDataImport ExcelDataImport { get; set; }

        public string Help { get; set; } =
            @"Для импорта данных книга Excel должна содержать два листа с названиями ""Участки"" и ""Сотрудники"". " +
            @"Лист ""Участки"" должен содержать три столбца: ID, Название, Номер участка. " +
            @"Лист ""Сотрудники"" должен содержать шесть столбцов: ID, Имя, Фамилия, Отчество, Должность, ID Участка. " +
            @"Заголовки столбцов обязательны. Порядок указанных столбцов обязателен. ";


        private string selectedFile = "Файл не выбран";
        public string SelectedFile
        {
            get => selectedFile;
            set
            {
                selectedFile = value;
                OnPropertyChanged(nameof(SelectedFile));
            }
        }

        public ImportData()
        {
            InitializeComponent();
            DataContext = this;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private void SelectFile_Click(object sender, RoutedEventArgs e)
        {
            ExcelDataImport = new();
            ExcelDataImport.AskFile();
            if (ExcelDataImport.IsPathAllowed && ExcelDataImport.IsRightFormat)
            {
                SelectedFile = ExcelDataImport.FilePath;
            }
            else
                SelectedFile = "Файл не выбран";
            FillTables(ExcelDataImport);
        }

        private ObservableCollection<Department> deltaDepartments = new();
        public ObservableCollection<Department> DeltaDepartments
        {
            get => deltaDepartments;
            set
            {
                deltaDepartments = value;
                OnPropertyChanged(nameof(DeltaDepartments));
            }
        }

        private ObservableCollection<Worker> deltaWorkers = new();
        public ObservableCollection<Worker> DeltaWorkers
        {
            get => deltaWorkers;
            set
            {
                deltaWorkers = value;
                OnPropertyChanged(nameof(DeltaWorkers));
            }
        }

        private void FillTables(ExcelDataImport ExcelDataImport)
        {
            try
            {
                ExcelDataImport.Read();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            DeltaDepartments.Clear();
            DeltaWorkers.Clear();

            foreach (var department in ExcelDataImport.Departments)
            {
                if (DatabaseContext.Departments.Any(a => a.DepartmentId == department.DepartmentId))
                    continue;

                DeltaDepartments.Add(department);
            }

            foreach (var worker in ExcelDataImport.Workers)
            {
                if (DatabaseContext.Workers.Any(a => a.WorkerId == worker.WorkerId))
                    continue;

                DeltaWorkers.Add(worker);
            }
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Grid_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                ExcelDataImport = new();

                string[] file = (string[])e.Data.GetData(DataFormats.FileDrop);
                ExcelDataImport.GetDropped(file[0]);

                if (ExcelDataImport.IsPathAllowed && ExcelDataImport.IsRightFormat)
                {
                    SelectedFile = ExcelDataImport.FilePath;
                }
                else
                    SelectedFile = "Файл не выбран";
                FillTables(ExcelDataImport);
            }
        }

        private void Grid_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.All;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = false;
        }
    }
}
