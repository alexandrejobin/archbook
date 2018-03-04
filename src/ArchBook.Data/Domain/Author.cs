using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchBook.Data.Domain
{
    public class Author
    {
        public const int NameMaxLength = 100;

        public Author()
        {
            this.Books = new HashSet<Book>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AuthorId { get; set; }

        [Required]
        [StringLength(NameMaxLength)]
        public string Name { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
