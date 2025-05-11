using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using ProblemsBoardLib.Commands;
using ProblemsBoardLib.Models;
using ProblemsBoardLib.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemsBoardLib.ViewModel
{
    public class ProblemDecisionViewModel : BaseViewModel
    {
        private Problem problem = new();
        public Problem Problem
        {
            get => problem;
            set
            {
                problem = value;
                OnPropertyChanged(nameof(Problem));
            }
        }

        private ProblemsViewModel vm;
        public ProblemsViewModel VM
        {
            get => vm;
            set
            {
                vm = value;
                OnPropertyChanged(nameof(VM));
            }
        }

        public ProblemDecisionViewModel()
        {

        }

        public ProblemDecisionViewModel(Problem problem, ProblemsViewModel vm, bool isAuthorized)
        {
            dbContext.Responsibles.Load();
            dbContext.Workers.Load();
            dbContext.Departments.Load();
            Problem = dbContext.Problems.Find(problem.ProblemId);
            VM = vm;
            IsAuthorized = isAuthorized;
        }

        public ProblemDecisionViewModel(bool isAuthorized)
        {
            IsAuthorized = isAuthorized;
        }

        private bool isAuthorized;
        public bool IsAuthorized
        {
            get => isAuthorized;
            set
            {
                isAuthorized = value;
                OnPropertyChanged(nameof(IsAuthorized));
            }
        }

        public string Title
        {
            get => $"Решение проблемы №{Problem.ProblemId}";
        }

        public DateTime Today
        {
            get => DateTime.Today;
        }

        private bool IsValid
        {
            get
            {
                if (Problem == null) return false;
                if (string.IsNullOrEmpty(Problem.Decision)) return false;
                if (Problem.DateElimination == null) return false;
                return true;
            }
        }

        private RelayCommand acceptDecision;
        public RelayCommand AcceptDecision
        {
            get
            {
                return acceptDecision ?? (acceptDecision = new(obj =>
                {
                    Problem.Status = "Решена";
                    dbContext.SaveChanges();
                    LoggingTool.ProblemDecided(Problem);
                    VM.ProblemsReload();
                },
                obj => IsValid));
            }
        }
    }
}
