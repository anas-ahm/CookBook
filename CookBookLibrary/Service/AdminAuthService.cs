namespace CookBookLibrary.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using CookBookLibrary.Model;
using Sodium;


using System.Security.Claims;

using CookBookLibrary.Repository;

public class AdminAuthService : IAdminAuth
{
    private readonly ICrud<Admin> _adminRepo;
    private readonly IAdmin _adminAuth;

    public AdminAuthService(ICrud<Admin> adminRepo, IAdmin adminAuth)
    {
        _adminRepo = adminRepo;
        _adminAuth = adminAuth;
    }
    

    public Admin? GetAdminByUsername(string admin)
    {
        return _adminAuth.GetAdminByUsername(admin);
    }

    public Admin? Login(string username, string password)
    {
        Admin? admin = _adminAuth.GetAdminByUsername(username);

        // IT Security: prevent enumeration attacks by throwing a generic exception for both invalid username and password
        if (admin == null || !VerifyPassword(admin.Password, password))
        {
            throw new UnauthorizedAccessException("Username or password wrong");
        }

        return admin;
    }

    public Admin Register(Admin admin)
    {
        admin.Password = HashPassword(admin.Password);
        
        return _adminRepo.Create(admin);
    }
    
    // Hash password helper
    public string HashPassword(string passwordToHash)
    {
        return PasswordHash.ArgonHashString(passwordToHash, PasswordHash.StrengthArgon.Medium);
    }

    public bool VerifyPassword(string storedHash, string password)
    {
        return PasswordHash.ArgonHashStringVerify(storedHash, password);
    }
    
    public ClaimsPrincipal LoginAdmin(Admin admin)
    {
        // Create claims for the logged-in user, meaning the information we want to store in the cookie
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()),

            new Claim(ClaimTypes.Name, admin.Username),

            new Claim(ClaimTypes.Role, "Admin")
        };

        // Create a ClaimsIdentity with the claims and the authentication scheme, which represents the user's identity
        ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        // Create a ClaimsPrincipal with the identity, which represents the logged-in user
        ClaimsPrincipal principal = new ClaimsPrincipal(identity);
        return principal;
    }
    
    public ClaimsPrincipal RegisterAdmin(Admin admin)
    {
        // Create claims for the registered user, meaning the information we want to store in the cookie
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()),

            new Claim(ClaimTypes.Name, admin.Username),

            new Claim(ClaimTypes.Role, "Admin")
        };

        // Create a ClaimsIdentity with the claims and the authentication scheme, which represents the user's identity
        ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        // Create a ClaimsPrincipal with the identity, which represents the registered user
        ClaimsPrincipal principal = new ClaimsPrincipal(identity);
        return principal;
    }
}
