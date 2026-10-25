namespace CookBookLibrary.Service;
using CookBookLibrary.Repository;
using CookBookLibrary.Model;

public class FoodItemService
{
    // Connecting to Repositories
    private readonly ICrud<FoodItem> _foodItemRepo;


    // Constructor
    public FoodItemService(ICrud<FoodItem> foodItemRepo)
    {
        _foodItemRepo = foodItemRepo;

    }

    // CREATE
    public FoodItem CreateFoodItem(FoodItem foodItem)
    {
        return _foodItemRepo.Create(foodItem);
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
    
}