using CreativityReportGenerator.Core.Models;
using CreativityReportGenerator.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using static Microsoft.AspNetCore.Http.StatusCodes;

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
        [ProducesResponseType(typeof(List<string>), Status200OK)]
        public IActionResult GetAllAuthors([FromQuery] string path, [FromQuery] DateTime date)
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
        [ProducesResponseType(typeof(List<string>), Status200OK)]
        public IActionResult GetMergeCommits(
            [FromQuery] DateTime date,
            [FromQuery] string userName,
            [FromQuery] string path)
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
        [ProducesResponseType(typeof(List<CreativityReportItem>), Status200OK)]
        public IActionResult GetCreativityReportItems(
            [FromQuery] DateTime date,
            [FromQuery] string userName,
            [FromQuery] string path,
            [FromQuery] int startWorkingHours,
            [FromQuery] int endWorkingHours)
        {
            return Ok(_creativityReportGeneratorService
                .GetCreativityReportItems(date, userName, path, startWorkingHours, endWorkingHours));
        }
    }
}
