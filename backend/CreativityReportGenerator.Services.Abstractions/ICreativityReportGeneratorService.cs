using CreativityReportGenerator.Core.Models;
using System;
using System.Collections.Generic;

namespace CreativityReportGenerator.Services.Abstractions
{
    public interface ICreativityReportGeneratorService
    {
        List<CreativityReportItem> GetCreativityReportItems(DateTime date, string userName, string path, int startWorkingHours, int endWorkingHours);
        List<Author> GetAllAuthors(string path);
        List<string> GetMergeCommitsIdsByAuthorAndDate(DateTime date, string userName, string path);
    }
}
