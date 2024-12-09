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
    public class Log
    {
        public int LogId { get; set; }
        public byte[]? LogFile { get; set; }
        public DateTime DateTime { get; set; }
    }
}
