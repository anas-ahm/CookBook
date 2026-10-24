namespace CookBookLibrary.Repository;
using CookBookLibrary.Model;
using MySqlConnector;

public class RecipeRepo : ICrud<Recipe>
{
    private readonly DBWorker _dbWorker;

    public RecipeRepo(DBWorker dbWorker)
    {
        _dbWorker = dbWorker;
    }
    
    public Recipe Create(Recipe item)
    {
        try
        {
            string sql =
                """
                INSERT INTO recipes (name, description, instructions) 
                VALUES (@name, @description, @instructions);
                """;

            using MySqlConnection conn = _dbWorker.ConnectToDB();

            using MySqlCommand cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("name", item._name);
            cmd.Parameters.AddWithValue("description", item._description);
            cmd.Parameters.AddWithValue("instructions", item._instructions);

            // inserting the new id from DB into item
            int newId = (int)cmd.LastInsertedId;
            item._id = newId;

            cmd.ExecuteNonQuery();
            
            // Running method helper that puts all ingredients into DB
            InsertRecipeIngredientsToDB(item);

        }
        catch (Exception ex)
        {
            throw new Exception("Cant create new Recipe", ex);
        }

        return item;
    }

    public Recipe GetById(int id)
    {
        throw new NotImplementedException();
    }

    public Recipe Update(Recipe newItem, int id)
    {
        throw new NotImplementedException();
    }

    public List<Recipe> GetAll()
    {
        throw new NotImplementedException();
    }

    public Recipe Delete(int id)
    {
        throw new NotImplementedException();
    }

    // helper that takes a recipe object, takes the ingredient list, and puts it into the DB
    // Foreach loop that makes a sql call for every ingredient
    private void InsertRecipeIngredientsToDB(Recipe recipe)
    {
        List<RecipeIngredient>  recipeIngredients = recipe._ingredients;

        foreach (RecipeIngredient i in recipeIngredients)
        {
            try
            {
                string sql = """
                             INSERT INTO recipe_ingredients (recipe_id, food_item_id, grams) 
                             VALUES (@recipe_id, @food_item_id, @grams)
                             """;

                using MySqlConnection conn = _dbWorker.ConnectToDB();

                using MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("recipe_id", recipe._id);
                cmd.Parameters.AddWithValue("food_item_id", i.FoodItem.Id);
                cmd.Parameters.AddWithValue("grams", i.Grams);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong inserting ingredient list into DB", ex);
            }
        }
    }
}