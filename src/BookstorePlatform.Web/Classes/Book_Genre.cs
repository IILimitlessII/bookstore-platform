using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookstorePlatform.Web.Classes
{
    public class Book_Genre
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
        [ForeignKey("Genre")]
        [Column(TypeName = "int")]
        public int GenreId { get; set; }
        public Genre? Genre { get; set; }
    }
}
