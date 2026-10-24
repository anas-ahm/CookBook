using Microsoft.AspNetCore.Mvc.RazorPages;
using CookBookLibrary.Service;
using CookBookLibrary.Model;

namespace CookBookRazor.Pages;

public class RecipeDetails : PageModel
{
    // Connecting to service
    private readonly CookBookService _cookBookService;

    public RecipeDetails(CookBookService cookBookService)
    {
        _cookBookService = cookBookService;
    }

    // List for RecipeIngredients objects to be shown in OnGet
    public List<RecipeIngredient> recipeIngredients { get; set; }
    
    public void OnGet()
    {
        
    }
}