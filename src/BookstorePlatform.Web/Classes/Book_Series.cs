using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookstorePlatform.Web.Classes
{
    public class Book_Series
    {
        [Key]
        [Column(TypeName = "int")]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Book")]
        [Column(TypeName = "int")]
        public int BookId { get; set; }
        public Book? Book { get; set; }

        [Required]
        [ForeignKey("Series")]
        [Column(TypeName = "int")]
        public int SeriesId { get; set; }
        public Series? Series { get; set; }
    }
}
