using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmailReport.Models
{
    public class AreaInfo
    {
        public string AreaCode { get; set; }
        public List<CountryCount> AreaCountryCount { get; set; }
    }
}