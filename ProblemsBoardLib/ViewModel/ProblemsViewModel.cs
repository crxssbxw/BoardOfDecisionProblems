using Microsoft.EntityFrameworkCore;
using ProblemsBoardLib.Commands;
using ProblemsBoardLib.DialogWindows;
using ProblemsBoardLib.Models;
using ProblemsBoardLib.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProblemsBoardLib.ViewModel
{
    public class ProblemsViewModel : BaseViewModel
    {
        private Roles currentRole;
        public Roles CurrentRole
        {
            get => currentRole;
            set
            {
                currentRole = value;
                OnPropertyChanged(nameof(CurrentRole));
                OnPropertyChanged(nameof(EnabledMenu));
            }
        }

        public bool EnabledMenu
        {
            get
            {
                if (CurrentRole == Roles.RHeaderWorker) return false;
                return true;
            }
        }

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

        private DateTime selectedDateFromFilter = DateTime.Today;
        public DateTime SelectedDateFromFilter
        {
            get => selectedDateFromFilter;
            set
            {
                selectedDateFromFilter = value;
                OnPropertyChanged(nameof(SelectedDateFromFilter));
            }
        }

        private DateTime selectedDateToFilter = DateTime.Today;
        public DateTime SelectedDateToFilter
        {
            get => selectedDateToFilter;
            set
            {
                selectedDateToFilter = value;
                OnPropertyChanged(nameof(SelectedDateToFilter));
            }
        }

        private DateTime selectedOneDateFilter = DateTime.Today;
        public DateTime SelectedOneDateFilter
        {
            get => selectedOneDateFilter;
            set
            {
                selectedOneDateFilter = value;
                OnPropertyChanged(nameof(SelectedOneDateFilter));
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

        private StatisticPanelViewModel statisticPanelVM;
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

        public ProblemsViewModel(Department department, Roles role = Roles.RAdmin)
        {
            Load();
            CurrentRole = role;
            Department = dbContext.Departments.Find(department.DepartmentId);
            if (Department.Problems != null) 
                foreach (var problem in Department.Problems.OrderByDescending(a => a.DateOccurance))
                {
                    Problems.Add(problem);
                }

            ViewSource.Source = Problems;
            OnPropertyChanged(nameof(FilterThemes));
        }

        public List<Theme> FilterThemes
        {
            get
            {
                List<Theme> themes;
                themes = dbContext.Themes.Where(a => a.Department == null).ToList();
                foreach (var theme in Department?.Themes)
                    themes.Add(theme);

                return themes;
            }
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
            foreach(var problem in Department.Problems.OrderByDescending(a => a.DateOccurance))
            {
                Problems.Add(problem);
            }

            OnPropertyChanged(nameof(AllCounter));
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
            dbContext.Workers.Load();
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
                obj => CurrentRole != Roles.RHeaderWorker));
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
                obj => SelectedProblem != null && SelectedProblem.Status != "Решена" && CurrentRole == Roles.RResponsible));
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

        private RelayCommand statisticView;
        public RelayCommand StatisticView
        {
            get
            {
                return statisticView ?? (statisticView = new(obj =>
                {
                    StatisticPanelVM = new(Department);
                },
                obj => Department.Problems != null));
            }
        }

        private RelayCommand responsibleFilter;
        public RelayCommand ResponsibleFilter
        {
            get
            {
                return responsibleFilter ?? (responsibleFilter = new(obj =>
                {
					CollectionView.Filter = (object item) =>
					{
						var problem = item as Problem;

						return problem.Responsible == SelectedResponsibleFilter;
					};
				},
                obj => true));
            }
        }

        private RelayCommand themeFilter;
        public RelayCommand ThemeFilter
        {
            get
            {
                return themeFilter ?? (themeFilter = new(obj =>
                {
					CollectionView.Filter = (object item) =>
					{
						var problem = item as Problem;

						return problem.Theme == SelectedThemeFilter;
					};
				},
                obj => true));
            }
        }

        private RelayCommand oneDateFilter;
        public RelayCommand OneDateFilter
        {
            get
            {
                return oneDateFilter ?? (oneDateFilter = new(obj =>
                {
					CollectionView.Filter = (object item) =>
					{
						var problem = item as Problem;

                        return problem.DateOccurance.Date == SelectedOneDateFilter;
					};
				},
                obj => true));
            }
        }

        private RelayCommand multiDateFilter;
        public RelayCommand MultiDateFilter
        {
            get
            {
                return multiDateFilter ?? (multiDateFilter = new(obj =>
                {
					CollectionView.Filter = (object item) =>
					{
						var problem = item as Problem;

                        return problem.DateOccurance.Date >= SelectedDateFromFilter && problem.DateOccurance.Date <= SelectedDateToFilter;
					};
				},
                obj => true));
            }
        }

        private RelayCommand urgentSort;
        public RelayCommand UrgentSort
        {
            get
            {
                return urgentSort ?? (urgentSort = new(obj =>
                {
                    CollectionView.SortDescriptions.Clear();
                    CollectionView.SortDescriptions.Add(new System.ComponentModel.SortDescription("DaysLeft", System.ComponentModel.ListSortDirection.Ascending));
					CollectionView.SortDescriptions.Add(new System.ComponentModel.SortDescription("Status", System.ComponentModel.ListSortDirection.Descending));
				},
                obj => true));
            }
        }

        private RelayCommand currentSort;
        public RelayCommand CurrentSort
        {
            get
            {
                return currentSort ?? (currentSort = new(obj =>
                {
                    CollectionView.SortDescriptions.Clear();
                    CollectionView.SortDescriptions.Add(new System.ComponentModel.SortDescription("DaysLeft", System.ComponentModel.ListSortDirection.Descending));
                    CollectionView.SortDescriptions.Add(new System.ComponentModel.SortDescription("Status", System.ComponentModel.ListSortDirection.Descending));
                },
                obj => true));
            }
        }

        private RelayCommand newSort;
        public RelayCommand NewSort
        {
            get
            {
                return newSort ?? (newSort = new(obj =>
                {
                    CollectionView.SortDescriptions.Clear();
                    CollectionView.SortDescriptions.Add(new System.ComponentModel.SortDescription("DateOccurance", System.ComponentModel.ListSortDirection.Descending));
                },
                obj => true));
            }
        }
        
        private RelayCommand oldSort;
        public RelayCommand OldSort
        {
            get
            {
                return oldSort ?? (oldSort = new(obj =>
                {
                    CollectionView.SortDescriptions.Clear();
                    CollectionView.SortDescriptions.Add(new System.ComponentModel.SortDescription("DateOccurance", System.ComponentModel.ListSortDirection.Ascending));
                },
                obj => true));
            }
        }

		#endregion
	}
}
