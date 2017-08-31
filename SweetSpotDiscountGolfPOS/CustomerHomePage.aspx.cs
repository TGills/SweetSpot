using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;

namespace SweetSpotDiscountGolfPOS
{
    public partial class CustomerHomePage : System.Web.UI.Page
    {
        ErrorReporting er = new ErrorReporting();
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["currPage"] = "CustomerHomePage";
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

        protected void btnCustomerSearch_Click(object sender, EventArgs e)
        {
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
                er.logError(ex, employeeID, currPage, this);
                string prevPage = Convert.ToString(Session["prevPage"]);
                Response.Redirect(prevPage);
            }
        }

        protected void btnAddNewCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                Session["prevPage"] = Session["currPage"];
                Response.Redirect("CustomerAddNew.aspx");
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

        protected void grdCustomersSearched_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string key = e.CommandArgument.ToString();
                if (e.CommandName == "ViewProfile")
                {
                    Session["key"] = key;
                    Session["prevPage"] = Session["currPage"];
                    Response.Redirect("CustomerAddNew.aspx");

                }
                else if (e.CommandName == "StartSale")
                {
                    Session["returnedFromCart"] = false;
                    Session["TranType"] = 1;
                    Session["key"] = key;
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