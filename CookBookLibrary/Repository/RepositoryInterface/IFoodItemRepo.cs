using CookBookLibrary.Model;

namespace CookBookLibrary.Repository;

public interface IFoodItemRepo
{
    FoodItem CreateFoodItem(FoodItem foodItem);
    FoodItem? GetFoodItemById(int id);
    List<FoodItem> GetAllFoodItems();
    FoodItem UpdateFoodItem(FoodItem foodItem);
    void DeleteFoodItem(int id);
}