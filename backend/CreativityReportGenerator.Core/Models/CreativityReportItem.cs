using System;
using System.Collections.Generic;
using System.Text;

namespace CreativityReportGenerator.Core.Models
{
    public class CreativityReportItem
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string CommitId { get; set; }
        public string Comment { get; set; }
        public string UserName { get; set; }
    }
}
