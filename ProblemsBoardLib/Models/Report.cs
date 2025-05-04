using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

<<<<<<< HEAD
namespace BoardOfDecisionProblems.Models
=======
namespace ProblemsBoardLib.Models
>>>>>>> 2c81db8 (Added models for Log, Report and Log Events, created base viewmodel for problems)
{
    public class Report
    {
        public int ReportId { get; set; }
        public string Type { get; set; }

        [NotMapped]
        public string FullType
        {
            get
            {
                if (Type == "ОП") return "Отчет по проблеме";
                if (Type == "ОС") return "Отчет по статистике";
                return "";
            }
        }

        public string Number { get; set; }
<<<<<<< HEAD
        public DateOnly Date { get; set; }
=======
>>>>>>> 2c81db8 (Added models for Log, Report and Log Events, created base viewmodel for problems)
        public byte[] ReportFile { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
