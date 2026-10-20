using CookBookLibrary.Model;

namespace CookBookLibrary.Repository;

public interface IFoodItemRepo
{
    FoodItem CreateFoodItem(FoodItem foodItem);
}