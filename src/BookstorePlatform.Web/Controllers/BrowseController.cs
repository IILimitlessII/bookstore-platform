using BookstorePlatform.Web.Areas.Identity.Data;
using BookstorePlatform.Web.Classes;
using BookstorePlatform.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using System.Security.Claims;

public class BrowseController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly Browse _browse;
    
    public BrowseController(ApplicationDbContext context, Browse browse)
    {
        _context = context;
        _browse = browse;
    }

    public IActionResult Index(int page = 1)
    {
        int pageSize = 60; // Number of books per page
        int totalBooks = _context.Books.Count();
        int totalPages = (int)Math.Ceiling((double)totalBooks / pageSize);

        // Ensure page is within valid range
        page = Math.Max(1, Math.Min(totalPages, page));

        List<Book> books = GetBooksForPage(page, pageSize);

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;
        ViewBag.SortType = _browse.SortType;  // Pass the current sort type to the view

        return View(books);
    }

    public IActionResult Sort(int page = 1, string sortOption = "")
    {
        _browse.SortType = sortOption; // Update SortType
        List<Book> books = GetBooksForPage(page, 60);
        return PartialView("_BooksPartial", books);
    }

    private List<Book> GetBooksForPage(int page, int pageSize)
    {
        IQueryable<Book> query = _context.Books;

        // Sorting logic based on the stored sort type
        switch (_browse.SortType)
        {
            case "alphabetic_asc":
                query = query.OrderBy(b => b.Title);
                break;
            case "alphabetic_desc":
                query = query.OrderByDescending(b => b.Title);
                break;
            case "price_asc":
                query = query.OrderBy(b => Convert.ToDouble(b.Price));
                break;
            case "price_desc":
                query = query.OrderByDescending(b => Convert.ToDouble(b.Price));
                break;
            default:
                break;
        }

        // Pagination
        var books = query.Skip((page - 1) * pageSize)
                         .Take(pageSize)
                         .ToList();

        return books;
    }

    public IActionResult NavigateToPage(int page, string sortType)
    {
        
        int totalBooks = _context.Books.Count();
        int totalPages = (int)Math.Ceiling((double)totalBooks / 60);
        ViewBag.TotalPages = totalPages;
        ViewBag.CurrentPage = page;
        ViewBag.SortType = sortType;
        _browse.SortType = sortType; // Set the sort type

        // Retrieve the books for the specified page and sort type
        List<Book> books = GetBooksForPage(page, 60);

        // Return the view with the sorted books
        return View("Index", books);
    }

}
