using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchBook.Services.Books.Dto
{
    public class BookDetailsReviewDto
    {
        public int ReviewId { get; set; }

        public int Rating { get; set; }

        public string VoterName { get; set; }

        public string Comment { get; set; }
    }
}
