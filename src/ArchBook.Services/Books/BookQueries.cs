using ArchBook.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchBook.Services.Books
{
    static class BookQueries
    {
        public static IQueryable<Dto.BookListDto> MapToBookListDto(this IQueryable<Book> books)
        {
            return books.Select(book => new Dto.BookListDto()
            {
                BookId = book.BookId,
                Title = book.Title,
                PublishedDate = book.PublishedOn,
                Authors = book.Authors.OrderBy(x => x.Name).Select(x => x.Name),
                Price = book.Price,
                PromotionalPrice = book.Promotion.NewPrice,
                Rating = book.Reviews.Average(x => x.Rating),
                RatingCount = book.Reviews.Count()
            });
        }

        public static IQueryable<Dto.BookDetailsDto> MapToBookDetailsDto(this IQueryable<Book> books)
        {
            return books.Select(book => new Dto.BookDetailsDto()
            {
                BookId = book.BookId,
                Title = book.Title,
                Reviews = book.Reviews.Select(review => new Dto.BookDetailsReviewDto()
                {
                    ReviewId = review.ReviewId,
                    Rating = review.Rating,
                    VoterName = review.VoterName,
                    Comment = review.Comment
                })
            });
        }
    }
}
