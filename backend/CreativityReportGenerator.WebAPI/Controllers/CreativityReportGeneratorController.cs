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

        [HttpGet]
        public IActionResult GetCreativityReportItems(DateTime startDate, DateTime endDate, string userName)
        {
            return Ok(_creativityReportGeneratorService.GetCreativityReportItems(startDate, endDate, userName));
        }
    }
}
