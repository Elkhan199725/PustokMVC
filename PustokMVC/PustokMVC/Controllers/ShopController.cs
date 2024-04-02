using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PustokMVC.Business.Interfaces;
using PustokMVC.Data;
using PustokMVC.Models;
using PustokMVC.ViewModels;

namespace PustokMVC.Controllers
{
    public class ShopController : Controller
    {
        private readonly IBookService _bookService;
        private readonly PustokDbContext _context;

        public ShopController(
                IBookService bookService,
                PustokDbContext context)
        {
            _bookService = bookService;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail(int id) 
        {
            var book = await _bookService.GetSingleAsync(x=>x.Id == id,"BookImages","Genre","Author");
            return View(book);
        }
        public async Task<IActionResult> AddToBasket(int bookId)
        {
            if(!await _context.Books.AnyAsync(x=>x.Id == bookId)) return NotFound(); // 404

            List<BasketItemViewModel> basketItems = new List<BasketItemViewModel>();
            BasketItemViewModel basketItem = null;
            var basketItemsStr = HttpContext.Request.Cookies["BasketItems"];

            if(basketItemsStr is not null)
            {
                basketItems = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(basketItemsStr);

                basketItem = basketItems.FirstOrDefault(x=>x.BookId == bookId);

                if(basketItem is not null)
                {
                    basketItem.Count++;
                }
                else
                {
                    basketItem = new BasketItemViewModel()
                    {
                        BookId = bookId,
                        Count = 1
                    };

                    basketItems.Add(basketItem);
                }
            }
            else
            {
                basketItem = new BasketItemViewModel()
                {
                    BookId = bookId,
                    Count = 1
                };

                basketItems.Add(basketItem);
            }

            basketItemsStr = JsonConvert.SerializeObject(basketItems);

            HttpContext.Response.Cookies.Append("BasketItems", basketItemsStr);

            return Ok(); //200
        }

        public IActionResult GetBasketItems()
        {
            List<BasketItemViewModel> basketItems = new List<BasketItemViewModel>();

            var basketItemsStr = HttpContext.Request.Cookies["BasketItems"];

            if(basketItemsStr is not null)
            {
                basketItems = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(basketItemsStr);
            }

            return Ok(basketItems);

        }

        //public IActionResult SetSession(int id)
        //{
        //    HttpContext.Session.SetString("UserName", id.ToString());

        //    return Content("Added to session");
        //}

        //public IActionResult GetSession()
        //{
        //    string? name =  HttpContext.Session.GetString("UserName");

        //    return Content(name);
        //}

        //public IActionResult RemoveSession()
        //{
        //    HttpContext.Session.Remove("UserName");
        //    return Content("Gorbagor oldu");

        //}

        //public IActionResult SetCookie(int id)
        //{
        //    List<int> ids = new List<int>();

        //    var idsStr = HttpContext.Request.Cookies["Ids"];

        //    if(idsStr is not null)
        //    {
        //        ids = JsonConvert.DeserializeObject<List<int>>(idsStr);
        //        ids.Add(id);
        //    }
        //    else
        //    {
        //        ids.Add(id);
        //    }

        //    idsStr = JsonConvert.SerializeObject(ids);

        //    HttpContext.Response.Cookies.Append("Ids", idsStr);

        //    return Content("Added to Cookie");
        //}

        //public IActionResult GetCookie()
        //{
        //    List<int> ids = new List<int>();
        //    var idsStr = HttpContext.Request.Cookies["Ids"];

        //    if(idsStr is not null) 
        //    {
        //        ids = JsonConvert.DeserializeObject<List<int>>(idsStr);
        //    }

        //    return Ok(ids);
        //}


    }
}
