using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmailReport.Models
{
    public class DateCount
    {
        public string Date { get; set; }
        public int Count { get; set; }
        public int APJCount { get; set; }
        public int EURCount { get; set; }
        public int AMSCount { get; set; }
        public int UnGrouped { get; set; }
    }
}