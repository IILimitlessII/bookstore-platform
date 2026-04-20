using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookstorePlatform.Web.Classes
{
    public class Book_Author
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
        [ForeignKey("Author")]
        [Column(TypeName = "int")]
        public int AuthorId { get; set; }
        public Author? Author { get; set; }
    }
}
