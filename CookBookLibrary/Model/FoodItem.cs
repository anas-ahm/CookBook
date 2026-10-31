namespace CookBookLibrary.Model;

public class FoodItem
{
    // All nutrition values are per 100 g
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public FoodCategory Category { get; set; }
    public double Calories { get; set; }
    public double Protein { get; set; }
    public double Fat { get; set; }
    public double SaturatedFat { get; set; }
    public double Carbs { get; set; }
    public double Sugar { get; set; }
    public double Fiber { get; set; }
    public double Salt { get; set; }

    // Constructor
    public FoodItem(string name, FoodCategory Category, double calories, double protein, double fat, double saturatedFat,
        double carbs, double sugar, double fiber, double salt)
    {
        Name = name;
        Category = Category;
        Calories = calories;
        Protein = protein;
        Fat = fat;
        SaturatedFat = saturatedFat;
        Carbs = carbs;
        Sugar = sugar;
        Fiber = fiber;
        Salt = salt;
    }
    
    // Empty Constructor
    public FoodItem() {}
}