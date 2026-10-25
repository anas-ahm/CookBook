using Microsoft.AspNetCore.Mvc.RazorPages;
using CookBookLibrary.Service;
using CookBookLibrary.Model;

namespace CookBookRazor.Pages;

public class IndexModel : PageModel
{
    private readonly RecipeService _recipeService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(RecipeService recipeService, ILogger<IndexModel> logger)
    {
        _recipeService = recipeService;
        _logger = logger;
    }
    
    public List<Recipe> allRecipes { get; set; }
    
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
}