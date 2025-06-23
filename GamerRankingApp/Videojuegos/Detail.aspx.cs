using GamerRankingApp.Models;
using System;
using System.Linq;
using System.Web.UI;

namespace GamerRankingApp.Videojuegos
{
    public partial class Detail : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null && int.TryParse(Request.QueryString["id"], out int videojuegoId))
                {
                    LoadVideojuegoDetails(videojuegoId);
                }
                else
                {
                    lblMessage.Text = "ID de videojuego no especificado o inválido.";
                    lblMessage.CssClass = "text-danger";
                }
            }
        }

        private void LoadVideojuegoDetails(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var videojuego = db.Videojuegos.Find(id);
                if (videojuego != null)
                {
                    litNombre.Text = videojuego.Nombre;
                    litCompania.Text = videojuego.Compania;
                    litAnoLanzamiento.Text = videojuego.AnoLanzamiento.ToString();
                    litPrecio.Text = videojuego.Precio.ToString("C"); // Formato de moneda
                    litPuntaje.Text = videojuego.Puntaje.ToString("F2"); // Formato de dos decimales
                    litFechaActualizacion.Text = videojuego.FechaActualizacion.ToString("dd/MM/yyyy HH:mm");
                    litUsuarioActualizacion.Text = videojuego.UsuarioActualizacion;
                }
                else
                {
                    lblMessage.Text = "Videojuego no encontrado.";
                    lblMessage.CssClass = "text-danger";
                }
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Videojuegos/Default.aspx");
        }
    }
}
