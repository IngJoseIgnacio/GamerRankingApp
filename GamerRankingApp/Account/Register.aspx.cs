using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using GamerRankingApp.Models; // Asegúrate de que el namespace sea correcto

namespace GamerRankingApp.Account
{
    public partial class Register : Page
    {
        protected void CreateUser_Click(object sender, EventArgs e)
        {
            // Validar la página antes de proceder
            if (!Page.IsValid)
            {
                return;
            }

            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
            var user = new ApplicationUser() { UserName = Email.Text, Email = Email.Text };
            IdentityResult result = manager.Create(user, Password.Text);
            if (result.Succeeded)
            {
                // Si el registro es exitoso, iniciar sesión al usuario
                signInManager.SignIn(user, isPersistent: false, rememberBrowser: false);

                // NOTIFICACIÓN DE ÉXITO (ejemplo, puedes usar un div visible)
                ScriptManager.RegisterStartupScript(this, GetType(), "successAlert", "alert('¡Usuario registrado exitosamente!');", true);

                // Redirigir a la página principal después del registro y login
                IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
            }
            else
            {
                // NOTIFICACIÓN DE ERROR
                ErrorMessage.Text = result.Errors.FirstOrDefault();
                ScriptManager.RegisterStartupScript(this, GetType(), "errorAlert", $"alert('{ErrorMessage.Text.Replace("'", "\\'")}');", true);
            }
        }
    }
}