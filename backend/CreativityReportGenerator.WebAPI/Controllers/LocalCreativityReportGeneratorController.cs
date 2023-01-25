using CreativityReportGenerator.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CreativityReportGenerator.WebAPI.Controllers
{
    /// <summary>
    /// The local creativity report generator controller.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class LocalCreativityReportGeneratorController : Controller
    {
        private readonly ILocalCreativityReportGenaratorService _creativityReportGeneratorService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalCreativityReportGeneratorController" /> class.
        /// </summary>
        /// <param name="creativityReportGeneratorService">The local creativity report generator service.</param>
        public LocalCreativityReportGeneratorController(ILocalCreativityReportGenaratorService creativityReportGeneratorService)
        {
            _creativityReportGeneratorService = creativityReportGeneratorService;
        }

        /// <summary>
        /// Gets the authors.
        /// </summary>
        /// <param name="path">The path to repository.</param>
        /// <param name="date">The date of creativity report.</param>
        /// <returns>Authors.</returns>
        [HttpGet("authors")]
        public IActionResult GetAllAuthors(string? path, DateTime date)
        {
            return Ok(_creativityReportGeneratorService
                .GetAllAuthors(path, date));
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
            string? path)
        {
            return Ok(_creativityReportGeneratorService
                .GetMergeCommitsIdsByAuthorAndDate(date, userName, path));
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
            string? path,
            int startWorkingHours,
            int endWorkingHours)
        {
            return Ok(_creativityReportGeneratorService
                .GetCreativityReportItems(date, userName, path, startWorkingHours, endWorkingHours));
        }
    }
}
