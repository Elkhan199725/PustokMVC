using PustokMVC.Models; // Importing models to be used in the view model.

namespace PustokMVC.ViewModels;

/// <summary>
/// Represents the data model for the home view, encapsulating slider and genre information.
/// </summary>
/// <remarks>
/// This view model is designed to bundle the necessary data for rendering the home page of the application.
/// It includes lists of sliders and genres, which are likely used to display dynamic content such as featured books,
/// categories, or promotional banners. This approach of using a view model helps in passing a strongly-typed
/// collection of data from the controller to the view, enhancing the maintainability and readability of the code.
/// </remarks>
public class HomeViewModel
{
    /// <summary>
    /// Gets or sets the collection of sliders.
    /// </summary>
    /// <value>A list of Slider objects.</value>
    /// <remarks>
    /// Sliders are typically used for carousel or slideshow components on the home page,
    /// showcasing featured books, promotions, or announcements.
    /// </remarks>
    public List<Slider>? Sliders { get; set; }

    /// <summary>
    /// Gets or sets the collection of genres.
    /// </summary>
    /// <value>A list of Genre objects.</value>
    /// <remarks>
    /// Genres represent the various categories or types of books available in the library or bookstore.
    /// This collection can be used to populate navigation menus, filters, or sections on the home page
    /// to assist users in discovering books by their preferred genres.
    /// </remarks>
    public List<Genre>? Genres { get; set; }
}
