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
        public List<string> GetAllAuthors(string path)
        {
            using (var repo = new Repository(@$"{path}"))   
            {
                return repo.Commits.Select(com => com.Author.Name).Distinct().ToList();
            }
        }

        public List<CreativityReportItem> GetCreativityReportItems(DateTime date, string userName, string path)
        {
            DateTime startDate = new DateTime(date.Year, date.Month, 1);
            DateTime endDate = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
            using (var repo = new Repository(path))
            {
                return repo.Commits
                    .Where(com => com.Author.Name == userName && 
                        com.Author.When > startDate && 
                        com.Author.When < endDate)
                    .Select(com => new CreativityReportItem 
                    { 
                        StartDate = com.Author.When.ToString("yyyy-MM-dd"), 
                        EndDate = com.Author.When.ToString("yyyy-MM-dd"),
                        ProjectName = repo.Info.WorkingDirectory.Split("\\")[^2],
                        CommitId = com.Sha, 
                        Comment = com.Message, 
                        UserName = com.Author.Name
                    }).ToList();
            }
        }

        public List<string> GetMergeCommitsIdsByAuthorAndDate(DateTime date, string userName, string path)
        {
            DateTime startDate = new DateTime(date.Year, date.Month, 1);
            DateTime endDate = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
            using (var repo = new Repository(path))
            {
                return repo.Commits
                    .Where(com => com.Author.Name == userName &&
                        com.Author.When > startDate &&
                        com.Author.When < endDate &&
                        com.Parents.Count() > 1)
                    .Select(com => com.Sha)
                    .ToList();
                    
            }
        }
    }
}
