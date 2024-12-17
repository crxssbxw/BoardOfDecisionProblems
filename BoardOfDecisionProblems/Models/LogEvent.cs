﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardOfDecisionProblems.Models
{
    public class LogEvent
    {
        public int LogEventId { get; set; }
        public string Title { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public string? User { get; set; }
        public string? Table { get; set; }
        public string? Object { get; set; }
        public string? Comment { get; set; }

        public LogEvent()
        {
            Date = DateOnly.FromDateTime(DateTime.Now);
            Time = TimeOnly.FromDateTime(DateTime.Now);
        }
    }
}
