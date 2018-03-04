using ArchBook.Data;
using ArchBook.Data.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchBook.Services.Books
{
    public class BookService
    {
        private readonly BookDbContext bookDbContext;

        public BookService(BookDbContext bookDbContext)
        {
            this.bookDbContext = bookDbContext;
        }

        public IEnumerable<Dto.BookListDto> GetBooksIndexDto()
        {
            return bookDbContext.Books
                    .AsNoTracking()
                    .OrderBy(x => x.Title)
                    .ThenBy(x => x.BookId)
                    .MapToBookListDto()
                    .ToList();
        }

        public Dto.BookDetailsDto GetBookDetailsDto(int bookId)
        {
            return bookDbContext.Books
                    .AsNoTracking()
                    .Where(x => x.BookId == bookId)
                    .MapToBookDetailsDto()
                    .Single();                    
        }
    }
}
