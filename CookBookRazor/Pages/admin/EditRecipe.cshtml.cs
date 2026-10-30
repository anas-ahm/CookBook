using System.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CookBookLibrary.Model;
using CookBookLibrary.Service;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace CookBookRazor.Pages;

public class EditRecipe : PageModel
{
    private readonly RecipeService _recipeService;
    private readonly FoodItemService _foodItemService;
    private readonly ILogger<EditRecipe> _logger;

    public EditRecipe(RecipeService recipeService, FoodItemService foodItemService, ILogger<EditRecipe> logger)
    {
        _recipeService = recipeService;
        _foodItemService = foodItemService;
        _logger = logger;
    }

    // Food items for the dropdowns
    public List<FoodItem> FoodItems { get; set; } = new List<FoodItem>();

    [BindProperty]
    public Recipe RecipeToEdit { get; set; } = new Recipe();

    public IActionResult OnGet(int id)
    {
        try
        {
            RecipeToEdit = _recipeService.GetByIdRecipe(id);
            FoodItems = _foodItemService.GetAllFoodItems();
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cant get Recipe (PageModel)");
            TempData["ErrorMessage"] = "Couldn't find recipe to edit";
            return RedirectToPage("/Index");
        }
    }

    public IActionResult OnPost()
    {
        // Keep only the rows the user actually filled in
        List<RecipeIngredient> filledIngredients = new List<RecipeIngredient>();

        foreach (RecipeIngredient ingredient in RecipeToEdit._ingredients)
        {
            if (ingredient.FoodItem != null && ingredient.FoodItem.Id > 0 && ingredient.Grams > 0)
            {
                filledIngredients.Add(ingredient);
            }
        }

        RecipeToEdit._ingredients = filledIngredients;

        try
        {
            _recipeService.Update(RecipeToEdit, RecipeToEdit._id);
            FoodItems = _foodItemService.GetAllFoodItems();
            TempData["SuccessMessage"] = $"{RecipeToEdit._name} has been updated successfully!";
            return Page();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Same food item used multiple times (PageModel)");
            TempData["ErrorMessage"] = "You can't have the same food item twice!";
            FoodItems = _foodItemService.GetAllFoodItems();
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cant edit the recipe (PageModel)");
            TempData["ErrorMessage"] = "Couldn't edit the recipe at this moment, try again later";
            FoodItems = _foodItemService.GetAllFoodItems();
            return Page();
        }
    }
    
    public IActionResult OnPostDelete(int id)
    {
        try
        {
            Recipe recipe = _recipeService.GetByIdRecipe(id);
            TempData["SuccessMessage"] =$"{recipe._name} has been deleted successfully!";
            _recipeService.Delete(id);
            return RedirectToPage("/Index");
        }
        
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cant delete the recipe (PageModel)");
            TempData["ErrorMessage"] = "Couldn't delete the recipe at this moment, try again later";
            return Page();
        }
    }
}