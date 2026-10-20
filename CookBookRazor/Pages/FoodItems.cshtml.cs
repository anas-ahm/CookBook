using Microsoft.AspNetCore.Mvc.RazorPages;
using CookBookLibrary.Model;
using CookBookLibrary.Repository;
using CookBookLibrary.Service;

namespace CookBookRazor.Pages;

public class FoodItems : PageModel
{
    public List<FoodItem> allFoodItems { get; set; } = new List<FoodItem>();
    
    public void OnGet()
    {
        // Connecting to Repo
        FoodItemRepo _foodItemRepo = new FoodItemRepo();
        CookBookService _cookBookService = new CookBookService(_foodItemRepo);
        
        // Getting fooditem list
        allFoodItems = _cookBookService.GetAllFoodItems();
    }
}