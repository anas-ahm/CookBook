namespace CookBookRazor.Service;

public class ImageService : IImageService
{
    
    private readonly IWebHostEnvironment _env;
    private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

    public ImageService(IWebHostEnvironment env)
    {
        _env = env;
    }
    
    public string? GetImagePath(int id)
    {
        return AllowedExtensions
            .Select(ext => $"/RecipePictures/{id}{ext}")
            .FirstOrDefault(p => System.IO.File.Exists(Path.Combine(_env.WebRootPath, p.TrimStart('/'))));
    }
}