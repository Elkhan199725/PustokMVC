using Microsoft.EntityFrameworkCore;
using PustokMVC.Business.Implementations;
using PustokMVC.Business.Interfaces;
using PustokMVC.Data;

var builder = WebApplication.CreateBuilder(args);

// Services are configured here.
ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

// The HTTP request pipeline is configured here.
ConfigureMiddleware(app);

app.Run();

/// <summary>
/// Configures services for the application.
/// </summary>
/// <param name="services">The IServiceCollection to add services to.</param>
/// <param name="configuration">The application's configuration properties.</param>
/// <remarks>
/// This method is responsible for adding all the required services to the application,
/// including setting up MVC, Entity Framework Core, and any custom services such as GenreService.
/// It utilizes dependency injection provided by ASP.NET Core to manage service lifetimes and dependencies.
/// </remarks>
void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddControllersWithViews();
    services.AddDbContext<PustokDbContext>(opt =>
        opt.UseSqlServer(configuration.GetConnectionString("default")));
    services.AddScoped<IGenreService, GenreService>();
}

/// <summary>
/// Configures the middleware pipeline for handling HTTP requests.
/// </summary>
/// <param name="app">The application builder to configure.</param>
/// <remarks>
/// This method sets up the middleware components that will handle every HTTP request to the application,
/// including error handling, security, static file serving, and routing.
/// It's critical for configuring how the app responds to requests and structures its responses.
/// </remarks>
void ConfigureMiddleware(WebApplication app)
{
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthorization();

    ConfigureRoutes(app);
}

/// <summary>
/// Configures the routes for the application.
/// </summary>
/// <param name="app">The application builder to define routes for.</param>
/// <remarks>
/// Defines the routing strategy for the application. This includes setting up default routes
/// and routes for areas, which help organize the app into segments (e.g., admin area).
/// Routing is essential for mapping incoming requests to controller actions.
/// </remarks>
void ConfigureRoutes(WebApplication app)
{
    app.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
}
