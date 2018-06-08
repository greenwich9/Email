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
        }
    }

    
}