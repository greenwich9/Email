using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmailReport.ViewModels
{
    public class BaseViewModel
    {
        public List<string> L1List { get; set; }
        public List<string> L2List { get; set; }
        public List<string> L3List { get; set; }
        public List<string> L4List { get; set; }
        public List<string> L5List { get; set; }
        public List<string> RegionList { get; set; }
        public List<string> CountryList { get; set; }
        public List<string> StatusList { get; set; }
        public List<string> SelectedL1List { get; set; }
        public List<string> SelectedL2List { get; set; }
        public List<string> SelectedL3List { get; set; }
        public List<string> SelectedL4List { get; set; }
        public List<string> SelectedL5List { get; set; }
        public List<string> SelectedRegionList { get; set; }
        public List<string> SelectedCountryList { get; set; }
        public List<string> SelectedStatusList { get; set; }
        public string Start { get; set; }
        public string End { get; set; }

        public BaseViewModel()
        {
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