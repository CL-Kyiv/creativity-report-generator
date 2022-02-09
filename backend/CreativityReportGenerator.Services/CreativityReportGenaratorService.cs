using CreativityReportGenerator.Core.Models;
using CreativityReportGenerator.Services.Abstractions;
using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CreativityReportGenerator.Services
{
    public class CreativityReportGenaratorService : ICreativityReportGeneratorService
    {
        public List<Author> GetAllAuthors(string path)
        {
            using (var repo = new Repository(@$"{path}"))   
            {
                return repo.Branches
                    .SelectMany(x => x.Commits)
                    .OrderBy(com => com.Author.Name)
                    .Select(com => new Author
                        { 
                            Name = com.Author.Name,
                            Email = com.Author.Email
                        })
                    .Distinct()
                    .ToList();
            }
        }

        public List<CreativityReportItem> GetCreativityReportItems(DateTime date, string userName, string path, int startWorkingHours, int endWorkingHours)
        {
            using (var repo = new Repository(path))
            {
                var allCommits = GetAllCommitsByAuthorAndDate(repo, date, userName);

                var commitsWithoutMergeCommits = allCommits.Where(com => (com.Parents.Count() < 2));

                var linkedListWithoutMergeCommits = new LinkedList<Commit>(commitsWithoutMergeCommits);

                return allCommits
                    .Select(com => new CreativityReportItem
                    {
                        StartDate = com.Author.When.ToString("yyyy-MM-dd"),
                        EndDate = com.Author.When.ToString("yyyy-MM-dd"),
                        ProjectName = repo.Info.WorkingDirectory.Split("\\")[^2],
                        CommitId = com.Sha,
                        Comment = com.Message,
                        UserName = com.Author.Name,
                        Hours = CalculateCreativeTime(com, linkedListWithoutMergeCommits.Find(com)?.Previous?.Value, startWorkingHours, endWorkingHours)
                    }).ToList();
            }
        }

        public List<string> GetMergeCommitsIdsByAuthorAndDate(DateTime date, string userName, string path)
        {
            using (var repo = new Repository(path))
            {
                return GetAllCommitsByAuthorAndDate(repo, date, userName)
                    .Where(com => com.Parents.Count() > 1)
                    .Select(com => com.Sha)
                    .ToList();      
            }
        }

        private List<Commit> GetAllCommitsByAuthorAndDate(Repository repo, DateTime date, string userName)
        {
            DateTime startDate = new DateTime(date.Year, date.Month, 1);
            DateTime endDate = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));

            return repo.Branches
                .Where(br => br.IsRemote)
                .SelectMany(x => x.Commits)
                .Where(com => com.Author.Name == userName &&
                    com.Author.When >= startDate &&
                    com.Author.When <= endDate)
                .OrderBy(com => com.Author.When)
                .GroupBy(com => com.Sha)
                .Select(id => id.FirstOrDefault())
                .ToList();
        }

        public int CalculateCreativeTime(Commit com, Commit previousCom, int startWorkingHours, int endWorkingHours)
        {
            double hours = 0;

            if (com.Parents.Count() > 1)
            {
                return (int)hours;
            }

            if (previousCom == null)
            { 
                hours = endWorkingHours - startWorkingHours;
            }
            else
            {
                var start = previousCom.Author.When;
                var end = com.Author.When;

                while (start < end)
                {
                    start = start.AddHours(1);
                    if (start.Hour > startWorkingHours &&
                       start.Hour < endWorkingHours &&
                       start.DayOfWeek != DayOfWeek.Saturday &&
                       start.DayOfWeek != DayOfWeek.Sunday)
                    {
                        hours++;
                    }
                }
            }
            return (int)Math.Round(hours / 2, MidpointRounding.AwayFromZero);
        }
    }
}
