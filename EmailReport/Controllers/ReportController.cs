using EmailReport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmailReport.ViewModels;
using EmailReport.BusinessLayer;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace EmailReport.Controllers
{
    public class ReportController : Controller
    {
        
        // GET: Report
        public ActionResult Index()
        {

            ReportListViewModel ReportListViewModel = new ReportListViewModel();
            ReportBusinessLayer empBal = new ReportBusinessLayer();

            //get log recordsords from DB
            List<LogRecord> allReports = empBal.GetRecords();
            List<ReportViewModel> reportViewModels = new List<ReportViewModel>();

            //get employee info from DB
            List<Employee> employees = new List<Employee>();
            employees = empBal.GetEmployees();
            var ep = from e in employees
                        group e by e.Email into grp
                        select new Employee{ Email = grp.Key, L1 = grp.Last().L1, L2 = grp.Last().L2, L3 = grp.Last().L3, L4 = grp.Last().L4, L5 = grp.Last().L5,
                            AreaCode = grp.Last().AreaCode, Country = grp.Last().Country, Status = grp.Last().Status};
            employees = (List<Employee>)ep.ToList();
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
            //if (allReports.Count() == 0)
            //{
            //    return View("Index", ReportListViewModel);
            //}

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
            List<string> L3l = L3.Distinct().ToList();
            L3l.Sort();

            Base.L3List = L3l;

            var L4 = from e in employees
                     select e.L4;
            List<string> L4l = L4.Distinct().ToList();
            L4l.Sort();
            Base.L4List = L4l;

            

            var L5 = from e in employees
                     select e.L5;
            List<string> L5l = L5.Distinct().ToList();
            L5l.Sort();
            Base.L5List = L5l;

            var CountryList = from e in employees
                     select e.Country;
            List<string> Countryl = CountryList.Distinct().ToList();
            Countryl.Sort();
            Base.CountryList = Countryl;

            var RegionList = from e in employees
                     select e.AreaCode;
            List<string> Regionl = RegionList.Distinct().ToList();
            if (Regionl.Contains("EUR"))
            {
                Regionl.Remove("EUR");
            }
            Regionl.Sort();
            Base.RegionList = Regionl;

            var StatusList = from e in employees
                             select e.Status;
            List<string> Statusl = StatusList.Distinct().ToList();
            Statusl.Sort();
            Base.StatusList = Statusl;

            GlobalVariables.Base = Base;

            GlobalVariables.L1List = L1l;
            GlobalVariables.L2List = L2l;
            GlobalVariables.L3List = L3l;
            GlobalVariables.L4List = L4l;
            GlobalVariables.L5List = L5l;
            GlobalVariables.RegionList = Regionl;
            GlobalVariables.CountryList = Countryl;
            GlobalVariables.StatusList = Statusl;

            ReportListViewModel.L1List = GlobalVariables.Base.L1List;
            ReportListViewModel.L2List = GlobalVariables.Base.L2List;
            ReportListViewModel.L3List = GlobalVariables.Base.L3List;
            ReportListViewModel.L4List = GlobalVariables.Base.L4List;
            ReportListViewModel.L5List = GlobalVariables.Base.L5List;
            ReportListViewModel.CountryList = GlobalVariables.Base.CountryList;
            ReportListViewModel.RegionList = GlobalVariables.Base.RegionList;
            ReportListViewModel.StatusList = GlobalVariables.Base.StatusList;

            //set the date range to show
            DateTime End = DateTime.Today;
            DateTime Start = DateTime.Today.AddMonths(-1);
            ReportListViewModel.Start = Start.ToString("d");
            ReportListViewModel.End = End.ToString("d");

            System.Diagnostics.Debug.WriteLine(Start);
            System.Diagnostics.Debug.WriteLine(End);

            GlobalVariables.Start = Start.ToString("o").Substring(0, 10);
            GlobalVariables.End = End.ToString("o").Substring(0, 10);

            var aReports = from m in allReports

                          where (GlobalVariables.Start.CompareTo(DateTime.Parse(m.DateInclude).ToString("o").Substring(0, 10)) <= 0 && GlobalVariables.End.CompareTo(DateTime.Parse(m.DateInclude).ToString("o").Substring(0, 10)) >= 0)
                          select m;
            List<LogRecord> Reports = aReports.ToList();
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
                
                int AMSCount = 0;
                int EMEACount = 0;
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
                                EMEACount++;
                                break;
                            case "APJ":
                                APJCount++;
                                break;
                            case "EMEA":
                                EMEACount++;
                                break;
                            case "":
                                UnGrouped++;
                                break;
                        }
                    }                  
                }
                item.AMSCount = AMSCount;
                
                item.APJCount = APJCount;
                item.EMEACount = EMEACount;
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

            // get the number of 
            var regionCode = from m in open.Distinct()
                             
                             join code in employees on m equals code.Email
                             group code by code.AreaCode into grp
                             select new { code = grp.Key, cnt = grp.Distinct().Count() };


            regionCodeCount = new List<RegionCodeCount>();
            
            for (i = 0; i < 4; i++)
            {
                RegionCodeCount region = new RegionCodeCount();
                regionCodeCount.Add(region);
            }
            var upgroupedCount = open.Distinct().Count();
            foreach (var item in regionCode)
            {

                switch (item.code)
                {
                    case "AMS":
                        regionCodeCount.ElementAt(0).RegionCode = item.code;
                        regionCodeCount.ElementAt(0).Count = item.cnt;
                        upgroupedCount -= item.cnt;
                        break;
                    case "EMEA":
                        regionCodeCount.ElementAt(1).RegionCode = item.code;
                        regionCodeCount.ElementAt(1).Count += item.cnt;
                        upgroupedCount -= item.cnt;
                        break;
                    case "EUR":
                        //regionCodeCount.ElementAt(1).RegionCode = item.code;
                        regionCodeCount.ElementAt(1).Count += item.cnt;
                        upgroupedCount -= item.cnt;
                        break;
                    case "APJ":
                        regionCodeCount.ElementAt(2).RegionCode = item.code;
                        regionCodeCount.ElementAt(2).Count = item.cnt;
                        upgroupedCount -= item.cnt;
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
            List<CountryCount> EMEA = new List<CountryCount>();
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
                        EMEA.Add(cty);
                        break;
                    case "APJ":
                        asianPacific.Add(cty);
                        break;
                    case "EMEA":
                        EMEA.Add(cty);
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
            var orderedEMEA = from m in EMEA
                              orderby m.Count descending
                              select m;
            ReportListViewModel.America = orderedAmerica.ToList();
            ReportListViewModel.Europe = orderedEurope.ToList();
            ReportListViewModel.AsianPacific = orderedAsianPacfic.ToList();
            ReportListViewModel.EMEA = orderedEMEA.ToList();


            foreach (LogRecord emp in Reports)
            {
                ReportViewModel reportViewModel = new ReportViewModel();
                reportViewModel.Email = emp.Email;
                reportViewModel.Event1 = emp.Event1;
                reportViewModels.Add(reportViewModel);
            }
            ReportListViewModel.Records = reportViewModels;
            JavaScriptSerializer jss = new JavaScriptSerializer();

            List<CountryCount> Map = new List<CountryCount>();
            Map.AddRange(america);
            Map.AddRange(asianPacific);
            Map.AddRange(EMEA);
            string output = jss.Serialize(Map);
            output = output.Replace("\"Country\"", "name").Replace("\"Count\"", "value").Replace("\"", "\'");
            ReportListViewModel.json = output;
            //System.Diagnostics.Debug.WriteLine(output);


            string GraphLine = jss.Serialize(ReportListViewModel.DateCount);
            GraphLine = GraphLine.Replace("\"Date\"", "Date").Replace("\"APJCount\"", "APJ").Replace("\"AMSCount\"", "AMS").Replace("\"EURCount\"", "EUR").Replace("\"EMEACount\"", "EMEA").Replace("\"", "\'");
            ReportListViewModel.GraphLine = GraphLine;
            //System.Diagnostics.Debug.WriteLine(GraphLine);
            return View("Index", ReportListViewModel);

        }

        public ActionResult Details(string id)
        {
            


            ReportListDetailsViewModel reportListDetailsViewModel = new ReportListDetailsViewModel();
            ReportBusinessLayer empBal = new ReportBusinessLayer();
            List<LogRecord> recordsords = empBal.GetRecords();
            reportListDetailsViewModel.Start = DateTime.Parse(GlobalVariables.Start).ToString("d");
            reportListDetailsViewModel.End = DateTime.Parse(GlobalVariables.End).ToString("d");

            List<ReportDetailsViewModel> reportViewModels = new List<ReportDetailsViewModel>();
            List<Employee> employees = new List<Employee>();
            employees = empBal.GetEmployees();
            employees = employees.Where(a => GlobalVariables.L1List.Contains(a.L1)).ToList();
            employees = employees.Where(a => GlobalVariables.L2List.Contains(a.L2)).ToList();
            employees = employees.Where(a => GlobalVariables.L3List.Contains(a.L3)).ToList();
            employees = employees.Where(a => GlobalVariables.L4List.Contains(a.L4)).ToList();
            employees = employees.Where(a => GlobalVariables.L5List.Contains(a.L5)).ToList();
            employees = employees.Where(a => GlobalVariables.RegionList.Contains(a.AreaCode)).ToList();
            employees = employees.Where(a => GlobalVariables.CountryList.Contains(a.Country)).ToList();
            employees = employees.Where(a => GlobalVariables.StatusList.Contains(a.Status)).ToList();

            reportListDetailsViewModel.TotalCount = recordsords.Count();
            var em = from m in recordsords
                       where m.Event1.Equals(id) && GlobalVariables.Start.CompareTo(DateTime.Parse(m.DateInclude).ToString("o").Substring(0, 10)) <= 0 && GlobalVariables.End.CompareTo(DateTime.Parse(m.DateInclude).ToString("o").Substring(0, 10)) >= 0
                       select m.Email;
            

            var empDetail = from m in em
                            join empl in employees on m equals empl.Email
                            group empl by empl.Email into grp
                            select new { Email = grp.Key, Cnt = grp.Count(), Code = grp.First().AreaCode, Cty = grp.First().Country};

            reportListDetailsViewModel.CurCount = em.Distinct().Count();
            foreach (var detail in empDetail)
            {
                ReportDetailsViewModel reportViewModel = new ReportDetailsViewModel();
                //reportViewModel.Id = detail.Id;
                reportViewModel.Email = detail.Email;
                reportViewModel.Country = detail.Cty;
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

            reportListDetailsViewModel.SelectedL1List = GlobalVariables.L1List;
            reportListDetailsViewModel.SelectedL2List = GlobalVariables.L2List;
            reportListDetailsViewModel.SelectedL3List = GlobalVariables.L3List;
            reportListDetailsViewModel.SelectedL4List = GlobalVariables.L4List;
            reportListDetailsViewModel.SelectedL5List = GlobalVariables.L5List;
            reportListDetailsViewModel.SelectedCountryList = GlobalVariables.CountryList;
            reportListDetailsViewModel.SelectedRegionList = GlobalVariables.RegionList;
            reportListDetailsViewModel.SelectedStatusList = GlobalVariables.StatusList;

            return View("Details", reportListDetailsViewModel);
        }



        public ActionResult DateRange()
        {
            if (Request.Form["StartDate"].Length != 0)
            {
                GlobalVariables.Start = DateTime.Parse(Request.Form["StartDate"]).ToString("o").Substring(0, 10);
            }
            if (Request.Form["EndDate"].Length != 0)
            {
                GlobalVariables.End = DateTime.Parse(Request.Form["EndDate"]).ToString("o").Substring(0, 10);
            }
            //string queryDate = "";
            //if (!Start.Equals(""))
            //{
            //    queryDate = Start + ".CompareTo(DateTime.Parse(m.DateInclude).ToString(\"o\").Substring(0, 10)) <= 0 &&" + End +
            //    ".CompareTo(DateTime.Parse(m.DateInclude).ToString(\"o\").Substring(0, 10)) >= 0)";
            //}

            
            ReportListViewModel ReportListViewModel = new ReportListViewModel();
            ReportBusinessLayer empBal = new ReportBusinessLayer();
            List<LogRecord> AllRecords = empBal.GetRecords();
            string Start = GlobalVariables.Start;
            string End = GlobalVariables.End;

            var Reports = from m in AllRecords

                          where (Start.CompareTo(DateTime.Parse(m.DateInclude).ToString("o").Substring(0, 10)) <= 0 && End.CompareTo(DateTime.Parse(m.DateInclude).ToString("o").Substring(0, 10)) >= 0)
                          select m;
            ReportListViewModel.Start = DateTime.Parse(Start).ToString("d");
            ReportListViewModel.End = DateTime.Parse(End).ToString("d");

            //System.Diagnostics.Debug.WriteLine(Reports.Count() + "count is");
            List<ReportViewModel> repViewModels = new List<ReportViewModel>();

            List<ReportViewModel> reportViewModels = new List<ReportViewModel>();

            List<Employee> employees = new List<Employee>();
            employees = empBal.GetEmployees();
            //System.Diagnostics.Debug.WriteLine("number of all employee " + Request.Form["L1"]);

            if (Request.Form.AllKeys.Contains("L1"))
            {
                string L1 = Request.Form["L1"];
                if (!L1.Equals(""))
                {
                    string[] L1Values = Regex.Split(L1, "(?<=^[^\"]*(?:\"[^\"]*\"[^\"]*)*),(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                    List<string> L1List = L1Values.OfType<string>().ToList();
                    employees = employees.Where(a => L1List.Contains(a.L1)).ToList();

                    GlobalVariables.L1List = L1List;
                    ReportListViewModel.SelectedL1List = L1List;
                }
            } else
            {
                GlobalVariables.L1List = GlobalVariables.Base.L1List;
                ReportListViewModel.SelectedL1List = GlobalVariables.Base.L1List;
            }

            if (Request.Form.AllKeys.Contains("L2"))
            {
                string L2 = Request.Form["L2"];
                if (!L2.Equals(""))
                {
                    string[] L2Values = Regex.Split(L2, "(?<=^[^\"]*(?:\"[^\"]*\"[^\"]*)*),(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                    List<string> L2List = L2Values.OfType<string>().ToList();
                    employees = employees.Where(a => L2List.Contains(a.L2)).ToList();

                    GlobalVariables.L2List = L2List;
                    ReportListViewModel.SelectedL2List = L2List;
                }
            }
            else
            {
                GlobalVariables.L2List = GlobalVariables.Base.L2List;
                ReportListViewModel.SelectedL2List = GlobalVariables.Base.L2List;
            }

            if (Request.Form.AllKeys.Contains("L3"))
            {
                string L3 = Request.Form["L3"];
                if (!L3.Equals(""))
                {
                    string[] L3Values = Regex.Split(L3, "(?<=^[^\"]*(?:\"[^\"]*\"[^\"]*)*),(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                    List<string> L3List = L3Values.OfType<string>().ToList();
                    employees = employees.Where(a => L3List.Contains(a.L3)).ToList();

                    GlobalVariables.L3List = L3List;
                    ReportListViewModel.SelectedL3List = L3List;
                }
            }
            else
            {
                GlobalVariables.L3List = GlobalVariables.Base.L3List;
                ReportListViewModel.SelectedL3List = GlobalVariables.Base.L3List;
            }

            if (Request.Form.AllKeys.Contains("L4"))
            {
                string L4 = Request.Form["L4"];
                if (!L4.Equals(""))
                {
                    string[] L4Values = Regex.Split(L4, "(?<=^[^\"]*(?:\"[^\"]*\"[^\"]*)*),(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                    List<string> L4List = L4Values.OfType<string>().ToList();
                    employees = employees.Where(a => L4List.Contains(a.L4)).ToList();

                    GlobalVariables.L4List = L4List;
                    ReportListViewModel.SelectedL4List = L4List;
                }
            }
            else
            {
                GlobalVariables.L4List = GlobalVariables.Base.L4List;
                ReportListViewModel.SelectedL4List = GlobalVariables.Base.L4List;
            }

            if (Request.Form.AllKeys.Contains("L5"))
            {
                string L5 = Request.Form["L5"];
                if (!L5.Equals(""))
                {
                    string[] L5Values = Regex.Split(L5, "(?<=^[^\"]*(?:\"[^\"]*\"[^\"]*)*),(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                    List<string> L5List = L5Values.OfType<string>().ToList();
                    employees = employees.Where(a => L5List.Contains(a.L5)).ToList();

                    GlobalVariables.L5List = L5List;
                    ReportListViewModel.SelectedL5List = L5List;
                }
            }
            else
            {
                GlobalVariables.L5List = GlobalVariables.Base.L5List;
                ReportListViewModel.SelectedL5List = GlobalVariables.Base.L5List;
            }

            if (Request.Form.AllKeys.Contains("Region"))
            {
                string Region = Request.Form["Region"];
                if (!Region.Equals(""))
                {
                    string[] RegionValues = Regex.Split(Region, "(?<=^[^\"]*(?:\"[^\"]*\"[^\"]*)*),(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                    List<string> RegionList = RegionValues.OfType<string>().ToList();
                    if (RegionList.Contains("EMEA"))
                    {
                        RegionList.Add("EUR");
                    }
                    employees = employees.Where(a => RegionList.Contains(a.AreaCode)).ToList();

                    GlobalVariables.RegionList = RegionList;
                    ReportListViewModel.SelectedRegionList = RegionList;
                }
            }
            else
            {
                GlobalVariables.RegionList = GlobalVariables.Base.RegionList;
                ReportListViewModel.SelectedRegionList = GlobalVariables.Base.RegionList;
            }

            if (Request.Form.AllKeys.Contains("Country"))
            {
                string Country = Request.Form["Country"];
                if (!Country.Equals(""))
                {
                    string[] CountryValues = Regex.Split(Country, "(?<=^[^\"]*(?:\"[^\"]*\"[^\"]*)*),(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                    List<string> CountryList = CountryValues.OfType<string>().ToList();
                    employees = employees.Where(a => CountryList.Contains(a.Country)).ToList();

                    GlobalVariables.CountryList = CountryList;
                    ReportListViewModel.SelectedCountryList = CountryList;
                }
            }
            else
            {
                GlobalVariables.CountryList = GlobalVariables.Base.CountryList;
                ReportListViewModel.SelectedCountryList = GlobalVariables.Base.CountryList;
            }

            if (Request.Form.AllKeys.Contains("Status"))
            {
                string Status = Request.Form["Status"];
                if (!Status.Equals(""))
                {
                    string[] StatusValues = Regex.Split(Status, "(?<=^[^\"]*(?:\"[^\"]*\"[^\"]*)*),(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                    List<string> StatusList = StatusValues.OfType<string>().ToList();
                    employees = employees.Where(a => StatusList.Contains(a.Status)).ToList();

                    GlobalVariables.StatusList = StatusList;
                    ReportListViewModel.SelectedStatusList = StatusList;
                }
            }
            else
            {
                GlobalVariables.StatusList = GlobalVariables.Base.StatusList;
                ReportListViewModel.SelectedStatusList = GlobalVariables.Base.StatusList;
            }

            Reports = from m in Reports
                      join e in employees on m.Email equals e.Email
                      select m;

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
                int EMEACount = 0;
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
                                EMEACount++;
                                break;
                            case "APJ":
                                APJCount++;
                                break;
                            case "EMEA":
                                EMEACount++;
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
                item.EMEACount = EMEACount;
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
                
                switch (item.code)
                {
                    case "AMS":
                        regionCodeCount.ElementAt(0).RegionCode = item.code;
                        regionCodeCount.ElementAt(0).Count = item.cnt;
                        upgroupedCount -= item.cnt;
                        break;
                    case "EMEA":
                        regionCodeCount.ElementAt(1).RegionCode = item.code;
                        regionCodeCount.ElementAt(1).Count += item.cnt;
                        upgroupedCount -= item.cnt;
                        break;
                    case "EUR":
                        //regionCodeCount.ElementAt(1).RegionCode = item.code;
                        regionCodeCount.ElementAt(1).Count += item.cnt;
                        upgroupedCount -= item.cnt;
                        break;
                    case "APJ":
                        regionCodeCount.ElementAt(2).RegionCode = item.code;
                        regionCodeCount.ElementAt(2).Count = item.cnt;
                        upgroupedCount -= item.cnt;
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
            List<CountryCount> EMEA = new List<CountryCount>();
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
                        EMEA.Add(cty);
                        break;
                    case "APJ":
                        asianPacific.Add(cty);
                        break;
                    case "EMEA":
                        EMEA.Add(cty);
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
            var orderedEMEA = from m in EMEA
                                     orderby m.Count descending
                                     select m;
            ReportListViewModel.America = orderedAmerica.ToList();
            ReportListViewModel.Europe = orderedEurope.ToList();
            ReportListViewModel.AsianPacific = orderedAsianPacfic.ToList();
            ReportListViewModel.EMEA = orderedEMEA.ToList();

            foreach (LogRecord emp in Reports)
            {
                ReportViewModel reportViewModel = new ReportViewModel();
                reportViewModel.Email = emp.Email;
                reportViewModel.Event1 = emp.Event1;
                reportViewModels.Add(reportViewModel);
            }
            ReportListViewModel.Records = reportViewModels;

            JavaScriptSerializer jss = new JavaScriptSerializer();
            List<CountryCount> Map = new List<CountryCount>();
            Map.AddRange(america);
            Map.AddRange(asianPacific);
            Map.AddRange(europe);
            Map.AddRange(EMEA);
            string output = jss.Serialize(Map);
            output = output.Replace("\"Country\"", "name").Replace("\"Count\"", "value").Replace("\"", "\'").Replace("\'Korea, Republic of\'", "South Korea");
            ReportListViewModel.json = output;
            ReportListViewModel.WorldMap = !output.Equals("");
            System.Diagnostics.Debug.WriteLine(output);

            string GraphLine = jss.Serialize(ReportListViewModel.DateCount);
            GraphLine = GraphLine.Replace("\"Date\"", "Date").Replace("\"APJCount\"", "APJ").Replace("\"AMSCount\"", "AMS").Replace("\"EURCount\"", "EUR").Replace("\"EMEACount\"", "EMEA").Replace("\"", "\'");
            ReportListViewModel.GraphLine = GraphLine;

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
