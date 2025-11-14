using System.ComponentModel.DataAnnotations;
using System;

namespace SistemaVentas.Web.ViewModels
{
    public class PresupuestosViewModel
    {
        public int IdPresupuesto {get; set;}
        
        //Validaciones
        [Display(Name = "Nombre o Email del Destinatario")]
        [Required(ErrorMessage = "El nombre o Email es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato de Email es obligatorio")]
        public string NombreDestinatario {get; set;}

        [Display(Name = "Fecha de Creacion")]
        [Required(ErrorMessage = "La fecha es obligatoria")]
        [DataType(DataType.Date)]
        public DateTime FechaCreacion {get; set;}

    }
}