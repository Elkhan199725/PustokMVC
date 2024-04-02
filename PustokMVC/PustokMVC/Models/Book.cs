using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace PustokMVC.Models
{
    /// <summary>
    /// Represents a book with properties that describe its attributes and relationships with genres, authors, and images.
    /// </summary>
    /// <remarks>
    /// The Book class is a foundational entity within the PustokMVC application, linking to authors and genres to provide
    /// a comprehensive representation of a book's information. It includes:
    /// - Core details such as title, description, and unique book code.
    /// - Pricing information, including cost and sale price, along with any applicable discount.
    /// - Stock status, highlighting availability and categorization as featured, new, bestseller, and general availability.
    /// - Relationships to other entities (Genre, Author) and collections of images to visually represent the book.
    /// 
    /// This class also supports the management of book images through non-mapped properties, allowing for image uploads
    /// directly associated with book records. These images are critical for online display and marketing purposes.
    /// The design facilitates extensibility and scalability, enabling the application to adapt to various book categorizations
    /// and marketing strategies.
    /// 
    /// Note: Non-mapped image fields are intended for use in forms and API payloads where books are created or updated,
    /// and should be processed accordingly to store or update image data in persistent storage.
    /// </remarks>
    public class Book : BaseEntity
    {
        public int GenreId { get; set; }
        public int AuthorId { get; set; }
        [Required]
        [StringLength(50)]
        public string? Title { get; set; }
        [StringLength(350)]
        public string? Description { get; set; }
        [Required, StringLength(50)]
        public string? BookCode { get; set; }
        [Required]
        public double? CostPrice { get; set; }
        [Required]
        public double? SalePrice { get; set; }
        public double? DiscountPercent { get; set; }
        [Required]
        public bool IsFeatured { get; set; }
        [Required]
        public bool IsNew { get; set; }
        [Required]
        public bool IsBestSeller { get; set; }
        [Required]
        public bool IsAvailable { get; set; }
        [Required]
        public int StockCount { get; set; }
        public Genre? Genre { get; set; }
        public Author? Author { get; set; }
        public List<BookImage>? BookImages { get; set; }
        [NotMapped]
        public IFormFile? PosterImageFile { get; set; }
        [NotMapped]
        public IFormFile? HoverImageFile { get; set; }
        [NotMapped]
        public List<IFormFile>? ImageFiles { get; set; }
        [NotMapped]
        public List<int>? BookImageIds { get; set; }
    }
}
