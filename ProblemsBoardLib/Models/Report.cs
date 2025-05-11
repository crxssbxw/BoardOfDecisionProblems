using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemsBoardLib.Models
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
        public byte[] ReportFile { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
