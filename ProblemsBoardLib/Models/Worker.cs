﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemsBoardLib.Models
{
    public class Worker
    {
        public int WorkerId { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string? MiddleName { get; set; }
        public string Post { get; set; }
        public int DepartmentId { get; set; }
        public bool IsHeader { get; set; } = false;
        public string? HeaderLogin { get; set; }
        public string? HeaderPassword { get; set; }
        public string? Email { get; set; }

        public Department Department { get; set; }
        [NotMapped]
        public string DepartmentNumber
        {
            get => Department.ViewerNumber;
        }

        [NotMapped]
        public string WorkerInfo { get => $"{SecondName} {FirstName} {MiddleName} - {Post}"; }

        public ICollection<Responsible>? Responsibles { get; set; }

        public override string ToString()
        {
            return $"{SecondName} {FirstName}";
        }
    }
}
