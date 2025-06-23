<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Form.aspx.cs" Inherits="GamerRankingApp.Videojuegos.Form" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <br /><br />
        <h2><asp:Literal ID="litFormTitle" runat="server"></asp:Literal> Videojuego</h2>

        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="text-danger" />

        <div class="mb-3">
            <asp:Label ID="Label1" runat="server" AssociatedControlID="txtNombre" CssClass="form-label">Nombre:</asp:Label>
            <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvNombre" runat="server" ControlToValidate="txtNombre" ErrorMessage="El nombre es obligatorio." CssClass="text-danger" Display="Dynamic" />
        </div>
        <div class="mb-3">
            <asp:Label ID="Label2" runat="server" AssociatedControlID="txtCompania" CssClass="form-label">Compañía:</asp:Label>
            <asp:TextBox ID="txtCompania" runat="server" CssClass="form-control"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvCompania" runat="server" ControlToValidate="txtCompania" ErrorMessage="La compañía es obligatoria." CssClass="text-danger" Display="Dynamic" />
        </div>
        <div class="mb-3">
            <asp:Label ID="Label3" runat="server" AssociatedControlID="txtAnoLanzamiento" CssClass="form-label">Año de Lanzamiento:</asp:Label>
            <asp:TextBox ID="txtAnoLanzamiento" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvAnoLanzamiento" runat="server" ControlToValidate="txtAnoLanzamiento" ErrorMessage="El año de lanzamiento es obligatorio." CssClass="text-danger" Display="Dynamic" />
            <asp:RangeValidator ID="rvAnoLanzamiento" runat="server" ControlToValidate="txtAnoLanzamiento" Type="Integer" MinimumValue="1900" MaximumValue="2100" ErrorMessage="El año debe ser entre 1900 y 2100." CssClass="text-danger" Display="Dynamic" />
        </div>
        <div class="mb-3">
            <asp:Label ID="Label4" runat="server" AssociatedControlID="txtPrecio" CssClass="form-label">Precio:</asp:Label>
            <asp:TextBox ID="txtPrecio" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvPrecio" runat="server" ControlToValidate="txtPrecio" ErrorMessage="El precio es obligatorio." CssClass="text-danger" Display="Dynamic" />
            <asp:CompareValidator ID="cvPrecio" runat="server" ControlToValidate="txtPrecio" Operator="GreaterThanEqual" ValueToCompare="0" Type="Currency" ErrorMessage="El precio debe ser un valor positivo." CssClass="text-danger" Display="Dynamic" />
        </div>

        <asp:Label ID="lblMessage" runat="server" CssClass="mt-3" EnableViewState="false"></asp:Label>

        <div class="mt-4">
            <asp:Button ID="btnSave" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnSave_Click" />
            <ajaxToolkit:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="btnSave"
                ConfirmText="¿Está seguro de que desea guardar los cambios?" Enabled="false" /> <%-- Se habilita dinámicamente si es actualización --%>
            <asp:Button ID="btnCancel" runat="server" Text="Cancelar" CssClass="btn btn-secondary ms-2" OnClick="btnCancel_Click" CausesValidation="false" />
        </div>
    </asp:Content>
