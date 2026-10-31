namespace CookBookLibrary.Model;

public class Admin
{
    // Properties
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    
    // Constructor
    public Admin(string username, string password)
    {
 
        Username = username;
        Password = password;
    }
    
    // Empty Constructor
    public Admin() {}
}


