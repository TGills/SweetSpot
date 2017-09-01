using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using SweetSpotProShop;

namespace SweetSpotDiscountGolfPOS
{
    public partial class CustomerHomePage : System.Web.UI.Page
    {
        ErrorReporting er = new ErrorReporting();
        protected void Page_Load(object sender, EventArgs e)
        {
            string method = "Page_Load";
            Session["currPage"] = "CustomerHomePage";
            Session["prevPage"] = "HomePage";
            try
            {
                if (Convert.ToBoolean(Session["loggedIn"]) == false)
                {
                    Server.Transfer("LoginPage.aspx", false);
                }
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

        protected void btnCustomerSearch_Click(object sender, EventArgs e)
        {
            string method = "btnCustomerSearch_Click";
            try
            {
                SweetShopManager ssm = new SweetShopManager();
                List<Customer> c = ssm.GetCustomerfromSearch(txtSearch.Text);

                grdCustomersSearched.Visible = true;
                grdCustomersSearched.DataSource = c;
                grdCustomersSearched.DataBind();
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

        protected void btnAddNewCustomer_Click(object sender, EventArgs e)
        {
            string method = "btnAddNewCustomer_Click";
            try
            {
                Session["prevPage"] = Session["currPage"];
                Server.Transfer("CustomerAddNew.aspx", false);
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

        protected void grdCustomersSearched_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string method = "grdCustomersSearched_RowCommand";
            try
            {
                string key = e.CommandArgument.ToString();
                if (e.CommandName == "ViewProfile")
                {
                    Session["key"] = key;
                    Session["prevPage"] = Session["currPage"];
                    Server.Transfer("CustomerAddNew.aspx", false);

                }
                else if (e.CommandName == "StartSale")
                {
                    Session["returnedFromCart"] = false;
                    Session["TranType"] = 1;
                    Session["key"] = key;
                    Session["prevPage"] = Session["currPage"];
                    Server.Transfer("SalesCart.aspx", false);
                }
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