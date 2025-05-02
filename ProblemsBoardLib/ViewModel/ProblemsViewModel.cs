using Microsoft.EntityFrameworkCore;
using ProblemsBoardLib.Commands;
using ProblemsBoardLib.Models;
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
            CollectionView.Refresh();
        }

        private void Load()
        {
            dbContext.Problems.Load();
            dbContext.Themes.Load();
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
                    DecisionVM = new(SelectedProblem, this);
                },
                obj => SelectedProblem != null && SelectedProblem.Status != "Решена"));
            }
        }
        #endregion
    }
}
