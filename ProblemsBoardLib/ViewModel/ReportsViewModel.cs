using DocumentFormat.OpenXml.Bibliography;
using Microsoft.Win32;
using ProblemsBoardLib.Commands;
using ProblemsBoardLib.Models;
using ProblemsBoardLib.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
                OnPropertyChanged(nameof(Reports));
            }
        }

        private Report selectedReport;
        public Report SelectedReport
        {
            get => selectedReport;
            set
            {
                selectedReport = value;
                OnPropertyChanged(nameof(SelectedReport));
            }
        }

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
            }
        }
    }
}
