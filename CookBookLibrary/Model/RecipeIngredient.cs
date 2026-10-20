namespace CookBookLibrary.Model;

public class RecipeIngredient
{
    private FoodItem _foodItem;
    private double _grams;

    public RecipeIngredient(FoodItem foodItem, double grams)
    {
        FoodItem = foodItem;
        Grams = grams;
    }

    public FoodItem FoodItem
    {
        get { return _foodItem; }
        set
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(FoodItem), "Food item cannot be null.");
            }
            _foodItem = value;
        }
    }

    // Grams for ONE person
    public double Grams
    {
        get { return _grams; }
        set
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Grams), "Grams must be greater than 0.");
            }
            _grams = value;
        }
    }

    public double GetGrams(int servings)
    {
        return _grams * servings;
    }

    public double GetCalories(int servings)
    {
        return _foodItem.Calories / 100 * _grams * servings;
    }
}