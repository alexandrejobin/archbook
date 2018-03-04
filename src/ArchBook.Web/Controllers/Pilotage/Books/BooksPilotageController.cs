using ArchBook.Services.Pilotage.Books;
using ArchBook.Services.Pilotage.Books.Dto;
using MTO.Framework.Web;
using MTO.Framework.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArchBook.Web.Controllers.Pilotage.Books
{
    [RouteArea("pilotage")]
    [RoutePrefix("books")]
    public class BooksPilotageController : Controller
    {
        private readonly BookPilotageService bookPilotageService;

        public BooksPilotageController(BookPilotageService bookPilotageService)
        {
            this.bookPilotageService = bookPilotageService;
        }

        [Route]
        public ActionResult Index()
        {
            var books = bookPilotageService.GetBooksIndexDto();

            return View(books);
        }

        [Route("add")]
        public JsonResult Add()
        {
            var bookFormDto = new BookFormDto();

            return GetBookFormBody(bookFormDto);
        }

        [Route("add")]
        [HttpPost]
        public JsonResult Add(BookFormDto bookFormDto)
        {
            if (ModelState.IsValid)
            {
                bookPilotageService.AddOrUpdateBook(bookFormDto);

                return Json(new JsonResult());
            }

            return GetBookFormBody(bookFormDto);
        }

        [Route("{bookId}/edit")]
        public JsonResult Edit(int bookId)
        {
            var bookFormDto = bookPilotageService.GetBookFormDto(bookId);

            return GetBookFormBody(bookFormDto);
        }

        [Route("{bookId}/edit")]
        [HttpPost]
        public JsonResult Edit(int bookId, BookFormDto bookFormDto)
        {
            if (ModelState.IsValid)
            {
                bookPilotageService.AddOrUpdateBook(bookFormDto, bookId);

                var bookIndexDto = bookPilotageService.GetBookIndexDto(bookId);
                var html = ViewRenderer.RenderPartialView("_Index_Book", bookIndexDto, this.ControllerContext);
                var data = new
                {
                    BookId = bookId,
                    Html = html
                };

                var jsonResponse = new JsonResponse(data);

                return Json(jsonResponse, JsonRequestBehavior.AllowGet);
            }

            return GetBookFormBody(bookFormDto);
        }

        [Route("{bookId}/edit/promotional-price")]
        public JsonResult EditPromotionalPrice(int bookId)
        {
            var promotionalPriceFormDto = bookPilotageService.GetPromotionalPriceFormDto(bookId);

            return GetPromotionalPriceFormBody(promotionalPriceFormDto);
        }

        [Route("{bookId}/edit/promotional-price")]
        [HttpPost]
        public JsonResult EditPromotionalPrice(int bookId, PromotionalPriceFormDto promotionalPriceFormDto)
        {
            if (ModelState.IsValid)
            {
                bookPilotageService.UpdatePromotionalPrice(promotionalPriceFormDto, bookId);

                var bookIndexDto = bookPilotageService.GetBookIndexDto(bookId);
                var html = ViewRenderer.RenderPartialView("_Index_Book", bookIndexDto, this.ControllerContext);
                var data = new
                {
                    BookId = bookId,
                    Html = html
                };

                var jsonResponse = new JsonResponse(data);

                return Json(jsonResponse, JsonRequestBehavior.AllowGet);
            }

            return GetPromotionalPriceFormBody(promotionalPriceFormDto);
        }

        private JsonResult GetBookFormBody(BookFormDto bookFormDto)
        {
            var bookFormReferenceDto = bookPilotageService.GetBookFormReferenceDto();

            ViewBag.Authors = bookFormReferenceDto.Authors;

            var view = ViewRenderer.RenderPartialView("_Modal_Book_Body", bookFormDto, this.ControllerContext);
            var jsonResponse = new JsonResponse(this.ModelState.IsValid, null, view);

            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }

        private JsonResult GetPromotionalPriceFormBody(PromotionalPriceFormDto promotionalPriceFormDto)
        {
            var view = ViewRenderer.RenderPartialView("_Modal_PromotionalPrice_Body", promotionalPriceFormDto, this.ControllerContext);
            var jsonResponse = new JsonResponse(this.ModelState.IsValid, null, view);

            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }
    }
}