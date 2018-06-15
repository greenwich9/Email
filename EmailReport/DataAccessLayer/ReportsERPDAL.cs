using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

using EmailReport.Models;

namespace EmailReport.DataAccessLayer
{

    public class ReportsERPDAL : DbContext
    {
        public DbSet<LogRecord> Records { get; set; }
        public DbSet<Employee> Employees { get; set; }
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{

        //    base.OnModelCreating(modelBuilder);
        //}
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LogRecord>().ToTable("newsletter_log");
            modelBuilder.Entity<Employee>().ToTable("employee");
            base.OnModelCreating(modelBuilder);
        }
    }
}