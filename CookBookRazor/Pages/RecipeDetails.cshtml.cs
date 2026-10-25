using Microsoft.AspNetCore.Mvc.RazorPages;
using CookBookLibrary.Service;
using CookBookLibrary.Model;
using Microsoft.AspNetCore.Mvc;

namespace CookBookRazor.Pages;

public class RecipeDetails : PageModel
{
    // Connecting to service
    private readonly RecipeService _recipeService;

    public RecipeDetails(RecipeService recipeService)
    {
        _recipeService = recipeService;
    }
    
    public Recipe recipeToShow { get; set; }
    
    public void OnGet(int id)
    {
        recipeToShow = _recipeService.GetRecipeById(id)
    }
}