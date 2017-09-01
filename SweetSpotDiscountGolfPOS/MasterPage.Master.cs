using SweetSpotDiscountGolfPOS.ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSpotDiscountGolfPOS
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        ErrorReporting er = new ErrorReporting();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session["loggedIn"] = false;
            Session["Admin"] = null;
            Session["Loc"] = null;
            Session["id"] = null;
            Session["locationID"] = null;
            Server.Transfer("LoginPage.aspx", false);
        }

    }
}