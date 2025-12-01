using System.IO.Compression;
using SistemaVentas.Web.ViewModels;

namespace tl2_tp8_2025_dan1dlm.Models;
public class Productos{
    private int idProducto;
    private string descripcion;
    private decimal precio;

    public Productos(){}

    public Productos(ProductosViewModel productoVm)
    {
        this.descripcion = productoVm.Descripcion;
        this.Precio = productoVm.Precio;
    }

    public int IdProducto{get => idProducto; set => idProducto = value;}
    public string Descripcion{get => descripcion; set => descripcion = value;}
    public decimal Precio{get => precio; set => precio = value;}

}