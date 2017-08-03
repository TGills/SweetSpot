using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using SweetSpotProShop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSpotDiscountGolfPOS
{
    public partial class SettingsHomePage : System.Web.UI.Page
    {

        SweetShopManager ssm = new SweetShopManager();
        EmployeeManager em = new EmployeeManager();
        Reports r = new Reports();
        internal static readonly Page aspx;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(Session["loggedIn"]) == false)
            {
                Response.Redirect("LoginPage.aspx");
            }
            if (Session["Admin"] == null)
            {
                //btnAddNewEmployee.Enabled = false;
                //btnLoadCustomers.Enabled = false;
                //btnLoadEmployees.Enabled = false;
                //btnLoadItems.Enabled = false;
            }
        }
        protected void btnAddNewEmployee_Click(object sender, EventArgs e)
        {
            Response.Redirect("EmployeeAddNew.aspx");
        }
        protected void grdEmployeesSearched_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string key = e.CommandArgument.ToString();
            if (e.CommandName == "ViewProfile")
            {
                Session["empKey"] = key;
                Response.Redirect("EmployeeAddNew.aspx");

            }
        }
        protected void btnEmployeeSearch_Click(object sender, EventArgs e)
        {


            List<Employee> emp = em.GetEmployeefromSearch(txtSearch.Text);

            grdEmployeesSearched.Visible = true;
            grdEmployeesSearched.DataSource = emp;
            grdEmployeesSearched.DataBind();

        }
        //Importing
        protected void btnLoadItems_Click(object sender, EventArgs e)
        {
            if (fupItemSheet.HasFile)
            {                                
                r.importItems(fupItemSheet);
            }
            //Show that it is done
            MessageBox.ShowMessage("Importing Complete", this);
        }
        protected void btnImportCustomers_Click(object sender, EventArgs e)
        {
            if (fupCustomers.HasFile)
            {
                r.importCustomers(fupCustomers);
            }
            //Show that it is done
            MessageBox.ShowMessage("Importing Complete", this);
        }

        //Exporting
        protected void btnExportAll_Click(object sender, EventArgs e)
        {
            r.exportAllItems();
            MessageBox.ShowMessage("Export Complete", this);
        }
        protected void btnExportClubs_Click(object sender, EventArgs e)
        {
            r.exportClubs();
            MessageBox.ShowMessage("Export Complete", this);
        }
        protected void btnExportClothing_Click(object sender, EventArgs e)
        {
            r.exportClothing();
            MessageBox.ShowMessage("Export Complete", this);
        }
        protected void btnExportAccessories_Click(object sender, EventArgs e)
        {
            r.exportAccessories();
            MessageBox.ShowMessage("Export Complete", this);
        }

       
    }
}