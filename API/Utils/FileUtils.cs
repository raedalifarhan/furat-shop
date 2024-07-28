namespace API.Lib
{
    public static class FileUtils
    {
        public static async Task<string> SaveImageAsync(IWebHostEnvironment _env, IFormFile? image)
        {
            if (image == null || image.Length == 0)
            {
                throw new ArgumentException("Image file is required.");
            }

            // Validate file extension
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(image.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new ArgumentException("Invalid image file format.");
            }

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            var fileName = Guid.NewGuid().ToString() + fileExtension;
            var filePath = Path.Combine(uploadsFolder, fileName);

            // Save the new file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return $"/uploads/{fileName}";
        }

        public static void DeleteImage(IWebHostEnvironment _env, string imageUrl)
        {
            if (!string.IsNullOrEmpty(imageUrl))
            {
                var filePath = Path.Combine(_env.WebRootPath, imageUrl.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
        }

    }
}
