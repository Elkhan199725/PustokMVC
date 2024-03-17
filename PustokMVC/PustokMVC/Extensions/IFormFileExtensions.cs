using Microsoft.AspNetCore.Http;
using System;

namespace PustokMVC.Extensions;

public static class IFormFileExtensions
{
    /// <summary>
    /// Validates an uploaded image file for content type and size.
    /// </summary>
    /// <param name="file">The file to validate.</param>
    /// <param name="validationMessage">Out parameter that returns a validation message.</param>
    /// <returns>true if the image is valid; otherwise, false.</returns>
    public static bool IsValidImage(this IFormFile file, out string validationMessage)
    {
        validationMessage = string.Empty;

        // Check for null file
        if (file == null)
        {
            validationMessage = "Please provide a file.";
            return false;
        }

        // Validate content type
        if (file.ContentType != "image/jpeg" && file.ContentType != "image/png")
        {
            validationMessage = "Content type must be png or jpeg.";
            return false;
        }

        // Validate file size (2MB in this example)
        if (file.Length > 2 * 1024 * 1024)
        {
            validationMessage = "File size must be less than 2MB.";
            return false;
        }

        return true; // File is valid
    }
}
