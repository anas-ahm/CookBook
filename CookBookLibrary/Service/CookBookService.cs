namespace CookBookLibrary.Service;
using CookBookLibrary.Repository;
using CookBookLibrary.Model;

public class CookBookService
{
    // Connecting to Repositories
    private readonly IFoodItemRepo _foodItemRepo;

    // Constructor
    public CookBookService(IFoodItemRepo foodItemRepo)
    {
        _foodItemRepo = foodItemRepo;
    }

    // CREATE
    public FoodItem CreateFoodItem(FoodItem foodItem)
    {
        return _foodItemRepo.CreateFoodItem(foodItem);
    }

    // READ (one)
    public FoodItem? GetFoodItemById(int id)
    {
        return _foodItemRepo.GetFoodItemById(id);
    }

    // READ (all)
    public List<FoodItem> GetAllFoodItems()
    {
        return _foodItemRepo.GetAllFoodItems();
    }

    // UPDATE
    public FoodItem UpdateFoodItem(FoodItem foodItem)
    {
        return _foodItemRepo.UpdateFoodItem(foodItem);
    }

    // DELETE
    public void DeleteFoodItem(int id)
    {
        _foodItemRepo.DeleteFoodItem(id);
    }
}