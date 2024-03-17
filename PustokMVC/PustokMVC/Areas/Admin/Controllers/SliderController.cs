using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokMVC.Data;
using PustokMVC.Extensions;
using PustokMVC.Models;

namespace PustokMVC.Areas.Admin.Controllers
{
    [Area("admin")]
    public class SliderController : Controller
    {
        private readonly PustokDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderController(PustokDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
            => View(await _context.Sliders.ToListAsync());


        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slider slider)
        {
            if(!ModelState.IsValid) return View();
            // Image Content Type
            if(slider.ImageFile is not null)
            {
                if (slider.ImageFile.ContentType != "image/jpeg" && slider.ImageFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("ImageFile", "Content type must be png or jpeg!");
                    return View();
                }

                // Image size
                if (slider.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "Size must be lower than 2mb!");
                    return View();
                }

                //slider.ImageUrl = FileManager.SaveFile(_env.WebRootPath, "uploads/sliders", slider.ImageFile);
                slider.ImageUrl = slider.ImageFile.SaveFile(_env.WebRootPath, "uploads/sliders");
            }
            else
            {
                ModelState.AddModelError("ImageFile", "Image is required!");
                return View();
            }
            
            slider.CreatedDate = DateTime.UtcNow.AddHours(4);
            slider.ModifiedDate = DateTime.UtcNow.AddHours(4);
            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var existSlider = await _context.Sliders.FindAsync(id);
            if (existSlider is null) throw new Exception();

            return View(existSlider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Slider slider)
        {
            if(!ModelState.IsValid) return View();
            var existSlider = await _context.Sliders.FindAsync(slider.Id);
            if (existSlider is null) throw new Exception();

            if(slider.ImageFile is not null)
            {
                if (slider.ImageFile.ContentType != "image/jpeg" && slider.ImageFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("ImageFile", "Content type must be png or jpeg!");
                    return View();
                }

                // Image size
                if (slider.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "Size must be lower than 2mb!");
                    return View();
                }

                FileManager.DeleteFile(_env.WebRootPath, "uploads/sliders", existSlider.ImageUrl);

                existSlider.ImageUrl = slider.ImageFile.SaveFile(_env.WebRootPath, "uploads/sliders");
            }

            existSlider.Title1 = slider.Title1;
            existSlider.Title2 = slider.Title2;
            existSlider.Description = slider.Description;
            existSlider.RedirectUrlText = slider.RedirectUrlText;
            existSlider.RedirectUrl = slider.RedirectUrl;
            existSlider.ModifiedDate = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var existSlider = await _context.Sliders.FindAsync(id);
            if (existSlider == null) return NotFound();

            FileManager.DeleteFile(_env.WebRootPath, "uploads/sliders", existSlider.ImageUrl);

            _context.Remove(existSlider);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
