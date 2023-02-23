using CreativityReportGenerator.Core.Models;
using CreativityReportGenerator.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace CreativityReportGenerator.WebAPI.Controllers
{
    /// <summary>
    /// The bitbucket creativity report generator controller.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class BitbucketCreativityReportGeneratorController : Controller
    {
        private readonly IBitbucketCreativityReportGeneratorService _creativityReportGeneratorService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BitbucketCreativityReportGeneratorController" /> class.
        /// </summary>
        /// <param name="creativityReportGeneratorService">The bitbucket creativity report generator service.</param>
        public BitbucketCreativityReportGeneratorController(IBitbucketCreativityReportGeneratorService creativityReportGeneratorService)
        {
            _creativityReportGeneratorService = creativityReportGeneratorService;
        }

        /// <summary>
        /// Try to authorization.
        /// </summary>
        /// <param name="consumerKey">The consumer key.</param>
        /// <param name="consumerSecretKey">The consumer secret key.</param>
        [HttpGet("Authorization")]
        [ProducesResponseType(typeof(bool), Status200OK)]
        [ProducesResponseType(Status400BadRequest)]
        public IActionResult TryAuthorization([FromQuery] string consumerKey, [FromQuery] string consumerSecretKey)
        {
            try
            {
                _creativityReportGeneratorService.TryAuthorization(consumerKey, consumerSecretKey);
            }
            catch (Exception)
            {
                return BadRequest($"Failed to connect using these keys. Please make sure that the consumer key and consumer secret key are correct and try again");
            }

            return Ok(true);
        }

        /// <summary>
        /// Gets the authors.
        /// </summary>
        /// <param name="repositoryName">The repository name.</param>
        /// <param name="consumerKey">The consumer key.</param>
        /// <param name="consumerSecretKey">The consumer secret key.</param>
        /// <param name="date">The date of creativity report.</param>
        /// <returns>Authors.</returns>
        [HttpGet("authors")]
        [ProducesResponseType(typeof(List<string>), Status200OK)]
        public IActionResult GetAllAuthors(
            [FromQuery] string repositoryName, 
            [FromQuery] string consumerKey, 
            [FromQuery] string consumerSecretKey, 
            [FromQuery] DateTime date)
        {
            return Ok(_creativityReportGeneratorService
                .GetAllAuthors(repositoryName, consumerKey, consumerSecretKey, date));
        }

        /// <summary>
        /// Gets the repositories.
        /// </summary>
        /// <param name="consumerKey">The consumer key.</param>
        /// <param name="consumerSecretKey">The consumer secret key.</param>
        /// <returns>Repositories.</returns>
        [HttpGet("repositories")]
        [ProducesResponseType(typeof(List<string>), Status200OK)]
        public IActionResult GetAllRepositories([FromQuery] string consumerKey, [FromQuery] string consumerSecretKey)
        {
            return Ok(_creativityReportGeneratorService
                .GetAllRepositories(consumerKey, consumerSecretKey));
        }

        /// <summary>
        /// Gets the merge commits Ids.
        /// </summary>
        /// <param name="repositoryName">The repository name.</param>
        /// <param name="userName">The author.</param>
        /// <param name="consumerKey">The consumer key.</param>
        /// <param name="consumerSecretKey">The consumer secret key.</param>
        /// <param name="date">The date of creativity report.</param>
        /// <returns>Merge commits Ids.</returns>
        [HttpGet("mergeCommits")]
        [ProducesResponseType(typeof(List<string>), Status200OK)]
        public IActionResult GetMergeCommits(
            [FromQuery] DateTime date,
            [FromQuery] string userName,
            [FromQuery] string repositoryName,
            [FromQuery] string consumerKey,
            [FromQuery] string consumerSecretKey)
        {
            return Ok(_creativityReportGeneratorService
                .GetMergeCommitsIdsByAuthorAndDate(date, userName, repositoryName, consumerKey, consumerSecretKey));
        }

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
        [HttpGet]
        [ProducesResponseType(typeof(List<CreativityReportItem>), Status200OK)]
        public IActionResult GetCreativityReportItems(
            [FromQuery] DateTime date,
            [FromQuery] string userName,
            [FromQuery] string repositoryName,
            [FromQuery] string consumerKey,
            [FromQuery] string consumerSecretKey,
            [FromQuery] int startWorkingHours,
            [FromQuery] int endWorkingHours)
        {
            return Ok(_creativityReportGeneratorService
                .GetCreativityReportItems(date, userName, repositoryName, consumerKey, consumerSecretKey, startWorkingHours, endWorkingHours));
        }
    }
}
