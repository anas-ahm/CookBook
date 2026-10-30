using Microsoft.AspNetCore.Mvc.RazorPages;
using CookBookLibrary.Model;
using CookBookLibrary.Service;
using Microsoft.AspNetCore.Mvc;

namespace CookBookRazor.Pages;

public class EditFoodItem : PageModel
{
    private readonly FoodItemService _foodItemService;
    private readonly ILogger<EditFoodItem> _logger;

    public EditFoodItem(FoodItemService foodItemService, ILogger<EditFoodItem> logger)
    {
        _foodItemService = foodItemService;
        _logger = logger;
    }

    [BindProperty] 
    public FoodItem FoodItemToEdit { get; set; }
    
    public void OnGet(int id)
    {
        try
        {
            FoodItemToEdit = _foodItemService.GetFoodItemById(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cant get FoodItem (PageModel");
            TempData["ErrorMessage"] = "Couldn't find food item to edit";
            Redirect("/Index");
        }
    }

    public IActionResult OnPost()
    {
        try
        {
            _foodItemService.UpdateFoodItem(FoodItemToEdit, FoodItemToEdit.Id);
            TempData["SuccessMessage"] = $"{FoodItemToEdit.Name} has been updated successfully!";
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cant edit the food item (PageModel");
            TempData["ErrorMessage"] = "Couldn't edit the food item at this moment, try again later";
            return Page();
        }
    }

    public IActionResult OnPostDelete(int id)
    {
        try
        {
            FoodItem foodItem = _foodItemService.GetFoodItemById(id);
            TempData["SuccessMessage"] =$"{foodItem.Name} has been deleted successfully!";
            _foodItemService.DeleteFoodItem(id);
            return RedirectToPage("/Index");
        }
        
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cant delete the food item (PageModel)");
            TempData["ErrorMessage"] = "Couldn't delete the food item at this moment, try again later";
            return Page();
        }
    }
}