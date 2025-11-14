using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaVentas.Web.ViewModels
{
    public class AgregarProductoViewModel
    {
        //id del presupuesto al que se va a agregar
        public int IdPresupuesto { get; set; }


        //id del producto seleccionado en el dropdown
        [Display(Name = "Producto a agregar")]
        public int IdProducto { get; set; }


        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero")]
        public int Cantidad { get; set; }

        
        //Lista para el DropDown
        public SelectList ListaProductos { get; set; }
    }
}