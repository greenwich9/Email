using EmailReport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmailReport.ViewModels;
using EmailReport.BusinessLayer;

namespace EmailReport.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult Index()
        {
            ReportListViewModel ReportListViewModel = new ReportListViewModel();
            ReportBusinessLayer empBal = new ReportBusinessLayer();
            List<LogRecord> Reports = empBal.GetRecords();
            List<ReportViewModel> empViewModels = new List<ReportViewModel>();

            List<Employee> areaInfo = new List<Employee>();
            areaInfo = empBal.GetAreaInfo();

            var eply = from m in Reports
                       select m;

            var email = from m in Reports
                        select m.Email;
            ReportListViewModel.UniqueUser = email.Distinct().Count();

            //var dateInfo = from m in Reports
            //               join region in areaInfo on m.email equals region.Email
            //               where m.Event1 == "open"
            //               orderby DateTime.Parse(m.DateInclude).Year.ToString() + "-" + DateTime.Parse(m.DateInclude).Month.ToString() + "-" + DateTime.Parse(m.DateInclude).Day.ToString()
            //               group m by new { date = DateTime.Parse(m.DateInclude).Year.ToString() + "-" + DateTime.Parse(m.DateInclude).Month.ToString() + "-" + DateTime.Parse(m.DateInclude).Day.ToString(), code = region.AreaCode } into grp
            //               select new { key = grp.Key, cnt = grp.Count() };

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
                                 join code in areaInfo on m.Email equals code.Email into ji
                                 from sub in ji.DefaultIfEmpty()
                                 select new { date = DateTime.Parse(m.DateInclude).Year.ToString() + "-" + DateTime.Parse(m.DateInclude).Month.ToString() + "-" + DateTime.Parse(m.DateInclude).Day.ToString(), region = sub?.AreaCode ?? String.Empty};
            List<DateCount> finalDateCount = new List<DateCount>();
            //System.Diagnostics.Debug.WriteLine("12t");
            //System.Diagnostics.Debug.WriteLine(dateCount.Count());
            foreach (var item in dateCount)
            {
                //System.Diagnostics.Debug.WriteLine("12t");
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
                                //System.Diagnostics.Debug.WriteLine(AMSCount);
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
                    //System.Diagnostics.Debug.WriteLine(AMSCount);
                   
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
                          orderby grp.Count() descending
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

            var open = eply.Where(s => s.Event1.Equals("open"));
            var click = eply.Where(s => s.Event1.Equals("click"));
            var processed = eply.Where(s => s.Event1.Equals("processed"));
            var delivered = eply.Where(s => s.Event1.Equals("delivered"));
            var deferred = eply.Where(s => s.Event1.Equals("deferred"));
            ReportListViewModel.OpenCount = open.Count();
            ReportListViewModel.ClickCount = click.Count();
            ReportListViewModel.ProcessedCount = processed.Count();
            ReportListViewModel.DeliveredCount = delivered.Count();
            ReportListViewModel.DeferredCount = deferred.Count();


            var regionCode = from m in Reports
                             where m.Event1 == "open"
                             join code in areaInfo on m.Email equals code.Email
                             group code by code.AreaCode into grp
                             select new { code = grp.Key, cnt = grp.Distinct().Count() };
            List<RegionCodeCount> regionCodeCount = new List<RegionCodeCount>();
            foreach (var item in regionCode)
            {
                RegionCodeCount region = new RegionCodeCount();
                region.RegionCode = item.code;
                region.Count = item.cnt;
                regionCodeCount.Add(region);
            }
            ReportListViewModel.RegionCodeCount = regionCodeCount;


            var countrySet = from m in Reports
                             where m.Event1 == "open"
                             join code in areaInfo on m.Email equals code.Email
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
            ReportListViewModel.America = america;
            ReportListViewModel.Europe = europe;
            ReportListViewModel.AsianPacific = asianPacific;

            foreach (LogRecord emp in Reports)
            {
                ReportViewModel empViewModel = new ReportViewModel();
                empViewModel.Id = emp.Id;
                empViewModel.Email = emp.Email;
                empViewModel.Event1 = emp.Event1;
                //empViewModel.Timestamp = emp.Timestamp;
                empViewModels.Add(empViewModel);
            }
            ReportListViewModel.Records = empViewModels;
            //ReportListViewModel.FooterData = new FooterViewModel();
            //ReportListViewModel.FooterData.CompanyName = "DYD Creative Solution";//Can be set to dynamic value
            //ReportListViewModel.FooterData.Year = DateTime.Now.Year.ToString();
            return View("Index", ReportListViewModel);

        }
    }
}