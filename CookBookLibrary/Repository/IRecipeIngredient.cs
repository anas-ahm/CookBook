using CookBookLibrary.Model;

namespace CookBookLibrary.Repository;

public interface IRecipeIngredient
{
   public List<RecipeIngredient> GetAllRecipeIngredients(List<FoodItem> item);
   
}