using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace PustokMVC.Models
{
    /// <summary>
    /// Represents a genre entity in the application, categorizing books into distinct genres.
    /// </summary>
    /// <remarks>
    /// The Genre class extends the BaseEntity, inheriting common properties such as ID.
    /// It encapsulates the genre's name and the associated collection of books within that genre.
    /// </remarks>
    public class Genre : BaseEntity
    {
        /// <summary>
        /// The name of the genre.
        /// </summary>
        /// <remarks>
        /// This property is required and limited to a maximum length of 50 characters.
        /// It serves as the unique identifier for different genres within the library or bookstore.
        /// </remarks>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        /// <summary>
        /// The collection of books associated with this genre.
        /// </summary>
        /// <remarks>
        /// This property represents the relationship between genres and books.
        /// A genre can have multiple books, but a book can be associated with only one genre.
        /// This collection is optional and can be null if no books are associated with the genre yet.
        /// </remarks>
        public List<Book>? Books { get; set; }
    }
}
