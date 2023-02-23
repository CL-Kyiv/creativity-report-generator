using CreativityReportGenerator.Core.Models;
using System;
using System.Collections.Generic;

namespace CreativityReportGenerator.Services.Abstractions
{
    /// <summary>
    /// Local Creativity report generator service
    /// </summary>
    public interface ILocalCreativityReportGenaratorService
    {
        /// <summary>
        /// Gets creativity report items.
        /// </summary>
        /// <param name="date">The date of creativity report.</param>
        /// <param name="userName">The author.</param>
        /// <param name="path">The path to repository.</param>
        /// <param name="startWorkingHours">Working day start time.</param>
        /// <param name="endWorkingHours">Working day end time.</param>
        /// <returns>Creativity report items.</returns>
        List<CreativityReportItem> GetCreativityReportItems(
            DateTime date,
            string userName,
            string path,
            int startWorkingHours,
            int endWorkingHours);

        /// <summary>
        /// Gets the authors.
        /// </summary>
        /// <param name="path">The path to repository.</param>
        /// <param name="date">The date of creativity report.</param>
        /// <returns>Authors.</returns>
        List<string> GetAllAuthors(string path, DateTime date);

        /// <summary>
        /// Gets the merge commits Ids.
        /// </summary>
        /// <param name="date">The date of creativity report.</param>
        /// <param name="userName">The author.</param>
        /// <param name="path">The path to repository.</param>
        /// <returns>Merge commits Ids.</returns>
        List<string> GetMergeCommitsIdsByAuthorAndDate(
            DateTime date,
            string userName,
            string path);
    }
}
