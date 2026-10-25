using Microsoft.AspNetCore.Mvc.RazorPages;
using CookBookLibrary.Service;
using CookBookLibrary.Model;

namespace CookBookRazor.Pages;

public class IndexModel : PageModel
{
    private readonly CookBookService _cookBookService;

    public IndexModel(CookBookService cookBookService)
    {
        _cookBookService = cookBookService;
    }
    
    private List<Recipe> allRecipes { get; set; } = new List<Recipe>();
    
    public void OnGet()
    {
    }
}