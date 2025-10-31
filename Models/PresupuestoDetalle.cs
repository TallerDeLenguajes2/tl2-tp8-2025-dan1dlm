public class PresupuestoDetalle{
    private Producto producto;
    private int cantidad;

    public PresupuestoDetalle(){}
    public int Cantidad { get => cantidad; set => cantidad = value; }
    public Producto Producto{get => producto; set => producto = value;}
}