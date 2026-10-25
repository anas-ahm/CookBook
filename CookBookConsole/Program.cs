using CookBookLibrary.Model;
using CookBookLibrary.Repository;
using CookBookLibrary.Service;

// Connecting to service
FoodItemRepo _foodItemRepo = new FoodItemRepo();
FoodItemService foodItemService = new FoodItemService(_foodItemRepo);






FoodItem foodItemToAdd = new FoodItem();
foodItemToAdd.Calories = 100;
foodItemToAdd.Protein = 10;
foodItemToAdd.Fat = 10;
foodItemToAdd.SaturatedFat = 5;
foodItemToAdd.Carbs = 14;
foodItemToAdd.Sugar = 5;
foodItemToAdd.Fiber = 2;
foodItemToAdd.Salt = 1;

foodItemService.CreateFoodItem(foodItemToAdd);

