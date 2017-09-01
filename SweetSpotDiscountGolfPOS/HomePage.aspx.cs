using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using SweetSpotProShop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSpotDiscountGolfPOS
{
    public partial class HomePage : System.Web.UI.Page
    {
        ErrorReporting er = new ErrorReporting();
        SweetShopManager ssm = new SweetShopManager();
        LocationManager lm = new LocationManager();
        ItemDataUtilities idu = new ItemDataUtilities();
        List<Invoice> invoiceList = new List<Invoice>();
        Boolean isDeleted = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            string method = "Page_Load";
            Session["currPage"] = "HomePage";
            Session["prevPage"] = "Login";
            if (Convert.ToBoolean(Session["loggedIn"]) == false)
            {
                Server.Transfer("LoginPage.aspx", false);
            }
            try
            {
                if (!this.IsPostBack)
                {
                    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString);
                    SqlCommand cmd = new SqlCommand("SELECT city FROM tbl_location ", con);

                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    ddlLocation.DataSource = cmd.ExecuteReader();
                    ddlLocation.DataTextField = "City";
                    ddlLocation.DataBind();
                    ddlLocation.SelectedValue = Convert.ToString(Session["Loc"]);
                    con.Close();
                }
                if (Session["Admin"] != null)
                {
                    lbluser.Text = "You have Admin Access";
                    lbluser.Visible = true;
                }
                else/* if (Session["Loc"] != null)*/
                {

                    lblLocation.Text = Convert.ToString(Session["Loc"]);
                    lblLocation.Visible = true;
                    ddlLocation.Visible = false;
                }
                //populate gridview with todays sales
                invoiceList = ssm.getInvoiceBySaleDate(DateTime.Today, Convert.ToInt32(Session["locationID"]));
                grdSameDaySales.DataSource = invoiceList;
                grdSameDaySales.DataBind();
            }
            catch (Exception ex)
            {
                int employeeID = Convert.ToInt32(Session["loginEmployeeID"]);
                string currPage = Convert.ToString(Session["currPage"]);
                er.logError(ex, employeeID, currPage, method, this);
                string prevPage = Convert.ToString(Session["prevPage"]);
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                Server.Transfer(prevPage, false);
            }
        }
        //Currently used for Removing the row
        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string method = "OnRowDeleting";
            try
            {
                string deletionReason = Microsoft.VisualBasic.Interaction.InputBox("Reason for deleting invioce", "", "", -1, -1);
                while (deletionReason == "")
                {
                    deletionReason = Microsoft.VisualBasic.Interaction.InputBox("Reason for deleting invioce", "", "", -1, -1);
                }
                int index = e.RowIndex;
                Label lblInvoice = (Label)grdSameDaySales.Rows[index].FindControl("lblInvoiceNumber");
                string invoice = lblInvoice.Text;
                char[] splitchar = { '-' };
                string[] invoiceSplit = invoice.Split(splitchar);
                int invoiceNum = Convert.ToInt32(invoiceSplit[0]);
                int invoiceSubNum = Convert.ToInt32(invoiceSplit[1]);
                idu.deleteInvoice(invoiceNum, invoiceSubNum, deletionReason);
                MessageBox.ShowMessage("Invoice " + invoice + " has been deleted", this);
                Server.Transfer(Request.RawUrl, false);
            }
            catch (Exception ex)
            {
                int employeeID = Convert.ToInt32(Session["loginEmployeeID"]);
                string currPage = Convert.ToString(Session["currPage"]);
                er.logError(ex, employeeID, currPage, method, this);
                string prevPage = Convert.ToString(Session["prevPage"]);
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                Server.Transfer(prevPage, false);
            }
        }

        protected void lbtnInvoiceNumber_Click(object sender, EventArgs e)
        {
            string method = "lbtnInvoiceNumber_Click";
            try
            {
                //Text of the linkbutton
                LinkButton btn = sender as LinkButton;
                string invoice = btn.Text;
                //Parsing into invoiceNum and invoiceSubNum
                char[] splitchar = { '-' };
                string[] invoiceSplit = invoice.Split(splitchar);
                int invoiceNum = Convert.ToInt32(invoiceSplit[0]);
                int invoiceSubNum = Convert.ToInt32(invoiceSplit[1]);

                if (isDeleted == false)
                {
                    Session["key"] = ssm.invoice_getCustID(invoiceNum, invoiceSubNum, "tbl_invoice");
                    Session["Invoice"] = invoice;
                    Session["useInvoice"] = true;
                    Session["ItemsInCart"] = ssm.invoice_getItems(invoiceNum, invoiceSubNum, "tbl_invoiceItem");
                    Session["CheckOutTotals"] = ssm.invoice_getCheckoutTotals(invoiceNum, invoiceSubNum, "tbl_invoice");
                    Session["MethodsOfPayment"] = ssm.invoice_getMOP(invoiceNum, invoiceSubNum, "tbl_invoiceMOP");
                }
                else if (isDeleted == true)
                {
                    Session["key"] = ssm.invoice_getCustID(invoiceNum, invoiceSubNum, "tbl_deletedInvoice");
                    Session["Invoice"] = invoice;
                    Session["useInvoice"] = true;
                    Session["ItemsInCart"] = ssm.invoice_getItems(invoiceNum, invoiceSubNum, "tbl_deletedInvoiceItem");
                    Session["CheckOutTotals"] = ssm.invoice_getCheckoutTotals(invoiceNum, invoiceSubNum, "tbl_deletedInvoice");
                    Session["MethodsOfPayment"] = ssm.invoice_getMOP(invoiceNum, invoiceSubNum, "tbl_deletedInvoiceMOP");
                }
                Session["prevPage"] = Session["currPage"];
                Server.Transfer("PrintableInvoice.aspx", false);
            }
            catch (Exception ex)
            {
                int employeeID = Convert.ToInt32(Session["loginEmployeeID"]);
                string currPage = Convert.ToString(Session["currPage"]);
                er.logError(ex, employeeID, currPage, method, this);
                string prevPage = Convert.ToString(Session["prevPage"]);
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                Server.Transfer(prevPage, false);
            }
        }
    }
}