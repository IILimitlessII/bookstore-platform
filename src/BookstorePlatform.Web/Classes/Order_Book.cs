using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookstorePlatform.Web.Classes
{
    public class Order_Book
    {
        [Key]
        [Column(TypeName = "int")]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Book")]
        [Column(TypeName = "int")]
        public int BookId { get; set; }
        public virtual Book? Book { get; set; }

        [Required]
        [ForeignKey("Order")]
        [Column(TypeName = "int")]
        public int OrderId { get; set; }
        public virtual Order? Order { get; set; }

        [Required]
        [Column(TypeName = "int")]
        public int Quantity { get; set; }
    }
}
