using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookstorePlatform.Web.Classes
{
    public class Book
    {
        [Key]
        [Required]
        [Column(TypeName = "int")]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public required string Title { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(MAX)")]
        public required string Description { get; set; }

        [Required]
        [Column(TypeName = "varchar(5)")]
        public required string Rating { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public required string ISNB { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string? Characters { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public required string BookFormat { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string? Edition { get; set; }

        [Required]
        [Column(TypeName = "int")]
        public int Pages { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string? Awards { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public required string CoverImg { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public required string Price { get; set; }
    }
}
