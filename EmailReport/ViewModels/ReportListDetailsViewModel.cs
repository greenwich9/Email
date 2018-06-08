using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmailReport.ViewModels
{
    public class ReportListDetailsViewModel : BaseViewModel
    {
        public List<ReportDetailsViewModel> Records { get; set; }
        public int CurCount { get; set; }
        public int TotalCount { get; set; }
        public string Event1 { get; set; }
    }
}