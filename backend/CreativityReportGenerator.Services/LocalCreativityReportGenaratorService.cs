﻿using CreativityReportGenerator.Core.Models;
using CreativityReportGenerator.Services.Abstractions;
using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CreativityReportGenerator.Services
{
    /// <summary>
    /// Local creativity report genaratorService.
    /// </summary>
    /// <seealso cref="ILocalCreativityReportGenaratorService" />
    public class LocalCreativityReportGenaratorService : BaseCreativityReportGenaratorService, ILocalCreativityReportGenaratorService
    {
        public List<string> GetAllAuthors(string path, DateTime date)
        {
            using (var repo = new Repository(@$"{path}"))
            {
                return GetCommitsByDate(repo, date)
                    .OrderBy(com => com.Author.Name)
                    .Select(com => $"{com.Author.Name} <{com.Author.Email}>")
                    .Distinct()
                    .ToList();
            }
        }

        public List<CreativityReportItem> GetCreativityReportItems(
            DateTime date,
            string userName,
            string path,
            int startWorkingHours,
            int endWorkingHours)
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

        public List<string> GetMergeCommitsIdsByAuthorAndDate(
            DateTime date,
            string userName,
            string path)
        {
            using (var repo = new Repository(path))
            {
                return GetAllCommitsByAuthorAndDate(repo, date, userName)
                    .Where(com => com.Parents.Count() > 1)
                    .Select(com => com.Sha)
                    .ToList();      
            }
        }

        /// <summary>
        /// Gets commits by author and date.
        /// </summary>
        /// <param name="repo">The repository.</param>
        /// <param name="date">The date of creativity report.</param>
        /// <param name="userName">The author.</param>
        /// <returns>Commits.</returns>
        private List<Commit> GetAllCommitsByAuthorAndDate(Repository repo, DateTime date, string userName)
        {
            var commitsByDate = GetCommitsByDate(repo, date);

            return commitsByDate
                .Where(com => $"{com.Author.Name} <{com.Author.Email}>" == userName)
                .OrderBy(com => com.Author.When)
                .GroupBy(com => com.Sha)
                .Select(id => id.FirstOrDefault())
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
        public int CalculateCreativeTime(Commit com, Commit previousCom, int startWorkingHours, int endWorkingHours)
        {
            double hours = 0;

            if (com.Parents.Count() > 1)
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
                hours = GetTimeDifferenceBetweenCommits(com.Author.When, previousCom.Author.When, startWorkingHours, endWorkingHours);
            }

            return (int)Math.Round(hours / 2, MidpointRounding.AwayFromZero);  
        }

        /// <summary>
        /// Gets commits by date.
        /// </summary>
        /// <param name="repo">The repository.</param>
        /// <param name="date">The date of creativity report.</param>
        /// <returns>Commits.</returns>
        private List<Commit> GetCommitsByDate(Repository repo, DateTime date)
        {
            DateTime startDate = new DateTime(date.Year, date.Month, 1);
            DateTime endDate = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));

            return repo.Branches
                .Where(br => br.IsRemote)
                .SelectMany(x => x.Commits)
                .Where(com =>
                com.Author.When >= startDate &&
                com.Author.When <= endDate).ToList();
        }
    }
}
