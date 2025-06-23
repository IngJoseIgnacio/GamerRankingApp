using GamerRankingApp.Models;
using System;
using System.Data.SqlClient; // Para ejecutar el SP
using System.Web.UI;

namespace GamerRankingApp.Admin
{
    public partial class GenerateCalificaciones : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // La seguridad de acceso por rol ya se maneja en Web.config para la carpeta Admin
            // Pero se puede añadir una comprobación extra aquí si se desea
            if (!User.IsInRole("Administrator"))
            {
                lblMessage.Text = "No tiene permisos para acceder a esta página.";
                lblMessage.CssClass = "text-danger";
                btnGenerate.Enabled = false; // Deshabilitar el botón
                txtQuantity.Enabled = false;
            }
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            if (!User.IsInRole("Administrator"))
            {
                lblMessage.Text = "Acceso denegado. Solo administradores pueden generar calificaciones.";
                lblMessage.CssClass = "text-danger";
                return;
            }

            int quantity;
            if (!int.TryParse(txtQuantity.Text, out quantity))
            {
                lblMessage.Text = "Por favor, ingrese una cantidad válida.";
                lblMessage.CssClass = "text-danger";
                return;
            }

            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var errorCodeParam = new SqlParameter("@ErrorCode", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output };
                    var errorMessageParam = new SqlParameter("@ErrorMessage", System.Data.SqlDbType.NVarChar, -1) { Direction = System.Data.ParameterDirection.Output };
                    var cantidadParam = new SqlParameter("@cantidad", quantity);

                    // Ejecutar el procedimiento almacenado
                    db.Database.ExecuteSqlCommand("EXEC [dbo].[GenerateRandomCalificaciones] @cantidad, @ErrorCode OUTPUT, @ErrorMessage OUTPUT",
                        cantidadParam, errorCodeParam, errorMessageParam);

                    int errorCode = (int)errorCodeParam.Value;
                    string errorMessage = errorMessageParam.Value as string;

                    if (errorCode == 0)
                    {
                        lblMessage.Text = $"Se han generado {quantity} calificaciones exitosamente.";
                        lblMessage.CssClass = "text-success";
                    }
                    else
                    {
                        lblMessage.Text = $"Error ({errorCode}): {errorMessage}";
                        lblMessage.CssClass = "text-danger";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Ha ocurrido un error inesperado: {ex.Message}";
                lblMessage.CssClass = "text-danger";
            }
        }
    }
}
