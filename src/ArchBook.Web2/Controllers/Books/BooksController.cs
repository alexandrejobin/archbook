using ArchBook.Services.Books;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchBook.Web2.Controllers.Books
{
    [Route("books")]
    public class BooksController : Controller
    {
        private readonly BookService bookService;
        private readonly ILogger logger;

        public BooksController(BookService bookService, ILogger<BooksController> logger)
        {
            this.bookService = bookService;
            this.logger = logger;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            logger.LogInformation("Obtention des livres.");
            var books = bookService.GetBooksIndexDto();

            return View(books);
        }

        [HttpGet("{bookId:int}")]
        public IActionResult Details(int bookId)
        {
            var book = bookService.GetBookDetailsDto(bookId);

            return View(book);
        }
    }
}
