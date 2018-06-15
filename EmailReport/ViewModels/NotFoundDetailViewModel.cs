using EmailReport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmailReport.ViewModels
{
    public class NotFoundDetailViewModel : BaseViewModel
    {
        public List<LogRecord> Records { get; set; }
        public int DeliveredCount { get; set; }
        public int ClickCount { get; set; }
        public int OpenCount { get; set; }
        public int UniqueUser { get; set; }
    }
}