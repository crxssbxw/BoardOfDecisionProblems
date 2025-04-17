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
<<<<<<< HEAD
                OnPropertyChanged(nameof(DepartmentProblems));
            }
<<<<<<< HEAD
        }        
        public void Add(Problem problem)
=======
        }

        private ObservableCollection<Problem> departmentProblems = new();
        public ObservableCollection<Problem> DepartmentProblems 
>>>>>>> 081a081 (Added Startup Window)
        {
            get => departmentProblems;
            set
            {
                departmentProblems = value;
                OnPropertyChanged(nameof(DepartmentProblems));
=======
>>>>>>> e8a7a46 (Now problems added to DB)
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

        private ThemesViewModel themesVM = new();
        public ThemesViewModel ThemesVM
        {
            get => themesVM;
            set
            {
                themesVM = value;
                OnPropertyChanged(nameof(ThemesVM));
            }
        }
        public ProblemsViewModel(Department department)
        {
            dbContext.Problems.Load();
            Department = department;
            if (Department.Problems != null) 
                foreach (var problem in Department.Problems)
                {
                    Problems.Add(problem);
                }

            ViewSource.Source = Problems;
        }

        #region Commands

        private RelayCommand newProblemAdd;
        public RelayCommand NewProblemAdd
        {
            get
            {
                return newProblemAdd ?? (newProblemAdd = new(obj =>
                {
                    Problem newproblem = new()
                    {
                        DateOccurance = DateTime.Now,
                        Department = Department,
                        Description = "Описание",
                        Status = "Решается"
                    };
                    NewProblemVM = new(newproblem, Problems);
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
                    ThemesVM = new();
                },
                obj => true));
            }
        }

        #endregion
    }
}
