using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BookstorePlatform.Web.Areas.Identity.Data;

namespace BookstorePlatform.Web.Classes
{
    public class Wishlist
    {
        [Key]
        [Column(TypeName = "int")]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(StoreUser))]
        public required string UserId { get; set; }
        public virtual StoreUser? StoreUser { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public required string Name { get; set; }

        [Required]
        [Column(TypeName = "int")]
        public int Active { get; set; }
    }
}
