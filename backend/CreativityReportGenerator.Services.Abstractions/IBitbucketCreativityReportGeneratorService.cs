using CreativityReportGenerator.Core.Models;
using System;
using System.Collections.Generic;

namespace CreativityReportGenerator.Services.Abstractions
{
    /// <summary>
    /// Bitbucket Creativity report generator service
    /// </summary>
    public interface IBitbucketCreativityReportGeneratorService
    {
        /// <summary>
        /// Gets creativity report items.
        /// </summary>
        /// <param name="date">The date of creativity report.</param>
        /// <param name="userName">The author.</param>
        /// <param name="repositoryName">The repository name.</param>
        /// <param name="consumerKey">The consumer key.</param>
        /// <param name="consumerSecretKey">The consumer secret key.</param>
        /// <param name="startWorkingHours">Working day start time.</param>
        /// <param name="endWorkingHours">Working day end time.</param>
        /// <returns>Creativity report items.</returns>
        List<CreativityReportItem> GetCreativityReportItems(
            DateTime date,
            string userName,
            string? repositoryName,
            string? consumerKey,
            string? consumerSecretKey,
            int startWorkingHours,
            int endWorkingHours);

        /// <summary>
        /// Gets the authors.
        /// </summary>
        /// <param name="repositoryName">The repository name.</param>
        /// <param name="consumerKey">The consumer key.</param>
        /// <param name="consumerSecretKey">The consumer secret key.</param>
        /// <param name="date">The date of creativity report.</param>
        /// <returns>Authors.</returns>
        List<string> GetAllAuthors(string? repositoryName, string? consumerKey, string? consumerSecretKey, DateTime date);

        /// <summary>
        /// Gets the repositories.
        /// </summary>
        /// <param name="consumerKey">The consumer key.</param>
        /// <param name="consumerSecretKey">The consumer secret key.</param>
        /// <returns>Repositories.</returns>
        List<string> GetAllRepositories(string? consumerKey, string? consumerSecretKey);

        /// <summary>
        /// Try to authorization.
        /// </summary>
        /// <param name="consumerKey">The consumer key.</param>
        /// <param name="consumerSecretKey">The consumer secret key.</param>
        void TryAuthorization(string? consumerKey, string? consumerSecretKey);

        /// <summary>
        /// Gets the merge commits Ids.
        /// </summary>
        /// <param name="repositoryName">The repository name.</param>
        /// <param name="userName">The author.</param>
        /// <param name="consumerKey">The consumer key.</param>
        /// <param name="consumerSecretKey">The consumer secret key.</param>
        /// <param name="date">The date of creativity report.</param>
        /// <returns>Merge commits Ids.</returns>
        List<string> GetMergeCommitsIdsByAuthorAndDate(
            DateTime date,
            string userName,
            string? repositoryName,
            string? consumerKey,
            string? consumerSecretKey);
    }
}
