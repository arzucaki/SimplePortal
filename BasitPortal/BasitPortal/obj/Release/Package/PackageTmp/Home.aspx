<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="BasitPortal.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="c1" runat="Server">
     <asp:Panel runat="server" DefaultButton="btnList">
        <div>
            <table class="auto-style1">
                <tr>
                     <td class="auto-style6" style="width: 678px">
                        
                    </td>
                     <td class="auto-style6" style="width: 678px">
                        
                    </td>
                     <td class="auto-style6" style="width: 678px">
                        
                    </td>
                     <td class="auto-style6" style="width: 678px">
                        
                    </td>
                     <td class="auto-style6" style="width: 678px">
                        
                    </td>
                     <td class="auto-style6" style="width: 678px">
                        
                    </td>
                     <td class="auto-style6" style="width: 678px">
                        
                    </td>
                     <td class="auto-style6" style="width: 678px">
                        
                    </td>
                     <td class="auto-style6" style="width: 678px">
                        
                    </td>
                     <td class="auto-style6" style="width: 678px">
                        <asp:Label ID="lblLogin" runat="server" Text="Giriş Yapıldı." ></asp:Label>
                        <asp:Button ID="btnCikis" runat="server" Text="Çıkış Yap" OnClick="btnCikis_Click" />
                    </td>
                      
                 </tr>
                <tr>
                    <td class="auto-style6" style="width: 678px">
                        
                    </td>
                    <td class="auto-style7" style="width: 965px">
                        &nbsp;


                        <asp:Label ID="lblwh_id" runat="server" Text="Depo Numarası: " ></asp:Label>
                        <asp:DropDownList ID="ddl_wh_id" class = "form-control" runat="server" AppendDataBoundItems="True" DataTextField="clientName" DataValueField="wh_id" OnSelectedIndexChanged="ddl_wh_id_SelectedIndexChanged" AutoPostBack="True">
                        </asp:DropDownList>
                        <asp:Label ID="lblprt" runat="server" Text="Ürün Kodu: "></asp:Label>
                        <asp:TextBox ID="txtprt"  class = "form-control" runat="server"></asp:TextBox>
                        <asp:Label ID="lbllot" runat="server" Text="Lot Numarası: "></asp:Label>
                        <asp:TextBox ID="txtlot"  class = "form-control" runat="server"></asp:TextBox>
                         <asp:Label ID="lblpo" runat="server" Text="PO Numarası: "></asp:Label>
                        <asp:TextBox ID="txtPO"  class = "form-control" runat="server"></asp:TextBox>
                        
                        <asp:Label ID="lblError" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                    </td>
                    <td>

                    </td>
                    <td  class="auto-style7" style="width: 965px" >   
                        <div id="date" aria-hidden="True">
                            <asp:Label ID="lblDate" runat="server" Text="Onay Tarihi : " Visible="false" ></asp:Label>
                            <asp:Calendar ID="cdate"  runat="server" Visible="false">   </asp:Calendar>
                        </div>
                    </td>
                    <td class="auto-style6">
                        <asp:CheckBox ID="cbConfirmed" runat="server" Text="Onaylananları getir." AutoPostBack="True" />
                        <asp:Button ID="btnList" class="btn btn-lg btn-primary btn-block" type="submit" runat="server" Text="Listele" Height="49px"  Width="178px" OnClick="btnList_Click" />
                        <asp:Button ID="btnConfirm" class="btn btn-lg btn-primary btn-block" type="submit" runat="server" Text="Onayla" Height="49px"  Width="178px" BorderStyle="None" OnClick="btnConfirm_Click" />
                        <asp:Button ID="btnConfirm2" Visible="false" runat="server" BorderStyle="None" class="btn btn-lg btn-primary btn-block" Height="49px" Text="F Onayı Ver" type="submit" Width="178px" OnClick="btnConfirm2_Click" />
                    </td>
                    <td style="width: 665px">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 678px">&nbsp; 
                        
                    </td>
                    <td class="auto-style2" style="width: 965px">&nbsp;&nbsp;<asp:Label ID="lblQantity" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label> &nbsp;
                        <asp:CheckBox ID="cbSelectAll" Text="Tümünü Seç" runat="server" OnCheckedChanged="cbSelectAll_CheckedChanged" AutoPostBack="True" />
                        <asp:GridView ID="gvInventory" runat="server" Width="1100px" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                             
                               <asp:TemplateField>                               
                                <ItemTemplate>                               
                                    <asp:CheckBox ID="CheckBox1"  OnCheckedChanged="CheckBox1_CheckedChanged" AutoPostBack="true" runat="server" />
                                </ItemTemplate>                                      
                               </asp:TemplateField>
                            
                            <asp:BoundField HeaderText="Ürün Kodu" DataField="prtnum" />
                            <asp:BoundField HeaderText="Ürün Tanımı" DataField="lngdsc" />
                            <asp:BoundField HeaderText="Lod Numarası" DataField="lodnum" />
                            <asp:BoundField HeaderText="Lot Numarası" DataField="lotnum" />
                            <asp:BoundField HeaderText="Detay ID" DataField="dtlnum" />
                            <asp:BoundField HeaderText="PO No" DataField="inv_attr_str8" />
                            <asp:BoundField HeaderText="Statü" DataField="invsts" />
                            <asp:BoundField HeaderText="SKT" DataField="fifdte" />
                            <asp:BoundField HeaderText="Miktar" DataField="untqty" />
                            <asp:BoundField HeaderText="Onay Numarası" DataField="inv_attr_str7" />
                            <asp:BoundField HeaderText="Onaylayan Kişi" DataField="inv_attr_str10" />
                            <asp:BoundField HeaderText="Onay Tarihi" DataField="inv_attr_dte1" />

                            </Columns>
                            <EditRowStyle BackColor="#2461BF" />
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#EFF3FB" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#F5F7FB" />
                            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                            <SortedDescendingCellStyle BackColor="#E9EBEF" />
                            <SortedDescendingHeaderStyle BackColor="#4870BE" />
                        </asp:GridView>
                    </td>
                    <td>   &nbsp;</td>
                    <td>
                      
                    </td>
                    <td> </td>
                    <td> </td>
                </tr>
            </table>
        </div>
          </asp:Panel>
</asp:Content>
