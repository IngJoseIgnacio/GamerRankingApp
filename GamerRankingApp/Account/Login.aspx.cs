using GamerRankingApp.Models; // Asegúrate de que el namespace sea correcto
using Microsoft.AspNet.Identity.Owin;
using Owin;
using System;
using System.Web;
using System.Web.UI;

namespace GamerRankingApp.Account
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterHyperLink.NavigateUrl = "Register";
            // Habilitar esto una vez que tenga la confirmación de la cuenta habilitada para la función de restablecimiento de contraseña
            //ForgotPasswordHyperLink.NavigateUrl = "Forgot";
            var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
            if (!String.IsNullOrEmpty(returnUrl))
            {
                RegisterHyperLink.NavigateUrl += "?ReturnUrl=" + returnUrl;
            }
        }

        protected void LogIn_Click(object sender, EventArgs e)
        {
            // Validar la página antes de proceder
            if (!Page.IsValid)
            {
                return;
            }

            var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();

            // Requiere que el usuario confirme su correo electrónico antes de iniciar sesión.
            // Requerir el bloqueo de la cuenta por errores de contraseña
            var result = signinManager.PasswordSignIn(Email.Text, Password.Text, RememberMe.Checked, shouldLockout: true);

            switch (result)
            {
                case SignInStatus.Success:
                    IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                    break;
                case SignInStatus.LockedOut:
                    // NOTIFICACIÓN DE ERROR
                    FailureText.Text = "Esta cuenta ha sido bloqueada, intente de nuevo en unos minutos.";
                    ScriptManager.RegisterStartupScript(this, GetType(), "lockedOutAlert", "alert('Esta cuenta ha sido bloqueada, intente de nuevo en unos minutos.');", true);
                    break;
                case SignInStatus.RequiresVerification:
                    // Si se implementa verificación de dos factores
                    // IdentityHelper.RedirectToReturnUrl("TwoFactorAuthenticationSignIn?ReturnUrl=" + Request.QueryString["ReturnUrl"], Response);
                    break;
                case SignInStatus.Failure:
                default:
                    // NOTIFICACIÓN DE ERROR
                    FailureText.Text = "Intento de inicio de sesión no válido.";
                    ScriptManager.RegisterStartupScript(this, GetType(), "failureAlert", "alert('Intento de inicio de sesión no válido.');", true);
                    break;
            }
        }
    }
}