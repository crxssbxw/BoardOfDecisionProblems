using BoardOfDecisionProblems.Commands;
using BoardOfDecisionProblems.Forms;
using BoardOfDecisionProblems.Models;
using BoardOfDecisionProblems.Windows;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BoardOfDecisionProblems.ViewModel
{
    /// <summary>
    /// Представление модели Проблем
    /// </summary>
    public class ProblemViewModel : BaseViewModel
    {
        /// <summary>
        /// Представление модели тем
        /// </summary>
        public static ThemesViewModel ThemesViewModel { get; set; } = new();
        /// <summary>
        /// Представление модели Отделов
        /// </summary>
        public static DepartmentsViewModel DepartmentsViewModel { get; set; } = new();
        /// <summary>
        /// Представление модели Работников
        /// </summary>
        public static WorkersViewModel WorkersViewModel { get; set; } = new();
        /// <summary>
        /// Представление модели Ответственных
        /// </summary>
        public static ResponsiblesViewModel ResponsiblesViewModel { get; set; } = new();
        public static LogsViewModel LogsViewModel { get; set; } = new();
        public static ReportsViewModel ReportsViewModel { get; set; } = new();
        /// <summary>
        /// Текущая дата
        /// </summary>
        public static DateTime Today { get; set; } = DateTime.Now;

        /// <summary>
        /// Список отделов
        /// </summary>
        public ObservableCollection<Department> DepartmentsList
        {
            get => DepartmentsViewModel.Departments;
        }


        #region Filters
        public ObservableCollection<Responsible> ResponsiblesSD
        {
            get
            {
                if(SelectedDepartmentFilter != null)
                {
                    var list = ResponsiblesViewModel.Responsibles.Where(a => a.Department == SelectedDepartmentFilter).ToList();
                    ObservableCollection<Responsible> collection = new();
                    foreach(var responsible in list)
                    {
                        collection.Add(responsible);
                    }
                    return collection;
                }
                else
                {
                    return ResponsiblesViewModel.Responsibles;
                }
            }
        }

        private Department selectedDepartmentFilter;
        public Department SelectedDepartmentFilter
        {
            get => selectedDepartmentFilter;
            set
            {
                selectedDepartmentFilter = value;
                OnPropertyChanged(nameof(SelectedDepartmentFilter));
                OnPropertyChanged(nameof(ResponsiblesSD));
            }
        }

        private Responsible selectedResponsibleFilter;
        public Responsible SelectedResponsibleFilter
        {
            get => selectedResponsibleFilter;
            set
            {
                selectedResponsibleFilter = value;
                OnPropertyChanged(nameof(SelectedResponsibleFilter));
            }
        }

        private Theme selectedThemeFilter;
        public Theme SelectedThemeFilter
        {
            get => selectedThemeFilter;
            set
            {
                selectedThemeFilter = value;
                OnPropertyChanged(nameof(SelectedThemeFilter));
            }
        }

        private int? decisionDaysInput;
        public int? DecisionDaysInput
        {
            get => decisionDaysInput;
            set
            {
                decisionDaysInput = value;
                OnPropertyChanged(nameof(DecisionDaysInput));
            }
        }

        private DateTime selectedDateFrom = DateTime.Now;
        public DateTime SelectedDateFrom
        {
            get => selectedDateFrom;
            set
            {
                selectedDateFrom = value;
                OnPropertyChanged(nameof(SelectedDateFrom));
            }
        }

        private DateTime selectedDateTo = DateTime.Now;
        public DateTime SelectedDateTo
        {
            get => selectedDateTo;
            set
            {
                selectedDateTo = value;
                OnPropertyChanged(nameof(SelectedDateTo));
            }
        }


        private bool dateCheck;
        public bool DateCheck 
        { 
            get => dateCheck;
            set
            {
                dateCheck = value;
                OnPropertyChanged(nameof(DateCheck));
            }
        }

        private bool departmentCheck;
        public bool DepartmentCheck
        {
            get => departmentCheck;
            set
            {
                departmentCheck = value;
                OnPropertyChanged(nameof(DepartmentCheck));
            }
        }

        private bool themeCheck;
        public bool ThemeCheck
        {
            get => themeCheck;
            set
            {
                themeCheck = value;
                OnPropertyChanged(nameof(ThemeCheck));
            }
        }

        private bool responsibleCheck;
        public bool ResponsibleCheck
        {
            get => responsibleCheck;
            set
            {
                responsibleCheck = value;
                OnPropertyChanged(nameof(ResponsibleCheck));
            }
        }

        private bool decisionTimeCheck;
        public bool DecisionTimeCheck
        {
            get => decisionTimeCheck;
            set
            {
                decisionTimeCheck = value;
                OnPropertyChanged(nameof(DecisionTimeCheck));
            }
        }

        #endregion

        /// <summary>
        /// Счетчик количества проблем (Всего)
        /// </summary>
        public int? TotalProblems => Problems.Count;
        /// <summary>
        /// Счетчик решенных проблем
        /// </summary>
        public int? TotalDecided => Problems.Where(a => a.Status == "Решено" || a.Status == "Решено оп.").Count();
        /// <summary>
        /// Счетчик проблем в процессе решения
        /// </summary>
        public int? TotalDeciding => Problems.Where(a => a.Status == "Решается" || a.Status == "Решается оп.").Count();

        public int? DecidedByFilter
        {
            get
            {
                return Problems
                    .Where(a => a.Status == "Решено" && a.DateOccurance.Date >= SelectedDateFrom.Date && a.DateOccurance <= SelectedDateTo.Date)
                    .Count();
            }
        }

        public int? AllByFilter
        {
            get
            {
                return Problems
                    .Where(a => a.DateOccurance.Date >= SelectedDateFrom.Date && a.DateOccurance <= SelectedDateTo.Date)
                    .Count();
            }
        }

        public int? TimeByFilter
        {
            get
            {
                List<Problem> filtered = new();
                foreach(var problem in Problems.Where(a => FilterLogic(a)).ToList())
                {
                    filtered.Add(problem);
                }
                int? time = 0;
                foreach(var problem in filtered)
                {
                    time += problem.DecisionTime;
                }
                return time;
            }
        }

        public float? AVGTimeByFilter
        {
            get
            {
                List<Problem> filtered = new();
                foreach (var problem in Problems.Where(a => FilterLogic(a)).ToList())
                {
                    filtered.Add(problem);
                }
                int? time = 0;
                foreach (var problem in filtered)
                {
                    time += problem.DecisionTime;
                }
                return (float)(time / filtered.Count);
            }
        }

        private ObservableCollection<Problem> problems = new();
        /// <summary>
        /// Коллекция проблем
        /// </summary>
        public ObservableCollection<Problem> Problems
        {
            get => problems;
            set
            {
                problems = value;
                OnPropertyChanged(nameof(Problems));
            }
        }
        /// <summary>
        /// Объект выбранной проблемы в таблице
        /// </summary>
        private Problem selectedProblem = new();

        public ProblemViewModel()
        {
            dbContext.Responsibles.Load();
            dbContext.Departments.Load();
            dbContext.Workers.Load();
            dbContext.Themes.Load();
            foreach (var problem in dbContext.Problems)
            {
                Problems.Add(problem);
            }
            ViewSource.Source = Problems;
        }

        public Problem SelectedProblem
        {
            get => selectedProblem;
            set
            {
                selectedProblem = value;
                OnPropertyChanged(nameof(SelectedProblem));
            }
        }

        #region Commands

        private RelayCommand newProblem;
        /// <summary>
        /// Команда создания новой проблемы
        /// </summary>
        public RelayCommand NewProblem
        {
            get
            {
                return newProblem ?? (newProblem = new(obj =>
                {
                    Problem newProblem = new();

                    newProblem.DateOccurance = DateTime.Now;
                    newProblem.Department = DepartmentsViewModel.Departments[0];
                    newProblem.Theme = ThemesViewModel.Themes[0];
                    newProblem.Description = "Описание";

                    Forms.NewProblem window = new();
                    window.DataContext = newProblem;
                    window.OccuranceDateBox.DisplayDateEnd = Today;
                    window.ThemesList.ItemsSource = ThemesViewModel.Themes;
                    window.DepartmentsList.ItemsSource = DepartmentsViewModel.Departments;

                    if (window.ShowDialog() == true)
                    {
                        newProblem.Responsible = ResponsiblesViewModel.Responsibles.Where
                            (a => a.Department == newProblem.Department && a.IsCurrent == true).FirstOrDefault();

                        if (Math.Abs(newProblem.DateOccurance.Subtract(DateTime.Now).Days) >= 20)
                        {
                            newProblem.Status = "Решается оп.";
                        }
                        else
                        {
                            newProblem.Status = "Решается";
                        }

                        //newProblem.Theme.Problems.Add(newProblem);
                        dbContext.Problems.Add(newProblem);
                        dbContext.SaveChanges();

                        LogEvent logEvent = new LogEvent()
                        {
                            Date = DateOnly.FromDateTime(DateTime.Now),
                            Time = TimeOnly.FromDateTime(DateTime.Now),
                            Title = $"Добавление проблемы №{newProblem.ProblemId}",
                            User = "Guest"
                        };
                        logEvent.Object = $"Проблема {newProblem.ProblemId}";
                        dbContext.Add(logEvent);
                        LogsViewModel.LogEvents.Add(logEvent);

                        dbContext.SaveChanges();
                        Problems.Add(newProblem);
                        OnPropertyChanged(nameof(TotalProblems));
                        OnPropertyChanged(nameof(TotalDecided));
                        OnPropertyChanged(nameof(TotalDeciding));
                    }
                },
                obj => true));
            }
        }

        private RelayCommand decide;
        /// <summary>
        /// Команда решения проблемы
        /// </summary>
        public RelayCommand Decide
        {
            get
            {
                return decide ?? (decide = new(obj =>
                {
                    /* Тестовая реализация
                    SelectedProblem.DateElimination = DateTime.Now;
                    if (SelectedProblem.DecisionTime > 20)
                    {
                        SelectedProblem.Status = "Решено оп.";
                    }
                    else SelectedProblem.Status = "Решено";
                    */

                    Problem temp = new();
                    temp = SelectedProblem;
                    temp.DateElimination = DateTime.Now;
                    temp.Decision = "Описание решения";

                    DecisionForm decisionForm = new DecisionForm();
                    decisionForm.EliminationDateBox.DisplayDateEnd = DateTime.Now;
                    decisionForm.EliminationDateBox.DisplayDateStart = temp.DateOccurance;
                    decisionForm.DataContext = temp;

                    if(decisionForm.ShowDialog() == true)
                    {
                        if (temp.DecisionTime >= 20)
                        {
                            temp.Status = "Решено оп.";
                            dbContext.Problems.Entry(SelectedProblem).CurrentValues.SetValues(temp);
                        }
                        else
                        {
                            temp.Status = "Решено";
                            dbContext.Problems.Entry(SelectedProblem).CurrentValues.SetValues(temp);
                        }
                        dbContext.SaveChanges();

                        LogEvent logEvent = new LogEvent()
                        {
                            Date = DateOnly.FromDateTime(DateTime.Now),
                            Time = TimeOnly.FromDateTime(DateTime.Now),
                            Title = $"Решение проблемы №{SelectedProblem.ProblemId}",
                            User = $"Ответственный {SelectedProblem.ResponsibleName}"
                        };
                        logEvent.Object = $"Проблема {SelectedProblem.ProblemId}";
                        dbContext.Add(logEvent);
                        LogsViewModel.LogEvents.Add(logEvent);

                        dbContext.SaveChanges();
                        SelectedProblem = temp;
                    }
                    else
                    {
                        SelectedProblem.DateElimination = null;
                        SelectedProblem.Decision = null;
                    }

                    OnPropertyChanged(nameof(TotalProblems));
                    OnPropertyChanged(nameof(TotalDecided));
                    OnPropertyChanged(nameof(TotalDeciding));

                    CollectionView.Refresh();
                },
                obj => SelectedProblem != null && (SelectedProblem.Status != "Решено" && SelectedProblem.Status != "Решено оп.")));
            }
        }

        private RelayCommand watch;
        /// <summary>
        /// Команда просмотра проблемы
        /// </summary>
        public RelayCommand Watch
        {
            get
            {
                return watch ?? (watch = new(obj =>
                {
                    ProblemView problemView = new ProblemView();
                    problemView.DataContext = SelectedProblem;
                    problemView.Title = $"Проблема №{SelectedProblem.ProblemId}";
                    LogEvent logEvent = new LogEvent()
                    {
                        Date = DateOnly.FromDateTime(DateTime.Now),
                        Time = TimeOnly.FromDateTime(DateTime.Now),
                        Title = $"Просмотр проблемы №{SelectedProblem.ProblemId}",
                        User = "Guest"
                    };
                    logEvent.Object = $"Проблема {SelectedProblem.ProblemId}";
                    dbContext.Add(logEvent);
                    LogsViewModel.LogEvents.Add(logEvent);
                    dbContext.SaveChanges();
                    problemView.ShowDialog();
                },
                obj => SelectedProblem != null));
            }
        }

        private RelayCommand delete;
        /// <summary>
        /// Команда удаления проблемы
        /// </summary>
        public RelayCommand Delete
        {
            get
            {
                return delete ?? (delete = new(obj =>
                {
                    var result = MessageBox.Show($"Вы уверены, что хотите удалить проблему?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        dbContext.Problems.Remove(SelectedProblem);
                        Problems.Remove(SelectedProblem);
                        dbContext.SaveChanges();

                        LogEvent logEvent = new LogEvent()
                        {
                            Date = DateOnly.FromDateTime(DateTime.Now),
                            Time = TimeOnly.FromDateTime(DateTime.Now),
                            Title = $"Удаление проблемы №{SelectedProblem.ProblemId}",
                            User = "Admin"
                        };
                        logEvent.Object = $"Проблема {SelectedProblem.ProblemId}";
                        dbContext.Add(logEvent);
                        LogsViewModel.LogEvents.Add(logEvent);

                        dbContext.SaveChanges();
                    }
                },
                obj => SelectedProblem != null));
            }
        }

        private RelayCommand totalProblemsFilter;
        /// <summary>
        /// Команда фильтрации проблем
        /// </summary>
        public RelayCommand TotalProblemsFilter
        {
            get
            {
                return totalProblemsFilter ?? (totalProblemsFilter = new(obj =>
                {
                    CollectionView.Filter = (a) => true;
                },
                obj => true));
            }
        }

        private RelayCommand totalDecidedFilter;
        /// <summary>
        /// Команда отображения решенных проблем
        /// </summary>
        public RelayCommand TotalDecidedFilter
        {
            get
            {
                return totalDecidedFilter ?? (totalDecidedFilter = new(obj =>
                {
                    CollectionView.Filter = (a) =>
                    {
                        var b = (Problem)a;
                        return b.Status == "Решено" || b.Status == "Решено оп.";
                    };
                },
                obj => true));
            }
        }
        

        private RelayCommand totalDecidingFilter;
        /// <summary>
        /// Команда отображения проблем в процессе решения
        /// </summary>
        public RelayCommand TotalDecidingFilter
        {
            get
            {
                return totalDecidingFilter ?? (totalDecidingFilter = new(obj =>
                {
                    CollectionView.Filter = (a) =>
                    {
                        var b = (Problem)a;
                        return b.Status == "Решается" || b.Status == "Решается оп.";
                    };
                },
                obj => true));
            }
        }

        /// <summary>
        /// Логика фильтра
        /// </summary>
        /// <param name="item">Проблема</param>
        /// <returns>Логическое значение фильтрации</returns>
        protected bool FilterLogic(object item)
        {
            Problem problem = item as Problem;
            return  problem.DateOccurance.Date >= SelectedDateFrom.Date && problem.DateOccurance.Date <= SelectedDateTo.Date
                && Problems.Any(a => problem.Department == SelectedDepartmentFilter)
                && Problems.Any(a => problem.Responsible == SelectedResponsibleFilter)
                && Problems.Any(a => problem.Theme == SelectedThemeFilter)
                && problem.DecisionTime >= DecisionDaysInput;
        }

        private RelayCommand acceptFilter;
        /// <summary>
        /// Команда применения фильтра
        /// </summary>
        public RelayCommand AcceptFilter
        {
            get
            {
                return acceptFilter ?? (acceptFilter = new(obj => 
                {
                    CollectionView.Filter = (a) => FilterLogic(a);
                    CollectionView.Refresh();

                    OnPropertyChanged(nameof(DecidedByFilter));
                    OnPropertyChanged(nameof(AllByFilter));
                    OnPropertyChanged(nameof(TimeByFilter));
                    OnPropertyChanged(nameof(AVGTimeByFilter));
                },
                obj => true));
            } 
        }

        private RelayCommand openLogger;
        public RelayCommand OpenLogger
        {
            get
            {
                return openLogger ?? (openLogger = new(obj =>
                {
                    Logger logger = new();
                    logger.DataContext = LogsViewModel;
                    logger.Show();
                }, obj => true));
            }
        }

        private RelayCommand openReports;
        public RelayCommand OpenReports
        {
            get
            {
                return openReports ?? (openReports = new(obj =>
                {
                    ReportsView reports = new();
                    reports.DataContext = ReportsViewModel;
                    reports.Show();
                }, obj => true));
            }
        }

        #endregion
    }
}
