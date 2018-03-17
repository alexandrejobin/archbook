using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ArchBook.Web.Controllers.Home
{
    public class ValuesController : ApiController
    {
        // GET api/<controller>/5
        public object Get()
        {
            return new { ContactId = 1, Name = "test" };
        }
    }
}