using CreativityReportGenerator.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CreativityReportGenerator.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CreativityReportGeneratorController : Controller
    {
        private AppSettings AppSettings { get; set; }

        private readonly IEnumerable<ICreativityReportGeneratorService> _creativityReportGeneratorServices;

        public CreativityReportGeneratorController(IEnumerable<ICreativityReportGeneratorService> creativityReportGeneratorServices, IOptions<AppSettings> settings)
        {
            _creativityReportGeneratorServices = creativityReportGeneratorServices;

            AppSettings = settings.Value;
        }

        [HttpGet("authors")]
        public IActionResult GetAllAuthors(string? path, DateTime date)
        {
            //if (!Directory.Exists(path))
            //{
            //    return BadRequest("wrong path");
            //}

            return Ok(_creativityReportGeneratorServices
                .FirstOrDefault(s => s.CurrentService == AppSettings.CurrentService)
                .GetAllAuthors(path, date));
        }

        [HttpGet("mergeCommits")]
        public IActionResult GetMergeCommits(DateTime date, string userName, string path)
        {
            return Ok(_creativityReportGeneratorServices
                .FirstOrDefault(s => s.CurrentService == AppSettings.CurrentService)
                .GetMergeCommitsIdsByAuthorAndDate(date, userName, path));
        }

        [HttpGet]
        public IActionResult GetCreativityReportItems(DateTime date, string userName, string path, int startWorkingHours, int endWorkingHours)
        {
            return Ok(_creativityReportGeneratorServices
                .FirstOrDefault(s => s.CurrentService == AppSettings.CurrentService)
                .GetCreativityReportItems(date, userName, path, startWorkingHours, endWorkingHours));
        }
    }
}
