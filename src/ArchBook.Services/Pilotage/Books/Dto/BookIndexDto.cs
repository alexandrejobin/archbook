using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchBook.Services.Pilotage.Books.Dto
{
    public class BookIndexDto
    {
        public int BookId { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public decimal? PromotionalPrice { get; set; }

        public DateTime? PublishedDate { get; set; }

        public IEnumerable<string> Authors { get; set; }
    }
}
