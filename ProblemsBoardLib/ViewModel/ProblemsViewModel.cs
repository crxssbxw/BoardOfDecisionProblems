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
                OnPropertyChanged(nameof(DepartmentProblems));
            }
        }

        private ObservableCollection<Problem> departmentProblems = new();
        public ObservableCollection<Problem> DepartmentProblems 
        {
            get => departmentProblems;
            set
            {
                departmentProblems = value;
                OnPropertyChanged(nameof(DepartmentProblems));
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
                foreach (var problem in Problems)
                {
                    if (problem.DepartmentId == value.DepartmentId)
                        DepartmentProblems.Add(problem);
                }
                OnPropertyChanged(nameof(DepartmentProblems));
            }
        }

        public ProblemsViewModel()
        {
            foreach (var problem in dbContext.Problems)
            {
                Problems.Add(problem);
            }

            ViewSource.Source = DepartmentProblems;
        }
    }
}
