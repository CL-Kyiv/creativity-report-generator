﻿using CreativityReportGenerator.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CreativityReportGenerator.Services.Abstractions
{
    public interface ICreativityReportGeneratorService
    {
        List<CreativityReportItem> GetCreativityReportItems(DateTime startDate, DateTime endDate, string userName);
    }
}
