namespace CookBookLibrary.Service;
using CookBookLibrary.Model;

public interface IAdminAuth
{
    public Admin? GetAdminByUsername(string username);

    public Admin Login(string username, string password);

    public Admin Register(Admin member);
}