using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EmailReport.Models
{
    public class Employee
    {
        [Key]
        [Column("Employee ID")]
        public string EmpId { get; set; }
        [Column("E-Mail Address")]
        public string Email { get; set; }
        [Column("Legal Company Region Code")]
        public string AreaCode { get; set; }
        [Column("Payroll Country Code")]
        public string Country { get; set; }
    }
}