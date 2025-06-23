<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GenerateCalificaciones.aspx.cs" Inherits="GamerRankingApp.Admin.GenerateCalificaciones" %>
    <asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <br /><br />
        <h2>Generación Masiva de Calificaciones</h2>

        <div class="mb-3">
            <asp:Label ID="Label1" runat="server" AssociatedControlID="txtQuantity" CssClass="form-label">Cantidad a Generar:</asp:Label>
            <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" TextMode="Number" Text="10000"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ControlToValidate="txtQuantity" ErrorMessage="La cantidad es obligatoria." CssClass="text-danger" Display="Dynamic" />
            <asp:RangeValidator ID="rvQuantity" runat="server" ControlToValidate="txtQuantity" MinimumValue="1" MaximumValue="2000000" Type="Integer" ErrorMessage="La cantidad debe ser entre 1 y 2,000,000." CssClass="text-danger" Display="Dynamic" />
        </div>

        <asp:Button ID="btnGenerate" runat="server" Text="Generar Calificaciones" CssClass="btn btn-primary" OnClick="btnGenerate_Click" />

        <asp:Label ID="lblMessage" runat="server" CssClass="mt-3" EnableViewState="false"></asp:Label>
    </asp:Content>
