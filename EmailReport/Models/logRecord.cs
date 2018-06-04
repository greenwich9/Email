using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmailReport.Models
{
    public class LogRecord
    {
        [Key]
        public string Id { get; set; }

        public string Event1 { get; set; }

        public string DateInclude { get; set; }

        public string Timestamp { get; set; }

        public string Email { get; set; }

        public string Url { get; set; }
    }
}