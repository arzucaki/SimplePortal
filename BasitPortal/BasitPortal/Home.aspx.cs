using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace BasitPortal
{
    public partial class Home : System.Web.UI.Page
    {
        string User = string.Empty;

        string test_url = "http://czcholstc000569:4525";
        string prod_url = "http://jdawm-trinsp1.dhl.com:7150";
        string test_user_Name = "can";
        string test_password = "can";
        string prod_user_name = "dsctr";
        string prod_password = "dsctr";

        string url = string.Empty;
        string userName = string.Empty;
        string password = string.Empty;
        string systype= string.Empty;

        string command = string.Empty;

        string wh_id = string.Empty;
        DataAdapterRest dataobj = new DataAdapterRest();
        string jsonresponse=string.Empty;

        SqlCommand cmd = new SqlCommand();
        SqlConnection con = new SqlConnection();
        SqlDataAdapter sda = new SqlDataAdapter();

        DataSet ds = new DataSet();
        int totalQantity = 0;
        string conn = ConfigurationManager.ConnectionStrings["JDAQuality"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            url = test_url;
            userName = test_user_Name;
            password = test_password;

            url = prod_url;
            userName = prod_user_name;
            password = prod_password;

            wh_id = ddl_wh_id.SelectedValue;

            using (SqlConnection myConnection = new SqlConnection(conn))
            {

                SqlCommand oCmd = new SqlCommand("select * from [connection] where wh_id=@wh_id and platform=\'prod\'", myConnection);
                oCmd.Parameters.AddWithValue("@wh_id", wh_id);
                myConnection.Open();
                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        url = oReader["url"].ToString();
                        userName = oReader["userName"].ToString();
                        password = oReader["password"].ToString();
                        systype = oReader["sysType"].ToString();
                    }
                    myConnection.Close();
                }
            }
            if (systype == "RP" && !cbConfirmed.Checked)
            {
                lblpo.Visible = false;
                txtPO.Visible = false;
                lblDate.Visible = false;
                cdate.Visible = false;
                if (wh_id=="BOE1")
                {
                    btnBarcode.Visible = true;
                }
                else
                {
                    btnBarcode.Visible = false;
                }
            }
            else if (systype == "JDA" && !cbConfirmed.Checked)
            {
                lblpo.Visible = true;
                txtPO.Visible = true;
                lblDate.Visible = false;
                cdate.Visible = false;
            }
            else
            {
                lblpo.Visible = true;
                txtPO.Visible = true;
                lblDate.Visible = true;
                cdate.Visible = true;
                if (wh_id == "BOE1")
                {
                    btnBarcode.Visible = true;
                }
                else
                {
                    btnBarcode.Visible = false;
                }
            }
            if (!Page.IsPostBack)
            {
                url = prod_url;
                userName = prod_user_name;
                password = prod_password;

                using (SqlConnection myConnection = new SqlConnection(conn))
                {

                    SqlCommand oCmd = new SqlCommand("select * from [Client]", myConnection);
                    myConnection.Open();
                    SqlDataAdapter sda = new SqlDataAdapter(oCmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    ddl_wh_id.DataSource = dt;
                    ddl_wh_id.DataBind();
                }

                wh_id = ddl_wh_id.SelectedValue;

                using (SqlConnection myConnection = new SqlConnection(conn))
                {

                    SqlCommand oCmd = new SqlCommand("select * from [connection] where wh_id=@wh_id  and platform=\'prod\'", myConnection);
                    oCmd.Parameters.AddWithValue("@wh_id", wh_id);
                    myConnection.Open();
                    using (SqlDataReader oReader = oCmd.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            url = oReader["url"].ToString();
                            userName = oReader["userName"].ToString();
                            password = oReader["password"].ToString();
                            systype = oReader["sysType"].ToString();
                        }
                        myConnection.Close();
                    }
                }

                User = Request.QueryString["User"];
                lblLogin.Text = User;
                cbSelectAll.CheckedChanged += new EventHandler(cbSelectAll_CheckedChanged);
                if (systype == "RP" && !cbConfirmed.Checked)
                {
                    lblpo.Visible = false;
                    txtPO.Visible = false;
                    lblDate.Visible = false;
                    cdate.Visible = false;
                    if (wh_id == "BOE1")
                    {
                        btnBarcode.Visible = true;
                    }
                    else
                    {
                        btnBarcode.Visible = false;
                    }
                }
                else if (systype == "JDA" && !cbConfirmed.Checked)
                {
                    lblpo.Visible = true;
                    txtPO.Visible = true;
                    lblDate.Visible = false;
                    cdate.Visible = false;
                }
                else
                {
                    lblpo.Visible = true;
                    txtPO.Visible = true;
                    lblDate.Visible = true;
                    cdate.Visible = true;
                    if (wh_id == "BOE1")
                    {
                        btnBarcode.Visible = true;
                    }
                    else
                    {
                        btnBarcode.Visible = false;
                    }
                }
            }
        }

        public void getList() {
            if (txtPO.Text != string.Empty && wh_id != "TR_0126_02")
            {
                command = "[select a.prtnum , p.lngdsc, a.lodnum, a.lotnum, a.dtlnum,  substr(a.inv_attr_str8, length(a.inv_attr_str8) - 5) inv_attr_str8, a.invsts, a.untqty, a.inv_attr_str7, a.inv_attr_str10, a.inv_attr_dte1, a.fifdte from all_inventory_view a left join prtdsc p on p.colval = a.prtnum || '|' || a.prt_client_id || '|' || a.wh_id and p.Locale_id = 'TURKISH'"
                           + " where a.inv_attr_str7 is null and a.invsts not in('-','QA2','AV','DES', 'SM') and a.wh_id = '" + wh_id
                           + "' and a.lotnum = '" + txtlot.Text
                           + "' and a.inv_attr_str8 like '%" + txtPO.Text
                           + "' and a.prtnum = '" + txtprt.Text + "']";
            }
            else if (txtPO.Text == string.Empty && wh_id == "TR_0126_02")
            {
                command = "[select a.prtnum , p.lngdsc, a.lodnum, a.lotnum, a.dtlnum, inv_attr_str8, a.invsts, a.untqty, a.inv_attr_str7, a.inv_attr_str10, a.inv_attr_dte1, a.fifdte from all_inventory_view a left join prtdsc p on p.colval = a.prtnum || '|' || a.prt_client_id || '|' || a.wh_id and p.Locale_id = 'TURKISH'"
                           + " where a.inv_attr_str7 is null and a.invsts='1K' and a.wh_id = '" + wh_id
                           + "' and a.lotnum = '" + txtlot.Text
                           + "' and a.prtnum = '" + txtprt.Text + "']";
            }
            else if (txtPO.Text != string.Empty && wh_id == "TR_0126_02")
            {
                command = "[select a.prtnum , p.lngdsc, a.lodnum, a.lotnum, a.dtlnum, inv_attr_str8, a.invsts, a.untqty, a.inv_attr_str7, a.inv_attr_str10, a.inv_attr_dte1, a.fifdte from all_inventory_view a left join prtdsc p on p.colval = a.prtnum || '|' || a.prt_client_id || '|' || a.wh_id and p.Locale_id = 'TURKISH'"
                           + " where a.inv_attr_str7 is null and a.invsts='1K' and a.wh_id = '" + wh_id
                           + "' and a.lotnum = '" + txtlot.Text
                           + "' and a.inv_attr_str8 like '%" + txtPO.Text
                           + "' and a.prtnum = '" + txtprt.Text + "']";
            }
            else{
                command = "[select a.prtnum , p.lngdsc, a.lodnum, a.lotnum, a.dtlnum,  substr(a.inv_attr_str8, length(a.inv_attr_str8) - 5) inv_attr_str8, a.invsts, a.untqty, a.inv_attr_str7, a.inv_attr_str10, a.inv_attr_dte1, a.fifdte from all_inventory_view a left join prtdsc p on p.colval = a.prtnum || '|' || a.prt_client_id || '|' || a.wh_id and p.Locale_id = 'TURKISH'"
                           + " where a.inv_attr_str7 is null and a.invsts not in('-','QA2','AV','DES','SM') and a.wh_id = '" + wh_id
                           + "' and a.lotnum = '" + txtlot.Text
                           + "' and a.prtnum = '" + txtprt.Text + "']";
            }           

            DataAdapterRest dataobj = new DataAdapterRest();
            string jsonresponse = dataobj.getData(url, userName, password, command, wh_id);

            
            if (jsonresponse=="No Data")
            {
                lblError.Text = "Onaylanacak ürün bulunamadı!";
                gvInventory.Visible = false;
            }
            else
            {
                gvInventory.Visible = true;
                Models.Inventory inventoryList = JsonConvert.DeserializeObject<Models.Inventory>(jsonresponse);
                gvInventory.DataSource = inventoryList.data;
                gvInventory.DataBind();
            }          
        }
        public void getConfirmedList()
        {
            DateTime defaultdate = Convert.ToDateTime("1.01.0001 00:00:00");
            DateTime selectedDate = cdate.SelectedDate;
            DateTime nextDay = cdate.SelectedDate.AddDays(1);
            string selectedDateChar = selectedDate.Year.ToString() + selectedDate.Month.ToString("00") + selectedDate.Day.ToString("00");
            string nextDayChar= nextDay.Year.ToString() + nextDay.Month.ToString("00") + nextDay.Day.ToString("00");

            if (txtPO.Text != string.Empty && selectedDate != defaultdate)//PO ve tarih doluysa
            {
                command = "[select a.prtnum , p.lngdsc, a.lodnum, a.lotnum, a.dtlnum,  substr(a.inv_attr_str8, length(a.inv_attr_str8) - 5) inv_attr_str8, a.invsts, a.untqty, a.inv_attr_str7, a.inv_attr_str10, a.inv_attr_dte1, a.fifdte from all_inventory_view a left join prtdsc p on p.colval = a.prtnum || '|' || a.prt_client_id || '|' || a.wh_id and p.Locale_id = 'TURKISH'"
                            + " where a.inv_attr_str7 is not null and a.wh_id = '" + wh_id
                            + "' and a.lotnum = '" + txtlot.Text
                            + "' and a.inv_attr_str8 like '%" + txtPO.Text
                            + "' and a.prtnum = '" + txtprt.Text
                            + "' and a.inv_attr_dte1<'" + nextDayChar
                            + "' and a.inv_attr_dte1>'" + selectedDateChar + "']";
            }
            else if (txtPO.Text != string.Empty && selectedDate == defaultdate)//Sadece PO doluysa
            {
                command = "[select a.prtnum , p.lngdsc, a.lodnum, a.lotnum, a.dtlnum,  substr(a.inv_attr_str8, length(a.inv_attr_str8) - 5) inv_attr_str8, a.invsts, a.untqty, a.inv_attr_str7, a.inv_attr_str10, a.inv_attr_dte1, a.fifdte  from all_inventory_view a left join prtdsc p on p.colval = a.prtnum || '|' || a.prt_client_id || '|' || a.wh_id and p.Locale_id = 'TURKISH'"
                            + " where a.inv_attr_str7 is not null and a.wh_id = '" + wh_id
                            + "' and a.lotnum = '" + txtlot.Text
                            + "' and a.inv_attr_str8 like '%" + txtPO.Text
                            + "' and a.prtnum = '" + txtprt.Text + "']";
            }
            else if (selectedDate == defaultdate && txtPO.Text == string.Empty)//ikisi de boşsa
            {
                command = "[select a.prtnum , p.lngdsc, a.lodnum, a.lotnum, a.dtlnum,  substr(a.inv_attr_str8, length(a.inv_attr_str8) - 5) inv_attr_str8, a.invsts, a.untqty, a.inv_attr_str7, a.inv_attr_str10, a.inv_attr_dte1, a.fifdte  from all_inventory_view a left join prtdsc p on p.colval = a.prtnum || '|' || a.prt_client_id || '|' || a.wh_id and p.Locale_id = 'TURKISH'"
                            + " where a.inv_attr_str7 is not null and a.wh_id = '" + wh_id
                            + "' and a.lotnum = '" + txtlot.Text
                            + "' and a.prtnum = '" + txtprt.Text + "']";
            }
            else if (selectedDate != defaultdate && txtPO.Text == string.Empty)//Sadece tarih doluysa
            {
                command = "[select a.prtnum , p.lngdsc, a.lodnum, a.lotnum, a.dtlnum,  substr(a.inv_attr_str8, length(a.inv_attr_str8) - 5) inv_attr_str8, a.invsts, a.untqty, a.inv_attr_str7, a.inv_attr_str10, a.inv_attr_dte1, a.fifdte from all_inventory_view a left join prtdsc p on p.colval = a.prtnum || '|' || a.prt_client_id || '|' || a.wh_id and p.Locale_id = 'TURKISH'"
                            + " where a.inv_attr_str7 is not null and a.wh_id = '" + wh_id
                            + "' and a.lotnum = '" + txtlot.Text
                            + "' and a.prtnum = '" + txtprt.Text
                            + "' and a.inv_attr_dte1<'" + nextDayChar
                            + "' and a.inv_attr_dte1>'" + selectedDateChar + "']";
            }

            DataAdapterRest dataobj = new DataAdapterRest();
            string jsonresponse = dataobj.getData(url, userName, password, command, wh_id);

            if (jsonresponse == "No Data")
            {
                lblError.Text = "Onaylanan ürün bulunamadı!";
                gvInventory.Visible = false;
            }
            else
            {
                gvInventory.Visible = true;
                Models.Inventory inventoryList = JsonConvert.DeserializeObject<Models.Inventory>(jsonresponse);
                gvInventory.DataSource = inventoryList.data;
                gvInventory.DataBind();
                btnConfirm.Text = "Onay Kaldır";
            }

        }
        public void getRPList()
        {
            command = "getlist";
            DataAdapterRP dataobj = new DataAdapterRP();
            string jsonresponse = dataobj.getDataList(url, userName, password, command, wh_id, txtprt.Text, txtlot.Text);

            if (jsonresponse == "No Data")
            {
                lblError.Text = "Onaylanacak ürün bulunamadı!";
                gvInventory.Visible = false;
            }
            else
            {
                gvInventory.Visible = true;
                Models.InventoryRP inventoryList = JsonConvert.DeserializeObject<Models.InventoryRP>(jsonresponse);
                gvInventory.DataSource = inventoryList.data;
                gvInventory.DataBind();
            }
        }
        public void getRPConfirmedList()
        {
            DateTime defaultdate = Convert.ToDateTime("1.01.0001 00:00:00");
            DateTime selectedDate = cdate.SelectedDate;
            DateTime nextDay = cdate.SelectedDate.AddDays(1);
            string selectedDateChar = selectedDate.Year.ToString() + selectedDate.Month.ToString("00") + selectedDate.Day.ToString("00");
            string nextDayChar = nextDay.Year.ToString() + nextDay.Month.ToString("00") + nextDay.Day.ToString("00");

            if (selectedDate == defaultdate)//Tarih boşsa
            {
                command = "getconfirmedlist";
            }
            else if (selectedDate != defaultdate )//Tarih doluysa
            {
                command = "getconfirmedlistdate";
            }
           
            DataAdapterRP dataobj = new DataAdapterRP();
            string jsonresponse = dataobj.getConfirmedDataList(url, userName, password, command, wh_id, txtprt.Text, txtlot.Text, selectedDateChar, nextDayChar);

            if (jsonresponse == "No Data")
            {
                lblError.Text = "Onaylanan ürün bulunamadı!";
                gvInventory.Visible = false;
            }
            else
            {
                gvInventory.Visible = true;
                Models.InventoryRP inventoryList = JsonConvert.DeserializeObject<Models.InventoryRP>(jsonresponse);
                gvInventory.DataSource = inventoryList.data;
                gvInventory.DataBind();
                btnConfirm.Text = "Onay Kaldır";
            }

        }
        public void getBaxterConfirmedList()
        {
            DateTime defaultdate = Convert.ToDateTime("1.01.0001 00:00:00");
            DateTime selectedDate = cdate.SelectedDate;
            DateTime nextDay = cdate.SelectedDate.AddDays(1);
            string selectedDateChar = selectedDate.Year.ToString() + selectedDate.Month.ToString("00") + selectedDate.Day.ToString("00");
            string nextDayChar = nextDay.Year.ToString() + nextDay.Month.ToString("00") + nextDay.Day.ToString("00");

            if (txtPO.Text != string.Empty && selectedDate != defaultdate)//PO ve tarih doluysa
            {
                command = "[select a.prtnum , p.lngdsc, a.lodnum, a.lotnum, a.dtlnum,  substr(a.inv_attr_str8, length(a.inv_attr_str8) - 5) inv_attr_str8, a.invsts, a.untqty, a.inv_attr_str7, a.inv_attr_str10, a.inv_attr_dte1, a.fifdte from all_inventory_view a left join prtdsc p on p.colval = a.prtnum || '|' || a.prt_client_id || '|' || a.wh_id and p.Locale_id = 'TURKISH'"
                            + " where (a.invsts='S' or a.invsts='-' or a.invsts = 'L' or (a.invsts='K' and a.inv_attr_str7 is not null)) and a.wh_id = '" + wh_id
                            + "' and a.lotnum = '" + txtlot.Text
                            + "' and a.inv_attr_str8 like '%" + txtPO.Text
                            + "' and a.prtnum = '" + txtprt.Text
                            + "' and a.inv_attr_dte1<'" + nextDayChar
                            + "' and a.inv_attr_dte1>'" + selectedDateChar + "']";
            }
            else if (txtPO.Text != string.Empty && selectedDate == defaultdate)//Sadece PO doluysa
            {
                command = "[select a.prtnum , p.lngdsc, a.lodnum, a.lotnum, a.dtlnum,  substr(a.inv_attr_str8, length(a.inv_attr_str8) - 5) inv_attr_str8, a.invsts, a.untqty, a.inv_attr_str7, a.inv_attr_str10, a.inv_attr_dte1, a.fifdte from all_inventory_view a left join prtdsc p on p.colval = a.prtnum || '|' || a.prt_client_id || '|' || a.wh_id and p.Locale_id = 'TURKISH'"
                            + " where (a.invsts='S' or a.invsts='-' or a.invsts = 'L' or (a.invsts='K' and a.inv_attr_str7 is not null)) and a.wh_id = '" + wh_id
                            + "' and a.lotnum = '" + txtlot.Text
                            + "' and a.inv_attr_str8 like '%" + txtPO.Text
                            + "' and a.prtnum = '" + txtprt.Text + "']";
            }
            else if (selectedDate == defaultdate && txtPO.Text == string.Empty)//ikisi de boşsa
            {
                command = "[select a.prtnum , p.lngdsc, a.lodnum, a.lotnum, a.dtlnum,  substr(a.inv_attr_str8, length(a.inv_attr_str8) - 5) inv_attr_str8, a.invsts, a.untqty, a.inv_attr_str7, a.inv_attr_str10, a.inv_attr_dte1, a.fifdte from all_inventory_view a left join prtdsc p on p.colval = a.prtnum || '|' || a.prt_client_id || '|' || a.wh_id and p.Locale_id = 'TURKISH'"
                            + " where (a.invsts='S' or a.invsts='-' or a.invsts = 'L' or (a.invsts='K' and a.inv_attr_str7 is not null)) and a.wh_id = '" + wh_id
                            + "' and a.lotnum = '" + txtlot.Text
                            + "' and a.prtnum = '" + txtprt.Text + "']";
            }
            else if (selectedDate != defaultdate && txtPO.Text == string.Empty)//Sadece tarih doluysa
            {
                command = "[select a.prtnum , p.lngdsc, a.lodnum, a.lotnum, a.dtlnum,  substr(a.inv_attr_str8, length(a.inv_attr_str8) - 5) inv_attr_str8, a.invsts, a.untqty, a.inv_attr_str7, a.inv_attr_str10, a.inv_attr_dte1, a.fifdte from all_inventory_view a left join prtdsc p on p.colval = a.prtnum || '|' || a.prt_client_id || '|' || a.wh_id and p.Locale_id = 'TURKISH'"
                            + " where (a.invsts='S' or a.invsts='-' or a.invsts = 'L' or (a.invsts='K' and a.inv_attr_str7 is not null)) and a.wh_id = '" + wh_id
                            + "' and a.lotnum = '" + txtlot.Text
                            + "' and a.prtnum = '" + txtprt.Text
                            + "' and a.inv_attr_dte1<'" + nextDayChar
                            + "' and a.inv_attr_dte1>'" + selectedDateChar + "']";
            }

            DataAdapterRest dataobj = new DataAdapterRest();
            string jsonresponse = dataobj.getData(url, userName, password, command, wh_id);


            if (jsonresponse == "No Data")
            {
                lblError.Text = "Onaylanan ürün bulunamadı!";
                gvInventory.Visible = false;
            }
            else
            {
                gvInventory.Visible = true;
                Models.Inventory inventoryList = JsonConvert.DeserializeObject<Models.Inventory>(jsonresponse);
                gvInventory.DataSource = inventoryList.data;
                gvInventory.DataBind();
                btnConfirm.Text = "Onay Kaldır";
            }

        }
       
        public void processStatusChange(string status, string dtlnum)
        {
            string command = "process inventory status change where prc_reacod = 'WEB' and wh_id = '" + wh_id + "' and dtlnum = '" + dtlnum + "' and to_invsts = '"+status+"'";         
            DataAdapterRest dataobj = new DataAdapterRest();
            string jsonresponse = dataobj.getData(url, userName, password, command, wh_id);          
        }
        public void processStatusChangeACINO(string status, string dtlnum)
        {
            string command = "[select dtlnum from all_inventory_view where dtlnum = '" + dtlnum + "' and stoloc not in ('SAHIT NUMUNE', 'SAHIT OFIS')] | process inventory status change where prc_reacod = 'WEB' and wh_id = '" + wh_id + "' and dtlnum = @dtlnum and to_invsts = '" + status + "'";
            DataAdapterRest dataobj = new DataAdapterRest();
            string jsonresponse = dataobj.getData(url, userName, password, command, wh_id);
        }

        public void confirmStatus()
        {
            string po = string.Empty;
            foreach (GridViewRow item in gvInventory.Rows)
            {
                CheckBox chk = (CheckBox)item.FindControl("CheckBox1");
                if (chk != null)
                {
                    if (chk.Checked)
                    {
                        po = item.Cells[6].Text.Replace("&nbsp;", "").Substring(item.Cells[6].Text.Length - 6) + "_" + txtlot.Text;
                        string command = "[update invdtl set inv_attr_str7='" + po + "', inv_attr_dte1='" + DateTime.Now.AddHours(-2).ToString("yyyyMMdd HHmm") + "', inv_attr_str10='" + Request.QueryString["User"] + "' where dtlnum='" + item.Cells[5].Text + "']";
                   
                        DataAdapterRest dataobj = new DataAdapterRest();
                        string jsonresponse = dataobj.getData(url, userName, password, command, wh_id);
                        if (wh_id=="TR_0126_02")
                        {
                            processStatusChange("1U", item.Cells[5].Text);
                        }
                        else if(wh_id == "TR_0041_09")
                        {
                            processStatusChangeACINO("QA2", item.Cells[5].Text);
                        }
                        else if(wh_id=="TR_0126")
                        {
                            processStatusChange("AV", item.Cells[5].Text);
                        }
                        else
                        {
                            processStatusChange("AV", item.Cells[5].Text);
                        }
                    }
                }
            }
            lblError.Text = po + " Onay Numarasıyla Onaylandı!";
            //Response.Write("<script>alert('"+txtlot.Text + "_" + txtprt.Text + " Onay Numarasıyla Onaylandı!"+"');</script>");
            totalQantity = 0;
            lblQantity.Text = "Toplam Miktar: " + totalQantity.ToString();
        }
        public void unConfirmStatus()
        {
            foreach (GridViewRow item in gvInventory.Rows)
            {
                CheckBox chk = (CheckBox)item.FindControl("CheckBox1");
                if (chk != null)
                {
                    if (chk.Checked)
                    {
                        string command = "[update invdtl set inv_attr_str7=null, inv_attr_str10=null, inv_attr_dte1=null where dtlnum='" + item.Cells[5].Text + "']";

                        DataAdapterRest dataobj = new DataAdapterRest();
                        string jsonresponse = dataobj.getData(url, userName, password, command, wh_id);

                        if (wh_id == "TR_0126_02")
                        {
                            processStatusChange("1K", item.Cells[5].Text);
                        }
                        else if (wh_id == "TR_0041_09")
                        {
                            processStatusChange("QA", item.Cells[5].Text);
                        }
                        else if (wh_id == "TR_0126")
                        {
                            processStatusChange("BL", item.Cells[5].Text);
                        }
                        else
                        {
                            processStatusChange("QA", item.Cells[5].Text);
                        }
                        
                    }
                }
            }
            lblError.Text = "Onaylar kaldırıldı! Kalite Statüsüne Çekildi.";
            totalQantity = 0;
            lblQantity.Text = "Toplam Miktar: " + totalQantity.ToString();
        }

        public void confirmStatusRP()
        {
            string po = string.Empty;
            string confirmDate = string.Empty;
            string confirmUser = string.Empty;
            string toStatus = string.Empty;
            string dtlnum = string.Empty;
            string command = string.Empty;

            foreach (GridViewRow item in gvInventory.Rows)
            {
                CheckBox chk = (CheckBox)item.FindControl("CheckBox1");
                if (chk != null)
                {
                    if (chk.Checked)
                    {
                        if (wh_id=="BOE1")
                        {
                            string statu = item.Cells[7].Text;
                            if (statu == "Q-010R")
                            {
                                lblError.Text = statu + " Barkodlanmamış bir ürüne onay vermeye çalışıyorsunuz!";
                            }
                            else if (statu == "Q-010F")
                            {
                                po = item.Cells[6].Text.Replace("&nbsp;", "").Substring(item.Cells[6].Text.Length - 6) + "_" + txtlot.Text;
                                confirmDate = DateTime.Now.AddHours(-2).ToString("yyyyMMdd HHmm");
                                confirmUser = Request.QueryString["User"];
                                dtlnum = item.Cells[5].Text;
                                command = "statuschange";

                                toStatus = "F";

                                DataAdapterRP dataobj = new DataAdapterRP();
                                string jsonresponse = dataobj.processStatusChange(url, userName, password, command, wh_id, po, confirmDate, confirmUser, dtlnum, toStatus);
                            }
                            else
                            {
                                lblError.Text = statu + " Bu statüdeki bir ürün onaylanamaz!";
                            }
                        }
                        else
                        {
                            po = item.Cells[6].Text.Replace("&nbsp;", "").Substring(item.Cells[6].Text.Length - 6) + "_" + txtlot.Text;
                            confirmDate = DateTime.Now.AddHours(-2).ToString("yyyyMMdd HHmm");
                            confirmUser = Request.QueryString["User"];
                            dtlnum = item.Cells[5].Text;
                            command = "statuschange";

                            toStatus = "Q";

                            DataAdapterRP dataobj = new DataAdapterRP();
                            string jsonresponse = dataobj.processStatusChange(url, userName, password, command, wh_id, po, confirmDate, confirmUser, dtlnum, toStatus);
                        }                                     
                    }
                }
            }
            lblError.Text = po +  " Onay Numarasıyla Onaylandı!";          
            totalQantity = 0;
            lblQantity.Text = "Toplam Miktar: " + totalQantity.ToString();
        }
        public void unConfirmStatusRP()
        {
            string po = string.Empty;
            string confirmDate = string.Empty;
            string confirmUser = string.Empty;
            string toStatus = string.Empty;
            string dtlnum = string.Empty;
            string command = string.Empty;

            foreach (GridViewRow item in gvInventory.Rows)
            {
                CheckBox chk = (CheckBox)item.FindControl("CheckBox1");
                if (chk != null)
                {
                    if (chk.Checked)
                    {
                        po = "";
                        confirmDate = "";
                        confirmUser = "";
                        dtlnum = item.Cells[5].Text;
                        command = "statuschange";

                        if (wh_id == "BOE1")
                        {
                            toStatus = "Q";
                        }
                        else if (wh_id=="LSH1")
                        {
                            toStatus = "H02";
                        }
                        DataAdapterRP dataobj = new DataAdapterRP();
                        string jsonresponse = dataobj.processStatusChange(url, userName, password, command, wh_id, po, confirmDate, confirmUser, dtlnum, toStatus);
                    }
                }
            }
            lblError.Text = "Onaylar kaldırıldı! Kalite Statüsüne Çekildi.";
            totalQantity = 0;
            lblQantity.Text = "Toplam Miktar: " + totalQantity.ToString();
        }
        public void barcodeStatusChangeRP()
        {
            string po = string.Empty;
            string confirmDate = string.Empty;
            string confirmUser = string.Empty;
            string toStatus = string.Empty;
            string dtlnum = string.Empty;
            string command = string.Empty;

            foreach (GridViewRow item in gvInventory.Rows)
            {
                CheckBox chk = (CheckBox)item.FindControl("CheckBox1");
                if (chk != null)
                {
                    if (chk.Checked)
                    {
                        string statu = item.Cells[7].Text;
                        if (statu== "Q-010R")
                        {
                            toStatus = "010F";
                        }
                        else if (statu == "Q-010F")
                        {
                            toStatus = "010R";
                        }
                        else
                        {
                            lblError.Text = statu+" barkodlanamadı ya da barkod kaldırılamadı!";
                        }
                        
                        dtlnum = item.Cells[5].Text;
                        command = "barcodestatuschange";


                        DataAdapterRP dataobj = new DataAdapterRP();
                        string jsonresponse = dataobj.processBarcodeStatusChange(url, userName, password, command, wh_id, dtlnum, toStatus);

                    }
                }
            }
            lblError.Text = "Barkod statü değişimleri yapıldı.";
            totalQantity = 0;
            lblQantity.Text = "Toplam Miktar: " + totalQantity.ToString();
        }
       

        public void confirmInventory()
        {          
                foreach (GridViewRow item in gvInventory.Rows)
                {
                    CheckBox chk = (CheckBox)item.FindControl("CheckBox1");
                    if (chk != null)
                    {
                        if (chk.Checked)
                        {
                            string command = "[update invdtl set inv_attr_str7='" + item.Cells[6].Text.Replace("&nbsp;", "") + "_"+txtlot.Text + "', inv_attr_dte1='" + DateTime.Now.AddHours(-2).ToString("yyyyMMdd  HHmm") + "', inv_attr_str10='"+ Request.QueryString["User"] + "' where dtlnum='" + item.Cells[5].Text + "']";
                        //string command = "[update invdtl set inv_attr_str7='" + item.Cells[6].Text.Replace("&nbsp;", "").Substring(item.Cells[6].Text.Length - 6) + "_" + txtlot.Text + "', inv_attr_dte1='" + DateTime.Now.AddHours(-2).ToString("yyyyMMdd  HHmm") + "', inv_attr_str10='" + Request.QueryString["User"] + "' where dtlnum='" + item.Cells[5].Text + "']";
                        lblError.Text =  item.Cells[6].Text.Replace("&nbsp;", "") + "_" + txtlot.Text + " Onay Numarasıyla Onaylandı!";
                        string wh_id = ddl_wh_id.SelectedValue;

                            DataAdapterRest dataobj = new DataAdapterRest();
                            string jsonresponse = dataobj.getData(url, userName, password, command, wh_id);
                        }
                    }
                }
            
            //Response.Write("<script>alert('" + txtlot.Text + "_" + txtprt.Text + " Onay Numarasıyla Onaylandı!" + "');</script>");
            totalQantity = 0;
            lblQantity.Text = "Toplam Miktar: " + totalQantity.ToString();
        }
        public void deleteInventoryConfirmation()
        {
            foreach (GridViewRow item in gvInventory.Rows)
            {
                CheckBox chk = (CheckBox)item.FindControl("CheckBox1");
                if (chk != null)
                {
                    if (chk.Checked)
                    {
                        if (item.Cells[7].Text=="-")
                        {
                            lblError.Text = "Bu ürün müşteri tarafından onaylandığı için onayı kaldırılamamaktadır.";
                        }
                        else
                        {
                            string command = "[update invdtl set inv_attr_str7=null, inv_attr_str10=null, inv_attr_dte1=null where dtlnum='" + item.Cells[5].Text + "']";

                            DataAdapterRest dataobj = new DataAdapterRest();
                            string jsonresponse = dataobj.getData(url, userName, password, command, wh_id);
                        }                        
                    }
                }
            }
            if (lblError.Text != "Bu ürün müşteri tarafından onaylandığı için onayı kaldırılamamaktadır.")
            {
                lblError.Text = "Onaylar kaldırıldı!";
            }         
            totalQantity = 0;
            lblQantity.Text = "Toplam Miktar: " + totalQantity.ToString();
        }
        public void getTotalQuantity()
        {
            totalQantity = 0;
            foreach (GridViewRow item in gvInventory.Rows)
            {
                CheckBox chk = (CheckBox)item.FindControl("CheckBox1");
                if (chk != null)
                {
                    if (chk.Checked)
                    {
                        totalQantity += Convert.ToInt32(item.Cells[9].Text);
                    }
                }
            }
            lblQantity.Text = "Toplam Miktar: " + totalQantity.ToString();
        }
        public void selectAll()
        {
            foreach (GridViewRow item in gvInventory.Rows)
            {
                CheckBox chk = (CheckBox)item.FindControl("CheckBox1");
                if (chk != null)
                {
                    if (cbSelectAll.Checked)
                    {
                        chk.Checked = true;
                    }
                    else
                    {
                        chk.Checked = false;
                    }
                }
            }
        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            if (txtprt.Text == null || txtprt.Text == string.Empty || txtprt.Text == "")
            {
                lblError.Text = "Listelemek için Ürün Numarası alanını boş bırakmayınız!";
            }
            else if (txtlot.Text == null || txtlot.Text == string.Empty || txtlot.Text == "")
            {
                lblError.Text = "Listelemek için Lot Numarası alanını boş bırakmayınız!";
            }
            else if (cbConfirmed.Checked)
            {
                btnConfirm.Text = "Onay Kaldır";
                lblDate.Visible = true;
                cdate.Visible = true;
                if (wh_id == "TR_0126_03")
                {
                    getBaxterConfirmedList();
                }
                else if (systype == "RP")
                {
                    getRPConfirmedList();
                }
                else { getConfirmedList(); }

            }
            else
            {
                btnConfirm.Text = "Onayla";
                lblDate.Visible = false;
                cdate.Visible = false;
                if (systype == "RP")
                {
                    getRPList();
                }
                else
                {
                    getList();
                }
            }
            cbSelectAll.Checked = false;
            cbSelectAll.CheckedChanged += CheckBox1_CheckedChanged;
        }
        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            getTotalQuantity();
        }     
        protected void cbSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            selectAll();
            getTotalQuantity();
        }
        protected void ddl_wh_id_SelectedIndexChanged(object sender, EventArgs e)//arranges all global values about client's properties.
        {
            wh_id = ddl_wh_id.SelectedValue;

            using (SqlConnection myConnection = new SqlConnection(conn))
            {

                SqlCommand oCmd = new SqlCommand("select * from [connection] where wh_id=@wh_id  and platform=\'prod\'", myConnection);
                oCmd.Parameters.AddWithValue("@wh_id", wh_id);
                myConnection.Open();
                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        url = oReader["url"].ToString();
                        userName = oReader["userName"].ToString();
                        password = oReader["password"].ToString();
                        systype= oReader["sysType"].ToString();
                    }
                    myConnection.Close();
                }
            }
            if (systype == "RP" && !cbConfirmed.Checked)
            {
                lblpo.Visible = false;
                txtPO.Visible = false;
                lblDate.Visible = false;
                cdate.Visible = false;
                if (wh_id == "BOE1")
                {
                    btnBarcode.Visible = true;
                }
                else
                {
                    btnBarcode.Visible = false;
                }
            }
            else if (systype == "JDA" && !cbConfirmed.Checked)
            {
                lblpo.Visible = true;
                txtPO.Visible = true;
                lblDate.Visible = false;
                cdate.Visible = false;
                if (wh_id == "BOE1")
                {
                    btnBarcode.Visible = true;
                }
                else
                {
                    btnBarcode.Visible = false;
                }
            }
            else
            {
                lblpo.Visible = true;
                txtPO.Visible = true;
                lblDate.Visible = true;
                cdate.Visible = true;
                if (wh_id == "BOE1")
                {
                    btnBarcode.Visible = true;
                }
                else
                {
                    btnBarcode.Visible = false;
                }
            }
        }
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            if (btnConfirm.Text == "Onay Kaldır")
            {
                if (systype == "RP")
                {
                    unConfirmStatusRP();
                    getRPConfirmedList();
                }
                else
                {
                    if (wh_id == "TR_0126_03")
                    {
                        deleteInventoryConfirmation();
                    }
                    else
                    {
                        unConfirmStatus();
                    }
                    getConfirmedList();
                }

            }
            else
            {
                if (systype == "RP")
                {
                    confirmStatusRP();
                    getRPList();
                }
                else
                {
                    if (wh_id == "TR_0126_03")
                    {

                        confirmInventory();
                    }
                    else
                    {
                        confirmStatus();
                    }
                    getList();
                }
            }
            cbSelectAll.Checked = false;
            cbSelectAll.CheckedChanged += CheckBox1_CheckedChanged;
        }
        protected void btnBarcode_Click(object sender, EventArgs e)
        {

            barcodeStatusChangeRP();

            cbSelectAll.Checked = false;
            cbSelectAll.CheckedChanged += CheckBox1_CheckedChanged;
        }
        protected void btnCikis_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }
    }
   
}