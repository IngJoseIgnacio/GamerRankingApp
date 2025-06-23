<%@ Page Title="Iniciar sesión" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="GamerRankingApp.Account.Login" Async="true" %>

    <asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
        <br />
        <br />
        <h2>Iniciar Sesión.</h2>
        <p class="text-danger">
            <asp:Literal runat="server" ID="FailureText" />
        </p>
        <div class="row">
            <div class="col-md-8">
                <section id="loginForm">
                    <asp:ValidationSummary runat="server" CssClass="text-danger" />
                    <div class="mb-3">
                        <asp:Label runat="server" AssociatedControlID="Email" CssClass="form-label">Correo electrónico</asp:Label>
                        <asp:TextBox runat="server" ID="Email" CssClass="form-control" TextMode="Email" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="Email" CssClass="text-danger" ErrorMessage="El campo Correo electrónico es obligatorio." />
                    </div>
                    <div class="mb-3">
                        <asp:Label runat="server" AssociatedControlID="Password" CssClass="form-label">Contraseña</asp:Label>
                        <asp:TextBox runat="server" ID="Password" CssClass="form-control" TextMode="Password" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="Password" CssClass="text-danger" ErrorMessage="El campo Contraseña es obligatorio." />
                    </div>
                    <div class="checkbox">
                        <asp:CheckBox runat="server" ID="RememberMe" Text="Recordarme" />
                    </div>
                    <div class="mb-3">
                        <asp:Button runat="server" OnClick="LogIn_Click" Text="Iniciar sesión" CssClass="btn btn-primary" />
                    </div>
                    <p>
                        <asp:HyperLink runat = "server" ID="RegisterHyperLink" ViewStateMode="Disabled">Registrarse como usuario nuevo</asp:HyperLink >
                    </p>
                </section>
            </div>
        </div>
    </asp:Content>