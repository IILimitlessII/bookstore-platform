using BookstorePlatform.Web.Classes;

namespace BookstorePlatform.Web.Models
{
    public class BrowseViewModel
    {
        public List<Book> Books { get; set; } = new();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string SortType { get; set; } = string.Empty;
    }
}
