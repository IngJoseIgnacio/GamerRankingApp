using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GamerRankingApp.Models
{
    public class Calificacion
    {
        [Key] // Marca 'Id' como la clave primaria
        public Guid Id { get; set; } // Identificador GUID

        [Required(ErrorMessage = "El Nickname del jugador es obligatorio.")]
        [StringLength(255)]
        public string NicknameJugador { get; set; } // Nickname del jugador

        [Required(ErrorMessage = "La puntuación es obligatoria.")]
        [Column(TypeName = "decimal")] // Permite valores entre 0 y 5 con dos decimales
        [Range(0.00, 5.00, ErrorMessage = "La puntuación debe ser entre 0 y 5.")]
        public decimal Puntuacion { get; set; } // Puntuación (valores entre 0 y 5)

        // Foreign Key a Videojuego
        [Required]
        public int VideojuegoId { get; set; }

        // Propiedad de navegación a Videojuego
        public virtual Videojuego Videojuego { get; set; }

        // Campos de Control
        [Required]
        public DateTime FechaActualizacion { get; set; }

        [Required]
        [StringLength(255)]
        public string UsuarioActualizacion { get; set; }
    }
}