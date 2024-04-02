using Microsoft.EntityFrameworkCore;
using PustokMVC.Business.Interfaces;
using PustokMVC.CustomExceptions.BookExceptions;
using PustokMVC.Data;
using PustokMVC.Extensions;
using PustokMVC.Models;
using System.Linq.Expressions;

namespace PustokMVC.Business.Implementations
{
    public class BookService : IBookService
    {
        private readonly PustokDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BookService(PustokDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task CreateAsync(Book book)
        {
            if (book.PosterImageFile.ContentType != "image/jpeg" && book.PosterImageFile.ContentType != "image/png")
            {
                throw new BookInvalidCredentialException("PosterImageFile", "Content type must be png or jpeg!");
            }

            if (book.PosterImageFile.Length > 2097152)
            {
                throw new BookInvalidCredentialException("PosterImageFile", "Size must be lower than 2mb!");
            }
            BookImage posterImage = new BookImage()
            {
                Book = book,
                ImageUrl = book.PosterImageFile.SaveFile(_env.WebRootPath, "uploads/books"),
                IsPoster = true
            };
            await _context.BookImages.AddAsync(posterImage);

            if (book.HoverImageFile.ContentType != "image/jpeg" && book.HoverImageFile.ContentType != "image/png")
            {
                throw new BookInvalidCredentialException("HoverImageFile", "Content type must be png or jpeg!");
            }

            if (book.HoverImageFile.Length > 2097152)
            {
                throw new BookInvalidCredentialException("HoverImageFile", "Size must be lower than 2mb!");
            }
            BookImage hoverImage = new BookImage()
            {
                Book = book,
                ImageUrl = book.HoverImageFile.SaveFile(_env.WebRootPath, "uploads/books"),
                IsPoster = false
            };
            await _context.BookImages.AddAsync(hoverImage);


            if (book.ImageFiles is not null)
            {
                foreach (var imageFile in book.ImageFiles)
                {
                    if (imageFile.ContentType != "image/jpeg" && imageFile.ContentType != "image/png")
                    {
                        throw new BookInvalidCredentialException("ImageFiles", "Content type must be png or jpeg!");
                    }

                    if (imageFile.Length > 2097152)
                    {
                        throw new BookInvalidCredentialException("ImageFiles", "Size must be lower than 2mb!");
                    }
                    BookImage bookImage = new BookImage()
                    {
                        Book = book,
                        ImageUrl = imageFile.SaveFile(_env.WebRootPath, "uploads/books"),
                        IsPoster = null
                    };
                    await _context.BookImages.AddAsync(bookImage);
                }
            }

            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Book>> GetAllAsync(Expression<Func<Book, bool>>? expression = null, params string[] includes)
        {
            var query = _context.Books.AsQueryable();

            query = _getIncludes(query, includes);

            return expression is not null
                        ? await query.Where(expression).ToListAsync()
                        : await query.ToListAsync();
        }

        public Task<Book> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Book> GetSingleAsync(Expression<Func<Book, bool>>? expression = null, params string[] includes)
        {
            var query = _context.Books.AsQueryable();

            query = _getIncludes(query, includes);

            return expression is not null
                        ? await query.Where(expression).FirstOrDefaultAsync()
                        : await query.FirstOrDefaultAsync();
        }

        public Task SoftDeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Book Book)
        {
            throw new NotImplementedException();
        }

        private IQueryable<Book> _getIncludes(IQueryable<Book> query, params string[] includes)
        {
            if (includes is not null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return query;
        }
    }
}
