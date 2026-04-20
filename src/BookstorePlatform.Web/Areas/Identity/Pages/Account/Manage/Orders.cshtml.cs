using BookstorePlatform.Web.Areas.Identity.Data;
using BookstorePlatform.Web.Classes;
using BookstorePlatform.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BookstorePlatform.Web.Areas.Identity.Pages.Account.Manage
{
    public class OrdersModel : PageModel
    {
        private readonly UserManager<StoreUser> _userManager;
        private readonly ILogger<OrdersModel> _logger;
        private readonly ApplicationDbContext _context;

        public OrdersModel(UserManager<StoreUser> userManager, ILogger<OrdersModel> logger, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _logger = logger;
            _context = dbContext;
        }
        public IList<Order> Orders { get; set; } = new List<Order>();
        public Dictionary<int, Address> AddressDictionary { get; set; } = new Dictionary<int, Address>();

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = _userManager.GetUserId(User); // Get the current user's ID

            if (userId == null)
            {
                return NotFound("User ID not found.");
            }

            // Query the Address table to get all addresses for the current user
            Orders = await _context.Order
                .Where(a => a.UserId == userId)
                .ToListAsync();

            // Get distinct address IDs associated with the orders
            var addressIds = Orders.Select(o => o.AddressId).Distinct().ToList();

            // Fetch the corresponding addresses from the database
            AddressDictionary = await _context.Addresses
                .Where(a => addressIds.Contains(a.AddressID))
                .ToDictionaryAsync(a => a.AddressID, a => a);

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var order = await _context.Order.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            var orderbooks = await _context.Order_Book
                .Where(a => a.OrderId == order.Id)
                .ToListAsync();

            foreach(var book in orderbooks)
            {
                _context.Order_Book.Remove(book);
                await _context.SaveChangesAsync();
            }

            _context.Order.Remove(order);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        public int GetBooksQuantity(int orderId)
        {
            return _context.Order_Book
                .Where(ob => ob.OrderId == orderId)
                .Sum(ob => ob.Quantity);
        }
    }
}
