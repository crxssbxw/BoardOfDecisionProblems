using Microsoft.EntityFrameworkCore;
using ProblemsBoardLib.Commands;
using ProblemsBoardLib.DialogWindows;
using ProblemsBoardLib.Models;
using ProblemsBoardLib.Reporting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemsBoardLib.ViewModel
{
    public class ProblemsViewModel : BaseViewModel
    {
        private ObservableCollection<Problem> problems = new();
        public ObservableCollection<Problem> Problems 
        {
            get => problems;
            set
            {
                problems = value;
                OnPropertyChanged(nameof(Problems));
            }
        }

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

        public Problem selectedProblem = new();
        public Problem SelectedProblem
        {
            get => selectedProblem;
            set
            {
                selectedProblem = value;
                OnPropertyChanged(nameof(SelectedProblem));
            }
        }

        private ProblemPanelViewModel problemVM = new();
        public ProblemPanelViewModel ProblemVM 
        { 
            get => problemVM; 
            set 
            {
                problemVM = value; 
                OnPropertyChanged(nameof(ProblemVM));
            }
        }

        private NewProblemPanelViewModel newProblemVM;
        public NewProblemPanelViewModel NewProblemVM
        {
            get => newProblemVM;
            set
            {
                newProblemVM = value;
                OnPropertyChanged(nameof(NewProblemVM));
            }
        }

        private StatisticPanelViewModel statisticPanelVM = new();
        public StatisticPanelViewModel StatisticPanelVM
        {
            get => statisticPanelVM;
            set
            {
                statisticPanelVM = value;
                OnPropertyChanged(nameof(StatisticPanelVM));
            }
        }

        private ThemesViewModel themesVM = new(new Department());
        public ThemesViewModel ThemesVM
        {
            get => themesVM;
            set
            {
                themesVM = value;
                OnPropertyChanged(nameof(ThemesVM));
            }
        }

        private ProblemDecisionViewModel decisionVM = new();
        public ProblemDecisionViewModel DecisionVM
        {
            get => decisionVM;
            set
            {
                decisionVM = value;
                OnPropertyChanged(nameof(DecisionVM));
            }
        }

        public ProblemsViewModel(Department department)
        {
            Load();
            Department = dbContext.Departments.Find(department.DepartmentId);
            if (Department.Problems != null) 
                foreach (var problem in Department.Problems)
                {
                    Problems.Add(problem);
                }

            ViewSource.Source = Problems;
        }

        public int AllCounter
        {
            get => Department.Problems.Count;
        }

        public int DecidedCounter
        {
            get => Department.Problems.Where(a => a.Status == "Решена").Count();
        }

        public int DecidingCounter
        {
            get => Department.Problems.Where(a => a.Status == "Решается").Count();
        }

        public int NotDecidedCounter
        {
            get => Department.Problems.Where(a => a.Status == "Решается" && a.DaysLeft <= 1).Count();
        }

        public void ProblemsReload()
        {
            dbContext = new();
            Load();
            Department = dbContext.Departments.Find(department.DepartmentId);
            Problems.Clear();
            foreach(var problem in Department.Problems)
            {
                Problems.Add(problem);
            }

            OnPropertyChanged(nameof(DecidedCounter));
            OnPropertyChanged(nameof(DecidingCounter));
            OnPropertyChanged(nameof(NotDecidedCounter));
            CollectionView.Refresh();
        }

        private void Load()
        {
            dbContext.Problems.Load();
            dbContext.Themes.Load();
            dbContext.Responsibles.Load();
        }

        #region Commands

        private RelayCommand newProblemAdd;
        public RelayCommand NewProblemAdd
        {
            get
            {
                return newProblemAdd ?? (newProblemAdd = new(obj =>
                {
                    NewProblemVM = new(this, Department);
                },
                obj => true));
            }
        }

        private RelayCommand themesView;
        public RelayCommand ThemesView
        {
            get
            {
                return themesView ?? (themesView = new(obj =>
                {
                    ThemesVM = new(Department);
                },
                obj => true));
            }
        }

        private RelayCommand problemWatch;
        public RelayCommand ProblemWatch
        {
            get
            {
                return problemWatch ?? (problemWatch = new(obj =>
                {
                    ProblemVM = new(SelectedProblem);
                },
                obj => SelectedProblem != null));
            }
        }

        private RelayCommand problemDecide;
        public RelayCommand ProblemDecide
        {
            get
            {
                return problemDecide ?? (problemDecide = new(obj =>
                {
                    ResponsibleAuthorization responsibleAuthorization = new(SelectedProblem.Responsible);
                    if (responsibleAuthorization.ShowDialog() == true)
                    {
                        DecisionVM = new(SelectedProblem, this, true);
                    }
                    else
                        DecisionVM = new(false);
                },
                obj => SelectedProblem != null && SelectedProblem.Status != "Решена"));
            }
        }

        private RelayCommand allFilter;
        public RelayCommand AllFilter
        {
            get
            {
                return allFilter ?? (allFilter = new(obj =>
                {
                    CollectionView.Filter = null;
                },
                obj => true));
            }
        }

        private RelayCommand decidedFilter;
        public RelayCommand DecidedFilter
        {
            get
            {
                return decidedFilter ?? (decidedFilter = new(obj =>
                {
                    CollectionView.Filter = (object item) =>
                    {
                        var problem = item as Problem;

                        return problem.Status == "Решена";
                    };
                },
                obj => true));
            }
        }

        private RelayCommand decidingFilter;
        public RelayCommand DecidingFilter
        {
            get
            {
                return decidingFilter ?? (decidingFilter = new(obj =>
                {
                    CollectionView.Filter = (object item) =>
                    {
                        var problem = item as Problem;

                        return problem.Status == "Решается";
                    };
                },
                obj => true));
            }
        }

        private RelayCommand notDecidedFilter;
        public RelayCommand NotDecidedFilter
        {
            get
            {
                return notDecidedFilter ?? (notDecidedFilter = new(obj =>
                {
                    CollectionView.Filter = (object item) =>
                    {
                        var problem = item as Problem;

                        return problem.Status == "Решается" && problem.DaysLeft <= 1;
                    };
                },
                obj => true));
            }
        }


        #endregion
    }
}
