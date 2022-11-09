using CreativityReportGenerator.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CreativityReportGenerator.WebAPI.Controllers
{
    /// <summary>
    /// The creativity report generator controller.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class CreativityReportGeneratorController : Controller
    {
        private AppSettings AppSettings { get; set; }

        private readonly IEnumerable<ICreativityReportGeneratorService> _creativityReportGeneratorServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractsController" /> class.
        /// </summary>
        /// <param name="creativityReportGeneratorServices">The creativity report generator service.</param>
        /// <param name="settings">The application settings.</param>
        public CreativityReportGeneratorController(IEnumerable<ICreativityReportGeneratorService> creativityReportGeneratorServices, IOptions<AppSettings> settings)
        {
            _creativityReportGeneratorServices = creativityReportGeneratorServices;

            AppSettings = settings.Value;
        }

        /// <summary>
        /// Gets the authors.
        /// </summary>
        /// <param name="path">The path to repository.</param>
        /// <param name="date">The date of creativity report.</param>
        /// <returns>Authors.</returns>
        [HttpGet("authors")]
        public IActionResult GetAllAuthors(string? path, string? repositoryName, string? consumerKey, string? consumerSecretKey, DateTime date)
        {
            return Ok(_creativityReportGeneratorServices
                .FirstOrDefault(s => s.CurrentService == AppSettings.CurrentService)
                .GetAllAuthors(path, repositoryName, consumerKey, consumerSecretKey, date));
        }

        /// <summary>
        /// Gets the authors.
        /// </summary>
        /// <param name="path">The path to repository.</param>
        /// <param name="date">The date of creativity report.</param>
        /// <returns>Authors.</returns>
        [HttpGet("repositories")]
        public IActionResult GetAllRepositories(string? consumerKey, string? consumerSecretKey)
        {
            return Ok(_creativityReportGeneratorServices
                .FirstOrDefault(s => s.CurrentService == AppSettings.CurrentService)
                .GetAllRepositories(consumerKey, consumerSecretKey));
        }   

        /// <summary>
        /// Gets the merge commits Ids.
        /// </summary>
        /// <param name="date">The date of creativity report.</param>
        /// <param name="userName">The author.</param>
        /// <param name="path">The path to repository.</param>
        /// <returns>Merge commits Ids.</returns>
        [HttpGet("mergeCommits")]
        public IActionResult GetMergeCommits(
            DateTime date,
            string userName,
            string? repositoryName,
            string? path,
            string? consumerKey,
            string? consumerSecretKey)
        {
            return Ok(_creativityReportGeneratorServices
                .FirstOrDefault(s => s.CurrentService == AppSettings.CurrentService)
                .GetMergeCommitsIdsByAuthorAndDate(date, userName, repositoryName, path, consumerKey, consumerSecretKey));
        }

        /// <summary>
        /// Gets creativity report items.
        /// </summary>
        /// <param name="date">The date of creativity report.</param>
        /// <param name="userName">The author.</param>
        /// <param name="path">The path to repository.</param>
        /// <param name="startWorkingHours">Working day start time.</param>
        /// <param name="endWorkingHours">Working day end time.</param>
        /// <returns>Creativity report items.</returns>
        [HttpGet]
        public IActionResult GetCreativityReportItems(
            DateTime date,
            string userName,
            string? repositoryName,
            string? path,
            string? consumerKey,
            string? consumerSecretKey,
            int startWorkingHours,
            int endWorkingHours)
        {
            return Ok(_creativityReportGeneratorServices
                .FirstOrDefault(s => s.CurrentService == AppSettings.CurrentService)
                .GetCreativityReportItems(date, userName, repositoryName, path, consumerKey, consumerSecretKey, startWorkingHours, endWorkingHours));
        }
    }
}
