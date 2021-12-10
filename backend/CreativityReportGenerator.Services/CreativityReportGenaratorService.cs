using CreativityReportGenerator.Core.Models;
using CreativityReportGenerator.Services.Abstractions;
using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreativityReportGenerator.Services
{
    public class CreativityReportGenaratorService : ICreativityReportGeneratorService
    {
        public List<CreativityReportItem> GetCreativityReports(GetCreativityReportsRequestData requestData)
        {
            using (var repo = new Repository("D:\\Work\\Project\\creativity-report-generator"))
            {
                return repo.Commits
                    .Where(com => com.Author.Name == requestData.UserName && 
                        com.Author.When > requestData.StartDate && 
                        com.Author.When < requestData.EndDate)
                    .Select(com => new CreativityReportItem 
                        { 
                            StartDate = com.Author.When.ToString("yyyy-MM-dd"), 
                            EndDate = com.Author.When.ToString("yyyy-MM-dd"), 
                            CommitId = com.Sha, Comment = com.Message, 
                            UserName = requestData.UserName 
                        })
                    .ToList();
            }
        }
    }
}
