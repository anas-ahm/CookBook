using Microsoft.AspNetCore.Mvc.RazorPages;
using CookBookRazor.Service;
using CookBookLibrary.Service;
using CookBookLibrary.Model;

namespace CookBookRazor.Pages;

public class IndexModel : PageModel
{
    private readonly RecipeService _recipeService;
    private readonly IImageService _imageService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(RecipeService recipeService, ILogger<IndexModel> logger, IWebHostEnvironment env, IImageService imageService)
    {
        _recipeService = recipeService;
        _imageService = imageService;
        _logger = logger;
    }
    
    public List<Recipe> allRecipes { get; set; } = new List<Recipe>();
    public string? GetImagePath(int id) => _imageService.GetImagePath(id);
    
    public void OnGet()
    {
        try
        {
            allRecipes = _recipeService.GetAllRecipes();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Couldn't get all recipes");
            TempData["errorMessage"] = "Something went wrong getting all recipes, come back later";
        }
    }
    
    
}