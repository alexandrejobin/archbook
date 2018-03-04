using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ArchBook.Web.Controllers.Home
{
    [RoutePrefix("")]
    public class HomeController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        [Route]
        public ActionResult Index()
        {
            return View();
        }

        [Route("about")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            throw new HttpException(
                (int)HttpStatusCode.NotFound,
                "This 404 exception was deliberately thrown from an action method.");

            return View();
        }

        [Route("contact")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Route("download")]
        public ActionResult FailedFileDownload()
        {
            return new FileStreamResult(new UnreadableStream(), "image/jpeg");
        }

        private class UnreadableStream : Stream
        {
            public override int Read(byte[] buffer, int offset, int count)
            {
                throw new Exception("This exception was deliberately thrown from within a FileStreamResult.");
            }

            public override void Flush()
            {
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                return offset;
            }

            public override void SetLength(long value)
            {
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
            }

            public override bool CanRead
            {
                get { return false; }
            }

            public override bool CanSeek
            {
                get { return false; }
            }

            public override bool CanWrite
            {
                get { return false; }
            }

            public override long Length
            {
                get { return 1; }
            }

            public override long Position { get; set; }
        }
    }
}