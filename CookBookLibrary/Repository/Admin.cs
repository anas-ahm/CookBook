using System.Xml;
using CookBookLibrary.Service;
using MySqlConnector;

namespace CookBookLibrary.Repository;
using CookBookLibrary.Model;

public class AdminRepo : ICrud<Admin>, IAdminAuth
{
    private readonly DBWorker _dbWorker;

    public AdminRepo(DBWorker dbWorker)
    {
        _dbWorker = dbWorker;
    }
    
    public Admin Create(Admin item)
    {
        try
        {
            string sql = """
                         INSERT into Admin (username, password) 
                         VALUES (@username, @password)
                         """;

            using MySqlConnection conn = _dbWorker.ConnectToDB();
            
            using MySqlCommand cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("username", item.Username);
            cmd.Parameters.AddWithValue("password", item.Password);
            cmd.ExecuteNonQuery();

            item.Id = (int)cmd.LastInsertedId;

        }
        catch (Exception ex)
        {
            throw new Exception("Couldn't create Admin in repository", ex);
        }

        return item;
    }

    public Admin GetById(int id)
    {
        throw new NotImplementedException();
    }

    public List<Admin> GetAll()
    {
        throw new NotImplementedException();
    }

    public Admin Update(Admin newItem, int id)
    {
        throw new NotImplementedException();
    }

    public Admin Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Admin? GetMemberByUsername(string username)
    {
        throw new NotImplementedException();
    }

    public Admin? GetAdminByUsername(string username)
    {
        try
        {
            string sql = """
                         SELECT username, password
                         FROM Admin 
                         WHERE username = @username
                         """;
            
            using MySqlConnection conn = _dbWorker.ConnectToDB();
            using MySqlCommand cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("username", username);

            using MySqlDataReader reader = cmd.ExecuteReader();

            Admin admin = new Admin();
            
            while (reader.Read())
            {
                admin.Id = reader.GetInt32("admin_id");
                admin.Username = reader.GetString("username");
                admin.Password = reader.GetString("password");
            }

            return admin;
        }
        catch (Exception ex)
        {
            throw new Exception("Couldn't find Admin by username in database (Repo)");
        }
    }
}