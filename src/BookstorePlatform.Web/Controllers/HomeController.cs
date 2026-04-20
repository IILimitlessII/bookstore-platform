using BookstorePlatform.Web.Classes;
using BookstorePlatform.Web.Data;
using BookstorePlatform.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace BookstorePlatform.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly Browse _browse;
        public List<int> BookIds;

        // Define lists to store displayed book IDs for each method
        private List<int> First12DisplayedBookIds = new List<int>();
        private List<int> Second12DisplayedBookIds = new List<int>();
        private List<int> Third12DisplayedBookIds = new List<int>();
        private List<int> Fourth12DisplayedBookIds = new List<int>();
        private List<int> Fifth12DisplayedBookIds = new List<int>();

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, Browse browse)
        {
            _context = context;
            _logger = logger;
            _browse = browse;
            BookIds = new List<int>();
        }

        public IActionResult Index(int page = 1)
        {
            int pageSize = 6; // Number of books per page
            int totalBooks = _context.Books.Count();
            int totalPages = (int)Math.Ceiling((double)totalBooks / pageSize);

            // Ensure page is within valid range
            page = Math.Max(1, Math.Min(totalPages, page));

            // Populate ViewBag with filtered lists of books for each row
            ViewBag.BooksForRows = new List<List<Book>>
            {
                GetFirst12Books(),
                GetSecond12Books(),
                GetThird12Books(),
                GetFourth12Books(),
                GetFifth12Books()
            };

            ViewBag.RandomBooks = GetRandomBooksNotInList(30);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            // Combine all displayed book IDs from different methods
            var displayedBookIds = First12DisplayedBookIds
                .Concat(Second12DisplayedBookIds)
                .Concat(Third12DisplayedBookIds)
                .Concat(Fourth12DisplayedBookIds)
                .Concat(Fifth12DisplayedBookIds)
                .ToList();

            // Ensure that only unique book IDs are added to the BookIds list
            BookIds.AddRange(displayedBookIds.Distinct());

            return View();
        }

        public List<Book> GetFirst12Books()
        {
            var books = _context.Books.ToList();
            // Sort the books by the number of awards they have in descending order
            var sortedBooks = books.OrderByDescending(b => GetAwardCount(b.Awards)).ToList();

            // Track displayed book IDs
            var displayedBookIds = sortedBooks.Take(12).Select(b => b.Id).ToList();
            First12DisplayedBookIds.AddRange(displayedBookIds);

            // Return the first 12 books with the most awards
            return sortedBooks.Take(12).ToList();
        }

        private int GetAwardCount(string? awards)
        {
            // Split the awards string by commas and count the number of awards
            if (string.IsNullOrEmpty(awards))
            {
                return 0;
            }
            else
            {
                return awards.Split(',').Length;
            }
        }

        private List<Book> GetSecond12Books()
        {
            var authorsWithBookCountGreaterThan12 = _context.Book_Authors
                .GroupBy(ba => ba.AuthorId)
                .Where(group => group.Count() > 12)
                .Select(group => group.Key)
                .ToList();

            if (authorsWithBookCountGreaterThan12.Count == 0)
                return new List<Book>();

            // Step 2: Randomly select one author from the list.
            var random = new Random();
            var randomAuthorId = authorsWithBookCountGreaterThan12[random.Next(authorsWithBookCountGreaterThan12.Count)];

            var authorName = _context.Authors.Where(a => a.Id == randomAuthorId).Select(a => a.Name);

            _logger.LogInformation("\n___________________________________\n" +
                "Selected Author: {AuthorId}\n" +
                "\n___________________________________\n", authorName);

            // Step 3: Retrieve 12 book IDs associated with the selected author.
            var bookIds = _context.Book_Authors
                .Where(ba => ba.AuthorId == randomAuthorId)
                .Select(ba => ba.BookId)
                .ToList();

            Shuffle(bookIds);

            var booksToTake = bookIds.Take(12);

            // Track displayed book IDs
            Second12DisplayedBookIds.AddRange(booksToTake);

            // Step 4: Use these book IDs to fetch the corresponding books from the database.
            var books = _context.Books
                .Where(book => booksToTake.Contains(book.Id))
                .ToList();

            return books;
        }

        private List<Book> GetThird12Books()
        {
            // Step 1: Query the database to find genres with book counts greater than 12.
            var genresWithBookCountGreaterThan12 = _context.Book_Genres
                .GroupBy(bg => bg.GenreId)
                .Where(group => group.Count() > 12)
                .Select(group => group.Key)
                .ToList();

            if (genresWithBookCountGreaterThan12.Count == 0)
                return new List<Book>();

            // Step 2: Randomly select one genre from the list.
            var random = new Random();
            var randomGenreId = genresWithBookCountGreaterThan12[random.Next(genresWithBookCountGreaterThan12.Count)];

            var genreName = _context.Genres.Where(a => a.Id == randomGenreId).Select(a => a.Name);

            _logger.LogInformation("\n___________________________________\n" +
                "Selected Genre: {GenreId}\n" +
                "\n___________________________________\n", genreName);

            // Step 3: Retrieve 12 book IDs associated with the selected genre.
            var bookIds = _context.Book_Genres
                .Where(bg => bg.GenreId == randomGenreId)
                .Select(bg => bg.BookId)
                .ToList();

            // Step 4: Shuffle the list of book IDs.
            Shuffle(bookIds);

            var booksToTake = bookIds.Take(12);

            // Track displayed book IDs
            Third12DisplayedBookIds.AddRange(booksToTake);

            // Step 5: Use these shuffled book IDs to fetch the corresponding books from the database.
            var books = _context.Books
                .Where(book => booksToTake.Contains(book.Id))
                .ToList();

            return books;
        }

        private List<Book> GetFourth12Books()
        {
            var seriesWithBookCountGreaterThan12 = _context.Book_Series
                .GroupBy(bs => bs.SeriesId)
                .Where(group => group.Count() > 12)
                .Select(group => group.Key)
                .ToList();

            if (seriesWithBookCountGreaterThan12.Count == 0)
                return new List<Book>();

            var random = new Random();
            var randomSeriesId = seriesWithBookCountGreaterThan12[random.Next(seriesWithBookCountGreaterThan12.Count)];

            var seriesName = _context.Seriess.Where(a => a.Id == randomSeriesId).Select(a => a.Name);

            _logger.LogInformation("\n___________________________________\n" +
                "Selected Series: {SeriesId}\n" +
                "\n___________________________________\n", seriesName);

            var bookIds = _context.Book_Series
                .Where(bs => bs.SeriesId == randomSeriesId)
                .Select(bs => bs.BookId)
                .ToList();

            Shuffle(bookIds);

            var booksToTake = bookIds.Take(12);

            // Track displayed book IDs
            Fourth12DisplayedBookIds.AddRange(booksToTake);

            var books = _context.Books
                .Where(book => booksToTake.Contains(book.Id))
                .ToList();

            return books;
        }

        private List<Book> GetFifth12Books()
        {
            var publisherWithBookCountGreaterThan12 = _context.Book_Publisher
                .GroupBy(bp => bp.PublisherId)
                .Where(group => group.Count() > 12)
                .Select(group => group.Key)
                .ToList();

            if (publisherWithBookCountGreaterThan12.Count == 0)
                return new List<Book>();

            var random = new Random();
            var randomPublisherId = publisherWithBookCountGreaterThan12[random.Next(publisherWithBookCountGreaterThan12.Count)];

            var publisherName = _context.Publishers.Where(a => a.Id == randomPublisherId).Select(a => a.Name);

            _logger.LogInformation("\n___________________________________\n" +
                "Selected Publisher: {PublisherId}\n" +
                "\n___________________________________\n", publisherName);

            var bookIds = _context.Book_Publisher
                .Where(bp => bp.PublisherId == randomPublisherId)
                .Select(bp => bp.BookId)
                .ToList();

            Shuffle(bookIds);

            var booksToTake = bookIds.Take(12);

            // Track displayed book IDs
            Fifth12DisplayedBookIds.AddRange(booksToTake);

            var books = _context.Books
                .Where(book => booksToTake.Contains(book.Id))
                .ToList();

            return books;
        }

        private List<Book> GetRandomBooksNotInList(int count)
        {
            // Get a list of book IDs
            var allBookIds = _context.Books.Select(book => book.Id).ToList();

            // Remove book IDs that are already in the BookIds list
            var remainingBookIds = allBookIds.Except(BookIds).ToList();

            // Shuffle the remaining book IDs
            Shuffle(remainingBookIds);

            // Take 'count' number of book IDs from the shuffled list
            var booksToTake = remainingBookIds.Take(count).ToList();

            // Retrieve the corresponding books from the database
            var books = _context.Books
                .Where(book => booksToTake.Contains(book.Id))
                .ToList();

            return books;
        }

        private void Shuffle<T>(IList<T> list)
        {
            var random = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public IActionResult BookDetails(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            // Retrieve Publisher
            var publisher = _context.Book_Publisher
                .Include(bp => bp.Publisher)
                .FirstOrDefault(bp => bp.BookId == id);

            // Retrieve Author(s)
            var authorIds = _context.Book_Authors
                .Where(ba => ba.BookId == id)
                .Select(ba => ba.AuthorId)
                .ToList();
            var authors = _context.Authors
                .Where(a => authorIds.Contains(a.Id))
                .ToList();

            // Retrieve Genres
            var genreIds = _context.Book_Genres
                .Where(bg => bg.BookId == id)
                .Select(bg => bg.GenreId)
                .ToList();
            var genres = _context.Genres
                .Where(g => genreIds.Contains(g.Id))
                .Select(g => g.Name)
                .ToList();

            var publisherId = _context.Book_Publisher.FirstOrDefault(bp => bp.BookId == id)?.PublisherId;
            var seriesId = _context.Book_Series.FirstOrDefault(bs => bs.BookId == id)?.SeriesId;

            var model = new BookDetailsViewModel
            {
                Book = book,
                PublisherName = publisher?.Publisher?.Name ?? string.Empty,
                PublisherId = publisherId,
                PublishDate = publisher?.PublishDate,
                FirstPublishDate = publisher?.FirstPublishDate ?? string.Empty,
                BookFormat = book.BookFormat,
                Genres = genres,
                Authors = authors,
                SeriesId = seriesId,
                AuthorIds = authorIds,
                GenreIds = genreIds
            };

            if (DateTime.TryParseExact(model.FirstPublishDate, "MM/dd/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime firstPublishDateTime))
            {
                model.FirstPublishDateMonth = firstPublishDateTime.ToString("MMMM", CultureInfo.InvariantCulture);
                model.FirstPublishDateDay = firstPublishDateTime.Day.ToString();
                model.FirstPublishDateYear = (firstPublishDateTime.Year <= 2000 ? "19" : "20") + firstPublishDateTime.ToString("yy");
            }

            // Fetch series name
            model.SeriesName = _context.Seriess.FirstOrDefault(s => s.Id == seriesId)?.Name;

            return View(model);
        }



        public IActionResult BooksByAuthor(List<int> authorIds)
        {
            var books = _context.Book_Authors
                                .Where(ba => authorIds.Contains(ba.AuthorId))
                                .Select(ba => ba.Book)
                                .ToList();
            ViewBag.BooksByAuthor = books;
            return View("Index");
        }

        public IActionResult BooksByGenre(List<int> genreIds)
        {
            var books = _context.Book_Genres
                                .Where(bg => genreIds.Contains(bg.GenreId))
                                .Select(bg => bg.Book)
                                .ToList();
            ViewBag.BooksByGenre = books;
            return View("Index", books);
        }

        public IActionResult BooksBySeries(int seriesId)
        {
            var books = _context.Book_Series
                                .Where(bs => bs.SeriesId == seriesId)
                                .Select(bs => bs.Book)
                                .ToList();
            ViewBag.BooksBySeries = books;
            return View("Index");
        }

        public IActionResult BooksByPublisher(int publisherId)
        {
            var books = _context.Book_Publisher
                                .Where(bp => bp.PublisherId == publisherId)
                                .Select(bp => bp.Book)
                                .ToList();
            ViewBag.BooksByPublisher = books;
            return View("Index");
        }

        public IActionResult BooksByFormat(string bookFormat)
        {
            var books = _context.Books
                                .Where(b => b.BookFormat == bookFormat)
                                .ToList();
            ViewBag.BooksByFormat = books;
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ImportAuthor()
        {
            try
            {
                // Read the provided CSV data
                string[] lines = await System.IO.File.ReadAllLinesAsync("unique_authors.csv");

                // Process each line (excluding header)
                foreach (string line in lines.Skip(1))
                {
                    // Parse the CSV line and split by comma
                    string[] fields = line.Split(',');

                    // Create a new Author object and parse the values
                    Author author = new Author
                    {
                        Name = fields[1].Trim()
                    };

                    // Add Author to the database
                    _context.Authors.Add(author);

                    // Save changes to the database
                    await _context.SaveChangesAsync();
                    Console.WriteLine($"Author Name: {author.Name}");
                }


                return Ok("Authors imported successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<IActionResult> ImportBooks()
        {
            try
            {
                string[] lines = await System.IO.File.ReadAllLinesAsync("output.csv");
                // Iterate over each line in the CSV file
                foreach (string line in lines.Skip(1)) // Skip header line
                {
                    // Split the line into fields considering quoted text
                    string[] fields = SplitCSVLine(line);

                    // Create a new Book object and populate its properties
                    var book = new Book
                    {
                        Title = fields[1].Trim('"'),
                        Description = fields[2].Trim('"'),
                        Rating = fields[3].Trim('"'),
                        ISNB = fields[4].Trim('"'),
                        Characters = fields[5].Trim('"'),
                        BookFormat = fields[6].Trim('"'),
                        Edition = fields[7].Trim('"'),
                        Awards = fields[9].Trim('"'),
                        CoverImg = fields[10].Trim('"'),
                        Price = fields[11].Trim('"')
                    };

                    // Add the book to the database context
                    _context.Books.Add(book);

                    // Save changes to the database
                    await _context.SaveChangesAsync();
                    Console.WriteLine($"Book Title: {book.Title}");
                }
                return Ok("Books imported successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}" +
                    $"Inner Exception {ex.InnerException}");
            }

        }

        public async Task<IActionResult> ImportBook_Author()
        {
            try
            {
                string[] lines = await System.IO.File.ReadAllLinesAsync("book_author_pairs+1.csv");

                if (lines.Length == 0)
                {
                    return BadRequest("CSV file is empty.");
                }

                foreach (string line in lines.Skip(1))
                {
                    string[] fields = line.Split(',');

                    bool bookid = _context.Books.Any(b => b.Id == Convert.ToInt32(fields[0]));
                    bool authorid =_context.Authors.Any(a => a.Id == Convert.ToInt32(fields[1]));

                    if (bookid && authorid == true)
                    {
                        var book_author = new Book_Author
                        {
                            BookId = Convert.ToInt32(fields[0].Trim('"')),
                            AuthorId = Convert.ToInt32(fields[1].Trim('"'))
                        };
                        _context.Book_Authors.Add(book_author);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        Console.WriteLine($"BookID: {fields[0]}\n" +
                            $"AuthorID: {fields[1]}");
                    }
                }

                return Ok("Book_Authors imported successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}" +
                    $"Inner Exception {ex.InnerException}");
            }
        }


        public async Task<IActionResult> ImportBook_Publisher()
        {
            try
            {
                string[] lines = await System.IO.File.ReadAllLinesAsync("book_pubs.csv");

                if (lines.Length == 0)
                {
                    return BadRequest("CSV file is empty.");
                }

                foreach (string line in lines.Skip(1))
                {
                    string[] fields = line.Split(',');

                    bool bookid = _context.Books.Any(b => b.Id == Convert.ToInt32(fields[0]));
                    bool publisherid = _context.Publishers.Any(p => p.Id == Convert.ToInt32(fields[1]));

                    if (bookid && publisherid == true)
                    {
                        var book_publisher = new Book_Publisher
                        {
                            BookId = Convert.ToInt32(fields[0].Trim('"')),
                            PublisherId = Convert.ToInt32(fields[1].Trim('"')),
                            PublishDate = fields[2].Trim('"'),
                            FirstPublishDate = fields[3].Trim('"')
                        };
                        _context.Book_Publisher.Add(book_publisher);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        Console.WriteLine($"BookID: {fields[0]}\n" +
                            $"PublisherID: {fields[1]}\n" +
                            $"PublishDate: {fields[2]}");
                    }
                }

                return Ok("Book_Publishers imported successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}" +
                    $"Inner Exception {ex.InnerException}");
            }
        }



        public async Task<IActionResult> ImportBook_Genre()
        {
            try
            {
                string[] lines = await System.IO.File.ReadAllLinesAsync("book_genre_pairs+1.csv");

                if (lines.Length == 0)
                {
                    return BadRequest("CSV file is empty.");
                }

                foreach (string line in lines.Skip(1))
                {
                    string[] fields = line.Split(',');

                    bool bookid = _context.Books.Any(b => b.Id == Convert.ToInt32(fields[0]));
                    bool genreid = _context.Genres.Any(g => g.Id == Convert.ToInt32(fields[1]));

                    if (bookid && genreid == true)
                    {
                        var book_genre = new Book_Genre
                        {
                            BookId = Convert.ToInt32(fields[0].Trim('"')),
                            GenreId = Convert.ToInt32(fields[1].Trim('"'))
                        };
                        _context.Book_Genres.Add(book_genre);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        Console.WriteLine($"BookID: {fields[0]}\n" +
                            $"GenreID: {fields[1]}");
                    }
                }

                return Ok("Book_Genres imported successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}" +
                    $"Inner Exception {ex.InnerException}");
            }
        }



        public async Task<IActionResult> ImportBook_Series()
        {
            try
            {
                string[] lines = await System.IO.File.ReadAllLinesAsync("book_series_pairs+1.csv");

                if (lines.Length == 0)
                {
                    return BadRequest("CSV file is empty.");
                }

                foreach (string line in lines.Skip(1))
                {
                    string[] fields = line.Split(',');

                    bool bookid = _context.Books.Any(b => b.Id == Convert.ToInt32(fields[0]));
                    bool seriesid = _context.Seriess.Any(s => s.Id == Convert.ToInt32(fields[1]));

                    if (bookid && seriesid == true)
                    {
                        var book_series = new Book_Series
                        {
                            BookId = Convert.ToInt32(fields[0].Trim('"')),
                            SeriesId = Convert.ToInt32(fields[1].Trim('"'))
                        };
                        _context.Book_Series.Add(book_series);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        Console.WriteLine($"BookID: {fields[0]}\n" +
                            $"SeriesID: {fields[1]}");
                    }
                }

                return Ok("Book_Series imported successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}" +
                    $"Inner Exception {ex.InnerException}");
            }
        }


        [HttpPost]
        public IActionResult CheckAuthorIdsInCsv()
        {
            try
            {
                string filePath = "book_author_pairs+1.csv";
                // Read the provided CSV data
                string[] lines = System.IO.File.ReadAllLines(filePath);

                // Check if there are any lines
                if (lines.Length == 0)
                {
                    return BadRequest("CSV file is empty.");
                }

                List<int> authorIdsInCsv = new List<int>();

                // Iterate over each line in the CSV file
                foreach (string line in lines.Skip(1)) // Skip header line
                {
                    // Split the line into fields
                    string[] fields = line.Split(',');

                    // Parse AuthorId from CSV
                    if (int.TryParse(fields[1], out int authorId))
                    {
                        authorIdsInCsv.Add(authorId);
                    }
                    else
                    {
                        
                    }
                }

                // Get distinct Author IDs from the CSV
                authorIdsInCsv = authorIdsInCsv.Distinct().ToList();

                // Get all Author IDs from the Authors table
                List<int> allAuthorIdsInDb = _context.Authors.Select(a => a.Id).ToList();

                // Find the Author IDs in the CSV that are not found in the Authors table
                List<int> authorIdsNotFoundInDb = authorIdsInCsv.Except(allAuthorIdsInDb).ToList();

                if (authorIdsNotFoundInDb.Any())
                {
                    // Print out the IDs from the CSV that are not found in the Authors table
                    Console.WriteLine("Author IDs from the CSV that are not found in the Authors table:");
                    foreach (int authorIdNotFound in authorIdsNotFoundInDb)
                    {
                        Console.WriteLine(authorIdNotFound);
                    }

                    return BadRequest("Some Author IDs from the CSV are not found in the Authors table. Check the console for details.");
                }
                else
                {
                    return Ok("All Author IDs from the CSV are found in the Authors table.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}" +
                    $"Inner Exception {ex.InnerException}");
            }
        }


        [HttpPost]
        public async Task<IActionResult> ImportSeries()
        {
            try
            {
                // Read the provided CSV data
                string[] lines = await System.IO.File.ReadAllLinesAsync("book_author_pairs+1.csv");

                // Check if there are any lines
                if (lines.Length == 0)
                {
                    return BadRequest("CSV file is empty.");
                }

                // Iterate over each line in the CSV file
                foreach (string line in lines.Skip(1)) // Skip header line
                {
                    // Split the line into fields
                    string[] fields = line.Split(',');

                    // Parse BookId and AuthorId from CSV
                    int bookId = int.Parse(fields[0]);
                    int authorId = int.Parse(fields[1]);

                    // Create a new Book_Author object
                    var bookAuthor = new Book_Author
                    {
                        BookId = bookId,
                        AuthorId = authorId
                    };

                    // Add the Book_Author to the database context
                    _context.Book_Authors.Add(bookAuthor);
                }

                // Save changes to the database
                await _context.SaveChangesAsync();

                return Ok("Book-Author relationships imported successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}" +
                    $"other : {ex.InnerException}");
            }
        }


        // Method to split a CSV line into fields considering quoted text
        private string[] SplitCSVLine(string line)
        {
            // Initialize a string array to store the fields
            var fields = new List<string>();

            // Initialize a string builder to construct each field
            var fieldBuilder = new StringBuilder();

            // Flag to track if inside a quoted text
            var inQuote = false;

            // Iterate through each character in the line
            foreach (var c in line)
            {
                // If character is a comma and not inside quoted text, add field to array and reset fieldBuilder
                if (c == ',' && !inQuote)
                {
                    fields.Add(fieldBuilder.ToString());
                    fieldBuilder.Clear();
                }
                // If character is a double quote, toggle inQuote flag
                else if (c == '"')
                {
                    inQuote = !inQuote;
                }
                // Otherwise, add character to fieldBuilder
                else
                {
                    fieldBuilder.Append(c);
                }
            }

            // Add the last field to array
            fields.Add(fieldBuilder.ToString());

            // Return the array of fields
            return fields.ToArray();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
