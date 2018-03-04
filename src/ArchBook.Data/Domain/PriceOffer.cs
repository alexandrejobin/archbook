using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchBook.Data.Domain
{
    public class PriceOffer
    {
        public const int PromotionalTextMaxLength = 200;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BookId { get; set; }

        public decimal NewPrice { get; set; }

        [StringLength(PromotionalTextMaxLength)]
        public string PromotionalText { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}