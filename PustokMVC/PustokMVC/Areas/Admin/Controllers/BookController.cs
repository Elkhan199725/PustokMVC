using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokMVC.Data;
using PustokMVC.Extensions;
using PustokMVC.Models;

namespace PustokMVC.Areas.Admin.Controllers
{
    [Area("admin")]
    public class BookController : Controller
    {
        private readonly PustokDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BookController(PustokDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
            => View(await _context.Books.Include(x=>x.Author).Include(x=>x.Genre).Include(x=>x.BookImages).ToListAsync());

        public async Task<IActionResult> Create()
        {
            ViewBag.Genres = await _context.Genres.ToListAsync();
            ViewBag.Authors = await _context.Authors.ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            ViewBag.Genres = await _context.Genres.ToListAsync();
            ViewBag.Authors = await _context.Authors.ToListAsync();
            if (!ModelState.IsValid) return View();
            if (book.PosterImageFile.ContentType != "image/jpeg" && book.PosterImageFile.ContentType != "image/png")
            {
                ModelState.AddModelError("PosterImageFile", "Content type must be png or jpeg!");
                return View();
            }

            if (book.PosterImageFile.Length > 2097152)
            {
                ModelState.AddModelError("PosterImageFile", "Size must be lower than 2mb!");
                return View();
            }
            //BookImage posterImage = new BookImage()
            //{
            //    Book = book,
            //    ImageUrl = book.PosterImageFile.SaveFile(_env.WebRootPath, "uploads/books",book.ImageFiles),
            //    IsPoster = true
            //};
            //await _context.BookImages.AddAsync(posterImage);

            //if (book.HoverImageFile.ContentType != "image/jpeg" && book.HoverImageFile.ContentType != "image/png")
            //{
            //    ModelState.AddModelError("HoverImageFile", "Content type must be png or jpeg!");
            //    return View();
            //}

            //if (book.HoverImageFile.Length > 2097152)
            //{
            //    ModelState.AddModelError("HoverImageFile", "Size must be lower than 2mb!");
            //    return View();
            //}
            //BookImage hoverImage = new BookImage()
            //{
            //    Book = book,
            //    ImageUrl = book.HoverImageFile.SaveFile(_env.WebRootPath, "uploads/books", book.ImageFiles),
            //    IsPoster = false
            //};
            //await _context.BookImages.AddAsync(hoverImage);


            if (book.ImageFiles is not null)
            {
                foreach (var imageFile in book.ImageFiles)
                {
                    if (imageFile.ContentType != "image/jpeg" && imageFile.ContentType != "image/png")
                    {
                        ModelState.AddModelError("ImageFiles", "Content type must be png or jpeg!");
                        return View(book); // Ensure to return the model to persist form data and show errors
                    }

                    if (imageFile.Length > 2097152) // 2 MB
                    {
                        ModelState.AddModelError("ImageFiles", "Size must be lower than 2mb!");
                        return View(book); // Ensure to return the model to persist form data and show errors
                    }

                    // Generate the file name and save the file
                    var savedFileName = FileManager.SaveFile(_env.WebRootPath, "uploads/books", imageFile); // Correct usage

                    BookImage bookImage = new BookImage()
                    {
                        Book = book,
                        ImageUrl = savedFileName, // Use the returned file name which includes the path
                        IsPoster = null // Assuming you handle the IsPoster assignment elsewhere
                    };
                    await _context.BookImages.AddAsync(bookImage);
                }
            }

            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
