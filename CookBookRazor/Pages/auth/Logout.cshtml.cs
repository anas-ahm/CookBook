using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CookBookRazor.Pages;

public class Logout : PageModel
{
    private readonly ILogger<Logout> _logger;
    
    public Logout(ILogger<Logout> logger)
    {
        _logger = logger;
    }
    
    public IActionResult OnGet()
    {
        _logger.LogError("Logout handler ran");
        
        // Sign out the user by clearing the authentication cookie
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).GetAwaiter().GetResult();

        TempData["SuccessMessage"] = "You're now logged out";

        return RedirectToPage("/auth/Login");
    }
}