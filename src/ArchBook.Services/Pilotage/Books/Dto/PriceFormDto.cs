using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchBook.Services.Pilotage.Books.Dto
{
    public class PromotionalPriceFormDto
    {
        [Required]
        public decimal? Price { get; set; }

        [Required]
        public DateTime? StartDate { get; set; }

        [Required]
        public DateTime? EndDate { get; set; }
    }
}
