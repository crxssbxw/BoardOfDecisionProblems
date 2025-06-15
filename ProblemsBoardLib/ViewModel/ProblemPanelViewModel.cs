using Microsoft.EntityFrameworkCore;
using ProblemsBoardLib.Commands;
using ProblemsBoardLib.DialogWindows;
using ProblemsBoardLib.Models;
using ProblemsBoardLib.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Xps.Packaging;
using System.Xml;

namespace ProblemsBoardLib.ViewModel
{
    public class ProblemPanelViewModel : BaseViewModel
    {
        private Problem problem;
        public Problem Problem
        {
            get => problem;
            set
            {
                problem = value;
                OnPropertyChanged(nameof(Problem));
            }
        }

        public ProblemPanelViewModel()
        {
            dbContext.Themes.Load();
            dbContext.Workers.Load();
        }

        public ProblemPanelViewModel(Problem problem) : base()
        {
            dbContext.Themes.Load();
            dbContext.Workers.Load();
            dbContext.Departments.Load();
            Problem = dbContext.Problems.Find(problem.ProblemId);
        }

        public bool IsDecided
        {
            get
            {
                if (Problem == null) return false;
                if (string.IsNullOrEmpty(Problem.Decision))
                    return false;
                return true;
            }
        }

        public string IsDecidedText
        {
            get
            {
                if (IsDecided)
                    return $"Решение";
                else return $"Решения нет";
            }
        }

        private FixedDocumentSequence document;
        public FixedDocumentSequence Document
        {
            get => document;
            set
            {
                document = value;
                OnPropertyChanged(nameof(Document));
            }
        }

        private RelayCommand generateReport;
        public RelayCommand GenerateReport
        {
            get
            {
                return generateReport ?? (generateReport = new(obj =>
                {
                    using (var reporting = new ReportingTool(false))
                    {
                        reporting.GenerateProblemReport(Problem);
                    }
                },
                obj => true));
            }
        }
    }
}
