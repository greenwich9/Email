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
        public int Id { get; set; }
        [Column("Email")]
        public string Email { get; set; }
        public string L1 { get; set; }
        public string L2 { get; set; }
        public string L3 { get; set; }
        public string L4 { get; set; }
        public string L5 { get; set; }
        [Column("Region")]
        public string AreaCode { get; set; }
        [Column("Country")]
        public string Country { get; set; }
        public string Status { get; set; }
    }
}