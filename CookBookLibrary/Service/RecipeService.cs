namespace CookBookLibrary.Service;
using CookBookLibrary.Repository;
using CookBookLibrary.Model;


public class RecipeService
{
    // Connecting to Repositories
    private readonly ICrud<Recipe> _recipeRepo;
    private readonly IRecipeIngredient _recipeIngredient;
    private readonly ICrud<FoodItem> _foodItemRepo;


    // Constructor
    public RecipeService(ICrud<Recipe> recipeRepo, IRecipeIngredient recipeIngredient, ICrud<FoodItem> foodItemRepo)
    {
        _recipeRepo = recipeRepo;
        _recipeIngredient = recipeIngredient;
        _foodItemRepo = foodItemRepo;
    }
    
    
    // Create method
    public Recipe CreateRecipe(Recipe item)
    {
        return _recipeRepo.Create(item);
    }
    
    // GetById
    public Recipe GetByIdRecipe(int id)
    {
        return _recipeRepo.GetById(id);
    }
    
        
    // GetAll
    public List<Recipe> GetAllRecipes()
    {
        return _recipeRepo.GetAll();
    }

    
    // Get all RecipeIngredients
    public List<RecipeIngredient> GetAllRecipeIngredients()
    {
        List<FoodItem> foodItems = _foodItemRepo.GetAll();
        return _recipeIngredient.GetAllRecipeIngredients(foodItems);
    }
}


