using CreativityReportGenerator.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace CreativityReportGenerator.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BitbucketAccountController : Controller
    {
        private readonly IBitbucketAccountService _bitbucketAccountService;

        public BitbucketAccountController(IBitbucketAccountService bitbucketAccountService)
        {
            _bitbucketAccountService = bitbucketAccountService;
        }

        [HttpGet]
        public IActionResult GetCreativityReportItems()
        {
            return Ok(_bitbucketAccountService.GetRepositories());
        }
    }
}
