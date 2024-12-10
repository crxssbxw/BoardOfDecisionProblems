using System;
using System.Collections.Generic;
<<<<<<< HEAD:ProblemsBoardLib/Models/Report.cs
using System.ComponentModel.DataAnnotations.Schema;
=======
>>>>>>> 953383f (Added Reporting system and some fixes for logger logic):BoardOfDecisionProblems/Models/Report.cs
using System.Linq;
using System.Text;
using System.Threading.Tasks;

<<<<<<< HEAD:ProblemsBoardLib/Models/Report.cs
namespace ProblemsBoardLib.Models
=======
namespace BoardOfDecisionProblems.Models
>>>>>>> 953383f (Added Reporting system and some fixes for logger logic):BoardOfDecisionProblems/Models/Report.cs
{
    public class Report
    {
        public int ReportId { get; set; }
        public string Type { get; set; }
<<<<<<< HEAD:ProblemsBoardLib/Models/Report.cs

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
        public byte[] ReportFile { get; set; }
        public DateTime CreatedAt { get; set; }
=======
        public string Number { get; set; }
        public DateOnly Date { get; set; }
        public byte[] ReportFile { get; set; }
>>>>>>> 953383f (Added Reporting system and some fixes for logger logic):BoardOfDecisionProblems/Models/Report.cs
    }
}
