using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace PustokMVC.Models
{
    /// <summary>
    /// Represents a slider entity for the application, typically used for displaying banners on the website.
    /// </summary>
    /// <remarks>
    /// The Slider class includes properties for titles, description, redirect URL, and related image details.
    /// It inherits from the BaseEntity class, gaining common entity properties such as ID.
    /// </remarks>
    public class Slider : BaseEntity
    {
        /// <summary>
        /// The primary title of the slider.
        /// </summary>
        /// <remarks>
        /// This title is required and has a maximum length of 20 characters.
        /// </remarks>
        [Required]
        [StringLength(20)]
        public string? Title1 { get; set; }

        /// <summary>
        /// The secondary title of the slider.
        /// </summary>
        /// <remarks>
        /// This title is also required and has a maximum length of 20 characters.
        /// </remarks>
        [Required]
        [StringLength(20)]
        public string? Title2 { get; set; }

        /// <summary>
        /// The description associated with the slider.
        /// </summary>
        /// <remarks>
        /// This field is required and supports up to 150 characters.
        /// </remarks>
        [Required]
        [StringLength(150)]
        public string? Description { get; set; }

        /// <summary>
        /// The URL to which the slider will redirect when clicked.
        /// </summary>
        /// <remarks>
        /// This property is optional.
        /// </remarks>
        public string? RedirectUrl { get; set; }

        /// <summary>
        /// The text to display for the redirect URL.
        /// </summary>
        /// <remarks>
        /// This text is required and supports up to 40 characters.
        /// </remarks>
        [Required]
        [StringLength(40)]
        public string? RedirectUrlText { get; set; }

        /// <summary>
        /// The URL of the image associated with the slider.
        /// </summary>
        /// <remarks>
        /// This property is optional and supports up to 100 characters.
        /// </remarks>
        [StringLength(100)]
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Represents the image file for the slider, used for upload purposes.
        /// </summary>
        /// <remarks>
        /// This property is not mapped to the database and is solely used for file uploads.
        /// </remarks>
        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        public void UpdateSlider(Slider updatedSlider)
        {
            Title1 = updatedSlider.Title1;
            Title2 = updatedSlider.Title2;
            Description = updatedSlider.Description;
            RedirectUrl = updatedSlider.RedirectUrl;
            RedirectUrlText = updatedSlider.RedirectUrlText;
            // Assuming ImageUrl is handled separately due to file upload
            // IsActive, CreatedDate, and ModifiedDate are handled elsewhere if needed
        }
    }
}
