

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace BookstorePlatform.Web.Areas.Identity.Data

{
    public class StoreUser : IdentityUser
    {
        [Required]
        [StringLength(255)]
        [Column(TypeName = "varchar(255)")]
        public required string FirstName { get; set; }

        [Required]
        [StringLength(255)]
        [Column(TypeName = "varchar(255)")]
        public required string LastName { get; set; }

        public virtual ICollection<Address>? Addresses { get; set; }
    }
    
}
