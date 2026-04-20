using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookstorePlatform.Web.Areas.Identity.Data
{
    public class Address
    {
        [Key]
        public int AddressID { get; set; }

        [Required]
        [StringLength(255)]
        [Column(TypeName = "varchar(255)")]
        public required string AddressLine1 { get; set; }

        [StringLength(255)]
        [Column(TypeName = "varchar(255)")]
        public string? AddressLine2 { get; set; }

        [Required]
        [StringLength(255)]
        [Column(TypeName = "varchar(255)")]
        public required string City { get; set; }

        [Required]
        [StringLength(255)]
        [Column(TypeName = "varchar(255)")]
        public required string Province { get; set; }

        [Required]
        [StringLength(10)]
        [Column(TypeName = "varchar(10)")]
        public required string ZipCode { get; set; }

        [Required]
        [ForeignKey(nameof(StoreUser))]
        public required string UserId { get; set; }

        public virtual StoreUser? StoreUser { get; set; }
    }
}
