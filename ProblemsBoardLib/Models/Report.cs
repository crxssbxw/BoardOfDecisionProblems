using System;
using System.Collections.Generic;
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
        public string Number { get; set; }
<<<<<<< HEAD
        public DateOnly Date { get; set; }
=======
>>>>>>> 2c81db8 (Added models for Log, Report and Log Events, created base viewmodel for problems)
        public byte[] ReportFile { get; set; }
    }
}
