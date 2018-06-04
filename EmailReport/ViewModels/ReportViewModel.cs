using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmailReport.ViewModels
{
    public class ReportViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        //public string Timestamp { get; set; }
        public string Event1 { get; set; }
        //public DateTime DateInclude { get; set; }
        public string Url { get; set; }
    }
}