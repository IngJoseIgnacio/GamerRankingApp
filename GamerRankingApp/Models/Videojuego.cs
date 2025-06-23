using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GamerRankingApp.Models
{
    public class Videojuego
    {
        [Key] // Marca 'Id' como la clave primaria
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Configura como auto-incremental
        public int Id { get; set; } // Identificador entero positivo auto-incremental

        [Required(ErrorMessage = "El nombre del videojuego es obligatorio.")]
        [StringLength(255)]
        public string Nombre { get; set; } // Nombre o título del videojuego

        [Required(ErrorMessage = "La compañía es obligatoria.")]
        [StringLength(255)]
        public string Compania { get; set; } // Compañía que lo comercializó

        [Required(ErrorMessage = "El año de lanzamiento es obligatorio.")]
        [Range(1900, 2100, ErrorMessage = "El año de lanzamiento debe ser entre 1900 y 2100.")]
        public int AnoLanzamiento { get; set; } // Año de lanzamiento

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Column(TypeName = "decimal")] // Tipo de dato para dos decimales
        [Range(0.01, 1000.00, ErrorMessage = "El precio debe ser un valor positivo.")]
        public decimal Precio { get; set; } // Precio

        [Required]
        [Column(TypeName = "decimal")] // Permite valores entre 0 y 5 con dos decimales
        public decimal Puntaje { get; set; } = 0.00m; // Promedio de la calificación (defecto 0.00)

        // Campos de Control
        [Required]
        public DateTime FechaActualizacion { get; set; }

        [Required]
        [StringLength(255)]
        public string UsuarioActualizacion { get; set; }

        // Propiedad de navegación para las calificaciones relacionadas
        public virtual ICollection<Calificacion> Calificaciones { get; set; }
    }
}