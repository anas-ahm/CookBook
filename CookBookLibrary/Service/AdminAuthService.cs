namespace CookBookLibrary.Service;
using CookBookLibrary.Model;
using Sodium;
using CookBookLibrary.Repository;

public class AdminAuthService : IAdminAuth
{
    private readonly ICrud<Admin> _adminRepo;
    private readonly IAdminAuth _adminAuth;

    public AdminAuthService(ICrud<Admin> adminRepo, IAdminAuth adminAuth)
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
}
