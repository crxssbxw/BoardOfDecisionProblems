using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardOfDecisionProblems.Models
{
    public class Log
    {
        public int LogId { get; set; }
        public byte[]? LogFile { get; set; }
        public DateTime DateTime { get; set; }
    }
}
