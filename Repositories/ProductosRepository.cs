using Microsoft.Data.Sqlite;

public class ProductosRepository
{
    string connectionString = "Data Source=Db/Tienda.db";
    public List<Productos> GetProductos()
    {
        string queryString = "SELECT * FROM Productos";
        List<Productos> productos = new List<Productos>();
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);
            connection.Open();
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Productos producto = new Productos
                    {
                        IdProducto = reader.GetInt32(0),             
                        Descripcion = reader.GetString(1),              
                        Precio = reader.GetDecimal(2)
                    };                 

                    productos.Add(producto);
                }
            }
            connection.Close();
        }

        return productos;
    }

    public Productos GetProducto(int idProducto)
    {
      using var conexion = new SqliteConnection(connectionString);
      conexion.Open();

      string sql = "SELECT idProducto, Descripcion, Precio FROM Productos WHERE idProducto = @idProducto";
      using var comando = new SqliteCommand(sql, conexion);
      comando.Parameters.Add(new SqliteParameter("@idProducto", idProducto));

      using var lector = comando.ExecuteReader();

      if(lector.Read()){
        var producto = new Productos {
            IdProducto = lector.GetInt32(0),
            Descripcion = lector.GetString(1),
            Precio = lector.GetDecimal(2)
        };

        return producto;
      } 

      return null;
    }

    public int Create(Productos productoNuevo){
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

    public void eliminarProducto(int idEliminar){
        using var conexion = new SqliteConnection(connectionString);
        conexion.Open();

        string sql = "DELETE FROM Productos WHERE idProducto = @idEliminar";
        using var comando = new SqliteCommand(sql, conexion);

        comando.Parameters.Add(new SqliteParameter("@idProducto", idEliminar));
        comando.ExecuteNonQuery();
    }

    public int Update(Productos producto){
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


}