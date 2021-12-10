using System;
using System.Collections.Generic;
using System.Text;

namespace CreativityReportGenerator.Core.Models
{
    public class GetCreativityReportsRequestData
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string UserName { get; set; }
    }
}
