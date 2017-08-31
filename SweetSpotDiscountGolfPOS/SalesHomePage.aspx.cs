using System;
using SweetShop;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using SweetSpotDiscountGolfPOS.ClassLibrary;

namespace SweetSpotDiscountGolfPOS
{
    public partial class SalesHomePage : System.Web.UI.Page
    {
        ErrorReporting er = new ErrorReporting();
        private String connectionString;
        SweetShopManager ssm = new SweetShopManager();
        LocationManager lm = new LocationManager();
        EmployeeManager em = new EmployeeManager();
        public SalesHomePage()
        {
            connectionString = ConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["currPage"] = "SalesHomePage";
            Session["prevPage"] = "HomePage";
            try
            {
                if (Convert.ToBoolean(Session["loggedIn"]) == false)
                {
                    Response.Redirect("LoginPage.aspx");
                }
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

        protected void btnQuickSale_Click(object sender, EventArgs e)
        {
            try
            {
                Session["returnedFromCart"] = false;
                Session["TranType"] = 1;
                int custId = 1;
                Session["key"] = custId;
                Response.Redirect("SalesCart.aspx");
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

        //Searches invoices and displays them 
        //By date or customer
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                bool bolSearchInvoice = rdbSearchByInvoiceNumber.Checked;
                string strSearch = txtInvoiceSearch.Text;
                List<Invoice> fullInvoices = new List<Invoice>();
                Nullable<DateTime> dtmSearch;
                if (txtSearchDate.Text == "")
                {
                    dtmSearch = null;
                }
                else
                {
                    dtmSearch = DateTime.ParseExact(txtSearchDate.Text, "M/dd/yy", null);
                }
                if (bolSearchInvoice)
                {
                    fullInvoices = ssm.getInvoice(Convert.ToInt32(strSearch));
                }
                else
                {
                    fullInvoices = ssm.multiTypeSearchInvoices(strSearch, dtmSearch);
                }
                List<Invoice> viewInvoices = new List<Invoice>();
                foreach (var i in fullInvoices)
                {
                    Customer c = ssm.GetCustomerbyCustomerNumber(i.customerID);
                    Employee emp = em.getEmployeeByID(i.employeeID);
                    Invoice iv = new Invoice(i.invoiceNum, i.invoiceSub, i.invoiceDate, c.firstName + " " + c.lastName, i.balanceDue, lm.locationName(i.locationID), emp.firstName + " " + emp.lastName);
                    viewInvoices.Add(iv);
                }
                grdInvoiceSelection.DataSource = viewInvoices;
                grdInvoiceSelection.DataBind();
                Session["searchReturnInvoices"] = fullInvoices;
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

        protected void grdInvoiceSelection_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string strInvoice = Convert.ToString(e.CommandArgument);
                int invNum = Convert.ToInt32(strInvoice.Split('-')[0]);
                int invSNum = Convert.ToInt32(strInvoice.Split('-')[1]);

                if (e.CommandName == "returnInvoice")
                {
                    List<Invoice> combData = (List<Invoice>)Session["searchReturnInvoices"];
                    Invoice returnInvoice = new Invoice();
                    foreach (var inv in combData)
                    {
                        if (inv.invoiceNum == invNum && inv.invoiceSub == invSNum)
                        {
                            Customer c = ssm.GetCustomerbyCustomerNumber(inv.customerID);
                            returnInvoice = inv;
                            returnInvoice.customerName = c.firstName + " " + c.lastName;
                            Session["key"] = inv.customerID;
                            Session["searchReturnInvoices"] = returnInvoice;
                        }
                    }
                    Session["TranType"] = 2;
                    Session["prevPage"] = Session["currPage"];
                    Response.Redirect("SalesCart.aspx");
                }
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