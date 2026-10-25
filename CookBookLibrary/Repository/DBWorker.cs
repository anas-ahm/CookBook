namespace CookBookLibrary.Repository;
using MySqlConnector;
using DotNetEnv;

public class DBWorker
{
    public MySqlConnection ConnectToDB()
    {
        Env.TraversePath().Load();
        
        string connString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

        if (string.IsNullOrEmpty(connString))
        {
            throw new Exception("Connection string not found. check if correct name or if the worker can find the file");
        }

        MySqlConnection connection = new MySqlConnection(connString);
        connection.Open();
        
        return connection;
    }
}