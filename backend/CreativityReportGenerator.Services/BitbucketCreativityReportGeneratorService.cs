using CreativityReportGenerator.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using SharpBucket.V2;
using CreativityReportGenerator.Core.Models;

namespace CreativityReportGenerator.Services
{
    /// <summary>
    /// Bitbucket creativity report genaratorService.
    /// </summary>
    /// <seealso cref="IBitbucketCreativityReportGeneratorService" />
    public class BitbucketCreativityReportGeneratorService : BaseCreativityReportGenaratorService, IBitbucketCreativityReportGeneratorService
    {
        public void TryAuthorization(string consumerKey, string consumerSecretKey)
        {
            var sharpBucket = new SharpBucketV2();
            sharpBucket.OAuth2ClientCredentials(consumerKey, consumerSecretKey);
        }

        public List<string> GetAllRepositories(string consumerKey, string consumerSecretKey)
        {
            var sharpBucket = new SharpBucketV2();
            sharpBucket.OAuth2ClientCredentials(consumerKey, consumerSecretKey);

            var user = sharpBucket.UserEndPoint().GetUser();

            var repositories = sharpBucket.RepositoriesEndPoint()
                .RepositoriesResource(user.username).ListRepositories();

            return repositories
                .OrderBy(repo => repo.name)
                .Select(repo => repo.name)
                .ToList();
        }

        public List<string> GetAllAuthors(string repositoryName, string consumerKey, string consumerSecretKey, DateTime date)
        {
            var sharpBucket = new SharpBucketV2();
            sharpBucket.OAuth2ClientCredentials(consumerKey, consumerSecretKey);

            var user = sharpBucket.UserEndPoint().GetUser();

            var repo = sharpBucket.RepositoriesEndPoint()
                .RepositoriesResource(user.username).RepositoryResource(repositoryName);
            return GetCommitsByDate(repo, date)
                .OrderBy(com => com.author.raw)
                .Select(com => com.author.raw)
                .Distinct()
                .ToList();

        }

        /// <summary>
        /// Gets commits by date.
        /// </summary>
        /// <param name="repo">The repository.</param>
        /// <param name="date">The date of creativity report.</param>
        /// <returns>Commits.</returns>
        private List<SharpBucket.V2.Pocos.Commit> GetCommitsByDate(SharpBucket.V2.EndPoints.RepositoryResource repo, DateTime date)
        {
            DateTime startDate = new DateTime(date.Year, date.Month, 1);
            DateTime endDate = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
           
            return repo.ListCommits()
                .Where(com =>
                Convert.ToDateTime(com.date) >= startDate &&
                Convert.ToDateTime(com.date) <= endDate).ToList();
        }

        public List<CreativityReportItem> GetCreativityReportItems(
            DateTime date, 
            string userName, 
            string repositoryName,
            string consumerKey, 
            string consumerSecretKey, 
            int startWorkingHours, 
            int endWorkingHours)
        {
            var sharpBucket = new SharpBucketV2();
            sharpBucket.OAuth2ClientCredentials(consumerKey, consumerSecretKey);

            var user = sharpBucket.UserEndPoint().GetUser();

            var repo = sharpBucket.RepositoriesEndPoint()
                .RepositoriesResource(user.username).RepositoryResource(repositoryName);

            var allCommits = GetAllCommitsByAuthorAndDate(repo, date, userName);

            var commitsWithoutMergeCommits = allCommits.Where(com => (com.parents.Count() < 2));

            var linkedListWithoutMergeCommits = new LinkedList<SharpBucket.V2.Pocos.Commit>(commitsWithoutMergeCommits);

            return allCommits
                    .Select(com => new CreativityReportItem
                    {
                        StartDate = Convert.ToDateTime(com.date).ToString("yyyy-MM-dd"),
                        EndDate = Convert.ToDateTime(com.date).ToString("yyyy-MM-dd"),
                        ProjectName = repositoryName,
                        CommitId = com.hash,
                        Comment = com.message,
                        UserName = com.author.user.display_name,
                        Hours = CalculateCreativeTime(com, linkedListWithoutMergeCommits.Find(com)?.Previous?.Value, startWorkingHours, endWorkingHours)
                    }).ToList();
        }

        /// <summary>
        /// Gets commits by author and date.
        /// </summary>
        /// <param name="repo">The repository.</param>
        /// <param name="date">The date of creativity report.</param>
        /// <param name="userName">The author.</param>
        /// <returns>Commits.</returns>
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

        public List<string> GetMergeCommitsIdsByAuthorAndDate(
            DateTime date,
            string userName,
            string repositoryName,
            string consumerKey,
            string consumerSecretKey)
        {
            var sharpBucket = new SharpBucketV2();
            sharpBucket.OAuth2ClientCredentials(consumerKey, consumerSecretKey);

            var user = sharpBucket.UserEndPoint().GetUser();

            var repo = sharpBucket.RepositoriesEndPoint()
                .RepositoriesResource(user.username).RepositoryResource(repositoryName);

            return GetAllCommitsByAuthorAndDate(repo, date, userName)
                    .Where(com => com.parents.Count() > 1)
                    .Select(com => com.hash)
                    .ToList();
        }

        /// <summary>
        /// Calculate creative time.
        /// </summary>
        /// <param name="com">Current commit.</param>
        /// <param name="previousCom">Previous commit.</param>
        /// <param name="startWorkingHours">Working day start time.</param>
        /// <param name="endWorkingHours">Working day end time.</param>
        /// <returns>Creative time.</returns>
        public int CalculateCreativeTime(SharpBucket.V2.Pocos.Commit com, SharpBucket.V2.Pocos.Commit previousCom, int startWorkingHours, int endWorkingHours)
        {
            double hours = 0;

            if (com.parents.Count() > 1)
            {
                return (int)hours;
            }

            int workingHoursPerDay = CalculateWorkingTimePerDay(startWorkingHours, endWorkingHours);

            if (previousCom == null)
            {
                hours = workingHoursPerDay;
            }
            else
            {
                hours = GetTimeDifferenceBetweenCommits(DateTimeOffset.Parse(com.date), DateTimeOffset.Parse(previousCom.date), startWorkingHours, endWorkingHours);
            }

            return (int)Math.Round(hours / 2, MidpointRounding.AwayFromZero);
        }
    }
}
