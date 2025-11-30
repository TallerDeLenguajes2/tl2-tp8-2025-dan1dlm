using System.ComponentModel.DataAnnotations;


namespace SistemaVentas.Web.ViewModels
{
    public class ProductosViewModel
    {
        public int idProducto { get; set; }

        //Validaciones
        [Display(Name = "Descripcion del producto")]
        [StringLength(250, ErrorMessage = "La descripcion no puede superar los 250 caracteres")]
        public string Descripcion { get; set; }

        [Display(Name = "Precio Unitario")]
        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser un valor positivo.")]
        public decimal Precio { get; set; }
    }
}





// public int IdProducto { get; set; }

// [Display(Name = "Nombre o Email del Destinatario")]
// [Required(ErrorMessage = "El nombre o email es obligatorio")]
// [EmailAddress(ErrorMessage = "Debe ingresar un email valido")]

// public string NombreDestinatario { get; set; }

// [Display(Name = "Fecha de Creacion")]
// [Required(ErrorMessage = "La fecha es obligatoria")]
// [DataType(DataType.Date)]
// public DateTime FechaCreacion { get; set; }