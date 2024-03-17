using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace PustokMVC.Extensions
{
    public static class FileManager
    {
        public static string SaveFile(this IFormFile file, string rootPath, string folderName)
        {
            // Extract and preserve the original file extension
            var originalExtension = Path.GetExtension(file.FileName);
            // Generate a new filename with a Guid and original extension, ignoring the original file name for security reasons
            var fileName = $"{Guid.NewGuid()}{originalExtension}"; // Ensures uniqueness and maintains file type

            var path = Path.Combine(rootPath, folderName, fileName);

            try
            {
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            catch (Exception ex)
            {
                // Log the error or throw a custom exception
                // Consider how you want to handle exceptions here
                throw new InvalidOperationException("File could not be saved.", ex);
            }

            return fileName;
        }

        public static void DeleteFile(string rootPath, string folderName, string fileName)
        {
            var deletePath = Path.Combine(rootPath, folderName, fileName);

            try
            {
                if (File.Exists(deletePath))
                {
                    File.Delete(deletePath);
                }
            }
            catch (Exception ex)
            {
                // Log the error or handle it as needed
                // Consider if failing to delete a file should stop the operation or just log a warning
                Console.WriteLine($"Error deleting file: {ex.Message}");
            }
        }
    }
}
