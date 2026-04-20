using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookstorePlatform.Web.Classes
{
    public class Book_Publisher
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
        [ForeignKey("Publisher")]
        [Column(TypeName = "int")]
        public int PublisherId { get; set; }
        public Publisher? Publisher { get; set; }

        [Required]
        public required string PublishDate { get; set; }

        public string? FirstPublishDate { get; set; }
    }
}
