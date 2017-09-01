﻿using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
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
        ErrorReporting er = new ErrorReporting();
        protected void Page_Load(object sender, EventArgs e)
        {
            string method = "Page_Load";
            Session["currPage"] = "InventoryHomePage";
            Session["prevPage"] = "HomePage";
            try
            {
                if (Convert.ToBoolean(Session["loggedIn"]) == false)
                {
                    Server.Transfer("LoginPage.aspx", false);
                }
                if (Session["Admin"] == null)
                {
                    btnAddNewInventory.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                int employeeID = Convert.ToInt32(Session["loginEmployeeID"]);
                string currPage = Convert.ToString(Session["currPage"]);
                er.logError(ex, employeeID, currPage, method, this);
                string prevPage = Convert.ToString(Session["prevPage"]);
                Server.Transfer(prevPage, false);
            }
        }

        protected void btnInventorySearch_Click(object sender, EventArgs e)
        {
            string method = "btnInventorySearch_Click";
            try
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
                    string itemType = ddlInventoryType.SelectedItem.ToString();
                    if (!int.TryParse(txtSearch.Text, out skuInt))
                    {
                        skuString = txtSearch.Text;
                        searched = ssm.GetItemfromSearch(txtSearch.Text, itemType);
                    }
                    else
                    {
                        skuString = txtSearch.Text;
                        // this looks for the item in the database
                        List<Items> i = idu.getItemByID(Convert.ToInt32(skuInt));
                        itemType = idu.typeName(i.ElementAt(0).typeID);
                        //if adding new item
                        if (i != null && i.Count >= 1)
                        {
                            searched.Add(i.ElementAt(0));
                        }
                    }
                    Session["itemType"] = itemType;
                    grdInventorySearched.Visible = true;
                    grdInventorySearched.DataSource = searched;
                    grdInventorySearched.DataBind();
                }
            }
            catch (Exception ex)
            {
                int employeeID = Convert.ToInt32(Session["loginEmployeeID"]);
                string currPage = Convert.ToString(Session["currPage"]);
                er.logError(ex, employeeID, currPage, method, this);
                string prevPage = Convert.ToString(Session["prevPage"]);
                Server.Transfer(prevPage, false);
            }
        }

        protected void btnAddNewInventory_Click(object sender, EventArgs e)
        {
            string method = "btnAddNewInventory_Click";
            try
            {
                Session["prevPage"] = Session["currPage"];
                Server.Transfer("InventoryAddNew.aspx", false);
            }
            catch (Exception ex)
            {
                int employeeID = Convert.ToInt32(Session["loginEmployeeID"]);
                string currPage = Convert.ToString(Session["currPage"]);
                er.logError(ex, employeeID, currPage, method, this);
                string prevPage = Convert.ToString(Session["prevPage"]);
                Server.Transfer(prevPage, false);
            }
        }

        protected void grdInventorySearched_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string method = "grdInventorySearched_RowCommand";
            try
            {
                string itemKey = e.CommandArgument.ToString();
                if (e.CommandName == "viewItem")
                {
                    Session["itemKey"] = itemKey;
                    Session["prevPage"] = Session["currPage"];
                    Server.Transfer("InventoryAddNew.aspx", false);
                }
            }
            catch (Exception ex)
            {
                int employeeID = Convert.ToInt32(Session["loginEmployeeID"]);
                string currPage = Convert.ToString(Session["currPage"]);
                er.logError(ex, employeeID, currPage, method, this);
                string prevPage = Convert.ToString(Session["prevPage"]);
                Server.Transfer(prevPage, false);
            }
        }
    }
}