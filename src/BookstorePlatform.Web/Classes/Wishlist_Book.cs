using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BookstorePlatform.Web.Areas.Identity.Data;

namespace BookstorePlatform.Web.Classes
{
    public class Wishlist_Book
    {
        [Key]
        [Column(TypeName = "int")]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Wishlist")]
        [Column(TypeName = "int")]
        public int WishlistId { get; set; }
        public virtual Wishlist? Wishlist { get; set; }

        [Required]
        [ForeignKey(nameof(StoreUser))]
        public required string UserId { get; set; }
        public virtual StoreUser? StoreUser { get; set; }

        [Required]
        [ForeignKey("Book")]
        [Column(TypeName = "int")]
        public int BookId { get; set; }
        public virtual Book? Book { get; set; }
    }
}
