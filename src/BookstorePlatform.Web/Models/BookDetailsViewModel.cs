using BookstorePlatform.Web.Classes;

namespace BookstorePlatform.Web.Models
{
    public class BookDetailsViewModel
    {
        public Book Book { get; set; } = null!;
        public string PublisherName { get; set; } = string.Empty;
        public string? PublishDate { get; set; }
        public int? PublisherId { get; set; }
        public string? BookFormat { get; set; }
        public string FirstPublishDate { get; set; } = string.Empty;
        public string FirstPublishDateMonth { get; set; } = string.Empty;
        public string FirstPublishDateDay { get; set; } = string.Empty;
        public string FirstPublishDateYear { get; set; } = string.Empty;
        public List<string> Genres { get; set; } = new();
        public List<Author> Authors { get; set; } = new();
        public int? SeriesId { get; set; }
        public string? SeriesName { get; set; }
        public List<int> AuthorIds { get; set; } = new();
        public List<int> GenreIds { get; set; } = new();
    }

}
