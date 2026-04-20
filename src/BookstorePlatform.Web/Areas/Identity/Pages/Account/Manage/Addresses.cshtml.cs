using BookstorePlatform.Web.Areas.Identity.Data;
using BookstorePlatform.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BookstorePlatform.Web.Areas.Identity.Pages.Account.Manage
{
    public class AddressesModel : PageModel
    {
        private readonly UserManager<StoreUser> _userManager;
        private readonly ILogger<AddressesModel> _logger;
        private readonly ApplicationDbContext _context;

        public AddressesModel(UserManager<StoreUser> userManager, ILogger<AddressesModel> logger, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _logger = logger;
            _context = dbContext;
        }

        [BindProperty]
        public InputModel Input { get; set; } = default!;

        public IList<Address> Addresses { get; set; } = new List<Address>();

        public class InputModel
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

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = _userManager.GetUserId(User); // Get the current user's ID

            if (userId == null)
            {
                return NotFound("User ID not found.");
            }

            // Query the Address table to get all addresses for the current user
            Addresses = await _context.Addresses
                .Where(a => a.UserId == userId)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User); // Get the current user's ID

                if (userId == null)
                {
                    return NotFound("User ID not found.");
                }

                // Query the Address table to get all addresses for the current user
                Addresses = await _context.Addresses
                    .Where(a => a.UserId == userId)
                    .ToListAsync();
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (user.Addresses == null)
            {
                user.Addresses = new List<Address>();
            }

            var address = new Address
            {
                AddressLine1 = Input.AddressLine1,
                AddressLine2 = Input.AddressLine2,
                City = Input.City,
                Province = Input.Province,
                ZipCode = Input.ZipCode,
                UserId = user.Id
            };

            user.Addresses.Add(address);
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                _logger.LogInformation("Address added successfully.");
                return RedirectToPage();
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                var userId = _userManager.GetUserId(User); // Get the current user's ID

                if (userId == null)
                {
                    return NotFound("User ID not found.");
                }

                // Query the Address table to get all addresses for the current user
                Addresses = await _context.Addresses
                    .Where(a => a.UserId == userId)
                    .ToListAsync();

                return Page();
            }
        }
    }
}
