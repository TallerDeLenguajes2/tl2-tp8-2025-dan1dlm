using tl2_tp8_2025_dan1dlm.Models;


namespace tl2_tp8_2025_dan1dlm.Interfaces;

public interface IProductosRepository
{
    List<Productos> GetProductos();
    Productos GetProducto(int idProducto);
    int Create(Productos productoNuevo);
    int Update(Productos producto);
    void Delete(int idEliminar);
}