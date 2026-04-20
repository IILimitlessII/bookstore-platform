using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookstorePlatform.Web.Classes
{
    public class Cart_Book
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
        [ForeignKey("Cart")]
        [Column(TypeName = "int")]
        public int CartId { get; set; }
        public virtual Cart? Cart { get; set; }

        [Required]
        [Column(TypeName = "int")]
        public int Quantity { get; set; }
    }
}
