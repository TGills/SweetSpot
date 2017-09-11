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
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "HomePage.aspx";
            Session["prevPage"] = "HomePage.aspx";
            //checks if the user has logged in
            if (Convert.ToBoolean(Session["loggedIn"]) == false)
            {
                //Go back to Login to log in
                Server.Transfer("LoginPage.aspx", false);
            }
            try
            {
                if (!this.IsPostBack)
                {
                    //Sets sql connection and executes location command
                    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString);
                    SqlCommand cmd = new SqlCommand("SELECT city FROM tbl_location ", con);
                    //Checks current location and populates drop down
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    ddlLocation.DataSource = cmd.ExecuteReader();
                    ddlLocation.DataTextField = "City";
                    ddlLocation.DataBind();
                    ddlLocation.SelectedValue = Convert.ToString(Session["Loc"]);
                    con.Close();
                }
                //Checks user for admin status
                if (Session["Admin"] != null)
                {
                    lbluser.Text = "You have Admin Access";
                    lbluser.Visible = true;
                }
                else/* if (Session["Loc"] != null)*/
                {
                    //If no admin status shows location as label instead of drop down
                    lblLocation.Text = Convert.ToString(Session["Loc"]);
                    lblLocation.Visible = true;
                    ddlLocation.Visible = false;
                }
                //populate gridview with todays sales
                int locationID = lm.locationIDfromCity(ddlLocation.SelectedValue);
                invoiceList = ssm.getInvoiceBySaleDate(DateTime.Today, locationID);
                grdSameDaySales.DataSource = invoiceList;
                grdSameDaySales.DataBind();
            }
            //Exception catch
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = Convert.ToInt32(Session["loginEmployeeID"]);
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
            }
        }
        //Currently used for Removing the row
        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //Collects current method for error tracking
            string method = "OnRowDeleting";
            try
            {
                string deleteReason = hidden.Value;
                //if (deleteReason.Equals("Code:CancelDelete"))
                //{

                //}
                //else
                //Checks fo the reason why invoice is being deleted
                if (!deleteReason.Equals("Code:CancelDelete") && !deleteReason.Equals(""))
                {
                    //Gathers selected invoice number
                    int index = e.RowIndex;
                    Label lblInvoice = (Label)grdSameDaySales.Rows[index].FindControl("lblInvoiceNumber");
                    string invoice = lblInvoice.Text;
                    char[] splitchar = { '-' };
                    string[] invoiceSplit = invoice.Split(splitchar);
                    int invoiceNum = Convert.ToInt32(invoiceSplit[0]);
                    int invoiceSubNum = Convert.ToInt32(invoiceSplit[1]);
                    string deletionReason = deleteReason;
                    //calls deletion method
                    idu.deleteInvoice(invoiceNum, invoiceSubNum, deletionReason);
                    MessageBox.ShowMessage("Invoice " + invoice + " has been deleted", this);
                    //Refreshes current  page
                    Server.Transfer(Request.RawUrl);
                }
            }
            //Exception catch
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = Convert.ToInt32(Session["loginEmployeeID"]);
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
            }
        }

        protected void lbtnInvoiceNumber_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
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
                string table = "i";
                
                if (isDeleted)
                {
                    table = "deletedI";
                    //Session["key"] = ssm.invoice_getCustID(invoiceNum, invoiceSubNum, "tbl_deletedInvoice");
                    //Session["Invoice"] = invoice;
                    //Session["useInvoice"] = true;
                    //Session["ItemsInCart"] = ssm.invoice_getItems(invoiceNum, invoiceSubNum, "tbl_deletedInvoiceItem");
                    //Session["CheckOutTotals"] = ssm.invoice_getCheckoutTotals(invoiceNum, invoiceSubNum, "tbl_deletedInvoice");
                    //Session["MethodsOfPayment"] = ssm.invoice_getMOP(invoiceNum, invoiceSubNum, "tbl_deletedInvoiceMOP");
                }
                //else
                //{
                //    //Session["key"] = ssm.invoice_getCustID(invoiceNum, invoiceSubNum, "tbl_invoice");
                //    //Session["Invoice"] = invoice;
                //    //Session["useInvoice"] = true;
                //    //Session["ItemsInCart"] = ssm.invoice_getItems(invoiceNum, invoiceSubNum, "tbl_invoiceItem");
                //    //Session["CheckOutTotals"] = ssm.invoice_getCheckoutTotals(invoiceNum, invoiceSubNum, "tbl_invoice");
                //    //Session["MethodsOfPayment"] = ssm.invoice_getMOP(invoiceNum, invoiceSubNum, "tbl_invoiceMOP");
                //}

                //Sets Sessions needed to display invoices
                Session["key"] = ssm.invoice_getCustID(invoiceNum, invoiceSubNum, "tbl_" + table + "nvoice");
                Session["Invoice"] = invoice;
                Session["useInvoice"] = true;
                Session["ItemsInCart"] = ssm.invoice_getItems(invoiceNum, invoiceSubNum, "tbl_" + table + "nvoiceItem");
                Session["CheckOutTotals"] = ssm.invoice_getCheckoutTotals(invoiceNum, invoiceSubNum, "tbl_" + table + "nvoice");
                Session["MethodsOfPayment"] = ssm.invoice_getMOP(invoiceNum, invoiceSubNum, "tbl_" + table + "nvoiceMOP");
                Session["TranType"] = 1;
                //Changes page to display a printable invoice
                Server.Transfer("PrintableInvoice.aspx", false);
            }
            //Exception catch
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = Convert.ToInt32(Session["loginEmployeeID"]);
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
            }
        }   
    }
}