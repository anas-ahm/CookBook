namespace CookBookLibrary.Repository;
using CookBookLibrary.Model;
using MySqlConnector;

public class RecipeRepo : ICrud<Recipe>, IRecipeIngredient
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

            cmd.ExecuteNonQuery();
            
            // inserting the new id from DB into item
            int newId = (int)cmd.LastInsertedId;
            item._id = newId;
            
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
        List<Recipe> recipeList = new List<Recipe>();
        
        try
        {
            string sql = """
                         SELECT id, name, description, instructions
                         FROM recipes
                         """;

            using MySqlConnection conn = _dbWorker.ConnectToDB();
            using MySqlCommand cmd = new MySqlCommand(sql, conn);
            using MySqlDataReader reader = cmd.ExecuteReader();
            
            while (reader.Read())
            {
                Recipe recipeToCreate = new Recipe();
                recipeToCreate._id = reader.GetInt32("recipe_id");
                recipeToCreate._name = reader.GetString("name");
                recipeToCreate._description = reader.GetString("description");
                recipeToCreate._instructions = reader.GetString("instructions");

                recipeList.Add(recipeToCreate);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("couldn't get all recipes", ex);
        }

        return recipeList;
    }

    public Recipe Delete(int id)
    {
        throw new NotImplementedException();
    }

    public List<RecipeIngredient> GetAllRecipeIngredients(List<FoodItem> item)
    {
        // the list we return 
        List<RecipeIngredient> recipeIngredientsList = new List<RecipeIngredient>();
        
        try
        {
            string sql = "SELECT food_item_id, grams FROM recipe_ingredients";

            using MySqlConnection conn = _dbWorker.ConnectToDB();

            using MySqlCommand cmd = new MySqlCommand(sql, conn);

            MySqlDataReader reader = cmd.ExecuteReader();

            // List for all foodItems passed through from service layer
            // we use this list to do a foreach loop in while reader.read()
            // to match the foodItemId to the foodItem and put it inside the fooditem parameter.
            List<FoodItem> foodItems = item;
        
            while (reader.Read())
            {
                RecipeIngredient recipeItem = new RecipeIngredient();
                
                int foodItemId = reader.GetInt32("food_item_id");

                foreach (FoodItem food in foodItems)
                {
                    if (foodItemId == food.Id)
                    {
                        recipeItem.FoodItem = food;
                    }
                }

                recipeItem.Grams = reader.GetDouble("grams");
                
                recipeIngredientsList.Add(recipeItem);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Couldnt get recipe ingredients.", ex);
        }

        return recipeIngredientsList;
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