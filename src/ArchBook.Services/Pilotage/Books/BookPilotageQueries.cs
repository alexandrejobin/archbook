using ArchBook.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchBook.Services.Pilotage.Books
{
    static class BookPilotageQueries
    {
        public static IQueryable<Dto.BookIndexDto> MapToBookIndexDto(this IQueryable<Book> books)
        {
            return books.Select(book => new Dto.BookIndexDto()
            {
                BookId = book.BookId,
                Title = book.Title,
                PublishedDate = book.PublishedOn,
                Authors = book.Authors.OrderBy(x => x.Name).Select(x => x.Name),
                Price = book.Price,
                PromotionalPrice = book.Promotion.NewPrice
            });
        }

        public static IQueryable<Dto.BookFormDto> MapToBookFormDto(this IQueryable<Book> books)
        {
            return books.Select(book => new Dto.BookFormDto()
            {
                Title = book.Title,
                Description = book.Description,
                Price = book.Price,
                AuthorIds = book.Authors.Select(author => author.AuthorId)
            });
        }

        public static IQueryable<Dto.PromotionalPriceFormDto> MapToPromotionalPriceFormDto(this IQueryable<Book> books)
        {
            return books.Select(book => new Dto.PromotionalPriceFormDto()
            {
                Price = book.Promotion.NewPrice,
                StartDate = book.Promotion.StartDate,
                EndDate = book.Promotion.EndDate
            });
        }
    }
}
