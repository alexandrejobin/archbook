using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchBook.Services.Pilotage.Books.Dto
{
    public class BookFormReferenceDto
    {
        public IEnumerable<KeyValuePair<int, string>> Authors { get; set; }
    }
}
