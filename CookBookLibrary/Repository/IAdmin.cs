namespace CookBookLibrary.Repository;
using CookBookLibrary.Model;

public interface IAdmin
{
    public Admin? GetAdminByUsername(string username);
}
