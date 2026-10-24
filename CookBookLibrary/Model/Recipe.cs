namespace CookBookLibrary.Model;

public class Recipe
{
    public int _id { get; set; }
    public string _name { get; set; }
    public string _description { get; set; }
    public List<RecipeIngredient> _ingredients { get; set; }
    public string _instructions { get; set; }
    
    // Constructor
    public Recipe(string name, string description, List<RecipeIngredient> ingredients, string instructions)
    {
        _name = name;
        _description = description;
        _ingredients = ingredients;
        _instructions = instructions;
    }
    
    // Empty Constructor
    public Recipe() {}

}