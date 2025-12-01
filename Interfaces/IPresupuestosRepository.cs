using tl2_tp8_2025_dan1dlm.Models;

namespace tl2_tp8_2025_dan1dlm.Interfaces;
public interface IPresupuestosRepository
{
    List<Presupuestos> GetPresupuestos();
    Presupuestos GetPresupuesto(int idPresupuesto);
    int Create(Presupuestos nuevo);
    int AddProduct(int idPresupuesto, Productos productoNuevo, int cantidad);
    int Update(int idPresupuesto, Presupuestos nuevoPresupuesto);
    void Delete(int idEliminar);
}