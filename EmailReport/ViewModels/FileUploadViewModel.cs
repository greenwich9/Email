using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmailReport.ViewModels
{
    public class FileUploadViewModel
    {
        public HttpPostedFileBase file { get; set; }
    }
}