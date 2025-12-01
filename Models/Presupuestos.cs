using tl2_tp8_2025_dan1dlm.ViewModels;

namespace tl2_tp8_2025_dan1dlm.Models;

public class Presupuestos
{
    private int idPresupuesto;
    private string nombreDestinatario;
    private DateTime fechaCreacion;
    private List<PresupuestoDetalle> detalle;

    
    public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }
    public string NombreDestinatario { get => nombreDestinatario; set => nombreDestinatario = value; }
    public DateTime FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }
    public List<PresupuestoDetalle> Detalle { get => detalle; set => detalle = value; }

    public Presupuestos(){}

    public Presupuestos(PresupuestosViewModel presupuestoVm)
    {
        this.nombreDestinatario = presupuestoVm.NombreDestinatario;
        this.fechaCreacion = presupuestoVm.FechaCreacion;
        this.Detalle = new List<PresupuestoDetalle>();
    }
    //metodos

    public decimal montoPresupuesto()
    {
        decimal total = 0;
        foreach (var p in detalle)
        {
            total += p.Cantidad * p.Producto.Precio;
        }

        return total;
    }

    public decimal montoPresupuestoConIVA()
    {
        return montoPresupuesto() * 1.21m;
    }

    public int cantidadProductos()
    {
        int total = 0;
        foreach (var d in detalle)
        {
            total += d.Cantidad;
        }

        return total;
    }
}