namespace CookBookLibrary.Service;
using System.Security.Claims;

using CookBookLibrary.Model;

public interface IAdminAuth
{
    public Admin? GetAdminByUsername(string username);

    public Admin Login(string username, string password);

    public Admin Register(Admin member);

    public ClaimsPrincipal RegisterAdmin(Admin admin);

    public ClaimsPrincipal LoginAdmin(Admin admin);
}