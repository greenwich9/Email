using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EmailReport.Models
{
    public class LogRecord
    {
        [Key]
        public int ID { get; set; }

        [Column("Email")]
        public string Email { get; set; }

        [Column("Event1")]
        public string Event1 { get; set; }
        [Column("DateInclude")]
        public string DateInclude { get; set; }
        
        public string Timestamp { get; set; }
        [Column("Url")]
        public string Url { get; set; }
        public string test { get; set; }
    }
}