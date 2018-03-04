using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchBook.Data.Domain
{
    public class Book : IValidatableObject
    {
        public const int TitleMaxLength = 100;
        public const int DescriptionMaxLength = 250;
        public const int PublisherMaxLength = 64;

        public Book()
        {
            this.Authors = new HashSet<Author>();
            this.Reviews = new HashSet<Review>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookId { get; set; }

        [Required]
        [StringLength(TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [StringLength(DescriptionMaxLength)]
        public string Description { get; set; }

        [MaxLength(PublisherMaxLength)]
        public string Publisher { get; set; }

        public decimal Price { get; set; }

        public DateTime? PublishedOn { get; set; }

        public virtual ICollection<Author> Authors { get; set; }

        public virtual PriceOffer Promotion { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!this.Authors.Any())
            {
                yield return new ValidationResult("Au moins un auteur doit être défini pour le livre.", new List<string>() { "Authors" });
            }
        }
    }
}
