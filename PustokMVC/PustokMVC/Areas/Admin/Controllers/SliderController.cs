using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokMVC.Business.Interfaces;
using PustokMVC.CustomExceptions.SliderException;
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
        private readonly ISliderService _sliderService;

        public SliderController(ISliderService sliderService)
        {
            _sliderService = sliderService;
        }

        // Display all sliders
        public async Task<IActionResult> Index(string searchTerm = null, int? detailsId = null)
        {
            IEnumerable<Slider> sliders;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                sliders = (IEnumerable<Slider>) await _sliderService.GetSingleAsync(s => s.Title1.Contains(searchTerm) || s.Title2.Contains(searchTerm));
                // Optionally filter sliders based on the search term
            }
            else
            {
                sliders = await _sliderService.GetAllAsync();
            }

            // If detailsId is provided, find that specific slider and add it to the ViewBag or ViewData for special handling in the view
            if (detailsId.HasValue)
            {
                var detailSlider = await _sliderService.GetByIdAsync(detailsId.Value);
                ViewBag.DetailSlider = detailSlider; // Pass this slider specifically for detailed view
            }

            return View(sliders); // Use the same view for listing and optionally showing details
        }
        public async Task<IActionResult> Details(int id)
        {
            // Attempt to fetch the slider. Consider checking IsActive if you're filtering out soft-deleted sliders.
            var slider = await _sliderService.GetSingleAsync(s => s.Id == id && s.IsActive);

            if (slider != null)
            {
                // If the slider exists (and is active), redirect to the Index action with the detailsId.
                return RedirectToAction("Index", new { detailsId = id });
            }
            else
            {
                // If no active slider is found with the given ID, return NotFound.
                return NotFound();
            }
        }
        // Display the form for creating a new slider
        public IActionResult Create()
        {
            return View(new Slider());
        }

        // Handle the creation of a new slider
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slider slider)
        {
            if (!ModelState.IsValid)
            {
                return View(slider);
            }

            try
            {
                await _sliderService.CreateAsync(slider);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(slider);
            }
        }

        // Display the form for editing an existing slider
        public async Task<IActionResult> Edit(int id)
        {
            var slider = await _sliderService.GetByIdAsync(id);
            if (slider == null)
            {
                return NotFound();
            }

            return View(slider);
        }

        // Handle the update of an existing slider
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Slider slider)
        {
            if (!ModelState.IsValid)
            {
                return View(slider);
            }

            var result = await _sliderService.UpdateAsync(slider);

            if (result.Errors.Any())
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Field, error.Message);
                }
                return View(slider);
            }

            return RedirectToAction(nameof(Index));
        }





        [HttpGet]
        public async Task<IActionResult> Search(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                // If the search term is empty, return to the Index with the full list of sliders.
                var sliders = await _sliderService.GetAllAsync();
                ViewBag.SearchMessage = "Please enter a search term.";
                return View("Index", sliders);
            }

            var matchedSlider = await _sliderService.GetSingleAsync(s => s.Title1.Contains(searchTerm) || s.Title2.Contains(searchTerm));
            if (matchedSlider != null)
            {
                // Wrap the single result in a list for compatibility with the Index view's model.
                return View("Index", new List<Slider> { matchedSlider });
            }
            else
            {
                // If no match is found, return to the Index with a message indicating no results.
                var allSliders = await _sliderService.GetAllAsync();
                ViewBag.SearchMessage = $"No sliders found with the term '{searchTerm}'.";
                return View("Index", allSliders);
            }
        }

        // Handle the soft delete of a slider
        public async Task<IActionResult> SoftDelete(int id)
        {
            try
            {
                await _sliderService.SoftDeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (SliderNotFoundException)
            {
                return NotFound();
            }
        }
        public async Task<IActionResult> Delete(int id)
        {
            var slider = await _sliderService.GetByIdAsync(id);
            if (slider == null)
            {
                return NotFound();
            }

            // Check if the slider is already soft-deleted
            if (!slider.IsActive)
            {
                try
                {
                    await _sliderService.DeleteAsync(id); // Perform hard delete
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Log the error and redirect to an appropriate error handling page
                    return BadRequest($"An error occurred: {ex.Message}");
                }
            }
            else
            {
                // Handle the case where the slider is not soft-deleted.
                // You might want to show an error message or redirect to a different page.
                return BadRequest("The slider must be deactivated before it can be permanently deleted.");
            }
        }
    }
}
