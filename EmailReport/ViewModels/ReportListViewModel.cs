using EmailReport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmailReport.ViewModels
{
    public class ReportListViewModel : BaseViewModel
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
        public int NotInListCount { get; set; }

        public List<RegionCodeCount> RegionCodeCount { get; set; }
        public List<CountryCount> America { get; set; }
        public List<CountryCount> AsianPacific { get; set; }
        public List<CountryCount> Europe { get; set; }
        public List<CountryCount> EMEA { get; set; }
        // public Boolean HasEmployeeData { get; set; }
        public string json { get; set; }
        public string GraphLine { get; set; }
        public Boolean WorldMap { get; set; }
        public Boolean LineGraph { get; set; }

        public ReportListViewModel()
        {
            Records = new List<ReportViewModel>();
            RegionCodeCount = new List<RegionCodeCount>();
            America = new List<CountryCount>();
            Europe = new List<CountryCount>();
            EMEA = new List<CountryCount>();
            AsianPacific = new List<CountryCount>();
            DateCount = new List<DateCount>();
            UrlCount = new List<UrlCount>();
            L1List = new List<string>();
            L2List = new List<string>();
            L3List = new List<string>();
            L4List = new List<string>();
            L5List = new List<string>();
            RegionList = new List<string>();
            CountryList = new List<string>();
            StatusList = new List<string>();
            SelectedL1List = new List<string>();
            SelectedL2List = new List<string>();
            SelectedL3List = new List<string>();
            SelectedL4List = new List<string>();
            SelectedL5List = new List<string>();
            SelectedRegionList = new List<string>();
            SelectedCountryList = new List<string>();
            SelectedStatusList = new List<string>();

        }
    
    }
}