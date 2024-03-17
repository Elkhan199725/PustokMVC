using Microsoft.EntityFrameworkCore;
using PustokMVC.Models;

namespace PustokMVC.Data;

/// <summary>
/// Application database context defining sets of entities and configurations for interacting with the database.
/// </summary>
/// <remarks>
/// The PustokDbContext class extends the DbContext class from Entity Framework Core, facilitating:
/// - Configuration of model entities and their relationships.
/// - Database connection management.
/// - Querying and saving data to the database.
/// 
/// This class declares properties for each entity set corresponding to database tables. These DbSet properties
/// are used by EF Core to perform data operations (CRUD) on the respective entities. The entity sets include:
/// - Sliders: Represents a collection of promotional or informational sliders.
/// - Genres: Represents various book genres.
/// - Authors: Represents authors of books.
/// - Books: Represents the collection of books.
/// - BookImages: Represents images associated with books, including covers and additional imagery.
///
/// The configuration of this context, including the database connection string and other options, is typically
/// done in the Startup.cs or Program.cs file of the application, leveraging dependency injection for initialization.
/// </remarks>
public class PustokDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the PustokDbContext with the specified options.
    /// </summary>
    /// <param name="options">The options for configuring the context.</param>
    public PustokDbContext(DbContextOptions<PustokDbContext> options) : base(options) { }

    /// <summary>
    /// Gets or sets the DbSet for Sliders.
    /// </summary>
    public DbSet<Slider> Sliders { get; set; }

    /// <summary>
    /// Gets or sets the DbSet for Genres.
    /// </summary>
    public DbSet<Genre> Genres { get; set; }

    /// <summary>
    /// Gets or sets the DbSet for Authors.
    /// </summary>
    public DbSet<Author> Authors { get; set; }

    /// <summary>
    /// Gets or sets the DbSet for Books.
    /// </summary>
    public DbSet<Book> Books { get; set; }

    /// <summary>
    /// Gets or sets the DbSet for BookImages.
    /// </summary>
    public DbSet<BookImage> BookImages { get; set; }
}
