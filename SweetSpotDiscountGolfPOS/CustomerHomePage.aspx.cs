using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SweetShop;

namespace SweetSpotDiscountGolfPOS
{
    public partial class CustomerHomePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
 
        }

        protected void btnCustomerSearch_Click(object sender, EventArgs e)
        {

            SweetShopManager ssm = new SweetShopManager();
            List<Customer> c = ssm.GetCustomerfromSearch(txtSearch.Text);

            grdCustomersSearched.Visible = true;
            grdCustomersSearched.DataSource = c;
            grdCustomersSearched.DataBind();
        }

        protected void btnAddNewCustomer_Click(object sender, EventArgs e)
        {
            Response.Redirect("CustomerAddNew.aspx");
        }

        protected void grdCustomersSearched_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string key = e.CommandArgument.ToString();
            if (e.CommandName == "ViewProfile")
            {
                Session["key"] = key;
                Response.Redirect("CustomerAddNew.aspx");

            }
            else if (e.CommandName == "StartSale")
            {
                Session["TranType"] = 1;
                Session["key"] = key;
                Response.Redirect("SalesCart.aspx");
            }
        }
    }
}