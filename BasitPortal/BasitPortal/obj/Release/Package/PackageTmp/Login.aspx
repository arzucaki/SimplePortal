<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="BasitPortal.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="c1" runat="Server">
    <asp:Panel runat="server" DefaultButton="btnLogin">
<div class="container" >
    <div class="col-md-12">
                    <h2 class="form-signin-heading"> </h2>
            <asp:Label ID="Label1" runat="server" BorderStyle="Dotted" Text="DHL Kalite Onay Portalı"></asp:Label>
            <table style="width: 100%; height: 113px;">
                <tr>
                    <td class="auto-style1">&nbsp;</td>
                    <td class="auto-style2">
                        <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style1">
                        <asp:Label ID="lblUserName"   runat="server" Text="Kullanıcı Adı :"></asp:Label>
                    </td>
                    <td class="auto-style2">
                        <asp:TextBox class = "form-control" ID="txtUserName" runat="server" Width="370px"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style1">&nbsp;</td>
                    <td class="auto-style2">
                        <asp:Button class="btn btn-lg btn-primary btn-block" type="submit" ID="btnLogin" runat="server" Text="Giriş Yap" OnClick="btnLogin_Click" />
                    </td>
                    <td>&nbsp;</td>
                </tr>
            </table>
         </div>
        </div>
        </asp:Panel>
</asp:Content>
