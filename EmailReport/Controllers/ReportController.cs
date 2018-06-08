using EmailReport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmailReport.ViewModels;
using EmailReport.BusinessLayer;
using System.Text.RegularExpressions;

namespace EmailReport.Controllers
{
    public class ReportController : Controller
    {
        
        // GET: Report
        public ActionResult Index()
        {

            ReportListViewModel ReportListViewModel = new ReportListViewModel();
            ReportBusinessLayer empBal = new ReportBusinessLayer();
            ReportListViewModel.HasEmployeeData = false;
            //get log recordsords from DB
            List<LogRecord> Reports = empBal.GetRecords();
            List<ReportViewModel> reportViewModels = new List<ReportViewModel>();

            //get employee info from DB
            List<Employee> employees = new List<Employee>();
            employees = empBal.GetEmployees();

            
            //System.Diagnostics.Debug.WriteLine(employees.Count());
            // initialize regionCodeCount list incase null pointer exception
            List < RegionCodeCount > regionCodeCount = new List<RegionCodeCount>();
            var i = 0;
            for (i = 0; i < 4; i++)
            {
                RegionCodeCount region = new RegionCodeCount();
                region.Count = 0;
                regionCodeCount.Add(region);
            }
            ReportListViewModel.RegionCodeCount = regionCodeCount;
           
            //ReportListViewModel.AsianPacific = new List<CountryCount>();
            if (Reports.Count() == 0)
            {
                return View("Index", ReportListViewModel);
            }

            // initialize baseview model
            BaseViewModel Base = new BaseViewModel();
            var L1 = from e in employees
                    select e.L1;
            List<string> L1l = L1.Distinct().ToList();
            L1l.Sort();
            Base.L1List = L1l;

            var L2 = from e in employees
                     select e.L2;
            List<string> L2l = L2.Distinct().ToList();
            L2l.Sort();
            Base.L2List = L2l;

            var L3 = from e in employees
                     select e.L3;
            List<string> L3l = L1.Distinct().ToList();
            L3l.Sort();

            Base.L3List = L3l;

            var L4 = from e in employees
                     select e.L4;
            List<string> L4l = L1.Distinct().ToList();
            L4l.Sort();
            Base.L4List = L4l;


            var L5 = from e in employees
                     select e.L5;
            List<string> L5l = L1.Distinct().ToList();
            L5l.Sort();
            Base.L5List = L5l;

            var Country = from e in employees
                     select e.Country;
            List<string> Countryl = Country.Distinct().ToList();
            Countryl.Sort();
            Base.CountryList = Countryl;

            var RegionList = from e in employees
                     select e.AreaCode;
            List<string> Regionl = RegionList.Distinct().ToList();
            Regionl.Sort();
            Base.RegionList = Regionl;

            var StatusList = from e in employees
                             select e.Status;
            List<string> Statusl = StatusList.Distinct().ToList();
            Statusl.Sort();
            Base.StatusList = Statusl;

            GlobalVariables.Base = Base;
                 

            ReportListViewModel.L1List = GlobalVariables.Base.L1List;
            ReportListViewModel.L2List = GlobalVariables.Base.L2List;
            ReportListViewModel.L3List = GlobalVariables.Base.L3List;
            ReportListViewModel.L4List = GlobalVariables.Base.L4List;
            ReportListViewModel.L5List = GlobalVariables.Base.L5List;
            ReportListViewModel.CountryList = GlobalVariables.Base.CountryList;
            ReportListViewModel.RegionList = GlobalVariables.Base.RegionList;
            ReportListViewModel.StatusList = GlobalVariables.Base.StatusList;

            //set the date range to show
            GlobalVariables.Start = "0000-00-00";
            GlobalVariables.End = "9999-99-99";
            //var records = from m in Reports
            //           select m;

            var email = from m in Reports                      
                        select m.Email;

            //get the number of unique users from their emails
            int uniqueUser = email.Distinct().Count();
            ReportListViewModel.UniqueUser = uniqueUser;

            // group the email info by date
            var dateInfo = from m in Reports
                           where m.Event1 == "open"
                           orderby DateTime.Parse(m.DateInclude).ToString("o").Substring(0, 10)
                           group m by DateTime.Parse(m.DateInclude).ToString("o").Substring(0, 10) into grp
                           select new { key = grp.Key, cnt = grp.Count() };

            // store the info of date into list
            List<DateCount> dateCount = new List<DateCount>();
            foreach (var item in dateInfo)
            {
                DateCount date = new DateCount();
                date.Date = item.key;

                date.Count = item.cnt;
                dateCount.Add(date);
            }


            //find out the infomaition of the openned emails
            var dateRegionInfo = from m in Reports
                                 where m.Event1 == "open"
                                 join code in employees on m.Email equals code.Email into ji
                                 from sub in ji.DefaultIfEmpty()
                                 select new { date = DateTime.Parse(m.DateInclude).ToString("o").Substring(0, 10), region = sub?.AreaCode ?? String.Empty};
            List<DateCount> finalDateCount = new List<DateCount>();

            //calculate the count for each region for each day
            foreach (var item in dateCount)
            {
                
                int APJCount = 0;
                int EURCount = 0;
                int AMSCount = 0;
                int UnGrouped = 0;
                foreach (var it in dateRegionInfo)
                {

                    if (item.Date.Equals(it.date))
                    {
                        switch (it.region)
                        {
                            case "AMS":
                                AMSCount++;
                                break;
                            case "EUR":
                                EURCount++;
                                break;
                            case "APJ":
                                APJCount++;
                                break;
                            case "":
                                UnGrouped++;
                                break;
                        }
                    }                  
                }
                item.AMSCount = AMSCount;
                item.EURCount = EURCount;
                item.APJCount = APJCount;
                item.UnGrouped = UnGrouped;
                finalDateCount.Add(item);
            }

            ReportListViewModel.DateCount = finalDateCount;


            //get the number of users who click in the url
            var clickUser = from m in Reports
                            where m.Event1 == "click"
                            select m.Email;
            int uniqueClickUser = clickUser.Distinct().Count();
            ReportListViewModel.UniqueClickUser = uniqueClickUser;

            // group the users by url and save the number into list
            var urlInfo = from m in Reports
                          where m.Url != ""
                          group m by m.Url.Split('/')[m.Url.Split('/').Count() - 1] into grp
                          orderby grp.Key
                          select new { key = grp.Key, cnt = grp.Count() };
            List<UrlCount> urlCount = new List<UrlCount>();
            foreach (var item in urlInfo)
            {
                UrlCount url = new UrlCount();
                url.Url = item.key;
                url.Count = item.cnt;
                urlCount.Add(url);
            }
            ReportListViewModel.UrlCount = urlCount;

            
            //get the unique users for open, click and deliver

            var open = from re in Reports
                             where re.Event1 == "open"
                             select re.Email;
            var click = from re in Reports
                        where re.Event1 == "click"
                       select re.Email;
            var delivered = from re in Reports
                            where re.Event1 == "delivered"
                            select re.Email;

            ReportListViewModel.OpenCount = open.Distinct().Count();
            ReportListViewModel.ClickCount = click.Distinct().Count();
            ReportListViewModel.DeliveredCount = delivered.Distinct().Count();
           
            if (employees.Count() == 0)
            {
                return View("Index", ReportListViewModel);
            }
            ReportListViewModel.HasEmployeeData = true;

            // get the number of 
            var regionCode = from m in Reports
                             where m.Event1 == "open"
                             join code in employees on m.Email equals code.Email
                             group code by code.AreaCode into grp
                             select new { code = grp.Key, cnt = grp.Distinct().Count() };
            
            
            regionCodeCount = new List<RegionCodeCount>();
            i = 0;
            for (i = 0; i < 4; i++)
            {
                RegionCodeCount region = new RegionCodeCount();
                regionCodeCount.Add(region);
            }
            var upgroupedCount = open.Distinct().Count();
            foreach (var item in regionCode)
            {
                upgroupedCount -= item.cnt;
                switch (item.code)
                {
                    case "AMS":
                        regionCodeCount.ElementAt(0).RegionCode = item.code;
                        regionCodeCount.ElementAt(0).Count = item.cnt;
                        break;
                    case "EUR":
                        regionCodeCount.ElementAt(1).RegionCode = item.code;
                        regionCodeCount.ElementAt(1).Count = item.cnt;
                        break;
                    case "APJ":
                        regionCodeCount.ElementAt(2).RegionCode = item.code;
                        regionCodeCount.ElementAt(2).Count = item.cnt;
                        break;
                }

            }

            regionCodeCount.ElementAt(3).RegionCode = "Ungrouped";
            regionCodeCount.ElementAt(3).Count = upgroupedCount;

            ReportListViewModel.RegionCodeCount = regionCodeCount;


            var countrySet = from m in Reports
                             where m.Event1 == "open"
                             join code in employees on m.Email equals code.Email
                             group code by code.Country into grp
                             select new { code = grp.Key, cnt = grp.Distinct().Count(), regionCode = grp.First().AreaCode };
            List<CountryCount> america = new List<CountryCount>();
            List<CountryCount> asianPacific = new List<CountryCount>();
            List<CountryCount> europe = new List<CountryCount>();
            foreach (var country in countrySet)
            {
                CountryCount cty = new CountryCount();
                cty.Country = country.code;
                cty.Count = country.cnt;
                switch (country.regionCode)
                {
                    case "AMS":
                        america.Add(cty);
                        break;
                    case "EUR":
                        europe.Add(cty);
                        break;
                    case "APJ":
                        asianPacific.Add(cty);
                        break;
                }
            }
            var orderedAmerica = from m in america
                                 orderby m.Count descending
                                 select m;
            var orderedEurope = from m in europe
                                orderby m.Count descending
                                select m;
            var orderedAsianPacfic = from m in asianPacific
                                     orderby m.Count descending
                                     select m;
            ReportListViewModel.America = orderedAmerica.ToList();
            ReportListViewModel.Europe = orderedEurope.ToList();
            ReportListViewModel.AsianPacific = orderedAsianPacfic.ToList();

            foreach (LogRecord emp in Reports)
            {
                ReportViewModel reportViewModel = new ReportViewModel();
                reportViewModel.Email = emp.Email;
                reportViewModel.Event1 = emp.Event1;
                reportViewModels.Add(reportViewModel);
            }
            ReportListViewModel.Records = reportViewModels;

            return View("Index", ReportListViewModel);

        }

        public ActionResult Details(string id)
        {
            
            ReportListDetailsViewModel reportListDetailsViewModel = new ReportListDetailsViewModel();
            ReportBusinessLayer empBal = new ReportBusinessLayer();
            List<LogRecord> recordsords = empBal.GetRecords();

            List<ReportDetailsViewModel> reportViewModels = new List<ReportDetailsViewModel>();
            List<Employee> employees = new List<Employee>();
            employees = empBal.GetEmployees();

            reportListDetailsViewModel.TotalCount = recordsords.Count();
            var em = from m in recordsords
                       where m.Event1.Equals(id) && GlobalVariables.Start.CompareTo(DateTime.Parse(m.DateInclude).ToString("o").Substring(0, 10)) <= 0 && GlobalVariables.End.CompareTo(DateTime.Parse(m.DateInclude).ToString("o").Substring(0, 10)) >= 0
                       select m.Email;
            

            var empDetail = from m in em
                            join empl in employees on m equals empl.Email
                            group empl by empl.Email into grp
                            select new { Email = grp.Key, Cnt = grp.Count(), Code = grp.First().AreaCode, Country = grp.First().Country};

            reportListDetailsViewModel.CurCount = em.Distinct().Count();
            foreach (var detail in empDetail)
            {
                ReportDetailsViewModel reportViewModel = new ReportDetailsViewModel();
                //reportViewModel.Id = detail.Id;
                reportViewModel.Email = detail.Email;
                reportViewModel.Country = detail.Country;
                reportViewModel.AreaCode = detail.Code;
                reportViewModel.Count = detail.Cnt;

                reportViewModels.Add(reportViewModel);
            }
            reportListDetailsViewModel.Event1 = id;
            reportListDetailsViewModel.Records = reportViewModels;

            reportListDetailsViewModel.L1List = GlobalVariables.Base.L1List;
            reportListDetailsViewModel.L2List = GlobalVariables.Base.L2List;
            reportListDetailsViewModel.L3List = GlobalVariables.Base.L3List;
            reportListDetailsViewModel.L4List = GlobalVariables.Base.L4List;
            reportListDetailsViewModel.L5List = GlobalVariables.Base.L5List;
            reportListDetailsViewModel.CountryList = GlobalVariables.Base.CountryList;
            reportListDetailsViewModel.RegionList = GlobalVariables.Base.RegionList;
            reportListDetailsViewModel.StatusList = GlobalVariables.Base.StatusList;

            return View("Details", reportListDetailsViewModel);
        }



        public ActionResult DateRange()
        {
            string Start = DateTime.Parse(Request.Form["StartDate"]).ToString("o").Substring(0, 10);
            string End = DateTime.Parse(Request.Form["EndDate"]).ToString("o").Substring(0, 10);

            string L1 = Request.Form["L1"];
            if (!L1.Equals(""))
            {
                string[] L1Values = Regex.Split(L1, "(?<=^[^\"]*(?:\"[^\"]*\"[^\"]*)*),(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
            }
            

            System.Diagnostics.Debug.WriteLine(L1);
            ReportListViewModel ReportListViewModel = new ReportListViewModel();
            ReportBusinessLayer empBal = new ReportBusinessLayer();
            List<LogRecord> AllRecords = empBal.GetRecords();
            GlobalVariables.Start = Start;
            GlobalVariables.End = End;
            
            var Reports = from m in AllRecords

                          where (Start.CompareTo(DateTime.Parse(m.DateInclude).ToString("o").Substring(0, 10)) <= 0 && End.CompareTo(DateTime.Parse(m.DateInclude).ToString("o").Substring(0, 10)) >= 0)
                          select m;
            System.Diagnostics.Debug.WriteLine(Reports.Count() + "count is");
            List<ReportViewModel> repViewModels = new List<ReportViewModel>();

            List<ReportViewModel> reportViewModels = new List<ReportViewModel>();

            List<Employee> employees = new List<Employee>();
            employees = empBal.GetEmployees();


            var email = from m in Reports
                        select m.Email;

            int uniqueUser = email.Distinct().Count();
            ReportListViewModel.UniqueUser = uniqueUser;

            var dateInfo = from m in Reports
                           where m.Event1 == "open"
                           orderby DateTime.Parse(m.DateInclude).Year.ToString() + "-" + DateTime.Parse(m.DateInclude).Month.ToString() + "-" + DateTime.Parse(m.DateInclude).Day.ToString()
                           group m by DateTime.Parse(m.DateInclude).Year.ToString() + "-" + DateTime.Parse(m.DateInclude).Month.ToString() + "-" + DateTime.Parse(m.DateInclude).Day.ToString() into grp
                           select new { key = grp.Key, cnt = grp.Count() };
            List<DateCount> dateCount = new List<DateCount>();
            foreach (var item in dateInfo)
            {
                DateCount date = new DateCount();
                date.Date = item.key;

                date.Count = item.cnt;
                dateCount.Add(date);
            }

            var dateRegionInfo = from m in Reports
                                 where m.Event1 == "open"
                                 join code in employees on m.Email equals code.Email into ji
                                 from sub in ji.DefaultIfEmpty()
                                 select new { date = DateTime.Parse(m.DateInclude).Year.ToString() + "-" + DateTime.Parse(m.DateInclude).Month.ToString() + "-" + DateTime.Parse(m.DateInclude).Day.ToString(), region = sub?.AreaCode ?? String.Empty };
            List<DateCount> finalDateCount = new List<DateCount>();

            foreach (var item in dateCount)
            {
                int APJCount = 0;
                int EURCount = 0;
                int AMSCount = 0;
                int UnGrouped = 0;
                foreach (var it in dateRegionInfo)
                {

                    if (item.Date.Equals(it.date))
                    {
                        switch (it.region)
                        {
                            case "AMS":
                                AMSCount++;
                                break;
                            case "EUR":
                                EURCount++;
                                break;
                            case "APJ":
                                APJCount++;
                                break;
                            case "":
                                UnGrouped++;
                                break;
                        }

                    }

                }
                item.AMSCount = AMSCount;
                item.EURCount = EURCount;
                item.APJCount = APJCount;
                item.UnGrouped = UnGrouped;
                finalDateCount.Add(item);
            }

            ReportListViewModel.DateCount = finalDateCount;



            var clickUser = from m in Reports
                            where m.Event1 == "click"
                            select m.Email;
            int uniqueClickUser = clickUser.Distinct().Count();
            ReportListViewModel.UniqueClickUser = uniqueClickUser;

            var urlInfo = from m in Reports
                          where m.Url != ""
                          group m by m.Url.Split('/')[m.Url.Split('/').Count() - 1] into grp
                          orderby grp.Key
                          select new { key = grp.Key, cnt = grp.Count() };
            List<UrlCount> urlCount = new List<UrlCount>();
            foreach (var item in urlInfo)
            {
                UrlCount url = new UrlCount();
                url.Url = item.key;
                url.Count = item.cnt;
                urlCount.Add(url);
            }
            ReportListViewModel.UrlCount = urlCount;




            var open = from re in Reports
                       where re.Event1 == "open"
                       select re.Email;
            var click = from re in Reports
                        where re.Event1 == "click"
                        select re.Email;
            var delivered = from re in Reports
                            where re.Event1 == "delivered"
                            select re.Email;

            ReportListViewModel.OpenCount = open.Distinct().Count();
            ReportListViewModel.ClickCount = click.Distinct().Count();
            ReportListViewModel.DeliveredCount = delivered.Distinct().Count();


            var regionCode = from m in Reports
                             where m.Event1 == "open"
                             join code in employees on m.Email equals code.Email
                             group code by code.AreaCode into grp
                             select new { code = grp.Key, cnt = grp.Distinct().Count() };
            List<RegionCodeCount> regionCodeCount = new List<RegionCodeCount>();
            int i = 0;
            for (i = 0; i < 4; i++)
            {
                RegionCodeCount region = new RegionCodeCount();
                regionCodeCount.Add(region);
            }
            var upgroupedCount = open.Distinct().Count();
            foreach (var item in regionCode)
            {
                upgroupedCount -= item.cnt;
                switch (item.code)
                {
                    case "AMS":
                        regionCodeCount.ElementAt(0).RegionCode = item.code;
                        regionCodeCount.ElementAt(0).Count = item.cnt;
                        break;
                    case "EUR":
                        regionCodeCount.ElementAt(1).RegionCode = item.code;
                        regionCodeCount.ElementAt(1).Count = item.cnt;
                        break;
                    case "APJ":
                        regionCodeCount.ElementAt(2).RegionCode = item.code;
                        regionCodeCount.ElementAt(2).Count = item.cnt;
                        break;
                }
               
            }

            regionCodeCount.ElementAt(3).RegionCode = "Ungrouped";
            regionCodeCount.ElementAt(3).Count = upgroupedCount;
            

            ReportListViewModel.RegionCodeCount = regionCodeCount;


            var countrySet = from m in Reports
                             where m.Event1 == "open"
                             join code in employees on m.Email equals code.Email
                             group code by code.Country into grp
                             select new { code = grp.Key, cnt = grp.Distinct().Count(), regionCode = grp.First().AreaCode };
            List<CountryCount> america = new List<CountryCount>();
            List<CountryCount> asianPacific = new List<CountryCount>();
            List<CountryCount> europe = new List<CountryCount>();
            foreach (var country in countrySet)
            {
                CountryCount cty = new CountryCount();
                cty.Country = country.code;
                cty.Count = country.cnt;
                switch (country.regionCode)
                {
                    case "AMS":
                        america.Add(cty);
                        break;
                    case "EUR":
                        europe.Add(cty);
                        break;
                    case "APJ":
                        asianPacific.Add(cty);
                        break;
                }
            }

            var orderedAmerica = from m in america
                                 orderby m.Count
                                 select m;
            var orderedEurope = from m in europe
                                orderby m.Count
                                select m;
            var orderedAsianPacfic = from m in asianPacific
                                orderby m.Count
                                select m;
            ReportListViewModel.America = orderedAmerica.ToList();
            ReportListViewModel.Europe = orderedEurope.ToList();
            ReportListViewModel.AsianPacific = orderedAsianPacfic.ToList();

            foreach (LogRecord emp in Reports)
            {
                ReportViewModel reportViewModel = new ReportViewModel();
                reportViewModel.Email = emp.Email;
                reportViewModel.Event1 = emp.Event1;
                reportViewModels.Add(reportViewModel);
            }
            ReportListViewModel.Records = reportViewModels;

            ReportListViewModel.L1List = GlobalVariables.Base.L1List;
            ReportListViewModel.L2List = GlobalVariables.Base.L2List;
            ReportListViewModel.L3List = GlobalVariables.Base.L3List;
            ReportListViewModel.L4List = GlobalVariables.Base.L4List;
            ReportListViewModel.L5List = GlobalVariables.Base.L5List;
            ReportListViewModel.CountryList = GlobalVariables.Base.CountryList;
            ReportListViewModel.RegionList = GlobalVariables.Base.RegionList;
            ReportListViewModel.StatusList = GlobalVariables.Base.StatusList;
            return View("Index", ReportListViewModel);

            
        }
    }
}
