namespace CookBookLibrary.Repository;
using DotNetEnv;

public class DBWorker
{
    public string ConnectToDB()
    {
        Env.TraversePath().Load();
        
        string connString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

        if (string.IsNullOrEmpty(connString))
        {
            throw new Exception("Connection string not found. check if correct name or if the worker can find the file");
        }

        return connString;
    }
}