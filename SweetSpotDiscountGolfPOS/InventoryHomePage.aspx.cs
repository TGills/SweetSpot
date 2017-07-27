using SweetShop;
using SweetSpotProShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSpotDiscountGolfPOS
{
    public partial class InventoryHomePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Admin"] == null)
            {
                btnAddNewInventory.Enabled = false;
            }
        }

        protected void btnInventorySearch_Click(object sender, EventArgs e)
        {
            List<Items> searched = new List<Items>();
            ItemDataUtilities idu = new ItemDataUtilities();
            string skuString;
            int skuInt;
            if (txtSearch.Text == "")
            {

            }
            else
            {
                string loc = Convert.ToString(Session["Loc"]);
                SweetShopManager ssm = new SweetShopManager();

                if (!int.TryParse(txtSearch.Text, out skuInt))
                {
                    skuString = txtSearch.Text;
                    searched = ssm.GetItemfromSearch(txtSearch.Text, ddlInventoryType.SelectedItem.Text);
                }
                else
                {
                    skuString = txtSearch.Text;
                    // this looks for the item in the database
                    List<Items> i = idu.getItemByID(Convert.ToInt32(skuInt));

                    //if adding new item
                    if (i != null && i.Count >= 1)
                    {
                        searched.Add(i.ElementAt(0));
                    }
                }

                grdInventorySearched.Visible = true;
                grdInventorySearched.DataSource = searched;
                grdInventorySearched.DataBind();
            }
        }

        protected void btnAddNewInventory_Click(object sender, EventArgs e)
        {
            Response.Redirect("InventoryAddNew.aspx");
        }

        protected void grdInventorySearched_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string itemKey = e.CommandArgument.ToString();
            string itemType = ddlInventoryType.SelectedItem.ToString();
            if (e.CommandName == "viewItem")
            {
                Session["itemType"] = itemType;
                Session["itemKey"] = itemKey;
                Response.Redirect("InventoryAddNew.aspx");
            }
        }
    }
}