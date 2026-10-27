using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using CookBookLibrary.Model;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.ComponentModel.DataAnnotations;
using CookBookLibrary.Service;


namespace CookBookRazor.Pages;

public class Login : PageModel
{
    private readonly ILogger<Login> _logger;
    private readonly IAdminAuth _adminAuth;

    public Login(ILogger<Login> logger, IAdminAuth adminAuth)
    {
        _logger = logger;
        _adminAuth = adminAuth;
    }
    
    [BindProperty]
    public InputModel Input { get; set; } = new();
    
    public void OnGet()
    {
        
    }

    public IActionResult OnPost()
    {
        try
        {
            Admin admin = _adminAuth.Login(Input.Username, Input.Password);

            if (admin == null)
            {
                return Page();
            }

            LoginAuth(admin);

            TempData["SuccessMessage"] = $"Welcome {admin.Username}! You are now logged in.";
            return RedirectToPage("/Index");

        }
        catch (UnauthorizedAccessException)
        {
            TempData["ErrorMessage"] = "Username or password wrong";
            return Page();
        }
        
        catch (Exception ex)
        {
            _logger.LogError(ex, "Couldn't login (PageMode)");
            TempData["ErrorMessage"] = "Can't login now, try again later";
            return Page();
        }
    }
    
    public class InputModel
    {
        [Required(ErrorMessage = "Username needed")]
        public string Username { get; set; } = "";

        [Required(ErrorMessage = "Password needed")]
        public string Password { get; set; } = "";
    }
    
    public void LoginAuth(Admin admin)
    {
        ClaimsPrincipal principal = _adminAuth.LoginAdmin(admin);

        // ASP.NET Core provides an asynchronous method.
        // GetAwaiter().GetResult() lets us call it synchronously.
        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal).GetAwaiter().GetResult();
    }
}