using EmailReport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmailReport.ViewModels
{
    public class ReportListViewModel
    {
        public List<ReportViewModel> Records { get; set; }
        public int ProcessedCount { get; set; }
        public int DeliveredCount { get; set; }
        public int ClickCount { get; set; }
        public int DeferredCount { get; set; }
        public int OpenCount { get; set; }
        public int UniqueUser { get; set; }
        public List<DateCount> DateCount { get; set; }
        public List<UrlCount> UrlCount { get; set; }
        public int UniqueClickUser { get; set; }
        // public Dictionary<string, int> dic { get; set; }
        public List<RegionCodeCount> RegionCodeCount { get; set; }
        public List<CountryCount> America { get; set; }
        public List<CountryCount> AsianPacific { get; set; }
        public List<CountryCount> Europe { get; set; }
    }
}