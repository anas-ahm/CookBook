namespace CookBookLibrary.Repository;
using CookBookLibrary.Model;
using MySqlConnector;

public class FoodItemRepo : IFoodItemRepo
{
    private string ConnectToDB()
    {
        DBWorker dbWorker = new DBWorker();

        string connString = dbWorker.ConnectToDB();

        return connString;
    }
    
    public FoodItem CreateFoodItem(FoodItem foodItem)
    {
        string sql = """
                     INSERT INTO FoodItem (name, calories, protein, fat, saturated_fat, carbs, sugar, fiber, salt) VALUES (@name, @calories, @protein, @fat, @saturated_fat, @carbs, @sugar, @fiber, @salt)
                     """;

        using MySqlConnection connString = new MySqlConnection(ConnectToDB());
        connString.Open();

        MySqlCommand cmd = new MySqlCommand(sql, connString);
        
        cmd.Parameters.AddWithValue("@name", foodItem.Name);
        cmd.Parameters.AddWithValue("@calories", foodItem.Calories);
        cmd.Parameters.AddWithValue("@protein", foodItem.Protein);
        cmd.Parameters.AddWithValue("@fat", foodItem.Fat);
        cmd.Parameters.AddWithValue("@saturated_fat", foodItem.SaturatedFat);
        cmd.Parameters.AddWithValue("@carbs", foodItem.Carbs);
        cmd.Parameters.AddWithValue("@sugar", foodItem.Sugar);
        cmd.Parameters.AddWithValue("@fiber", foodItem.Fiber);
        cmd.Parameters.AddWithValue("@salt", foodItem.Salt);

        cmd.ExecuteNonQuery();

        return foodItem;
    }
}