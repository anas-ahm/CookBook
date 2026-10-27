using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.ComponentModel.DataAnnotations;
using CookBookLibrary.Service;

using CookBookLibrary.Model;

namespace CookBookRazor.Pages;

public class Signup : PageModel
{

    private readonly IAdminAuth _auth;

    public Signup(IAdminAuth auth)
    {
        _auth = auth;
    }
    
    
    [BindProperty]
    public InputModel Input { get; set; } = new();
    
    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
            
        // Now this sees the error above as well as the attribute errors
        if (!ModelState.IsValid)
        {
            return Page();
        }
        
        // Building the object and pass it though the repo
        Admin adminToCreate = new Admin();
        adminToCreate.Username = Input.Username;
        adminToCreate.Password = Input.Password;
        
        try
        {
            Admin registeredMember = _auth.Register(adminToCreate);

            // Check if the password and confirm password match
            if (Input.Password != Input.ConfirmPassword)
            {
                ModelState.AddModelError("Input.ConfirmPassword", "Passwords doesn't match");
                return Page();
            }

            AuthRegister(registeredMember);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
            
        TempData["SuccessMessage"] = $"Welcome {adminToCreate.Username}, you're now signed up - login to access website as admain!";
        return RedirectToPage("/Login");
    }
    
    public class InputModel
    {
        [Required(ErrorMessage = "Username needed")]
        public string Username { get; set; } = "";

        [Required(ErrorMessage = "Password needed")]
        public string Password { get; set; } = "";
        
        [Compare(nameof(Password), ErrorMessage = "Passwords doesn't match")]
        [Required(ErrorMessage = "Confirm password"), DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = "";
    }
    
    public void AuthRegister(Admin registeredMember)
    {
        ClaimsPrincipal principal = _auth.RegisterAdmin(registeredMember);

        // ASP.NET Core provides an asynchronous method.
        // GetAwaiter().GetResult() lets us call it synchronously.
        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal).GetAwaiter().GetResult();
    }
}