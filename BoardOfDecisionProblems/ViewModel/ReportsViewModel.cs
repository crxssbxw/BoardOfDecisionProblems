using BoardOfDecisionProblems.Commands;
using BoardOfDecisionProblems.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Xps.Packaging;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace BoardOfDecisionProblems.ViewModel
{
    public class ReportsViewModel : BaseViewModel
    {
        public ReportsViewModel()
        {
            dbContext.Problems.Load();
            dbContext.Responsibles.Load();
            dbContext.Departments.Load();

            foreach(var report in dbContext.Reports)
            {
                Reports.Add(report);
            }
        }

        private ObservableCollection<Report> _reports = new();

        public ObservableCollection<Report> Reports
        {
            get => _reports;
            set
            {
                _reports = value;
                OnPropertyChanged(nameof(Reports));
            }
        }

        public static ObservableCollection<Problem> Problems
        {
            get => MainWindow.ProblemViewModel.Problems;
        }

        private Report _selectedReport;
        public Report SelectedReport
        {
            get => _selectedReport;
            set
            {
                _selectedReport = value;
                OnPropertyChanged(nameof(SelectedReport));
            }
        }

        private static string ReportHeader =
            "Акционерное Общество" + Environment.NewLine + "\"КОВРОВСКИЙ ЭЛЕКТРОМЕХАНИЧЕСКИЙ ЗАВОД\"";

        private Problem _selectedProblem;
        public Problem SelectedProblem
        {
            get => _selectedProblem;
            set
            {
                _selectedProblem = value;
                OnPropertyChanged(nameof(SelectedProblem));
            }
        }

        private void GenerateReportProblems(Problem problem)
        {
            Report report = new()
            {
                Date = DateOnly.FromDateTime(DateTime.Now),
                Type = "ОП",
                Number = (Reports.Where(a => a.Type == "ОП").Count() + 1).ToString()
            };

            string path = $"{report.Type}{report.Number}.docx";
            using (DocX document = DocX.Create(path))
            {
                Formatting titleFormat = new Formatting();
                titleFormat.FontFamily = new Font("Times New Roman");
                titleFormat.Size = 14D;

                Formatting paragraphFormat = new Formatting();
                paragraphFormat.FontFamily = new Font("Times New Roman");
                paragraphFormat.Size = 14D;

                Paragraph header = document.InsertParagraph("", false, titleFormat).Append(ReportHeader);
                header.Font("Times New Roman");
                header.AppendLine("Отчет ").Font("Times New Roman").Bold().AppendLine($"№{report.Number} от {report.Date}");

                Paragraph text = document.InsertParagraph("", false, paragraphFormat).Font("Times New Roman");
                text.AppendLine($"Проблема, появившаяся {problem.DateOccurance} в отделе ").Font("Times New Roman").Append($"№{problem.Department.ViewerNumber} ").Font("Times New Roman").Bold();
                if (problem.Status == "Решена" || problem.Status == "Решена оп.")
                {
                    text.Append($"была решена {problem.DateElimination}").Font("Times New Roman").Append($"ответственным {problem.ResponsibleName}").Bold().Font("Times New Roman");
                }
                else
                {
                    text.Append("не была решена").Font("Times New Roman").Append($" {problem.DecisionTime} ").Font("Times New Roman").Bold().Append("дней").Font("Times New Roman");
                }
                header.Alignment = Alignment.left;
                text.Alignment = Alignment.both;

                document.Save();
            }

            byte[] FileData;
            using (FileStream fs = new(path, FileMode.Open))
            {
                FileData = new byte[fs.Length];
                fs.Read(FileData, 0, FileData.Length);
            }
            report.ReportFile = FileData;
            dbContext.Add(report);

            dbContext.SaveChanges();
            Reports.Add(report);

            FileInfo fileInfo = new FileInfo(path);
            fileInfo.Delete();
        }

        private void SaveReport(Report report)
        {
            string Path = "";

            // Открытие диалогового окна для сохранения файла
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = $"{report.Type}{report.Number}";
            saveFileDialog.DefaultExt = ".docx";
            saveFileDialog.Filter = "Word documents (.docx)|*.docx";


            if (saveFileDialog.ShowDialog() == true)
            {
                Path = saveFileDialog.FileName;
            }
            else return;

            using(FileStream fs = new(Path, FileMode.OpenOrCreate))
            {
                fs.Write(report.ReportFile, 0, report.ReportFile.Length);
            }

            var openFolder = MessageBox.Show("Открыть папку с файлом?", "Внимание", MessageBoxButton.YesNo);

            if (openFolder == MessageBoxResult.Yes)
            {
                string argument = "/select, \"" + Path + "\"";
                System.Diagnostics.Process.Start("explorer.exe", argument);
            }
        }

        private RelayCommand problemReport;
        public RelayCommand ProblemReport
        {
            get
            {
                return problemReport ?? (problemReport = new(obj =>
                { 
                    GenerateReportProblems(SelectedProblem);
                }, obj => SelectedProblem != null));
            }
        }

        private RelayCommand save;
        public RelayCommand Save
        {
            get
            {
                return save ?? (save = new(obj =>
                {
                    SaveReport(SelectedReport);
                }, obj => SelectedReport != null));
            }
        }
    }
}
