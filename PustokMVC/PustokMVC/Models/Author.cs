using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace PustokMVC.Models
{
    /// <summary>
    /// Represents an author of books within the application, including a list of books written by the author.
    /// </summary>
    /// <remarks>
    /// The Author class extends BaseEntity, inheriting properties like Id, IsActive, CreatedDate, and ModifiedDate.
    /// It's designed to encapsulate the data and relationships specific to authors, such as their full name and the books they've authored.
    /// This model plays a crucial role in the management of author data and the relational mapping between authors and books, facilitating:
    /// - The storage and retrieval of author-specific information.
    /// - The association of books with their respective authors, allowing for queries that can fetch books by a specific author.
    /// - The potential for expanding author-related information (e.g., biographies, awards) in the future without significant schema changes.
    /// </remarks>
    public class Author : BaseEntity
    {
        /// <summary>
        /// The full name of the author.
        /// </summary>
        /// <remarks>
        /// This property allows up to 50 characters for the author's full name. While it's not marked as required,
        /// best practices suggest ensuring data integrity through application logic or UI validation to prevent records with missing names.
        /// </remarks>
        [StringLength(50)]
        public string? FullName { get; set; }

        /// <summary>
        /// A collection of books written by the author.
        /// </summary>
        /// <remarks>
        /// This navigation property establishes a one-to-many relationship between an author and their books,
        /// allowing each author to be associated with multiple books. It's a key feature for cataloging books by author
        /// and supports complex queries within the application, such as fetching all books by a particular author.
        /// </remarks>
        public List<Book>? Books { get; set; }
    }
}
