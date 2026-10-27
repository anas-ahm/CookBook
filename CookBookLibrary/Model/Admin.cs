namespace CookBookLibrary.Model;

public class Admin
{
    // Properties
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    
    // Constructor
    public Admin(int id, string username, string password)
    {
        Id = id;
        Username = username;
        Password = password;
    }
    
    // Empty Constructor
    public Admin() {}
}


