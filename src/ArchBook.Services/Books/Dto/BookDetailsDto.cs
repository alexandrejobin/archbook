using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchBook.Services.Books.Dto
{
    public class BookDetailsDto
    {
        public int BookId { get; set; }

        public string Title { get; set; }

        public IEnumerable<BookDetailsReviewDto> Reviews { get; set; }
    }
}
