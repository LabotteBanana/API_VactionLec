using System;
using MySqlConnector;
using Microsoft.Extensions.Configuration;
public interface IDB
{
        MySqlConnection GetConnection();
}

public class DB : IDB
{
    private readonly string _connectionString;

    public DB(IConfiguration configuration){
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }
    
    public MySqlConnection GetConnection()
    {
        var connection = new MySqlConnection(_connectionString);
        connection.Open();
        return connection;
    }

}