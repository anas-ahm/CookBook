using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CookBookLibrary.Service;
using CookBookLibrary.Model;
using Microsoft.AspNetCore.Mvc;

namespace CookBookRazor.Pages;

public class CreateFoodItem : PageModel
{
    // Connecting to service & ILogger
    private readonly FoodItemService _foodItemService;
    private readonly RecipeService _recipeService;
    private readonly ILogger<CreateFoodItem> _logger;
    public CreateFoodItem(FoodItemService foodItemService, ILogger<CreateFoodItem> logger, RecipeService recipeService)
    {
        _foodItemService = foodItemService;
        _logger = logger;
        _recipeService = recipeService;
    }
    
    [BindProperty] public FoodItem foodItem { get; set; }
    
    public void OnGet()
    {
    }

    public IActionResult OnPostCreateFoodItem()
    {
        try
        {
            _foodItemService.CreateFoodItem(foodItem);
            TempData["successMessage"] = "Food item created successfully!";
            return RedirectToPage("/admin/CreateFoodItem");
        }
        catch (Exception ex)
        {
            _logger.LogError("Couldn't create fooditem");
            TempData["errorMessage"] = "Couldn't create food item, try again later :(";
            return RedirectToPage("/admin/CreateFoodItem");
        }
    }
    
}