using System.ComponentModel.DataAnnotations;

namespace PustokMVC.Models
{
    /// <summary>
    /// Represents an image associated with a book, including cover images, back covers, and detail images.
    /// </summary>
    /// <remarks>
    /// The BookImage class extends the BaseEntity and is designed to link images to books,
    /// supporting multiple images per book. Images can be categorized as main posters, back posters, or detail images.
    /// </remarks>
    public class BookImage : BaseEntity
    {
        /// <summary>
        /// The ID of the associated book.
        /// </summary>
        /// <remarks>
        /// This property establishes a foreign key relationship with the Book entity,
        /// indicating which book the image belongs to. It is required for all book images.
        /// </remarks>
        [Required]
        public int BookId { get; set; }
        /// <summary>
        /// The URL of the image.
        /// </summary>
        /// <remarks>
        /// This property stores the location of the image file. It is required and should be a valid URL to the image resource.
        /// </remarks>
        [Required]
        public string? ImageUrl { get; set; }
        /// <summary>
        /// Indicates the type of the image: true for the main poster (cover), false for the back poster, and null for additional detail images.
        /// </summary>
        /// <remarks>
        /// This property is used to categorize images. It allows for distinguishing between main cover images,
        /// back cover images, and any other detail images that are neither the main nor the back cover.
        /// </remarks>
        public bool? IsPoster { get; set; } // true for Poster, false for BackPoster, null for Detail
        /// <summary>
        /// Navigation property to the associated book.
        /// </summary>
        /// <remarks>
        /// This property facilitates the navigation from a book image back to the book entity it is associated with,
        /// enabling easier access to the book details from its images.
        /// </remarks>
        public Book? Book { get; set; }
    }
}
