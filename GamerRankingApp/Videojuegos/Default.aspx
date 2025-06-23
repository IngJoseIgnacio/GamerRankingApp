<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="GamerRankingApp.Videojuegos.Default" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br /><br />
        <h2>Gestión de Videojuegos</h2>

        <div class="row mb-3">
            <div class="col-md-12">
                <asp:Button ID="btnCreateNew" runat="server" Text="Crear Nuevo Videojuego" CssClass="btn btn-success" OnClick="btnCreateNew_Click" />
                <asp:Button ID="btnShowRankingModal" runat="server" Text="Generar Ranking CSV" CssClass="btn btn-info ms-2" OnClick="btnShowRankingModal_Click" />
            </div>
        </div>

        <div class="row mb-3">
            <div class="col-md-4">
                <asp:Label ID="Label1" runat="server" Text="Nombre:"></asp:Label>
                <asp:TextBox ID="txtFilterName" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="col-md-4">
                <asp:Label ID="Label2" runat="server" Text="Compañía:"></asp:Label>
                <asp:TextBox ID="txtFilterCompany" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="col-md-4">
                <asp:Label ID="Label3" runat="server" Text="Año de Lanzamiento:"></asp:Label>
                <asp:TextBox ID="txtFilterYear" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RegularExpressionValidator ID="regexYear" runat="server" ControlToValidate="txtFilterYear"
                    ValidationExpression="^\d{4}$" ErrorMessage="Año inválido (YYYY)" CssClass="text-danger" Display="Dynamic" />
            </div>
        </div>
        <div class="row mb-3">
            <div class="col-md-12">
                <asp:Button ID="btnFilter" runat="server" Text="Filtrar" CssClass="btn btn-primary" OnClick="btnFilter_Click" />
            </div>
        </div>

        <asp:Label ID="lblMessage" runat="server" CssClass="mt-3" EnableViewState="false"></asp:Label>

        <asp:GridView ID="gvVideojuegos" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered mt-3"
            AllowPaging="True" PageSize="5" OnPageIndexChanging="gvVideojuegos_PageIndexChanging" OnRowCommand="gvVideojuegos_RowCommand"
            DataKeyNames="Id">
            <Columns>
                <asp:TemplateField HeaderText="Id">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkDetail" runat="server" CommandName="VerDetalle" CommandArgument='<%# Eval("Id") %>' Text='<%# Eval("Id") %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                <asp:BoundField DataField="Compania" HeaderText="Compañía" />
                <asp:BoundField DataField="AnoLanzamiento" HeaderText="Año" />
                <asp:BoundField DataField="Precio" HeaderText="Precio" DataFormatString="{0:C}" />
                <asp:BoundField DataField="Puntaje" HeaderText="Puntaje" DataFormatString="{0:F2}" />
                <asp:TemplateField HeaderText="Acciones">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Editar" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-sm btn-warning">Editar</asp:LinkButton>
                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Eliminar" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-sm btn-danger ms-1"
                            OnClientClick='<%# (User.IsInRole("Administrator") ? "return confirm(\"¿Está seguro de que desea eliminar este videojuego?\");" : "return false;") %>'
                            Enabled='<%# User.IsInRole("Administrator") %>'>Eliminar</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerStyle CssClass="pagination-ys" />
        </asp:GridView>

        <%-- Modal para Generar Ranking (RETO 06) --%>
        <asp:Button ID="HiddenTargetControlForModal" runat="server" style="display:none;" />
        <ajaxToolkit:ModalPopupExtender ID="RankingModalPopupExtender" runat="server" TargetControlID="HiddenTargetControlForModal"
            PopupControlID="PanelRanking" CancelControlID="btnCloseRankingModal" BackgroundCssClass="modalBackground">
        </ajaxToolkit:ModalPopupExtender>

        <asp:Panel ID="PanelRanking" runat="server" CssClass="modal-content-custom" style="display:none;">
            <div class="modal-header-custom">
                <h5 class="modal-title-custom">Generar Ranking de Videojuegos</h5>
                <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="$find('RankingModalPopupExtender').hide(); return false;" CssClass="btn-close-custom" Text="&times;"></asp:LinkButton>
            </div>
            <div class="modal-body-custom">
                <div class="mb-3">
                    <asp:Label ID="Label4" runat="server" Text="Top deseado (0 para todos):" CssClass="form-label"></asp:Label>
                    <asp:TextBox ID="txtTopDesired" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                    <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtTopDesired"
                        MinimumValue="0" MaximumValue="10000000" Type="Integer" CssClass="text-danger" ErrorMessage="El Top debe ser un número entero positivo o 0."></asp:RangeValidator>
                </div>
                <asp:Label ID="lblRankingMessage" runat="server" CssClass="text-danger mb-3" EnableViewState="false"></asp:Label>
            </div>
            <div class="modal-footer-custom">
                <asp:Button ID="btnCloseRankingModal" runat="server" Text="Cerrar" CssClass="btn btn-secondary" />
                <asp:Button ID="btnGenerateDownloadRanking" runat="server" Text="Generar y Descargar Ranking" CssClass="btn btn-primary" OnClick="btnGenerateDownloadRanking_Click" />
            </div>
        </asp:Panel>

        <style>
            /* Estilos básicos para el modal */
            .modalBackground {
                background-color: rgba(0, 0, 0, 0.7);
                filter: alpha(opacity=70);
            }
            .modal-content-custom {
                background-color: #fefefe;
                margin: 15% auto;
                padding: 20px;
                border: 1px solid #888;
                width: 80%;
                max-width: 500px;
                border-radius: 8px;
                box-shadow: 0 4px 8px rgba(0,0,0,0.2);
            }
            .modal-header-custom {
                display: flex;
                justify-content: space-between;
                align-items: center;
                border-bottom: 1px solid #dee2e6;
                padding-bottom: 10px;
                margin-bottom: 15px;
            }
            .modal-title-custom {
                margin: 0;
                font-size: 1.25rem;
            }
            .btn-close-custom {
                font-size: 1.5rem;
                font-weight: bold;
                border: none;
                background: none;
                cursor: pointer;
            }
            .modal-body-custom {
                padding-bottom: 15px;
            }
            .modal-footer-custom {
                border-top: 1px solid #dee2e6;
                padding-top: 10px;
                display: flex;
                justify-content: flex-end;
                gap: 10px;
            }
            /* Estilos de paginación para GridView */
            .pagination-ys {
                display: flex;
                padding-left: 0;
                list-style: none;
                border-radius: .25rem;
                justify-content: center;
                margin-top: 20px;
            }
            .pagination-ys li {
                display: list-item;
            }
            .pagination-ys a, .pagination-ys span {
                position: relative;
                display: block;
                padding: .5rem .75rem;
                margin-left: -1px;
                line-height: 1.25;
                color: #007bff;
                background-color: #fff;
                border: 1px solid #dee2e6;
                text-decoration: none;
            }
            .pagination-ys li:first-child a, .pagination-ys li:first-child span {
                border-top-left-radius: .25rem;
                border-bottom-left-radius: .25rem;
            }
            .pagination-ys li:last-child a, .pagination-ys li:last-child span {
                border-top-right-radius: .25rem;
                border-bottom-right-radius: .25rem;
            }
            .pagination-ys .active a, .pagination-ys .active span {
                z-index: 1;
                color: #fff;
                background-color: #007bff;
                border-color: #007bff;
            }
            .pagination-ys .disabled a, .pagination-ys .disabled span {
                color: #6c757d;
                pointer-events: none;
                cursor: auto;
                background-color: #fff;
                border-color: #dee2e6;
            }
        </style>

    </asp:Content>
