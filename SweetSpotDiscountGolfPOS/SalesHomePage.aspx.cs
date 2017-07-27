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

        }

        protected void btnQuickSale_Click(object sender, EventArgs e)
        {
            Session["TranType"] = 1;
            int custId = 1;
            Session["key"] = custId;
            Response.Redirect("SalesCart.aspx");
        }

        //Searches invoices and displays them 
        //By date or customer
        protected void btnSearch_Click(object sender, EventArgs e)
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

        protected void grdInvoiceSelection_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Not currently functional
            int invNum = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "returnInvoice")
            {
                List<Invoice> combData = (List<Invoice>)Session["searchReturnInvoices"];
                Invoice returnInvoice = new Invoice();
                foreach (var inv in combData)
                {
                    if (inv.invoiceNum == invNum)
                    {
                        Customer c = ssm.GetCustomerbyCustomerNumber(inv.customerID);
                        returnInvoice = inv;
                        returnInvoice.customerName = c.firstName + " " + c.lastName;
                        Session["searchReturnInvoices"] = returnInvoice;
                    }
                }
                Session["TranType"] = 2;
                Response.Redirect("SalesCart.aspx");
            }
        }
    }
}