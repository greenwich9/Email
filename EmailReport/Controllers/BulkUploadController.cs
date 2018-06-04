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

namespace EmailReport.Controllers
{
    public class BulkUploadController : Controller
    {
        // GET: BulkUpload
        public ActionResult Index()
        {
            FileUploadViewModel fileUpload = new FileUploadViewModel();

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
            StreamReader csvreader = new StreamReader(model.FileUpload.InputStream);
            csvreader.ReadLine(); // Assuming first line is header
            while (!csvreader.EndOfStream)
            {
                var line = csvreader.ReadLine();
                string[] values = Regex.Split(line, "(?<=^[^\"]*(?:\"[^\"]*\"[^\"]*)*),(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                //var values = line.Split(',');//Values are comma separated
                if (!values[0].Equals("Id"))
                {
                    LogRecord e = new LogRecord();
                    e.Id = values[0];
                    e.DateInclude = values[1];
                    e.Email = values[2];
                    e.Event1 = values[3];
                    e.Url = values[9];
                    employees.Add(e);
                }
            }
            return employees;
        }

        private List<Employee> GetEmployees(FileUploadViewModel model)
        {
            List<Employee> employeesInfo = new List<Employee>();
            StreamReader csvreader = new StreamReader(model.FileUpload.InputStream);
            csvreader.ReadLine(); // Assuming first line is header
            int i = 0;
            while (!csvreader.EndOfStream)
            {
                i++;
                var line = csvreader.ReadLine();
                string[] values = Regex.Split(line, "(?<=^[^\"]*(?:\"[^\"]*\"[^\"]*)*),(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                //var values = line.Split(',', "(?= ([^\\\"]*\\\"[^\\\"]*\\\")*[^\\\"]*$)");//Values are comma separated
                if (values.Count() != 13)
                {
                    System.Diagnostics.Debug.WriteLine(values.Count());
                    System.Diagnostics.Debug.WriteLine(i);
                    foreach (var v in values)
                    {
                        System.Diagnostics.Debug.Write(v + "--");
                    }
                }

                if (!values[0].Equals("Employee ID"))
                {
                    Employee e = new Employee();
                    e.EmpId = values[0];
                    e.Email = values[1];
                    e.AreaCode = values[9];
                    e.Country = values[12];
                    employeesInfo.Add(e);
                }
            }
            return employeesInfo;
        }
    }
}