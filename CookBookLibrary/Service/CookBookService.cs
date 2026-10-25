namespace CookBookLibrary.Service;
using CookBookLibrary.Repository;
using CookBookLibrary.Model;

public class CookBookService
{
    // Connecting to Repositories
    private readonly ICrud<FoodItem> _foodItemRepo;
    private readonly ICrud<Recipe> _recipeRepo;
    private readonly IRecipeIngredient _recipeIngredient;

    // Constructor
    public CookBookService(ICrud<FoodItem> foodItemRepo, ICrud<Recipe> recipeRepo, IRecipeIngredient recipeIngredient)
    {
        _foodItemRepo = foodItemRepo;
        _recipeRepo = recipeRepo;
        _recipeIngredient = recipeIngredient;
    }

    // CREATE
    public FoodItem CreateFoodItem(FoodItem foodItem)
    {
        return _foodItemRepo.Create(foodItem);
    }

    public Recipe CreateRecipe(Recipe item)
    {
        return _recipeRepo.Create(item);
    }

    // READ (one)
    public FoodItem? GetFoodItemById(int id)
    {
        return _foodItemRepo.GetById(id);
    }

    // READ (all)
    public List<FoodItem> GetAllFoodItems()
    {
        return _foodItemRepo.GetAll();
    }

    // UPDATE
    public FoodItem UpdateFoodItem(FoodItem foodItem, int id)
    {
        return _foodItemRepo.Update(foodItem, id);
    }

    // DELETE
    public void DeleteFoodItem(int id)
    {
        _foodItemRepo.Delete(id);
    }
    
    // Get all RecipeIngredients
    public List<RecipeIngredient> GetAllRecipeIngredients()
    {
        List<FoodItem> foodItems = _foodItemRepo.GetAll();
        return _recipeIngredient.GetAllRecipeIngredients(foodItems);
    }
}