<%@ Page Title="Registrarse" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="GamerRankingApp.Account.Register" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <br />
    <br />
    <main aria-labelledby="title">
        <h2 id="title"><%: Title %>.</h2>
        <p class="text-danger">
            <asp:Literal runat="server" ID="ErrorMessage" />
        </p>
        <h4>Registrar cuenta nueva</h4>
        <hr />
        <asp:ValidationSummary runat="server" CssClass="text-danger" />
        <div class="row">
            <div class="col-md-8">
                <section id="loginForm">
                    <asp:ValidationSummary runat="server" CssClass="text-danger" />
                    <div class="mb-3">
                        <asp:Label runat="server" AssociatedControlID="Email" CssClass="form-label">Correo electrónico</asp:Label>
                        <asp:TextBox runat="server" ID="Email" CssClass="form-control" TextMode="Email" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="Email" CssClass="text-danger" ErrorMessage="El campo Correo electrónico es obligatorio." />
                        <asp:RegularExpressionValidator runat="server" ControlToValidate="Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" CssClass="text-danger" ErrorMessage="El formato del correo electrónico no es válido." />
                    </div>
                    <div class="mb-3">
                        <asp:Label runat="server" AssociatedControlID="Password" CssClass="form-label">Contraseña</asp:Label>
                        <asp:TextBox runat="server" ID="Password" CssClass="form-control" TextMode="Password" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="Password" CssClass="text-danger" ErrorMessage="El campo Contraseña es obligatorio." />
                    </div>
                    <div class="mb-3">
                        <asp:Label runat="server" AssociatedControlID="ConfirmPassword" CssClass="form-label">Confirmar contraseña</asp:Label>
                        <asp:TextBox runat="server" ID="ConfirmPassword" CssClass="form-control" TextMode="Password" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmPassword" CssClass="text-danger" ErrorMessage="El campo Confirmar contraseña es obligatorio." />
                        <asp:CompareValidator runat="server" ControlToValidate="ConfirmPassword" ControlToCompare="Password" CssClass="text-danger" ErrorMessage="La contraseña y la contraseña de confirmación no coinciden." />
                    </div>
                    <div class="mb-3">
                        <asp:Button runat="server" OnClick="CreateUser_Click" Text="Registrarse" CssClass="btn btn-primary" />
                    </div>
                </section>
            </div>
        </div>
    </main>
</asp:Content>
