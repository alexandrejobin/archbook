using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchBook.Web2.Controllers.Errors
{
    [Route("errors")]
    public class ErrorsController : Controller
    {
        [HttpGet("404")]
        public IActionResult Error404()
        {
            return View();
        }

        [HttpGet("{code:int}")]
        public IActionResult Error(int code)
        {
            // handle different codes or just return the default error view
            return View();
        }
    }
}
