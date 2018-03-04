using ArchBook.Data;
using ArchBook.Data.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchBook.Services.Pilotage.Books
{
    public class BookPilotageService
    {
        private readonly BookDbContext bookDbContext;

        public BookPilotageService(BookDbContext bookDbContext)
        {
            this.bookDbContext = bookDbContext;
        }

        public IEnumerable<Dto.BookIndexDto> GetBooksIndexDto()
        {
            return bookDbContext.Books
                    .AsNoTracking()
                    .OrderBy(x => x.Title)
                    .ThenBy(x => x.BookId)
                    .MapToBookIndexDto()
                    .ToList();
        }

        public Dto.BookIndexDto GetBookIndexDto(int bookId)
        {
            return bookDbContext.Books
                    .AsNoTracking()
                    .Where(x => x.BookId == bookId)
                    .MapToBookIndexDto()
                    .Single();
        }

        public Dto.BookFormDto GetBookFormDto(int bookId)
        {
            return bookDbContext.Books
                    .Where(x => x.BookId == bookId)
                    .MapToBookFormDto()
                    .Single();
        }

        public Dto.BookFormReferenceDto GetBookFormReferenceDto()
        {
            var bookFormReferenceDto = new Dto.BookFormReferenceDto();

            bookFormReferenceDto.Authors = bookDbContext.Authors
                                                .Select(x => new { x.AuthorId, x.Name })
                                                .AsEnumerable()
                                                .Select(x => new KeyValuePair<int, string>(x.AuthorId, x.Name))
                                                .ToList();

            return bookFormReferenceDto;
        }

        public Dto.PromotionalPriceFormDto GetPromotionalPriceFormDto(int bookId)
        {
            return bookDbContext.Books
                    .Where(x => x.BookId == bookId)
                    .MapToPromotionalPriceFormDto()
                    .Single();
        }

        public void AddOrUpdateBook(Dto.BookFormDto bookFormDto, int? bookId = null)
        {
            Book book;

            if (bookId.HasValue)
            {
                book = bookDbContext.Books
                        .Include(x => x.Authors)
                        .Single(x => x.BookId == bookId.Value);
            }
            else
            {
                book = new Book();
                bookDbContext.Books.Add(book);
            }

            book.Title = bookFormDto.Title;
            book.Description = bookFormDto.Description;
            book.Price = bookFormDto.Price.Value;

            var authorsToDelete = book.Authors.Where(x => !bookFormDto.AuthorIds.Contains(x.AuthorId)).ToList();
            var authorIdsToAdd = bookFormDto.AuthorIds.Where(authorId => !book.Authors.Any(x => x.AuthorId == authorId)).ToList();
            var authorsToAdd = bookDbContext.Authors.Where(x => authorIdsToAdd.Contains(x.AuthorId)).ToList();

            authorsToDelete.ForEach(deletedAuthor => book.Authors.Remove(deletedAuthor));
            authorsToAdd.ForEach(addedAuthor => book.Authors.Add(addedAuthor));

            bookDbContext.SaveChanges();
        }

        public void UpdatePromotionalPrice(Dto.PromotionalPriceFormDto promotionalPriceFormDto, int bookId)
        {
            var priceOffer = bookDbContext.Books
                                .Where(x => x.BookId == bookId)
                                .Select(x => x.Promotion)
                                .SingleOrDefault();

            if (priceOffer == null)
            {
                priceOffer = new PriceOffer();
                priceOffer.BookId = bookId;

                bookDbContext.PricesOffers.Add(priceOffer);
            }

            priceOffer.NewPrice = promotionalPriceFormDto.Price.Value;
            priceOffer.StartDate = promotionalPriceFormDto.StartDate.Value;
            priceOffer.EndDate = promotionalPriceFormDto.EndDate.Value;

            bookDbContext.SaveChanges();
        }
    }
}
