using Microsoft.AspNetCore.Mvc.RazorPages;
using CookBookLibrary.Service;
using CookBookRazor.Service;
using CookBookLibrary.Model;
using Microsoft.AspNetCore.Mvc;

namespace CookBookRazor.Pages;

public class RecipeDetails : PageModel
{
    // Connecting to service
    private readonly RecipeService _recipeService;
    private readonly IImageService _imageService;
    private readonly FoodItemService _foodItemService;


    public RecipeDetails(RecipeService recipeService, FoodItemService foodItemService, IImageService imageService)
    {
        _recipeService = recipeService;
        _imageService = imageService;
        _foodItemService = foodItemService;
    }
    
    // Property for selecting amount of servings
    // default = 1
    [BindProperty] public int Servings { get; set; } = 1;
    public string? GetImagePath(int id) => _imageService.GetImagePath(id);
    
    // Properties for macros
    public double Calories { get; set; }
    public double Protein { get; set; }
    public double Fat { get; set; }
    public double SaturatedFat { get; set; }
    public double Carbs { get; set; }
    public double Sugar { get; set; }
    public double Fiber { get; set; }
    public double Salt { get; set; }
    
    public Recipe recipeToShow { get; set; }
    
    
    public void OnGet(int id)
    {
        recipeToShow = _recipeService.GetByIdRecipe(id);
        
        
        // Filling in Macros
        foreach (RecipeIngredient recipeIngredientItem in recipeToShow._ingredients)
        {
            FoodItem foodItem = _foodItemService.GetFoodItemById(recipeIngredientItem.FoodItem.Id);

            Calories = Calories + ( (foodItem.Calories * (recipeIngredientItem.Grams / 100)) * Servings);
            Protein = Protein + ((foodItem.Protein * (recipeIngredientItem.Grams / 100)) * Servings);
            Fat = Fat + ((foodItem.Fat * (recipeIngredientItem.Grams / 100)) * Servings);
            SaturatedFat = SaturatedFat + ((foodItem.SaturatedFat * (recipeIngredientItem.Grams / 100)) * Servings);
            Carbs = Carbs + ((foodItem.Carbs * (recipeIngredientItem.Grams / 100)) * Servings);
            Sugar = Sugar + ((foodItem.Sugar * (recipeIngredientItem.Grams / 100)) * Servings);
            Fiber = Fiber + ((foodItem.Fiber * (recipeIngredientItem.Grams / 100)) * Servings);
            Salt = Salt + ((foodItem.Salt * (recipeIngredientItem.Grams / 100)) * Servings);
        }
    }

    public IActionResult OnPostPortion(int id)
    {
        try
        { 
            recipeToShow = _recipeService.GetByIdRecipe(id);
            
            // Filling in Macros
            foreach (RecipeIngredient recipeIngredientItem in recipeToShow._ingredients)
            {
                FoodItem foodItem = _foodItemService.GetFoodItemById(recipeIngredientItem.FoodItem.Id);

                Calories = Calories + ( (foodItem.Calories * (recipeIngredientItem.Grams / 100)) * Servings);
                Protein = Protein + ((foodItem.Protein * (recipeIngredientItem.Grams / 100)) * Servings);
                Fat = Fat + ((foodItem.Fat * (recipeIngredientItem.Grams / 100)) * Servings);
                SaturatedFat = SaturatedFat + ((foodItem.SaturatedFat * (recipeIngredientItem.Grams / 100)) * Servings);
                Carbs = Carbs + ((foodItem.Carbs * (recipeIngredientItem.Grams / 100)) * Servings);
                Sugar = Sugar + ((foodItem.Sugar * (recipeIngredientItem.Grams / 100)) * Servings);
                Fiber = Fiber + ((foodItem.Fiber * (recipeIngredientItem.Grams / 100)) * Servings);
                Salt = Salt + ((foodItem.Salt * (recipeIngredientItem.Grams / 100)) * Servings);
                recipeIngredientItem.Grams = recipeIngredientItem.Grams * Servings;
            }

            return Page();
        }
        catch (Exception ex)
        {
            TempData["errorMessage"] = "Cant do that right now, try again later";
            return Page();
        }
    }
}