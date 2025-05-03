using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemsBoardLib.Models
{
    public class Report
    {
        public int ReportId { get; set; }
        public string Type { get; set; }
        public string Number { get; set; }
        public byte[] ReportFile { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
