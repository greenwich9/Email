using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using EmailReport.Models;
using EmailReport.BusinessLayer;
using EmailReport.ViewModels;
using System.IO;
using System.Text.RegularExpressions;
using System.Data.Entity;

namespace EmailReport.Controllers
{
    public class BulkUploadController : Controller
    {
        // GET: BulkUpload
        public ActionResult Index()
        {
            FileUploadViewModel fileUpload = new FileUploadViewModel();

            fileUpload.Start = DateTime.Parse(GlobalVariables.Start).ToString("d");
            fileUpload.End = DateTime.Parse(GlobalVariables.End).ToString("d");
            fileUpload.L1List = GlobalVariables.Base.L1List;
            fileUpload.L2List = GlobalVariables.Base.L2List;
            fileUpload.L3List = GlobalVariables.Base.L3List;
            fileUpload.L4List = GlobalVariables.Base.L4List;
            fileUpload.L5List = GlobalVariables.Base.L5List;
            fileUpload.CountryList = GlobalVariables.Base.CountryList;
            fileUpload.RegionList = GlobalVariables.Base.RegionList;
            fileUpload.StatusList = GlobalVariables.Base.StatusList;
            return View(fileUpload);
        }

        public ActionResult Upload(FileUploadViewModel model)
        {
            List<LogRecord> records = GetRecords(model);
            ReportBusinessLayer bal = new ReportBusinessLayer();
            bal.UploadRecords(records);
            return RedirectToAction("Index", "Report");
        }
        public ActionResult UploadEmployeeInfo(FileUploadViewModel model)
        {
            List<Employee> employees = GetEmployees(model);
            ReportBusinessLayer bal = new ReportBusinessLayer();
            
            bal.UploadEmployee(employees);
            return RedirectToAction("Index", "Report");
        }

        private List<LogRecord> GetRecords(FileUploadViewModel model)
        {
            List<LogRecord> employees = new List<LogRecord>();
            StreamReader csvreader = new StreamReader(model.file.InputStream);
            csvreader.ReadLine(); // Assuming first line is header
            System.Diagnostics.Debug.WriteLine("begin");
            while (!csvreader.EndOfStream)
            {
                var line = csvreader.ReadLine();
                string[] values = Regex.Split(line, "(?<=^[^\"]*(?:\"[^\"]*\"[^\"]*)*),(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                //var values = line.Split(',');//Values are comma separated
                if (values.Count() == 0 || values[0].Equals("")) break;
                if (!values[0].Equals("DataInclude"))
                {
                    
                    LogRecord e = new LogRecord();
                    
                    e.DateInclude = values[0];
                    e.Email = values[1].ToLower();
                    e.Event1 = values[2];
                    e.Url = values[8];
                    employees.Add(e);
                }
            }
            return employees;
        }

        private List<Employee> GetEmployees(FileUploadViewModel model)
        {
            List<Employee> employeesInfo = new List<Employee>();
            StreamReader csvreader = new StreamReader(model.file.InputStream);
            csvreader.ReadLine(); // Assuming first line is header
            int i = 0;
            while (!csvreader.EndOfStream)
            {
                i++;
                var line = csvreader.ReadLine();
                string[] values = Regex.Split(line, "(?<=^[^\"]*(?:\"[^\"]*\"[^\"]*)*),(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                //var values = line.Split(',', "(?= ([^\\\"]*\\\"[^\\\"]*\\\")*[^\\\"]*$)");//Values are comma separated
                if (values.Count() != 15)
                {
                    System.Diagnostics.Debug.WriteLine(values.Count());
                    System.Diagnostics.Debug.WriteLine(i);
                    foreach (var v in values)
                    {
                        System.Diagnostics.Debug.Write(v + "--");
                    }
                }

                if (!values[0].Equals("Email"))
                {
                    Employee e = new Employee();
                    e.Email = values[0].ToLower();
                    e.L1 = values[3];
                    e.L2 = values[4];
                    e.L3 = values[5];
                    e.L4 = values[6];
                    e.L5 = values[7];
                    e.AreaCode = values[8];
                    e.Country = values[9];
                    e.Status = values[11];
                    employeesInfo.Add(e);
                }
            }
            return employeesInfo;
        }
    }
}