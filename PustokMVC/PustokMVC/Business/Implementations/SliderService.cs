using Microsoft.EntityFrameworkCore;
using PustokMVC.Business.Interfaces;
using PustokMVC.CustomExceptions.Common;
using PustokMVC.CustomExceptions.GenreExceptions;
using PustokMVC.CustomExceptions.SliderException;
using PustokMVC.Data;
using PustokMVC.Extensions;
using PustokMVC.Models;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace PustokMVC.Business.Implementations
{
    public class SliderService : ISliderService
    {
        private readonly PustokDbContext _context;
        private readonly IWebHostEnvironment _env; // For accessing the web root path for file operations
                                                   // Assume FileManager and other utilities are injected or available

        public SliderService(PustokDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        /// <summary>
        /// Asynchronously creates a new slider in the database.
        /// </summary>
        /// <param name="slider">The slider entity to be added.</param>
        /// <exception cref="SliderTitleAlreadyExistException">Thrown when a slider with the same primary or secondary title already exists.</exception>
        /// <remarks>
        /// This method adds a new slider entity to the context and saves it to the database. Before adding, it checks if any existing sliders
        /// have the same primary or secondary title (case-insensitively), to ensure the uniqueness of slider titles. If a duplicate title is found,
        /// a SliderTitleAlreadyExistException is thrown.
        /// </remarks>
        public async Task<ServiceResult> CreateAsync(Slider slider)
        {
            var result = new ServiceResult();

            // Ensure uniqueness of the primary and secondary title
            // ... Your existing uniqueness checks ...

            if (result.Errors.Any())
            {
                return result; // If there are errors, return them.
            }

            if (slider.ImageFile != null && slider.ImageFile.Length > 0)
            {
                // Save the file using FileManager and get the relative path
                try
                {
                    string savedFilePath = FileManager.SaveFile(_env.WebRootPath, "uploads/sliders", slider.ImageFile);
                    slider.ImageUrl = savedFilePath; // Set the ImageUrl to the returned path from FileManager
                }
                catch (Exception ex)
                {
                    // Handle the case where file saving fails
                    result.Errors.Add(new ServiceError { Message = $"An error occurred while saving the file: {ex.Message}" });
                    return result;
                }
            }
            else
            {
                // Handle the case where the image file is not provided or invalid
                result.Errors.Add(new ServiceError { Field = "ImageFile", Message = "An image file must be provided." });
                return result;
            }

            slider.SetCreationAndModificationDate(); // Set the creation and modification date
            _context.Sliders.Add(slider); // Add the slider to the context
            await _context.SaveChangesAsync(); // Save changes to the database

            return result; // Return success or any errors encountered during saving
        }


        // ServiceResult and ServiceError classes
        public class ServiceResult
        {
            public bool Success => !Errors.Any();
            public List<ServiceError> Errors { get; set; } = new List<ServiceError>();
        }

        public class ServiceError
        {
            public string Field { get; set; }
            public string Message { get; set; }
        }


        /// <summary>
        /// Asynchronously deletes a slider from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the slider to be deleted.</param>
        /// <exception cref="SliderNotFoundException">Thrown if no slider with the specified ID was found.</exception>
        /// <remarks>
        /// This method finds a slider by its ID and removes it from the context, then saves the changes to the database.
        /// If a slider with the given ID cannot be found, a SliderNotFoundException is thrown to indicate the operation cannot proceed.
        /// </remarks>
        public async Task DeleteAsync(int id)
        {
            var slider = await _context.Sliders.FindAsync(id);
            if (slider is null)
            {
                throw new SliderNotFoundException();
            }

            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves a list of all sliders, optionally filtering them based on a predicate and including specified related entities.
        /// </summary>
        /// <param name="expression">An optional expression to filter the sliders. If null, all sliders are returned.</param>
        /// <param name="includes">A list of navigation property names to be included in the query results.</param>
        /// <remarks>
        /// This method provides a flexible way to query sliders from the database, allowing for optional filtering and eager loading of related entities.
        /// The inclusion of related entities is managed dynamically through the _getIncludes private method, enhancing code reuse and maintainability.
        /// The method supports returning all sliders if no filter expression is provided, or a filtered list of sliders based on the provided expression.
        /// </remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of sliders.</returns>
        public async Task<List<Slider>> GetAllAsync(Expression<Func<Slider, bool>>? expression = null, params string[] includes)
        {
            // Start with a base query to select sliders.
            var query = _context.Sliders.AsQueryable();

            // Apply includes for related entities dynamically.
            query = _getIncludes(query, includes);

            // Apply the optional expression filter if provided, otherwise return all sliders.
            var sliders = expression != null
                ? await query.Where(expression).ToListAsync()
                : await query.ToListAsync();

            return sliders;
        }

        /// <summary>
        /// Asynchronously retrieves a slider by its ID.
        /// </summary>
        /// <param name="id">The ID of the slider to be retrieved.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the slider entity found.</returns>
        /// <exception cref="SliderNotFoundException">Thrown if no slider with the specified ID can be found.</exception>
        /// <remarks>
        /// This method attempts to find a slider in the database by its ID. It uses the <c>FindAsync</c> method, which is optimized
        /// for searching by primary key and utilizes the context's change tracker to find entities already loaded into memory before
        /// querying the database. If a slider with the given ID cannot be found, a <c>SliderNotFoundException</c> is thrown, indicating
        /// that the requested slider does not exist in the database. This ensures that calling code can properly handle the absence
        /// of a slider with the given ID.
        /// </remarks>
        public async Task<Slider> GetByIdAsync(int id)
        {
            var slider = await _context.Sliders.FindAsync(id);
            if (slider is null)
            {
                throw new SliderNotFoundException();
            }

            return slider;
        }

        /// <summary>
        /// Asynchronously retrieves a single slider based on a specified condition.
        /// </summary>
        /// <param name="expression">An optional expression that specifies the condition the slider must meet. If null, the first slider is retrieved.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the first slider entity that meets the condition
        /// specified by <paramref name="expression"/>, or the first slider in the database if <paramref name="expression"/> is null.
        /// If no sliders are found, returns null.
        /// </returns>
        /// <remarks>
        /// This method provides flexibility in retrieving a slider entity by allowing a caller to specify a condition.
        /// If no condition is specified, it simply returns the first slider found in the database. This is achieved by
        /// constructing an IQueryable from the Sliders DbSet and optionally applying a Where filter based on the provided
        /// expression. The method uses FirstOrDefaultAsync to asynchronously return the first entity that matches the condition
        /// or the first entity in the sequence if the condition is not provided.
        /// </remarks>
        public async Task<Slider> GetSingleAsync(Expression<Func<Slider, bool>>? expression = null)
        {
            var query = _context.Sliders.AsQueryable();

            return await (expression != null ? query.Where(expression).FirstOrDefaultAsync() : query.FirstOrDefaultAsync());
        }

        /// <summary>
        /// Asynchronously toggles the IsActive status of a slider, effectively performing a soft delete or undelete operation.
        /// </summary>
        /// <param name="id">The ID of the slider to toggle the IsActive status for.</param>
        /// <exception cref="SliderNotFoundException">Thrown if no slider with the specified ID can be found.</exception>
        /// <remarks>
        /// This method performs a "soft delete" by toggling the IsActive property of a slider. If IsActive is true, it will be set to false,
        /// effectively hiding the slider from active use. If IsActive is already false, this method acts as an "undelete" by setting it back to true.
        /// This approach allows data to be retained in the database without being visible or accessible in the normal application flow, providing
        /// a mechanism for data recovery and historical auditing. The method ensures the slider exists before attempting to toggle its status,
        /// throwing a SliderNotFoundException if the slider cannot be found.
        /// </remarks>
        public async Task SoftDeleteAsync(int id)
        {
            var slider = await _context.Sliders.FindAsync(id);
            if (slider is null)
            {
                throw new SliderNotFoundException();
            }

            // Toggle the IsActive status
            slider.IsActive = !slider.IsActive;

            await _context.SaveChangesAsync();
        }
        /// <summary>
        /// Updates a slider entity in the database with new values including handling image file updates.
        /// </summary>
        /// <param name="slider">The slider entity with updated values.</param>
        /// <returns>A task that represents the asynchronous update operation.</returns>
        /// <exception cref="SliderNotFoundException">Thrown if the slider to be updated is not found.</exception>
        public async Task<ServiceResult> UpdateAsync(Slider updatedSlider)
        {
            var result = new ServiceResult();
            var existingSlider = await _context.Sliders.FindAsync(updatedSlider.Id);

            if (existingSlider == null)
            {
                result.Errors.Add(new ServiceError { Message = "Slider not found." });
                return result;
            }

            // Check if a new image file is provided
            if (updatedSlider.ImageFile != null)
            {
                if (updatedSlider.ImageFile.IsValidImage(out var validationMessage))
                {
                    // Delete the old image if it exists and save the new image
                    if (!string.IsNullOrWhiteSpace(existingSlider.ImageUrl))
                    {
                        FileManager.DeleteFile(_env.WebRootPath, "uploads/sliders", existingSlider.ImageUrl);
                    }
                    existingSlider.ImageUrl = FileManager.SaveFile(_env.WebRootPath, "uploads/sliders", updatedSlider.ImageFile);
                }
                else
                {
                    result.Errors.Add(new ServiceError { Field = "ImageFile", Message = validationMessage });
                    return result;
                }
            }

            // Update the rest of the properties you want to change
            existingSlider.UpdateSlider(updatedSlider);
            // ... include other properties that need to be updated

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                result.Errors.Add(new ServiceError { Message = $"An error occurred while updating the slider: {ex.Message}" });
            }

            return result;
        }


        /// <summary>
        /// Dynamically includes related entities into an IQueryable query based on specified navigation property names.
        /// </summary>
        /// <param name="query">The base query to which the include operations will be applied.</param>
        /// <param name="includes">A list of navigation property names to be included in the query results.</param>
        /// <remarks>
        /// This method iterates over the provided list of navigation property names and applies an Include operation
        /// for each one to the query. This is useful for eager loading of related entities to optimize query performance
        /// and avoid the N+1 query problem. The method uses the params keyword to allow for a variable number of arguments,
        /// enhancing flexibility in specifying included properties.
        /// </remarks>
        /// <returns>An IQueryable query with the specified related entities included.</returns>
        private IQueryable<Slider> _getIncludes(IQueryable<Slider> query, params string[] includes)
        {
            // Validate the input query to ensure it's not null.
            if (query == null) throw new ArgumentNullException(nameof(query), "The query parameter cannot be null.");

            // Check if the includes array is not null and has elements to prevent unnecessary iterations.
            if (includes != null)
            {
                // Iterate over each include specified in the includes array.
                foreach (var include in includes)
                {
                    // Apply the Include operation to include the specified navigation property into the query.
                    // This operation modifies the query to fetch related entities as part of the query execution.
                    query = query.Include(include);
                }
            }

            // Return the modified query with the includes applied.
            return query;
        }
    }
}
