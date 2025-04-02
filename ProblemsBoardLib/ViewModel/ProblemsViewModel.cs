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

        public ProblemsViewModel(Department department)
        {
            Department = department;
            if (Department.Problems != null) 
                foreach (var problem in Department.Problems)
                {
                    Problems.Add(problem);
                }

            ViewSource.Source = Problems;
        }

        #region Commands

        #endregion
    }
}
