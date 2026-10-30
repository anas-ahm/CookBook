namespace CookBookLibrary.Repository;
using CookBookLibrary.Model;
using MySqlConnector;

public class RecipeRepo : ICrud<Recipe>, IRecipeIngredient
{
    private readonly DBWorker _dbWorker;
    private readonly ICrud<FoodItem> _foodItemRepo;

    public RecipeRepo(DBWorker dbWorker, ICrud<FoodItem> foodItemRepo)
    {
        _dbWorker = dbWorker;
        _foodItemRepo = foodItemRepo;
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
        catch (MySqlException ex) when (ex.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
        {
            // Same food item selected in more than one row
            throw new InvalidOperationException("The same food item is used more than once in this recipe.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("Cant create new Recipe", ex);
        }

        return item;
    }

    public Recipe GetById(int id)
    {
        Recipe recipeItem = new Recipe();
        List<RecipeIngredient> recipeIngredients = new List<RecipeIngredient>();
        
        try
        {
            string sql = """
                         SELECT recipes.id, recipes.name, recipes.description, recipes.instructions,
                                recipe_ingredients.food_item_id, recipe_ingredients.grams
                         FROM recipes
                         LEFT JOIN recipe_ingredients ON recipes.id = recipe_ingredients.recipe_id
                         WHERE recipes.id = @id
                         """;
            
            using MySqlConnection conn = _dbWorker.ConnectToDB();
            using MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("id", id);
            using MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                recipeItem._id = id;
                recipeItem._name = reader.GetString("name");
                recipeItem._description = reader.GetString("description");
                recipeItem._instructions = reader.GetString("instructions");
                
                if (!reader.IsDBNull(reader.GetOrdinal("food_item_id")))
                {
                    int foodItemId = reader.GetInt32("food_item_id");
                    double grams = reader.GetDouble("grams");

                    FoodItem foodItem = _foodItemRepo.GetById(foodItemId)!;

                    recipeIngredients.Add(new RecipeIngredient(foodItem, grams));
                }
            }

            recipeItem._ingredients = recipeIngredients;

            return recipeItem;
        }
        catch (Exception ex)
        {
            throw new Exception("Couldn't get recipe", ex);
        }
    }

    public Recipe Update(Recipe newItem, int id) 
    { 
        try 
        {
        using MySqlConnection conn = _dbWorker.ConnectToDB();
        using MySqlTransaction transaction = conn.BeginTransaction();

        // 1. Update the recipe row
        string updateSql = """
                           UPDATE recipes
                           SET name = @name, description = @description, instructions = @instructions
                           WHERE id = @id
                           """;

        using MySqlCommand cmd = new MySqlCommand(updateSql, conn, transaction);
        
        cmd.Parameters.AddWithValue("name", newItem._name);
        cmd.Parameters.AddWithValue("description", newItem._description);
        cmd.Parameters.AddWithValue("instructions", newItem._instructions);
        cmd.Parameters.AddWithValue("id", id);
        cmd.ExecuteNonQuery();
        
        // 2. Remove the old ingredient rows for this recipe
        string deleteSql = "DELETE FROM recipe_ingredients WHERE recipe_id = @id";

        using MySqlCommand cmdDelete = new MySqlCommand(deleteSql, conn, transaction); 
        cmdDelete.Parameters.AddWithValue("id", id); 
        cmdDelete.ExecuteNonQuery();
        
        // 3. Insert the submitted ingredients
        string insertSql = """
                           INSERT INTO recipe_ingredients (recipe_id, food_item_id, grams)
                           VALUES (@recipe_id, @food_item_id, @grams)
                           """;

        foreach (RecipeIngredient ingredient in newItem._ingredients)
        {
            using MySqlCommand cmdUpdate = new MySqlCommand(insertSql, conn, transaction);
            cmdUpdate.Parameters.AddWithValue("recipe_id", id);
            cmdUpdate.Parameters.AddWithValue("food_item_id", ingredient.FoodItem.Id);
            cmdUpdate.Parameters.AddWithValue("grams", ingredient.Grams);
            cmdUpdate.ExecuteNonQuery();
        }

        transaction.Commit();

        newItem._id = id;
        return newItem; 
        
        }
        catch (MySqlException ex) when (ex.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
        {
            // Same food item selected in more than one row
            throw new InvalidOperationException("The same food item is used more than once in this recipe.", ex);
        }
        catch (Exception ex) 
        {
        throw new Exception("Couldn't update recipe", ex); 
        } 
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
                recipeToCreate._id = reader.GetInt32("id");
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
        // Get the recipe first so it can be returned (like FoodItemRepo.Delete)
        Recipe recipeToDelete = GetById(id);

        try
        {
            using MySqlConnection conn = _dbWorker.ConnectToDB();
            using MySqlTransaction transaction = conn.BeginTransaction();

            // 1. Remove the ingredient rows for this recipe (foreign key)
            string deleteIngredientsSql = "DELETE FROM recipe_ingredients WHERE recipe_id = @id";

            using MySqlCommand cmdIngredients = new MySqlCommand(deleteIngredientsSql, conn, transaction);
            cmdIngredients.Parameters.AddWithValue("id", id);
            cmdIngredients.ExecuteNonQuery();

            // 2. Remove the recipe itself
            string deleteRecipeSql = "DELETE FROM recipes WHERE id = @id";

            using MySqlCommand cmdRecipe = new MySqlCommand(deleteRecipeSql, conn, transaction);
            cmdRecipe.Parameters.AddWithValue("id", id);
            cmdRecipe.ExecuteNonQuery();

            transaction.Commit();

            return recipeToDelete;
        }
        catch (Exception ex)
        {
            throw new Exception("Couldn't delete recipe", ex);
        }
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