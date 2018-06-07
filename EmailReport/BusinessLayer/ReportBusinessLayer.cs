using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EmailReport.Models;

using EmailReport.DataAccessLayer;

namespace EmailReport.BusinessLayer
{
    public class ReportBusinessLayer
    {
        public List<LogRecord> GetRecords()
        {
            ReportsERPDAL ReportsDal = new ReportsERPDAL();
            return ReportsDal.Records.ToList();
        }

        public List<Employee> GetEmployees()
        {
            
            ReportsERPDAL ReportsDal = new ReportsERPDAL();
            return ReportsDal.Employees.ToList();
        }

        public LogRecord SaveReport(LogRecord e)
        {
            ReportsERPDAL saleERP = new ReportsERPDAL();
            saleERP.Records.Add(e);
            saleERP.SaveChanges();
            return e;
        }

        //public UserStatus GetUserValidity(UserDetails u)
        //{
        //    if (u.UserName == "Admin" && u.Password == "Admin")
        //    {
        //        return UserStatus.AuthenticatedAdmin;
        //    }
        //    else if (u.UserName == "Sukesh" && u.Password == "Sukesh")
        //    {
        //        return UserStatus.AuthentucatedUser;
        //    }
        //    else
        //    {
        //        return UserStatus.NonAuthenticatedUser;
        //    }
        //}

        public void UploadRecords(List<LogRecord> Reports)
        {
            ReportsERPDAL ReportsDal = new ReportsERPDAL();
            ReportsDal.Records.AddRange(Reports);
            ReportsDal.SaveChanges();
        }

        public void UploadEmployee(List<Employee> employees)
        {
            ReportsERPDAL ReportsDal = new ReportsERPDAL();
            ReportsDal.Employees.AddRange(employees);
            ReportsDal.SaveChanges();
        }

        public void UploadReportsRegionInfo(List<Employee> employees)
        {
            ReportsERPDAL ReportsDal = new ReportsERPDAL();
            ReportsDal.Employees.AddRange(employees);
            ReportsDal.SaveChanges();
        }
    }
}