using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchBook.Services.Pilotage.Books.Dto
{
    public class BookFormDto : IValidatableObject
    {
        public BookFormDto()
        {
            this.AuthorIds = new HashSet<int>();
        }

        [Required]
        [StringLength(Data.Domain.Book.TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [StringLength(Data.Domain.Book.DescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        public decimal? Price { get; set; }

        public IEnumerable<int> AuthorIds { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!this.AuthorIds.Any())
            {
                yield return new ValidationResult("Au moins un auteur doit être défini pour le livre.", new List<string>() { "AuthorIds" });
            }
        }
    }
}
