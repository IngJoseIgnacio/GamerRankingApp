using AjaxControlToolkit; // Para ModalPopupExtender
using GamerRankingApp.Models;
using GamerRankingApp.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity; // Para Include
using System.Linq;
using System.Text; // Para CSV
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GamerRankingApp.Videojuegos
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGridView();
                // Ocultar o deshabilitar botón de generar ranking si el usuario no es Administrador
                if (!User.IsInRole("Administrator"))
                {
                    btnShowRankingModal.Visible = false;
                    // Si el botón está visible pero deshabilitado, la condición de OnClientClick lo maneja
                }
            }
        }

        private void BindGridView()
        {
            using (var db = new ApplicationDbContext())
            {
                IQueryable<Videojuego> query = db.Videojuegos;

                // Aplicar filtros
                if (!string.IsNullOrWhiteSpace(txtFilterName.Text))
                {
                    query = query.Where(v => v.Nombre.Contains(txtFilterName.Text.Trim()));
                }
                if (!string.IsNullOrWhiteSpace(txtFilterCompany.Text))
                {
                    query = query.Where(v => v.Compania.Contains(txtFilterCompany.Text.Trim()));
                }
                if (!string.IsNullOrWhiteSpace(txtFilterYear.Text) && int.TryParse(txtFilterYear.Text, out int year))
                {
                    query = query.Where(v => v.AnoLanzamiento == year);
                }

                // Calcular el número total de registros para la paginación virtual
                gvVideojuegos.VirtualItemCount = query.Count();

                // Ordenar y paginar
                gvVideojuegos.DataSource = query.OrderBy(v => v.Id) // Ordenar por Id para paginación consistente
                                                .Skip(gvVideojuegos.PageIndex * gvVideojuegos.PageSize)
                                                .Take(gvVideojuegos.PageSize)
                                                .ToList();
                gvVideojuegos.DataBind();
            }
        }

        protected void gvVideojuegos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvVideojuegos.PageIndex = e.NewPageIndex;
            BindGridView();
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            gvVideojuegos.PageIndex = 0; // Resetear a la primera página al filtrar
            BindGridView();
        }

        protected void btnCreateNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Videojuegos/Form"); // Redirigir al formulario de creación
        }

        protected void gvVideojuegos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int videojuegoId;
            if (!int.TryParse(e.CommandArgument.ToString(), out videojuegoId))
            {
                lblMessage.Text = "Error: ID de videojuego inválido.";
                lblMessage.CssClass = "text-danger";
                return;
            }

            if (e.CommandName == "VerDetalle")
            {
                Response.Redirect($"~/Videojuegos/Detail.aspx?id={videojuegoId}");
            }
            else if (e.CommandName == "Editar")
            {
                Response.Redirect($"~/Videojuegos/Form.aspx?id={videojuegoId}");
            }
            else if (e.CommandName == "Eliminar")
            {
                // La comprobación de rol ya se hace en el OnClientClick y en el Enable del LinkButton
                // Pero siempre hacer la comprobación final en el servidor
                if (!User.IsInRole("Administrator"))
                {
                    lblMessage.Text = "No tiene permisos para eliminar videojuegos.";
                    lblMessage.CssClass = "text-danger";
                    return;
                }

                try
                {
                    using (var db = new ApplicationDbContext())
                    {
                        var videojuego = db.Videojuegos.Find(videojuegoId);
                        if (videojuego != null)
                        {
                            db.Videojuegos.Remove(videojuego);
                            db.SaveChanges();
                            lblMessage.Text = $"Videojuego '{videojuego.Nombre}' eliminado exitosamente.";
                            lblMessage.CssClass = "text-success";
                            BindGridView(); // Rebind para reflejar el cambio
                        }
                        else
                        {
                            lblMessage.Text = "Videojuego no encontrado.";
                            lblMessage.CssClass = "text-danger";
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = $"Error al eliminar videojuego: {ex.Message}";
                    lblMessage.CssClass = "text-danger";
                }
            }
        }

        // RETO 06: Generación de Ranking CSV
        protected void btnShowRankingModal_Click(object sender, EventArgs e)
        {
            if (!User.IsInRole("Administrator"))
            {
                lblRankingMessage.Text = "Solo los administradores pueden generar el ranking.";
                lblRankingMessage.CssClass = "text-danger";
                RankingModalPopupExtender.Show(); // Asegura que el modal se muestre para el mensaje
                return;
            }
            txtTopDesired.Text = string.Empty; // Limpiar campo cada vez que se abre el modal
            lblRankingMessage.Text = string.Empty; // Limpiar mensaje previo
            RankingModalPopupExtender.Show();
        }

        protected void btnGenerateDownloadRanking_Click(object sender, EventArgs e)
        {
            if (!User.IsInRole("Administrator"))
            {
                lblRankingMessage.Text = "No tiene permisos para generar el ranking.";
                lblRankingMessage.CssClass = "text-danger";
                RankingModalPopupExtender.Show();
                return;
            }

            int topDesired = 0;
            if (!string.IsNullOrWhiteSpace(txtTopDesired.Text))
            {
                if (!int.TryParse(txtTopDesired.Text, out topDesired) || topDesired < 0)
                {
                    lblRankingMessage.Text = "El valor ingresado para el Top deseado no es válido. Debe ser un entero positivo o 0.";
                    lblRankingMessage.CssClass = "text-danger";
                    RankingModalPopupExtender.Show();
                    return;
                }
            }

            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var rankingService = new RankingService(db); // Instanciar el servicio
                    var finalRankingData = rankingService.GenerateRanking(topDesired); // Usar el servicio

                    if (!finalRankingData.Any())
                    {
                        lblRankingMessage.Text = "No hay datos para generar el ranking.";
                        lblRankingMessage.CssClass = "text-info";
                        RankingModalPopupExtender.Show();
                        return;
                    }

                    StringBuilder csvContent = new StringBuilder();
                    csvContent.AppendLine("Nombre|Compañía|Puntaje|Clasificación");

                    foreach (var entry in finalRankingData) // Usar la clase RankingEntry
                    {
                        csvContent.AppendLine($"{entry.Nombre}|{entry.Compania}|{entry.Puntaje:F2}|{entry.Clasificacion}");
                    }

                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "text/csv";
                    Response.AddHeader("Content-Disposition", "attachment;filename=VideoGameRanking.csv");
                    Response.Charset = "UTF-8";
                    Response.Output.Write(csvContent.ToString());
                    Response.Flush();
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                lblRankingMessage.Text = $"Error al generar el ranking: {ex.Message}";
                lblRankingMessage.CssClass = "text-danger";
                RankingModalPopupExtender.Show();
            }
        }

    }
}
