using Microsoft.AspNetCore.Mvc.RazorPages;
using CookBookLibrary.Model;
using CookBookLibrary.Repository;
using CookBookLibrary.Service;
using Microsoft.AspNetCore.Mvc;

namespace CookBookRazor.Pages;

public class FoodItems : PageModel
{
    // Connecting to service & ILogger
    private readonly FoodItemService _foodItemService;

    private readonly ILogger<FoodItems> _logger;
    public FoodItems(FoodItemService foodItemService, ILogger<FoodItems> logger)
    {
        _foodItemService = foodItemService;
        _logger = logger;
    }

    public List<FoodItem> allFoodItems { get; set; } = new List<FoodItem>();
    
    [BindProperty(SupportsGet = true)] public List<FoodCategory> Categories { get; set; } = new();
    
    public void OnGet()
    {
        try
        {
            allFoodItems = _foodItemService.GetAllFoodItems();
            
            if (Categories.Any())
            {
                allFoodItems = allFoodItems.Where(f => Categories.Contains(f.Category)).ToList();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Couldn't get food items (PageModel)");
            TempData["ErrorMessage"] = "Couldn't load the food items, try again later";
            RedirectToPage("/Index");
        }
    }
}