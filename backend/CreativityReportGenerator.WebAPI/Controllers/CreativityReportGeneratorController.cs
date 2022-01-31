using CreativityReportGenerator.Core.Models;
using CreativityReportGenerator.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreativityReportGenerator.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CreativityReportGeneratorController : Controller
    {
        private readonly ICreativityReportGeneratorService _creativityReportGeneratorService;

        public CreativityReportGeneratorController(ICreativityReportGeneratorService creativityReportGeneratorService)
        {
            _creativityReportGeneratorService = creativityReportGeneratorService;
        }

        [HttpGet("authors")]
        public IActionResult GetAllAuthors(string path)
        {
            return Ok(_creativityReportGeneratorService.GetAllAuthors(path));
        }

        [HttpGet("mergeCommits")]
        public IActionResult GetMergeCommits(DateTime date, string userName, string path)
        {
            return Ok(_creativityReportGeneratorService.GetMergeCommitsIdsByAuthorAndDate(date, userName, path));
        }

        [HttpGet]
        public IActionResult GetCreativityReportItems(DateTime date, string userName, string path, int startWorkingHours, int endWorkingHours)
        {
            return Ok(_creativityReportGeneratorService.GetCreativityReportItems(date, userName, path, startWorkingHours, endWorkingHours));
        }
    }
}
