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
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["currPage"] = "HomePage";
            Session["prevPage"] = "Login";
            if (Convert.ToBoolean(Session["loggedIn"]) == false)
            {
                Response.Redirect("LoginPage.aspx");
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
                er.logError(ex, employeeID, currPage, this);
                string prevPage = Convert.ToString(Session["prevPage"]);
                Response.Redirect(prevPage);
            }
        }
        //Currently used for Removing the row
        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
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
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            catch (Exception ex)
            {
                int employeeID = Convert.ToInt32(Session["loginEmployeeID"]);
                string currPage = Convert.ToString(Session["currPage"]);
                er.logError(ex, employeeID, currPage, this);
                string prevPage = Convert.ToString(Session["prevPage"]);
                Response.Redirect(prevPage);
            }
        }

        protected void lbtnInvoiceNumber_Click(object sender, EventArgs e)
        {
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
                Session["TranType"] = 3;
                Session["key"] = ssm.invoice_getCustID(invoiceNum, invoiceSubNum);
                Session["Invoice"] = invoice;
                Session["useInvoice"] = true;
                Session["ItemsInCart"] = ssm.invoice_getItems(invoiceNum, invoiceSubNum);
                Session["CheckOutTotals"] = ssm.invoice_getCheckoutTotals(invoiceNum, invoiceSubNum);
                Session["MethodsOfPayment"] = ssm.invoice_getMOP(invoiceNum, invoiceSubNum);
                Response.Redirect("PrintableInvoice.aspx");
            }
            catch (Exception ex)
            {
                int employeeID = Convert.ToInt32(Session["loginEmployeeID"]);
                string currPage = Convert.ToString(Session["currPage"]);
                er.logError(ex, employeeID, currPage, this);
                string prevPage = Convert.ToString(Session["prevPage"]);
                Response.Redirect(prevPage);
            }
        }
    }
}