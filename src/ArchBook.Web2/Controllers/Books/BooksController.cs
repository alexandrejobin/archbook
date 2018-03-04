using ArchBook.Services.Books;
using Microsoft.AspNetCore.Mvc;
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

        public BooksController(BookService bookService)
        {
            this.bookService = bookService;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
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
