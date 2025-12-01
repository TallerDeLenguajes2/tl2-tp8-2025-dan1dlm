using tl2_tp8_2025_dan1dlm.Models;
using Microsoft.Data.Sqlite;
using tl2_tp8_2025_dan1dlm.Interfaces;
namespace tl2_tp8_2025_dan1dlm.Repositories;

public class ProductosRepository : IProductosRepository
{
    string connectionString = "Data Source=Db/Tienda.db";
    public List<Productos> GetProductos()
    {
        string queryString = "SELECT * FROM Productos";
        List<Productos> productos = new List<Productos>();
        using var conecction = new SqliteConnection(connectionString);
        conecction.Open();

        var command = new SqliteCommand(queryString, conecction);

        using (SqliteDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var producto = new Productos
                {
                    IdProducto = reader.GetInt32(0),
                    Descripcion = reader.GetString(1),
                    Precio = reader.GetDecimal(2)
                };
                productos.Add(producto);
            }
        }
        
    //no hace falta conecction.Close(), porque se está usando using var conecction = new SqliteConnection(connectionString); que garantiza que la conexión se cierre y libere automáticamente al finalizar el bloque donde está declarada.

        return productos;
    }

    public Productos GetProducto(int idProducto)
    {
        using var conexion = new SqliteConnection(connectionString);
        conexion.Open();

        string sql = "SELECT idProducto, Descripcion, Precio FROM Productos WHERE idProducto = @idProducto";
        using var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.Add(new SqliteParameter("@idProducto", idProducto));

        using var reader = comando.ExecuteReader();

        if (reader.Read())
        {
            var producto = new Productos
            {
                IdProducto = reader.GetInt32(0),
                Descripcion = reader.GetString(1),
                Precio = reader.GetDecimal(2)
            };

            return producto;
        }

        return null;
    }

    public int Create(Productos productoNuevo)
    {
        string query = "INSERT INTO Productos(Descripcion, Precio) VALUES(@descripcion, @precio)";
        using var conecction = new SqliteConnection(connectionString);
        conecction.Open();

        var command = new SqliteCommand(query, conecction);
        command.Parameters.Add(new SqliteParameter("@descripcion", productoNuevo.Descripcion));
        command.Parameters.Add(new SqliteParameter("@precio", productoNuevo.Precio));
        int filas = command.ExecuteNonQuery();
        conecction.Close();
        return filas;
    }


    public int Update(Productos producto)
    {
        string query = "UPDATE Productos SET Descripcion = @descripcion, Precio = @precio WHERE idProducto = @id";
        using var conecction = new SqliteConnection(connectionString);
        conecction.Open();

        var command = new SqliteCommand(query, conecction);
        command.Parameters.Add(new SqliteParameter("@id", producto.IdProducto));
        command.Parameters.Add(new SqliteParameter("@descripcion", producto.Descripcion));
        command.Parameters.Add(new SqliteParameter("@precio", producto.Precio));
        int filas = command.ExecuteNonQuery();
        conecction.Close();
        return filas;
    }

    public void Delete(int idEliminar)
    {
        using var conexion = new SqliteConnection(connectionString);
        conexion.Open();

        string sql = "DELETE FROM Productos WHERE idProducto = @idEliminar";
        using var comando = new SqliteCommand(sql, conexion);

        comando.Parameters.Add(new SqliteParameter("@idEliminar", idEliminar));
        comando.ExecuteNonQuery();
    }

}