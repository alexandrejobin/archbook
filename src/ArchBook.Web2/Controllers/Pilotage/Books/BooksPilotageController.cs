using ArchBook.Services.Pilotage.Books;
using ArchBook.Services.Pilotage.Books.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Net;

namespace ArchBook.Web2.Controllers.Pilotage.Books
{
    [Route("pilotage/books")]
    public class BooksPilotageController : Controller
    {
        private readonly BookPilotageService bookPilotageService;
        private readonly IViewRendererService viewRendererService;

        public BooksPilotageController(BookPilotageService bookPilotageService, IViewRendererService viewRendererService)
        {
            this.bookPilotageService = bookPilotageService;
            this.viewRendererService = viewRendererService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var books = bookPilotageService.GetBooksIndexDto();

            return View("views/pilotage/books/index.cshtml", books);
        }

        [HttpGet("add")]
        public IActionResult Add()
        {
            var bookFormDto = new BookFormDto();

            return GetBookFormBody(bookFormDto);
        }

        [HttpPost("add")]
        public IActionResult Add(BookFormDto bookFormDto)
        {
            if (ModelState.IsValid)
            {
                bookPilotageService.AddOrUpdateBook(bookFormDto);

                return Ok();
            }

            return GetBookFormBody(bookFormDto);
        }

        [HttpGet("{bookId:int}/edit")]
        public JsonResult Edit(int bookId)
        {
            var bookFormDto = bookPilotageService.GetBookFormDto(bookId);

            return GetBookFormBody(bookFormDto);
        }

        [Route("{bookId:int}/edit")]
        [HttpPost]
        public JsonResult Edit(int bookId, BookFormDto bookFormDto)
        {
            if (ModelState.IsValid)
            {
                bookPilotageService.AddOrUpdateBook(bookFormDto, bookId);

                var bookIndexDto = bookPilotageService.GetBookIndexDto(bookId);
                var html = PartialView("views/pilotage/books/_Index_Book.cshtml", bookIndexDto).Render(this.HttpContext);
                var data = new
                {
                    BookId = bookId,
                    Html = html
                };

                return Json(data);
            }

            return GetBookFormBody(bookFormDto);
        }

        [Route("{bookId:int}/edit/promotional-price")]
        public JsonResult EditPromotionalPrice(int bookId)
        {
            var promotionalPriceFormDto = bookPilotageService.GetPromotionalPriceFormDto(bookId);

            return GetPromotionalPriceFormBody(promotionalPriceFormDto);
        }

        [Route("{bookId:int}/edit/promotional-price")]
        [HttpPost]
        public JsonResult EditPromotionalPrice(int bookId, PromotionalPriceFormDto promotionalPriceFormDto)
        {
            if (ModelState.IsValid)
            {
                bookPilotageService.UpdatePromotionalPrice(promotionalPriceFormDto, bookId);

                var bookIndexDto = bookPilotageService.GetBookIndexDto(bookId);
                var html = PartialView("views/pilotage/books/_Index_Book.cshtml", bookIndexDto).Render(this.HttpContext);
                var data = new
                {
                    BookId = bookId,
                    Html = html
                };

                return Json(data);
            }

            return GetPromotionalPriceFormBody(promotionalPriceFormDto);
        }

        private JsonResult GetBookFormBody(BookFormDto bookFormDto)
        {
            var bookFormReferenceDto = bookPilotageService.GetBookFormReferenceDto();

            ViewBag.Authors = bookFormReferenceDto.Authors;

            var html = PartialView("views/pilotage/books/_Modal_Book_Body.cshtml", bookFormDto).Render(this.HttpContext);

            Response.StatusCode = ModelState.IsValid ? (int)HttpStatusCode.OK : (int)HttpStatusCode.BadRequest;
            return Json(html);
        }

        private JsonResult GetPromotionalPriceFormBody(PromotionalPriceFormDto promotionalPriceFormDto)
        {
            var html = PartialView("views/pilotage/books/_Modal_PromotionalPrice_Body.cshtml", promotionalPriceFormDto).Render(this.HttpContext);

            Response.StatusCode = ModelState.IsValid ? (int)HttpStatusCode.OK : (int)HttpStatusCode.BadRequest;
            return Json(html);
        }
    }
}
