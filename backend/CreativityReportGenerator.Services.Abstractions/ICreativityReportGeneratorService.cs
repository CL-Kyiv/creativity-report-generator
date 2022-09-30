using CreativityReportGenerator.Core.Models;
using System;
using System.Collections.Generic;

namespace CreativityReportGenerator.Services.Abstractions
{
    public interface ICreativityReportGeneratorService
    {
        string CurrentService { get; }
        List<CreativityReportItem> GetCreativityReportItems(DateTime date, string userName, string path, int startWorkingHours, int endWorkingHours);
        List<string> GetAllAuthors(string path, DateTime date);
        List<string> GetMergeCommitsIdsByAuthorAndDate(DateTime date, string userName, string path);
    }
}
