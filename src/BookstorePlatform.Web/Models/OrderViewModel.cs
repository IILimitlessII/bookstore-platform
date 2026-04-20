using BookstorePlatform.Web.Areas.Identity.Data;
using BookstorePlatform.Web.Classes;
using System.ComponentModel.DataAnnotations;

namespace BookstorePlatform.Web.Models
{
    public class OrderViewModel
    {
        public List<Cart_Book> CartItems { get; set; } = new();
        public decimal Total { get; set; }
        public List<Address> Addresses { get; set; } = new();
        public OrderInputModel Input { get; set; } = null!;
        public Address? SelectedAddress { get; set; }
    }

    public class OrderInputModel
    {
        [Required]
        public required string AddressLine1 { get; set; }

        public string? AddressLine2 { get; set; }

        [Required]
        public required string City { get; set; }

        [Required]
        public required string Province { get; set; }

        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Please enter valid Zip Code.")]
        [Range(1000, 9999, ErrorMessage = "Zip Code must be a 4-digit number.")]
        [Display(Name = "Zip Code")]
        public required string ZipCode { get; set; }
    }
}
