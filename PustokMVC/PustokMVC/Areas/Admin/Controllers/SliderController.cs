using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokMVC.Data;
using PustokMVC.Extensions;
using PustokMVC.Models;

namespace PustokMVC.Areas.Admin.Controllers
{
    /// <summary>
    /// Manages CRUD operations for Sliders within the Admin area.
    /// </summary>
    /// <remarks>
    /// The SliderController handles the creation, reading, updating, and deletion of Slider entities.
    /// It utilizes the PustokDbContext for data access and IWebHostEnvironment for file management tasks.
    /// This controller ensures that slider images meet specified criteria (format and size) before proceeding with database operations.
    /// </remarks>
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly PustokDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderController(PustokDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        /// <summary>
        /// Displays a list of all sliders.
        /// </summary>
        /// <returns>A view with a list of sliders.</returns>
        public async Task<IActionResult> Index() => View(await _context.Sliders.ToListAsync());

        /// <summary>
        /// Renders the Create view for sliders.
        /// </summary>
        /// <returns>The Create view.</returns>
        public IActionResult Create() => View();

        /// <summary>
        /// Handles the submission of the Create form for sliders, including image validation and storage.
        /// </summary>
        /// <param name="slider">The slider to create.</param>
        /// <returns>Redirects to the Index action if successful, otherwise returns the view with validation messages.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slider slider)
        {
            if (!ModelState.IsValid) return View(slider);

            if (!slider.ImageFile.IsValidImage(out var validationMessage))
            {
                ModelState.AddModelError("ImageFile", validationMessage);
                return View(slider);
            }

            slider.ImageUrl = slider.ImageFile.SaveFile(_env.WebRootPath, "uploads/sliders");
            slider.SetCreationAndModificationDate(); // Assuming extension method or Slider model method

            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Displays the Update view for a slider.
        /// </summary>
        /// <param name="id">The ID of the slider to update.</param>
        /// <returns>The Update view if slider found, otherwise NotFound.</returns>
        public async Task<IActionResult> Update(int id)
        {
            var existSlider = await _context.Sliders.FindAsync(id);
            if (existSlider == null) return NotFound();

            return View(existSlider);
        }


        /// <summary>
        /// Updates an existing slider based on provided information.
        /// </summary>
        /// <param name="slider">The updated slider information.</param>
        /// <returns>Redirects to the Index action if successful, otherwise returns to the view with validation messages.</returns>
        /// <remarks>
        /// This method performs several critical functions:
        /// - Validates the model state to ensure that the submitted data meets all required constraints.
        /// - Finds the existing slider in the database. If not found, returns a NotFound result.
        /// - Validates the uploaded image file if present, ensuring it meets specific criteria (e.g., file type, size).
        /// - Deletes the old image file and saves the new one, updating the slider's ImageUrl to reference the new file.
        /// - Updates other properties of the slider using the UpdateSlider method defined in the Slider model.
        /// - Saves changes to the database and redirects to the Index action, or returns validation messages if necessary.
        /// </remarks>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Slider slider)
        {
            if (!ModelState.IsValid) return View(slider);

            var existingSlider = await _context.Sliders.FindAsync(slider.Id);
            if (existingSlider == null) return NotFound();

            if (slider.ImageFile != null)
            {
                if (!slider.ImageFile.IsValidImage(out var validationMessage))
                {
                    ModelState.AddModelError("ImageFile", validationMessage);
                    return View(slider);
                }

                FileManager.DeleteFile(_env.WebRootPath, "uploads/sliders", existingSlider.ImageUrl);
                existingSlider.ImageUrl = slider.ImageFile.SaveFile(_env.WebRootPath, "uploads/sliders");
            }

            existingSlider.UpdateSlider(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Handles the deletion of a slider.
        /// </summary>
        /// <param name="id">The ID of the slider to delete.</param>
        /// <returns>Ok result if deletion is successful, otherwise NotFound.</returns>
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
