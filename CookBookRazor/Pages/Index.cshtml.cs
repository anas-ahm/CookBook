using Microsoft.AspNetCore.Mvc.RazorPages;
using CookBookLibrary.Service;
using CookBookLibrary.Model;

namespace CookBookRazor.Pages;

public class IndexModel : PageModel
{
    private readonly RecipeService _recipeService;
    private readonly ILogger<IndexModel> _logger;
    private readonly IWebHostEnvironment _env;

    public IndexModel(RecipeService recipeService, ILogger<IndexModel> logger, IWebHostEnvironment env)
    {
        _recipeService = recipeService;
        _logger = logger;
        _env = env;
    }
    
    public List<Recipe> allRecipes { get; set; } = new List<Recipe>();
    private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
    
    public void OnGet()
    {
        try
        {
            allRecipes = _recipeService.GetAllRecipes();
        }
        catch (Exception ex)
        {
            _logger.LogError("Couldn't get all recipes", ex);
            TempData["errorMessage"] = "Something went wrong getting all recipes, come back later";
        }
    }
    
    public string? GetImagePath(int id)
    {
        return AllowedExtensions
            .Select(ext => $"/RecipePictures/{id}{ext}")
            .FirstOrDefault(p => System.IO.File.Exists(Path.Combine(_env.WebRootPath, p.TrimStart('/'))));
    }
}