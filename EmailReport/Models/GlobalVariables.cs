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

        public static List<string> L1List
        {
            get
            {
                return (List<string>)HttpContext.Current.Application["L1List"];
            }
            set
            {
                HttpContext.Current.Application["L1List"] = value;
            }
        }

        public static List<string> L2List
        {
            get
            {
                return (List<string>)HttpContext.Current.Application["L2List"];
            }
            set
            {
                HttpContext.Current.Application["L2List"] = value;
            }
        }

        public static List<string> L3List
        {
            get
            {
                return (List<string>)HttpContext.Current.Application["L3List"];
            }
            set
            {
                HttpContext.Current.Application["L3List"] = value;
            }
        }

        public static List<string> L4List
        {
            get
            {
                return (List<string>)HttpContext.Current.Application["L4List"];
            }
            set
            {
                HttpContext.Current.Application["L4List"] = value;
            }
        }

        public static List<string> L5List
        {
            get
            {
                return (List<string>)HttpContext.Current.Application["L5List"];
            }
            set
            {
                HttpContext.Current.Application["L5List"] = value;
            }
        }

        public static List<string> RegionList
        {
            get
            {
                return (List<string>)HttpContext.Current.Application["RegionList"];
            }
            set
            {
                HttpContext.Current.Application["RegionList"] = value;
            }
        }

        public static List<string> CountryList
        {
            get
            {
                return (List<string>)HttpContext.Current.Application["CountryList"];
            }
            set
            {
                HttpContext.Current.Application["CountryList"] = value;
            }
        }

        public static List<string> StatusList
        {
            get
            {
                return (List<string>)HttpContext.Current.Application["StatusList"];
            }
            set
            {
                HttpContext.Current.Application["StatusList"] = value;
            }
        }

    }
}