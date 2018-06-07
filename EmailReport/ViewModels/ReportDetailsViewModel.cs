using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmailReport.ViewModels
{
    public class ReportDetailsViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        //public string Timestamp { get; set; }
        public string AreaCode { get; set; }
        public string Country { get; set; }
        public int Count { get; set; }
    }
}