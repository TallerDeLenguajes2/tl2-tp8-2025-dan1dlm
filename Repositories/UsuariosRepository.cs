using Microsoft.Data.Sqlite;
using SQLitePCL;
using tl2_tp8_2025_dan1dlm.Interfaces;
using tl2_tp8_2025_dan1dlm.Models;
namespace tl2_tp8_2025_dan1dlm.Repositories;

public class UsuariosRepository : IUsuariosRepository
{
    private string connectionString = "Data Source=Db/Tienda.db";
    public Usuarios GetUser(string User, string Pass)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        string sql = "SELECT * FROM Usuarios where User = @User AND Pass = @Pass";

        using var command = new SqliteCommand(sql, connection);
        command.Parameters.Add(new SqliteParameter("@User", User));
        command.Parameters.Add(new SqliteParameter("@Pass", Pass));

        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            var usuario = new Usuarios
            {
                Id = reader.GetInt32(0),
                Nombre = reader.GetString(1),
                User = reader.GetString(2),
                Pass = reader.GetString(3),
                Rol = reader.GetString(4)
            };

            return usuario;
        }

        return null;
    }
}