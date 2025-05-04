<<<<<<< HEAD
﻿using BoardOfDecisionProblems.Commands;
using BoardOfDecisionProblems.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
=======
﻿using DocumentFormat.OpenXml.Bibliography;
using Microsoft.Win32;
using ProblemsBoardLib.Commands;
using ProblemsBoardLib.Models;
<<<<<<< HEAD
>>>>>>> 1be2c83 (Added reporting window, more functional statistic and reporting for it)
=======
using ProblemsBoardLib.Tools;
>>>>>>> fb5eff7 (Added Logging tool and logging window)
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
<<<<<<< HEAD
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
=======
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProblemsBoardLib.ViewModel
{
    public class ReportsViewModel : BaseViewModel
    {
        private ObservableCollection<Report> reports = new();
        public ObservableCollection<Report> Reports
        {
            get => reports;
            set
            {
                reports = value;
>>>>>>> 1be2c83 (Added reporting window, more functional statistic and reporting for it)
                OnPropertyChanged(nameof(Reports));
            }
        }

<<<<<<< HEAD
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
=======
        private Report selectedReport;
        public Report SelectedReport
        {
            get => selectedReport;
            set
            {
                selectedReport = value;
>>>>>>> 1be2c83 (Added reporting window, more functional statistic and reporting for it)
                OnPropertyChanged(nameof(SelectedReport));
            }
        }

<<<<<<< HEAD
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
=======
        public ReportsViewModel()
        {
            foreach(var report in dbContext.Reports)
            {
                Reports.Add(report);
            }
            ViewSource.Source = Reports;
        }

        private DateTime dateFrom = DateTime.Today;
        public DateTime DateFrom
        {
            get => dateFrom;
            set
            {
                dateFrom = value;
                OnPropertyChanged(nameof(DateFrom));
            }
        }

        public DateTime MinDate
        {
            get
            {
                if (Reports.Count == 0)
                    return DateTime.Today;
                return Reports.Select(r => r.CreatedAt).Min();
            }
        }

        public DateTime MaxDate
        {
            get
            {
                if (Reports.Count == 0)
                    return DateTime.Today;
                return Reports.Select(r => r.CreatedAt).Max();
            }
        }

        public List<string> Types { get; set; } = new()
        {
            "ОП",
            "ОС"
        };

        private string selectedType;
        public string SelectedType
        {
            get => selectedType;
            set
            {
                selectedType = value;
                OnPropertyChanged(nameof(SelectedType));
            }
        }

        private RelayCommand acceptFilter;
        public RelayCommand AcceptFilter
        {
            get
            {
                return acceptFilter ?? (acceptFilter = new(obj =>
                {
                    CollectionView.Filter = (object item) =>
                    {
                        var report = item as Report;

                        if (DateFrom == null && SelectedType == null)
                            return true;
                        if (DateFrom != null)
                        {
                            if (SelectedType != null)
                                return report.Type == SelectedType && report.CreatedAt == DateFrom;
                            return report.CreatedAt == DateFrom;
                        }
                        if (SelectedType != null)
                            return report.Type == SelectedType;
                        return false;
                    };
                },
                obj => DateFrom != null || SelectedType != null));
            }
        }

        private RelayCommand resetFilter;
        public RelayCommand ResetFilter
        {
            get
            {
                return resetFilter ?? (resetFilter = new(obj =>
                {
                    CollectionView.Filter = null;
                },
                obj => true));
            }
        }

        private RelayCommand saveReport;
        public RelayCommand SaveReport
        {
            get
            {
                return saveReport ?? (saveReport = new(obj =>
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.FileName = $"{SelectedReport.Type}{SelectedReport.Number}";
                    saveFileDialog.Filter = "Документ MS Office Word 2007(*.docx)|*.docx";

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        using (FileStream fs = new(saveFileDialog.FileName, FileMode.Create, FileAccess.Write))
                        {
                            fs.Write(SelectedReport.ReportFile, 0, SelectedReport.ReportFile.Length);
                        }
                        LoggingTool.ReportSaved(SelectedReport, saveFileDialog.FileName);
                        var result = MessageBox.Show("Открыть папку с файлом?", "Подтвердждение действия", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes)
                        {
                            string argument = "/select, \"" + saveFileDialog.FileName + "\"";
                            System.Diagnostics.Process.Start("explorer.exe", argument);
                        }
                    }
                },
                obj => SelectedReport != null));
>>>>>>> 1be2c83 (Added reporting window, more functional statistic and reporting for it)
            }
        }
    }
}
