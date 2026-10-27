namespace CookBookLibrary.Repository;
using CookBookLibrary.Model;

public interface IAdminAuth
{
    public Admin? GetAdminByUsername(string username);
    
}
