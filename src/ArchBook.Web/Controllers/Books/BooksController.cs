﻿using ArchBook.Services.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ArchBook.Services.Books.Dto;
using MTO.Framework.Web.Mvc;
using MTO.Framework.Web;
using NLog;

namespace ArchBook.Web.Controllers.Books
{
    [RoutePrefix("books")]
    public class BooksController : Controller
    {
        private readonly BookService bookService;
        private readonly ILogger logger = NLog.LogManager.GetCurrentClassLogger();

        public BooksController(BookService bookService)
        {
            this.bookService = bookService;
        }

        [Route]
        public ActionResult Index()
        {
            logger.Info("Obtention des livres.");
            var books = bookService.GetBooksIndexDto();

            return View(books);
        }

        [Route("{bookId}")]
        public ActionResult Details(int bookId)
        {
            var book = bookService.GetBookDetailsDto(bookId);

            return View(book);
        }        
    }
}