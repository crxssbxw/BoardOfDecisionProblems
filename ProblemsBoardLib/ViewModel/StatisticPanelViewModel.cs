using Microsoft.EntityFrameworkCore;
using ProblemsBoardLib.Commands;
using ProblemsBoardLib.DialogWindows;
using ProblemsBoardLib.Models;
using ProblemsBoardLib.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Xps.Packaging;

namespace ProblemsBoardLib.ViewModel
{
    public class StatisticPanelViewModel : BaseViewModel
    {
        private Department department = new();
        public Department Department
        {
            get => department;
            set
            {
                department = value;
                OnPropertyChanged(nameof(Department));
            }
        }

        public StatisticPanelViewModel()
        {
            
        }

        public StatisticPanelViewModel(Department department)
        {
            dbContext.Themes.Load();
            dbContext.Problems.Load();
            //dbContext.Responsibles.Load();
            dbContext.Workers.Load();

            Department = dbContext.Departments.Find(department.DepartmentId);
            //Responsibles = Department.Responsibles.ToList();
            Themes = Department.Themes.ToList();
            //Themes = dbContext.Themes.Where(a => a.Department == null).ToList();
        }

        public int AllProblems
        {
            get => Department.Problems.Count;
        }

        public int DecidedProblems
        {
            get => Department.Problems.Where(a => a.Status == "Решена").Count();
        }

        public int DecidingProblems
        {
            get => Department.Problems.Where(a => a.Status == "Решается").Count();
        }

        public int UrgentProblems
        {
            get => Department.Problems.Where(a => a.Status == "Решается" && a.DaysLeft <= 1).Count();
        }

        public float DecidedPercent
        {
            get
            {
                if (AllProblems == 0) return 0;
                return ((float)DecidedProblems / (float)AllProblems) * 100;
            }
        }

        //private List<Responsible> responsibles = new();
        //public List<Responsible> Responsibles
        //{
        //    get => responsibles;
        //    set
        //    {
        //        responsibles = value;
        //        OnPropertyChanged(nameof(Responsibles));
        //    }
        //}

        //private Responsible selectedResponsible;
        //public Responsible SelectedResponsible
        //{
        //    get => selectedResponsible;
        //    set
        //    {
        //        selectedResponsible = value;
        //        OnPropertyChanged(nameof(SelectedResponsible));
        //        OnPropertyChanged(nameof(ResponsibleAllProblems));
        //        OnPropertyChanged(nameof(ResponsibleDecidedProblems));
        //        OnPropertyChanged(nameof(ResponsibleDecidingProblems));
        //        OnPropertyChanged(nameof(ResponsibleUrgentProblems));
        //    }
        //}

        //public int ResponsibleAllProblems
        //{
        //    get => Department.Problems.Where(a => a.Responsible == SelectedResponsible).Count();
        //}

        //public int ResponsibleDecidedProblems
        //{
        //    get => Department.Problems.Where(a => a.Responsible == SelectedResponsible && a.Status == "Решена").Count();
        //}
        //public int ResponsibleDecidingProblems
        //{
        //    get => Department.Problems.Where(a => a.Responsible == SelectedResponsible && a.Status == "Решается").Count();
        //}
        //public int ResponsibleUrgentProblems
        //{
        //    get => Department.Problems.Where(a => a.Responsible == SelectedResponsible && a.Status == "Решается" && a.DaysLeft <= 1).Count();
        //}

        private List<Theme> themes = new();
        public List<Theme> Themes
        {
            get => themes;
            set
            {
                themes = value;
                OnPropertyChanged(nameof(Themes));
            }
        }

        private Theme selectedTheme;
        public Theme SelectedTheme
        {
            get => selectedTheme;
            set
            {
                selectedTheme = value;
                OnPropertyChanged(nameof(SelectedTheme));
                OnPropertyChanged(nameof(ThemesAllProblems));
                OnPropertyChanged(nameof(ThemesDecidedProblems));
                OnPropertyChanged(nameof(ThemesDecidingProblems));
                OnPropertyChanged(nameof(ThemesPercentDecided));
            }
        }

        public int ThemesAllProblems
        {
            get => Department.Problems.Where(a => a.Theme == SelectedTheme).Count();
        }

        public int ThemesDecidedProblems
        {
            get => Department.Problems.Where(a => a.Theme == SelectedTheme && a.Status == "Решена").Count();
        }

        public int ThemesDecidingProblems
        {
            get => Department.Problems.Where(a => a.Theme == SelectedTheme && a.Status == "Решается").Count();
        }

        public float ThemesPercentDecided
        {
            get
            {
                if (ThemesAllProblems == 0) return 0;
                return ((float)ThemesDecidedProblems / (float)ThemesAllProblems) * 100;
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

        //private string GetStatisticToString()
        //{
        //    string statistic =
        //        $"На участке {Department} всего {AllProblems} проблем. " +
        //        $"Из них решено {DecidedProblems} проблем. Находится в процессе решения {DecidingProblems} проблем, из них {UrgentProblems} \u2014 срочные. ";
        //    if (SelectedResponsible != null)
        //        statistic +=
        //            $"\nОтветственный {SelectedResponsible.FullName} решил {ResponsibleDecidedProblems} проблем. " +
        //            $"Находится в процессе решения {ResponsibleDecidingProblems} проблем, из них {ResponsibleUrgentProblems} \u2014 срочные. ";
        //    if (SelectedTheme != null)
        //        statistic +=
        //            $"\nНа данном участке по теме «{SelectedTheme.Name}» всего {ThemesAllProblems}. Из них решено {ThemesDecidedProblems} проблем. " +
        //            $"Находится в процессе решения {ThemesDecidingProblems} проблем. ";
        //    return statistic;
        //}

        private string GetGeneralToString()
        {
            return
               $"На участке {Department} всего {AllProblems} проблем. " +
               $"Из них решено {DecidedProblems} проблем. Находится в процессе решения {DecidingProblems} проблем, из них {UrgentProblems} \u2014 срочные.\r";
        }

        //private string GetResponsibleToString()
        //{
        //    if (SelectedResponsible == null) return "";
        //    return
        //            $"Ответственный {SelectedResponsible.FullName} решил {ResponsibleDecidedProblems} проблем. " +
        //            $"Находится в процессе решения {ResponsibleDecidingProblems} проблем, из них {ResponsibleUrgentProblems} \u2014 срочные.\r";
        //}

        private string GetThemeToString()
        {
            if (SelectedTheme == null) return "";
            return
                $"На данном участке по теме «{SelectedTheme.Name}» всего {ThemesAllProblems}. Из них решено {ThemesDecidedProblems} проблем. " +
                    $"Находится в процессе решения {ThemesDecidingProblems} проблем.\r";
        }

        private RelayCommand generateReport;
        public RelayCommand GenerateReport
        {
            get
            {
                return generateReport ?? (generateReport = new(obj =>
                {
                    //using (var reporting = new ReportingTool(false))
                    //{
                    //    reporting.GenerateStatisticReport(GetGeneralToString(), GetResponsibleToString(), GetThemeToString());

                    //    XpsDocument doc = new XpsDocument(reporting.Xps.FullName, FileAccess.Read);
                    //    Document = doc.GetFixedDocumentSequence();

                    //    DocumentPreWatch documentPreWatch = new()
                    //    {
                    //        DataContext = this
                    //    };
                    //    if (documentPreWatch.ShowDialog() == true)
                    //    {
                    //        reporting.SaveToDatabase("ОС");
                    //        Document = null;
                    //        doc.Close();
                    //    }
                    //}
                },
                obj => true));
            }
        }

    }
}
