using BookstorePlatform.Web.Areas.Identity.Data;
using BookstorePlatform.Web.Classes;
using BookstorePlatform.Web.Data;
using BookstorePlatform.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookstorePlatform.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly UserManager<StoreUser> _userManager;
        private readonly ILogger<OrderController> _logger;
        private readonly ApplicationDbContext _context;

        public OrderController(UserManager<StoreUser> userManager, ILogger<OrderController> logger, ApplicationDbContext context)
        {
            _userManager = userManager;
            _logger = logger;
            _context = context;
        }

        [BindProperty]
        public OrderInputModel Input { get; set; } = null!;

        public List<Cart_Book> CartItems { get; set; } = new();
        public decimal Total { get; set; }
        public List<Address> Addresses { get; set; } = new();

        public async Task<IActionResult> ViewOrderAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var userId = user.Id;
            // Convert the result of ToListAsync() to List<Cart_Book>
            CartItems = await _context.Cart_Books
                .Include(cb => cb.Book)
                .Where(cb => cb.Cart != null && cb.Cart.UserId == userId)
                .ToListAsync();

            var culture = new System.Globalization.CultureInfo("en-US");
            Total = CartItems.Sum(cb => Convert.ToInt32(cb.Quantity) * decimal.Parse(cb.Book!.Price, culture));

            // Convert the result of ToListAsync() to List<Address>
            Addresses = await _context.Addresses.Where(a => a.UserId == userId).ToListAsync();

            var viewModel = new OrderViewModel
            {
                CartItems = CartItems,
                Total = Total,
                Addresses = Addresses
            };

            return View(viewModel);
        }

        public async Task<IActionResult> ConfirmOrder(int cartId, int addressId)
        {

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // Get the current UTC time
            DateTime utcNow = DateTime.UtcNow;

            // Get the time zone information for South Africa
            TimeZoneInfo saTimeZone = TimeZoneInfo.FindSystemTimeZoneById("South Africa Standard Time");

            // Convert the UTC time to the local time in South Africa
            DateTime saLocalTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, saTimeZone);


            var order = new Order
            {
                UserId = user.Id,
                AddressId = addressId,
                OrderTimeStamp = saLocalTime
            };

            _context.Order.Add(order);

            await _context.SaveChangesAsync();

            var cartItems = await _context.Cart_Books.Where(cb => cb.CartId == cartId).ToListAsync();

            foreach (var cartItem in cartItems)
            {
                var orderBook = new Order_Book
                {
                    BookId = cartItem.BookId,
                    OrderId = order.Id,
                    Quantity = cartItem.Quantity
                };
                _context.Order_Book.Add(orderBook);
                _context.Cart_Books.Remove(cartItem);
            }

            var cartToRemove = await _context.Carts.FindAsync(cartId);
            if (cartToRemove != null)
            {
                _context.Carts.Remove(cartToRemove);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home", new { area = "" });
        }


    }
}
