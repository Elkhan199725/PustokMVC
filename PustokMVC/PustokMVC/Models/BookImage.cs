using System.ComponentModel.DataAnnotations;

namespace PustokMVC.Models
{
    public class BookImage : BaseEntity
    {
        [Required]
        public int BookId { get; set; }
        [Required]
        public string? ImageUrl { get; set; }
        public bool? IsPoster { get; set; } // true Poster False BackPoster null Detail
        public Book? Book { get; set; }

    }
}
