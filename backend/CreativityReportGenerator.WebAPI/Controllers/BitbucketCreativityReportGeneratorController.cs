using CreativityReportGenerator.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System;

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
        public IActionResult TryAuthorization(string? consumerKey, string? consumerSecretKey)
        {
            try
            {
                _creativityReportGeneratorService.TryAuthorization(consumerKey, consumerSecretKey);
            }
            catch (Exception)
            {
                return BadRequest($"Failed to connect using these keys.");
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
        public IActionResult GetAllAuthors(string? repositoryName, string? consumerKey, string? consumerSecretKey, DateTime date)
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
        public IActionResult GetAllRepositories(string? consumerKey, string? consumerSecretKey)
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
        public IActionResult GetMergeCommits(
            DateTime date,
            string userName,
            string? repositoryName,
            string? consumerKey,
            string? consumerSecretKey)
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
        public IActionResult GetCreativityReportItems(
            DateTime date,
            string userName,
            string? repositoryName,
            string? consumerKey,
            string? consumerSecretKey,
            int startWorkingHours,
            int endWorkingHours)
        {
            return Ok(_creativityReportGeneratorService
                .GetCreativityReportItems(date, userName, repositoryName, consumerKey, consumerSecretKey, startWorkingHours, endWorkingHours));
        }
    }
}
