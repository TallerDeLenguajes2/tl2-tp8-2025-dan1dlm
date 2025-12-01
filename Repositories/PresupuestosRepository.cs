using tl2_tp8_2025_dan1dlm.Interfaces;
using tl2_tp8_2025_dan1dlm.Models;
using Microsoft.Data.Sqlite;
namespace tl2_tp8_2025_dan1dlm.Repositories;
public class PresupuestosRepository : IPresupuestosRepository
{
    string connectionString = "Data Source=db/Tienda.db";

    public List<Presupuestos> GetPresupuestos()
    {
        string queryString = "SELECT * FROM Presupuestos LEFT JOIN PresupuestosDetalle using (idPresupuesto) LEFT JOIN Productos using (idProducto)";
        var presupuestos = new Dictionary<int, Presupuestos>();

        using var connection = new SqliteConnection(connectionString);

        connection.Open();
        using var command = new SqliteCommand(queryString, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            int idPresupuesto = reader.GetInt32(0);

            if (!presupuestos.ContainsKey(idPresupuesto))
            {
                presupuestos[idPresupuesto] = new Presupuestos
                {
                    IdPresupuesto = idPresupuesto,
                    NombreDestinatario = reader.GetString(1),
                    FechaCreacion = reader.GetDateTime(2),
                    Detalle = new List<PresupuestoDetalle>()

                };
            }

            //Si hay detalle agrego el detalle
            if (!reader.IsDBNull(3))
            {
                var producto = new Productos
                {
                    IdProducto = reader.GetInt32(3),
                    Descripcion = reader.IsDBNull(5) ? "" : reader.GetString(5),
                    Precio = reader.IsDBNull(6) ? 0 : reader.GetDecimal(6)

                };

                var detalle = new PresupuestoDetalle
                {
                    Producto = producto,
                    Cantidad = reader.IsDBNull(4) ? 0 : reader.GetInt32(4)
                };

                presupuestos[idPresupuesto].Detalle.Add(detalle);
            }

        }


        //presupuestos es una coleccion de objetos presupuestos, todavia no es una lista, el Valuues agarra la parte del valor, y 
        // ToList para convertirlo en lista
        return presupuestos.Values.ToList();
    }

    public Presupuestos GetPresupuesto(int idPresupuesto)
    {
        Presupuestos presupuesto = null;

        using var conexion = new SqliteConnection(connectionString);
        conexion.Open();

        string sql = "SELECT * FROM Presupuestos LEFT JOIN PresupuestosDetalle using (idPresupuesto) LEFT JOIN Productos using (idProducto) WHERE idPresupuesto = @idPresupuesto";

        using var command = new SqliteCommand(sql, conexion);
        command.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            if (presupuesto == null)
            {
                presupuesto = new Presupuestos
                {
                    IdPresupuesto = reader.GetInt32(0),
                    NombreDestinatario = reader.GetString(1),
                    FechaCreacion = reader.GetDateTime(2),
                    Detalle = new List<PresupuestoDetalle>()
                };
            }

            if (!reader.IsDBNull(3))
            {
                var producto = new Productos
                {
                    IdProducto = reader.GetInt32(3),
                    Descripcion = reader.IsDBNull(5) ? "" : reader.GetString(5),
                    Precio = reader.IsDBNull(6) ? 0 : reader.GetDecimal(6)
                };

                var detalle = new PresupuestoDetalle
                {
                    Producto = producto,
                    Cantidad = reader.IsDBNull(4) ? 0 : reader.GetInt32(4)
                };

                presupuesto.Detalle.Add(detalle);
            }
        }

        return presupuesto;
    }

//     public Presupuestos GetPresupuesto(int idPresupuesto)
// {
//     Presupuestos presupuesto = null;

//     using var conexion = new SqliteConnection(connectionString);
//     conexion.Open();

//     string sql = "SELECT idPresupuesto, NombreDestinatario, FechaCreacion FROM Presupuestos WHERE idPresupuesto = @idPresupuesto";

//     using var command = new SqliteCommand(sql, conexion);
//     command.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);

//     using var reader = command.ExecuteReader();

//     if (reader.Read())
//     {
//         presupuesto = new Presupuestos
//         {
//             IdPresupuesto = reader.GetInt32(0),
//             NombreDestinatario = reader.GetString(1),
//             FechaCreacion = reader.GetDateTime(2),
//             // No inicializamos detalle porque no lo cargamos aquí
//             Detalle = new List<PresupuestoDetalle>() 
//         };
//     }

//     return presupuesto;
// }

    public int Create(Presupuestos nuevo)
    {
        using var conexion = new SqliteConnection(connectionString);
        conexion.Open();

        string sql = "INSERT INTO Presupuestos (idPresupuesto, nombreDestinatario, FechaCreacion) VALUES (@idPresupuesto, @nombreDestinatario, @fechaCreacion)";
        using var command = new SqliteCommand(sql, conexion);

        command.Parameters.Add(new SqliteParameter("@idPresupuesto", nuevo.IdPresupuesto));
        command.Parameters.Add(new SqliteParameter("@nombreDestinatario", nuevo.NombreDestinatario));
        command.Parameters.Add(new SqliteParameter("@fechaCreacion", nuevo.FechaCreacion));
        int rows = command.ExecuteNonQuery();

        return rows;
    }

    public int AddProduct(int idPresupuesto, Productos producto, int cantidad)
    {
        // Verificar que el presupuesto exista
        var presupuesto = GetPresupuesto(idPresupuesto) ?? throw new Exception($"No existe un presupuesto con el ID {idPresupuesto}");

        // Reutilizar el repositorio de detalle
        using var conexion = new SqliteConnection(connectionString);

        conexion.Open();

        string sql = @"INSERT INTO PresupuestoDetalle (idPresupuesto, idProducto, Cantidad)
                        VALUES (@idPresupuesto, @idProducto, @Cantidad)";

        using var command = new SqliteCommand(sql, conexion);

        command.Parameters.Add(new SqliteParameter("@idPresupuesto", idPresupuesto));
        command.Parameters.Add(new SqliteParameter("@idProducto", producto.IdProducto));
        command.Parameters.Add(new SqliteParameter("@Cantidad", cantidad));

        int rows = command.ExecuteNonQuery(); 

        return rows;
    }

    public int Update(int idPresupuesto, Presupuestos nuevoPresupuesto)
    {
        using var conexion = new SqliteConnection(connectionString);
        conexion.Open();

        string sql = "UPDATE Presupuestos SET NombreDestinatario = @NombreDestinatario, FechaCreacion = @FechaCreacion WHERE idPresupuesto = @idPresupuesto";
        using var command = new SqliteCommand(sql, conexion);

        command.Parameters.Add(new SqliteParameter("@idPresupuesto", idPresupuesto));
        command.Parameters.Add(new SqliteParameter("@FechaCreacion", nuevoPresupuesto.FechaCreacion));
        command.Parameters.Add(new SqliteParameter("@NombreDestinatario", nuevoPresupuesto.NombreDestinatario));

        int rows = command.ExecuteNonQuery();

        return rows;
    }
    public void Delete(int idEliminar)
    {
        using var conexion = new SqliteConnection(connectionString);
        conexion.Open();

        string sql = "DELETE FROM Presupuestos WHERE idPresupuesto = @idPresupuesto";
        using var command = new SqliteCommand(sql, conexion);

        command.Parameters.Add(new SqliteParameter("@idPresupuesto", idEliminar));
        command.ExecuteNonQuery();
    }



   

}