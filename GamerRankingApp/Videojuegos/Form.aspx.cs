using GamerRankingApp.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity; // Para EntityState
using System.Web.UI;
using AjaxControlToolkit; // Para ConfirmButtonExtender

namespace GamerRankingApp.Videojuegos
{
    public partial class Form : Page
    {
        private int _videojuegoId = 0; // Para saber si estamos editando (0 = nuevo, >0 = editando)

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null && int.TryParse(Request.QueryString["id"], out _videojuegoId))
                {
                    // Modo edición
                    litFormTitle.Text = "Editar";
                    LoadVideojuegoForEdit(_videojuegoId);
                    // Habilitar la confirmación para el botón de guardar en modo edición
                    ConfirmButtonExtender1.Enabled = true;
                }
                else
                {
                    // Modo creación
                    litFormTitle.Text = "Crear";
                    ConfirmButtonExtender1.Enabled = false; // No es necesaria la confirmación para crear
                }
            }
            // Si es un postback, asegurarnos de que _videojuegoId se mantenga (si viene de QueryString)
            if (IsPostBack && Request.QueryString["id"] != null && int.TryParse(Request.QueryString["id"], out int tempId))
            {
                _videojuegoId = tempId;
                ConfirmButtonExtender1.Enabled = (_videojuegoId > 0); // Re-habilita si es edición
            }
        }

        private void LoadVideojuegoForEdit(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var videojuego = db.Videojuegos.Find(id);
                if (videojuego != null)
                {
                    txtNombre.Text = videojuego.Nombre;
                    txtCompania.Text = videojuego.Compania;
                    txtAnoLanzamiento.Text = videojuego.AnoLanzamiento.ToString();
                    txtPrecio.Text = videojuego.Precio.ToString();
                }
                else
                {
                    lblMessage.Text = "Videojuego no encontrado para edición.";
                    lblMessage.CssClass = "text-danger";
                    // Podrías redirigir o deshabilitar el formulario aquí
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return; // Detener si hay errores de validación
            }

            try
            {
                using (var db = new ApplicationDbContext())
                {
                    Videojuego videojuego;
                    string currentUser = User.Identity.GetUserName() ?? "Desconocido";

                    if (_videojuegoId > 0)
                    {
                        // Actualizar existente
                        videojuego = db.Videojuegos.Find(_videojuegoId);
                        if (videojuego == null)
                        {
                            lblMessage.Text = "Videojuego a actualizar no encontrado.";
                            lblMessage.CssClass = "text-danger";
                            return;
                        }
                        // Las propiedades de auditoría se actualizan aquí
                        videojuego.FechaActualizacion = DateTime.Now;
                        videojuego.UsuarioActualizacion = currentUser;
                    }
                    else
                    {
                        // Crear nuevo
                        videojuego = new Videojuego();
                        // Inicializar propiedades de auditoría para la creación
                        videojuego.FechaActualizacion = DateTime.Now;
                        videojuego.UsuarioActualizacion = currentUser;
                        db.Videojuegos.Add(videojuego);
                    }

                    // Asignar valores del formulario
                    videojuego.Nombre = txtNombre.Text.Trim();
                    videojuego.Compania = txtCompania.Text.Trim();
                    videojuego.AnoLanzamiento = int.Parse(txtAnoLanzamiento.Text);
                    videojuego.Precio = decimal.Parse(txtPrecio.Text);
                    // El puntaje promedio se actualizará con el reto 06, no se edita directamente

                    db.SaveChanges();

                    lblMessage.Text = (_videojuegoId > 0) ? "Videojuego actualizado exitosamente." : "Videojuego creado exitosamente.";
                    lblMessage.CssClass = "text-success";

                    // Limpiar formulario si es una creación exitosa
                    if (_videojuegoId == 0)
                    {
                        txtNombre.Text = string.Empty;
                        txtCompania.Text = string.Empty;
                        txtAnoLanzamiento.Text = string.Empty;
                        txtPrecio.Text = string.Empty;
                    }

                    // Opcional: Redirigir al listado después de guardar
                    // Response.Redirect("~/Videojuegos/Default.aspx");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Error al guardar el videojuego: {ex.Message}";
                lblMessage.CssClass = "text-danger";
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Videojuegos/Default.aspx");
        }
    }
}
