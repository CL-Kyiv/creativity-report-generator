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
        public IActionResult GetAllAuthors(string path, DateTime date)
        {
            if (!Directory.Exists(path))
            {
                return BadRequest("wrong path");
            }
            
            return Ok(_creativityReportGeneratorServices
                .FirstOrDefault(s => s.CurrentService == AppSettings.CurrentService)
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
        public IActionResult GetMergeCommits(DateTime date, string userName, string path)
        {
            return Ok(_creativityReportGeneratorServices
                .FirstOrDefault(s => s.CurrentService == AppSettings.CurrentService)
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
        public IActionResult GetCreativityReportItems(DateTime date, string userName, string path, int startWorkingHours, int endWorkingHours)
        {
            return Ok(_creativityReportGeneratorServices
                .FirstOrDefault(s => s.CurrentService == AppSettings.CurrentService)
                .GetCreativityReportItems(date, userName, path, startWorkingHours, endWorkingHours));
        }
    }
}
