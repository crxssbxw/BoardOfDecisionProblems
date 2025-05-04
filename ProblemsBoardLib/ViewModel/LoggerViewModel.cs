using ProblemsBoardLib.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemsBoardLib.ViewModel
{
    public class LoggerViewModel : BaseViewModel
    {
        public LoggerViewModel()
        {
            foreach (var logevent in dbContext.LogEvents)
            {
                LogEvents.Add(logevent);
            }
        }

        private ObservableCollection<LogEvent> logEvents = new();
        public ObservableCollection<LogEvent> LogEvents
        {
            get => logEvents;
            set
            {
                logEvents = value;
                OnPropertyChanged(nameof(LogEvents));
            }
        }

        private LogEvent selectedEvent;
        public LogEvent SelectedEvent
        {
            get => selectedEvent;
            set
            {
                selectedEvent = value;
                OnPropertyChanged(nameof(SelectedEvent));
            }
        }
    }
}
