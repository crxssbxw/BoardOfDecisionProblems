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
        
        public void Add(Problem problem)
        {
            problems.Add(problem);
            OnPropertyChanged(nameof(Problems));
        }

        public ProblemsViewModel()
        {
            ViewSource.Source = Problems;
        }
    }
}
