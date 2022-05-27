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
    public class AppSettingsController : Controller
    {
        private readonly IAppSettingsService _appSettingsService;

        public AppSettingsController(IAppSettingsService appSettingsService, I)
        {
            _appSettingsService = appSettingsService;
        }

        [HttpPut]
        public IActionResult ChangeCurrentService()
        {
            _appSettingsService.ChangeCurrentService("");
            return Ok();
        }
    }
}
