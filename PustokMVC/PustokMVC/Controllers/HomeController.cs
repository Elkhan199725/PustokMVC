using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokMVC.Business.Interfaces;
using PustokMVC.Data;
using PustokMVC.ViewModels;

namespace PustokMVC.Controllers;

/// <summary>
/// Manages the home page interactions by serving the necessary view models to the view.
/// </summary>
/// <remarks>
/// The HomeController facilitates the fetching and preparation of data required by the home view,
/// such as sliders and genres, demonstrating the use of asynchronous operations within an MVC application.
/// </remarks>
public class HomeController : Controller
{
    private readonly PustokDbContext _context;
    private readonly IGenreService _genreService;

    /// <summary>
    /// Initializes a new instance of the HomeController class.
    /// </summary>
    /// <param name="context">The database context used for accessing application data.</param>
    /// <param name="genreService">The service responsible for handling genre data operations.</param>
    /// <remarks>
    /// This constructor leverages dependency injection to obtain instances of PustokDbContext and IGenreService,
    /// enabling the controller to perform data operations.
    /// </remarks>
    public HomeController(PustokDbContext context, IGenreService genreService)
    {
        _context = context;
        _genreService = genreService;
    }

    /// <summary>
    /// Asynchronously renders the home page with sliders and genre data.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation, yielding an IActionResult that renders the Index view.</returns>
    /// <remarks>
    /// The Index action method asynchronously retrieves sliders from the database and genres through the genre service,
    /// packaging them into a HomeViewModel. This approach demonstrates the handling of asynchronous operations
    /// and the use of services for data retrieval in an ASP.NET Core MVC application.
    /// Note: The lambda expression used in fetching genres can be adjusted based on the requirement to fetch
    /// active or inactive genres, ensuring the view is populated with relevant data.
    /// </remarks>
    public async Task<IActionResult> Index()
    {
        var homeViewModel = new HomeViewModel
        {
            Sliders = await _context.Sliders.ToListAsync(),
            Genres = await _genreService.GetAllAsync(x => x.IsActive)
        };
        return View(homeViewModel);
    }
}
