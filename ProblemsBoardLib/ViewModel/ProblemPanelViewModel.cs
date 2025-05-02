using Microsoft.EntityFrameworkCore;
using ProblemsBoardLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            dbContext.Responsibles.Load();
            dbContext.Workers.Load();
        }

        public ProblemPanelViewModel(Problem problem) : base()
        {
            dbContext.Themes.Load();
            dbContext.Responsibles.Load();
            dbContext.Workers.Load();
            dbContext.Departments.Load();
            Problem = dbContext.Problems.Find(problem.ProblemId);
        }

        public bool IsDecided
        {
            get
            {
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
    }
}
