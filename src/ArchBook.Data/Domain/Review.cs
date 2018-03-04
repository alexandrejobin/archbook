using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchBook.Data.Domain
{
    public class Review
    {
        public const int VoterNameMaxLength = 100;
        public const int CommentMaxLength = 2000;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReviewId { get; set; }

        [Required]
        [StringLength(VoterNameMaxLength)]
        public string VoterName { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [StringLength(CommentMaxLength)]
        public string Comment { get; set; }

        public int BookId { get; set; }

        public virtual Book Book { get; set; }
    }
}