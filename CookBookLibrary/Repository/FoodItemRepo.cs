namespace CookBookLibrary.Repository;
using CookBookLibrary.Model;
using MySqlConnector;

public class FoodItemRepo : ICrud<FoodItem>
{
    private readonly DBWorker _dbWorker;

    public FoodItemRepo(DBWorker dbWorker)
    {
        _dbWorker = dbWorker;
    }
    
    public FoodItem Create(FoodItem foodItem)
    {
        string sql = """
                     INSERT INTO FoodItem (name, calories, protein, fat, saturated_fat, carbs, sugar, fiber, salt) VALUES (@name, @calories, @protein, @fat, @saturated_fat, @carbs, @sugar, @fiber, @salt)
                     """;

        using MySqlConnection connString = _dbWorker.ConnectToDB();
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
    
    // READ (one)
    public FoodItem? GetById(int id)
    {
        string sql = " SELECT * FROM FoodItem WHERE id = @id ";

        using MySqlConnection conn = _dbWorker.ConnectToDB();
        conn.Open();

        using MySqlCommand cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", id);

        using MySqlDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            FoodItem foodItem = new FoodItem();

            foodItem.Id = reader.GetInt32("id");
            foodItem.Name = reader.GetString("name");
            foodItem.Calories = reader.GetDouble("calories");
            foodItem.Protein = reader.GetDouble("protein");

            // Order matters because of the validation rules:
            // fat before saturated fat, carbs before sugar and fiber
            foodItem.Fat = reader.GetDouble("fat");
            foodItem.SaturatedFat = reader.GetDouble("saturated_fat");
            foodItem.Carbs = reader.GetDouble("carbs");
            foodItem.Sugar = reader.GetDouble("sugar");
            foodItem.Fiber = reader.GetDouble("fiber");

            foodItem.Salt = reader.GetDouble("salt");

            return foodItem;
        }

        return null;
    }

    // READ (all)
    public List<FoodItem> GetAll()
    {
        string sql = """
                     SELECT *
                     FROM FoodItem
                     ORDER BY name
                     """;

        List<FoodItem> foodItems = new List<FoodItem>();

        using MySqlConnection conn = _dbWorker.ConnectToDB();
        conn.Open();

        using MySqlCommand cmd = new MySqlCommand(sql, conn);
        using MySqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            FoodItem foodItem = new FoodItem();
            
            string name = reader.GetString("name");

            if (string.IsNullOrWhiteSpace(name))
            {
                name = "N/A";
            }

            foodItem.Id = reader.GetInt32("id");
            foodItem.Name = name;
            foodItem.Calories = reader.GetDouble("calories");
            foodItem.Protein = reader.GetDouble("protein");

            // Order matters because of the validation rules:
            // fat before saturated fat, carbs before sugar and fiber
            foodItem.Fat = reader.GetDouble("fat");
            foodItem.SaturatedFat = reader.GetDouble("saturated_fat");
            foodItem.Carbs = reader.GetDouble("carbs");
            foodItem.Sugar = reader.GetDouble("sugar");
            foodItem.Fiber = reader.GetDouble("fiber");

            foodItem.Salt = reader.GetDouble("salt");
            foodItems.Add(foodItem);
        }

        return foodItems;
    }

    // UPDATE
    public FoodItem Update(FoodItem foodItem, int id)
    {
        string sql = """
                     UPDATE FoodItem
                     SET name = @name,
                         calories = @calories,
                         protein = @protein,
                         fat = @fat,
                         saturated_fat = @saturated_fat,
                         carbs = @carbs,
                         sugar = @sugar,
                         fiber = @fiber,
                         salt = @salt
                     WHERE id = @id
                     """;

        using MySqlConnection conn = _dbWorker.ConnectToDB();
        conn.Open();

        using MySqlCommand cmd = new MySqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@id", foodItem.Id);
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

    // DELETE
    public FoodItem Delete(int id)
    {
        FoodItem foodItem = GetById(id);
        
        string sql = "DELETE FROM FoodItem WHERE id = @id";

        using MySqlConnection conn = _dbWorker.ConnectToDB();
        conn.Open();

        using MySqlCommand cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", id);

       cmd.ExecuteNonQuery();

       return foodItem;
    }
}