using Microsoft.AspNetCore.Mvc.RazorPages;
using CookBookLibrary.Service;
using CookBookLibrary.Model;
using Microsoft.AspNetCore.Mvc;

namespace CookBookRazor.Pages;

public class RecipeDetails : PageModel
{
    // Connecting to service
    private readonly RecipeService _recipeService;
    private readonly FoodItemService _foodItemService;


    public RecipeDetails(RecipeService recipeService, FoodItemService foodItemService)
    {
        _recipeService = recipeService;
        _foodItemService = foodItemService;
    }
    
    // Property for selecting amount of servings
    // default = 1
    public int Servings { get; set; } = 1;
    
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
}