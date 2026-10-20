namespace CookBookLibrary.Model;

public class Recipe
{
    private int _id;
    private string _name = string.Empty;
    private string _description = string.Empty;
    private List<RecipeIngredient> _ingredients = new List<RecipeIngredient>();
    private string _instructions = string.Empty;

    public int Id
    {
        get { return _id; }
        set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Id), "Id cannot be negative.");
            }
            _id = value;
        }
    }

    public string Name
    {
        get { return _name; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Name cannot be empty.", nameof(Name));
            }
            _name = value;
        }
    }

    public string Description
    {
        get { return _description; }
        set
        {
            if (value == null)
            {
                _description = string.Empty;
            }
            else
            {
                _description = value;
            }
        }
    }

    // Amounts in the ingredients are for ONE person
    public List<RecipeIngredient> Ingredients
    {
        get { return _ingredients; }
        set
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(Ingredients), "Ingredients list cannot be null.");
            }
            _ingredients = value;
        }
    }

    public string Instructions
    {
        get { return _instructions; }
        set
        {
            if (value == null)
            {
                _instructions = string.Empty;
            }
            else
            {
                _instructions = value;
            }
        }
    }

    public double GetTotalCalories(int servings)
    {
        CheckServings(servings);

        double total = 0;
        foreach (RecipeIngredient ingredient in _ingredients)
        {
            total += ingredient.GetCalories(servings);
        }
        return total;
    }

    public double GetTotalGrams(int servings)
    {
        CheckServings(servings);

        double total = 0;
        foreach (RecipeIngredient ingredient in _ingredients)
        {
            total += ingredient.GetGrams(servings);
        }
        return total;
    }

    private static void CheckServings(int servings)
    {
        if (servings < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(servings), "Servings must be at least 1.");
        }
    }
}