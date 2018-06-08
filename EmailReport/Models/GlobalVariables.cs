using EmailReport.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmailReport.Models
{
    public static class GlobalVariables
    {
        // readonly variable
        
        public static string Start
        {
            get
            {
                return HttpContext.Current.Application["Start"] as string;
            }
            set
            {
                HttpContext.Current.Application["Start"] = value;
            }
        }

        public static string End
        {
            get
            {
                return HttpContext.Current.Application["End"] as string;
            }
            set
            {
                HttpContext.Current.Application["End"] = value;
            }
        }

        public static BaseViewModel Base
        {
            get
            {
                return (BaseViewModel)HttpContext.Current.Application["Base"];
            }
            set
            {
                HttpContext.Current.Application["Base"] = value;
            }
        }
    }
}