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

            string headertext = $"{ReportHeader}" + Environment.NewLine + $"Отчет №{report.Number} от {report.Date}" + Environment.NewLine;
            string paragraphtext = $"Проблема №{problem.ProblemId}, созданная {problem.DateOccurance}";
            if (problem.Decision != null)
            {
                paragraphtext += $", была решена {problem.DateElimination} ответственным {problem.Responsible.Worker.SecondName} {problem.Responsible.Worker.FirstName} за {problem.DecisionTime} дня(ей)";
            }
            else if (problem.DaysLeft == 1)
            {
                paragraphtext += $", требует срочного решения в течении {problem.DaysLeft} дня!";
            }
            else if (problem.DaysLeft == 0) paragraphtext += $", требует срочного решения уже сегодня!";
            else paragraphtext += $", решается в течении {problem.DecisionTime} дня(ей). До конца срока осталось {problem.DaysLeft} дня(ей)";

            using (DocX document = DocX.Create(path))
            {
                Paragraph header = document.InsertParagraph(headertext).Font("Times New Roman").FontSize(14).SpacingAfter(10);
                header.Alignment = Alignment.left;
                Paragraph paragraph = document.InsertParagraph(paragraphtext).Font("Times New Roman").FontSize(14);
                paragraph.Alignment = Alignment.both;
                Paragraph description = document.InsertParagraph($"Описание: <<{problem.Description}>>").FontSize(14).Font("Times New Roman");
                description.Alignment = Alignment.both;

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
