<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="GamerRankingApp.Videojuegos.Detail" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <br /><br />
        <h2>Detalle del Videojuego</h2>

        <div class="card">
            <div class="card-body">
                <h5 class="card-title"><asp:Literal ID="litNombre" runat="server"></asp:Literal></h5>
                <p class="card-text"><strong>Compañía:</strong> <asp:Literal ID="litCompania" runat="server"></asp:Literal></p>
                <p class="card-text"><strong>Año de Lanzamiento:</strong> <asp:Literal ID="litAnoLanzamiento" runat="server"></asp:Literal></p>
                <p class="card-text"><strong>Precio:</strong> <asp:Literal ID="litPrecio" runat="server"></asp:Literal></p>
                <p class="card-text"><strong>Puntaje Promedio:</strong> <asp:Literal ID="litPuntaje" runat="server"></asp:Literal></p>
                <p class="card-text"><small class="text-muted">Última Actualización: <asp:Literal ID="litFechaActualizacion" runat="server"></asp:Literal> por <asp:Literal ID="litUsuarioActualizacion" runat="server"></asp:Literal></small></p>
                <asp:Button ID="btnBack" runat="server" Text="Volver al Listado" CssClass="btn btn-secondary mt-3" OnClick="btnBack_Click" />
            </div>
        </div>

        <asp:Label ID="lblMessage" runat="server" CssClass="mt-3" EnableViewState="false"></asp:Label>
    </asp:Content>
