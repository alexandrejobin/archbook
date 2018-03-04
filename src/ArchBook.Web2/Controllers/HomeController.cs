using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ArchBook.Web2.Models;
using ArchBook.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ArchBook.Web2.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        ILogger logger;

        public HomeController(ILogger<HomeController> logger)
        {
            this.logger = logger;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("about")]
        public IActionResult About()
        {
            throw new Exception("problème dans la page About");
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [HttpGet("contact")]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [HttpGet("error")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
