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
        public string L1 { get; set; }
        public string L2 { get; set; }
        public string L3 { get; set; }
        public string L4 { get; set; }
        public string L5 { get; set; }
        public string Status { get; set; }
    }
}