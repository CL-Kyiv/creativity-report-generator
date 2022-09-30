using CreativityReportGenerator.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SharpBucket.V2;
using CreativityReportGenerator.Core.Models;

namespace CreativityReportGenerator.Services
{
    public class BitbucketCreativityReportGeneratorService : ICreativityReportGeneratorService
    {
        public string CurrentService => nameof(BitbucketCreativityReportGeneratorService);

        public List<string> GetAllAuthors(string path, DateTime date)
        {
            var sharpBucket = new SharpBucketV2();
            sharpBucket.OAuth2ClientCredentials("uBK78CejQr9ghaUrYL", "62DPtXaJQmP9cQTE3NUe6ScYWa9bUjRz");
            var repo = sharpBucket.RepositoriesEndPoint()
                .RepositoriesResource("dinikul").RepositoryResource("test");
            return GetCommitsByDate(repo, date)
                .OrderBy(com => com.author.raw)
                .Select(com => com.author.raw)
                .Distinct()
                .ToList();

        }

        private List<SharpBucket.V2.Pocos.Commit> GetCommitsByDate(SharpBucket.V2.EndPoints.RepositoryResource repo, DateTime date)
        {
            DateTime startDate = new DateTime(date.Year, date.Month, 1);
            DateTime endDate = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
           
            return repo.ListCommits()
                .Where(com =>
                Convert.ToDateTime(com.date) >= startDate &&
                Convert.ToDateTime(com.date) <= endDate).ToList();
        }

        public List<CreativityReportItem> GetCreativityReportItems(DateTime date, string userName, string path, int startWorkingHours, int endWorkingHours)
        {
            var sharpBucket = new SharpBucketV2();
            sharpBucket.OAuth2ClientCredentials("uBK78CejQr9ghaUrYL", "62DPtXaJQmP9cQTE3NUe6ScYWa9bUjRz");
            var repo = sharpBucket.RepositoriesEndPoint()
                .RepositoriesResource("dinikul").RepositoryResource("test");

            var allCommits = GetAllCommitsByAuthorAndDate(repo, date, userName);

            var commitsWithoutMergeCommits = allCommits.Where(com => (com.parents.Count() < 2));

            var linkedListWithoutMergeCommits = new LinkedList<SharpBucket.V2.Pocos.Commit>(commitsWithoutMergeCommits);

            return allCommits
                    .Select(com => new CreativityReportItem
                    {
                        StartDate = Convert.ToDateTime(com.date).ToString("yyyy-MM-dd"),
                        EndDate = Convert.ToDateTime(com.date).ToString("yyyy-MM-dd"),
                        ProjectName = "test",
                        CommitId = com.hash,
                        Comment = com.message,
                        UserName = com.author.user.display_name,
                        Hours = CalculateCreativeTime(com, linkedListWithoutMergeCommits.Find(com)?.Previous?.Value, startWorkingHours, endWorkingHours)
                    }).ToList();
        }

        private List<SharpBucket.V2.Pocos.Commit> GetAllCommitsByAuthorAndDate(SharpBucket.V2.EndPoints.RepositoryResource repo, DateTime date, string userName)
        {
            var commitsByDate = GetCommitsByDate(repo, date);

            return commitsByDate
                .Where(com => com.author.raw == userName)
                .OrderBy(com => Convert.ToDateTime(com.date))
                .GroupBy(com => com.hash)
                .Select(id => id.FirstOrDefault())
                .ToList();
        }

        public List<string> GetMergeCommitsIdsByAuthorAndDate(DateTime date, string userName, string path)
        {
            var sharpBucket = new SharpBucketV2();
            sharpBucket.OAuth2ClientCredentials("uBK78CejQr9ghaUrYL", "62DPtXaJQmP9cQTE3NUe6ScYWa9bUjRz");
            var repo = sharpBucket.RepositoriesEndPoint()
                .RepositoriesResource("dinikul").RepositoryResource("test");

            return GetAllCommitsByAuthorAndDate(repo, date, userName)
                    .Where(com => com.parents.Count() > 1)
                    .Select(com => com.hash)
                    .ToList();
        }

        public int CalculateCreativeTime(SharpBucket.V2.Pocos.Commit com, SharpBucket.V2.Pocos.Commit previousCom, int startWorkingHours, int endWorkingHours)
        {
            double hours = 0;

            if (com.parents.Count() > 1)
            {
                return (int)hours;
            }

            if (previousCom == null)
            {
                hours = endWorkingHours - startWorkingHours;
            }
            else
            {
                var start = Convert.ToDateTime(previousCom.date);
                var end = Convert.ToDateTime(com.date);

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
