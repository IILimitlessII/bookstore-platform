using BookstorePlatform.Web.Areas.Identity.Data;
using BookstorePlatform.Web.Classes;
using BookstorePlatform.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BookstorePlatform.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ILogger<CartController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<StoreUser> _userManager;

        public CartController(ILogger<CartController> logger, ApplicationDbContext context, UserManager<StoreUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> ViewCart()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                // User is not authenticated, handle accordingly
                return Unauthorized(); // Or any other appropriate action
            }

            // User is authenticated, get the user ID
            var userId = user.Id;

            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                // Handle case where the cart is not found
                return NotFound();
            }

            // Retrieve the cart books associated with the cart
            var cartBooks = await _context.Cart_Books
                .Include(cb => cb.Book)
                .Where(cb => cb.CartId == cart.Id)
                .ToListAsync();

            return View(cartBooks);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int bookId)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                // User is not authenticated, handle accordingly
                return Unauthorized(); // Or any other appropriate action
            }

            // User is authenticated, get the user ID
            var userId = user.Id;

            // Check if the user has a cart
            var cart = _context.Carts.FirstOrDefault(c => c.UserId == userId);

            // If the user doesn't have a cart, create a new one
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
                _context.SaveChanges();
            }

            // Check if the book is already in the cart
            var existingCartBook = _context.Cart_Books.FirstOrDefault(cb => cb.BookId == bookId && cb.CartId == cart.Id);

            if (existingCartBook != null)
            {

                return BadRequest("The book is already in the cart.");
            }

            // Create a new Cart_Book entry
            var cartBook = new Cart_Book
            {
                BookId = bookId,
                CartId = cart.Id,
                Quantity = 1
            };

            // Add the cart book to the database
            _context.Cart_Books.Add(cartBook);
            _context.SaveChanges();

            return Ok("added"); // Return success response
        }


        [HttpPost]
        public async Task<IActionResult> DeleteBookFromCart(int cartBookId)
        {
            // Find the cart book to delete
            var cartBook = await _context.Cart_Books.FindAsync(cartBookId);

            if (cartBook == null)
            {
                // If cart book not found, return NotFound
                return NotFound();
            }

            _context.Cart_Books.Remove(cartBook);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Return success response
            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int cartBookId, int quantity, bool confirmDeletion)
        {
            var cartBook = await _context.Cart_Books.FindAsync(cartBookId);

            if (cartBook == null)
            {
                return NotFound();
            }

            if (quantity <= 0 && confirmDeletion)
            {
                _context.Cart_Books.Remove(cartBook);
            }
            else
            {
                cartBook.Quantity = quantity;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("ViewCart");
        }
    }
}
