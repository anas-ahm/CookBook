using System.Net.Mime;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CookBookLibrary.Service;
using CookBookLibrary.Model;
using Microsoft.AspNetCore.Mvc;

namespace CookBookRazor.Pages;

public class CreateRecipe : PageModel
{
    // Connecting to service & ILogger
    private readonly FoodItemService _foodItemService;
    private readonly RecipeService _recipeService;
    private readonly ILogger<CreateRecipe> _logger;
    private readonly IWebHostEnvironment _env;
    
    public CreateRecipe(FoodItemService foodItemService, ILogger<CreateRecipe> logger, RecipeService recipeService, IWebHostEnvironment env)
    {
        _foodItemService = foodItemService;
        _logger = logger;
        _recipeService = recipeService;
        _env = env;
    }
    
    
    // List for FoodItem objects to be shown in OnGet
    public List<FoodItem> FoodItems { get; set; }
    
    // Parameters for building Recipe Object
    [BindProperty] public Recipe recipeToCreate { get; set; } = new Recipe();
    
    // Parameter for saving image
    [BindProperty] public IFormFile? image { get; set; }
    
    // allowed image formats
    private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
    private const long MaxBytes = 5 * 1024 * 1024;
    
    public void OnGet()
    {
        // Getting all RecipeIngredients
        try
        {
            FoodItems = _foodItemService.GetAllFoodItems();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            TempData["errorMessage"] = "Something went wrong, please try again later";
        }
    }

    
    
    public IActionResult OnPostCreateRecipe()
    {
        // Keep only the rows the user actually filled in
        List<RecipeIngredient> filledIngredients = new List<RecipeIngredient>();
        
        // Checking through all recipe values from form and puts the ones filled inside
        // filledIngredients list
        foreach (RecipeIngredient ingredient in recipeToCreate._ingredients)
        {
            if (ingredient.FoodItem != null && ingredient.FoodItem.Id > 0 && ingredient.Grams > 0)
            {
                filledIngredients.Add(ingredient);
            }
        }

        recipeToCreate._ingredients = filledIngredients;
        
        try
        {
            _recipeService.CreateRecipe(recipeToCreate);

            if (image != null || image.Length != 0)
            {
                
                if (image.Length > MaxBytes)
                {
                    TempData["errorMessage"] = "File is too large (max 5 MB).";
                    return Page();
                }
                
                var ext = Path.GetExtension(image.FileName).ToLowerInvariant();
                if (!AllowedExtensions.Contains(ext))
                {
                    TempData["errorMessage"] = "Only image files are allowed.";
                    return Page();
                }
                
            }
            
            
            TempData["successMessage"] = "Recipe Created Successfully!";
            return RedirectToPage("/admin/CreateRecipe"); 
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
            _logger.LogError(ex, "Couldnt crete recipe");
            TempData["errorMessage"] = "Couldn't create recipe";
            return RedirectToPage("/admin/CreateRecipe");
        }
    }
}