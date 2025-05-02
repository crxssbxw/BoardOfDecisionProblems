using Microsoft.VisualBasic;
using ProblemsBoardLib.Commands;
using ProblemsBoardLib.Models;
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

        public ProblemDecisionViewModel(Problem problem, ProblemsViewModel vm)
        {
            Problem = dbContext.Problems.Find(problem.ProblemId);
            VM = vm;
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
                    VM.ProblemsReload();
                },
                obj => IsValid));
            }
        }
    }
}
