using Microsoft.Data.Sqlite;

public class PresupuestosRepository
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

    public void InsertarPresupuesto(Presupuestos nuevo)
    {
        using var conexion = new SqliteConnection(connectionString);
        conexion.Open();

        string sql = "INSERT INTO Presupuestos (idPresupuesto, nombreDestinatario, FechaCreacion) VALUES (@idPresupuesto, @nombreDestinatario, @fechaCreacion)";
        using var comando = new SqliteCommand(sql, conexion);

        comando.Parameters.Add(new SqliteParameter("@idPresupuesto", nuevo.IdPresupuesto));
        comando.Parameters.Add(new SqliteParameter("@nombreDestinatario", nuevo.NombreDestinatario));
        comando.Parameters.Add(new SqliteParameter("@fechaCreacion", nuevo.FechaCreacion));

        comando.ExecuteNonQuery();
    }

    public void AgregarProductoAPresupuesto(int idPresupuesto, Productos producto, int cantidad)
    {
        // Verificar que el presupuesto exista
        var presupuesto = GetPresupuesto(idPresupuesto) ?? throw new Exception($"No existe un presupuesto con el ID {idPresupuesto}");

        // Reutilizar el repositorio de detalle
        using var conexion = new SqliteConnection(connectionString);

        conexion.Open();

        string sql = @"INSERT INTO PresupuestoDetalle (idPresupuesto, idProducto, Cantidad)
                        VALUES (@idPresupuesto, @idProducto, @Cantidad)";

        using var comando = new SqliteCommand(sql, conexion);

        comando.Parameters.Add(new SqliteParameter("@idPresupuesto", idPresupuesto));
        comando.Parameters.Add(new SqliteParameter("@idProducto", producto.IdProducto));
        comando.Parameters.Add(new SqliteParameter("@Cantidad", cantidad));

        comando.ExecuteNonQuery(); //para actualizar la BD
    }

    public void EliminarPresupuesto(int idEliminar)
    {
        using var conexion = new SqliteConnection(connectionString);
        conexion.Open();

        string sql = "DELETE FROM Presupuestos WHERE idPresupuesto = @idPresupuesto";
        using var comando = new SqliteCommand(sql, conexion);

        comando.Parameters.Add(new SqliteParameter("@idPresupuesto", idEliminar));
        comando.ExecuteNonQuery();
    }

    public void ActualizarPresupuesto(int idPresupuesto, string nuevoNombreDestinatario)
    {
        using var conexion = new SqliteConnection(connectionString);
        conexion.Open();

        string sql = "UPDATE Presupuestos SET nombreDestinatario = @nombreDestinatario WHERE idPresupuesto = @idPresupuesto";
        using var comando = new SqliteCommand(sql, conexion);

        comando.Parameters.Add(new SqliteParameter("@nombreDestinatario", nuevoNombreDestinatario));
        comando.Parameters.Add(new SqliteParameter("@idPresupuesto", idPresupuesto));

        comando.ExecuteNonQuery();
    }


    //METODOS PRIVADOS

}