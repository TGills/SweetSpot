using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SweetShop;
using SweetSpotProShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;

namespace SweetSpotDiscountGolfPOS
{
    public partial class InventoryAddNew : System.Web.UI.Page
    {
        Object o = new Object();
        SweetShopManager ssm = new SweetShopManager();
        Clubs c = new Clubs();
        Accessories a = new Accessories();
        Clothing cl = new Clothing();
        ItemDataUtilities idu = new ItemDataUtilities();
        LocationManager lm = new LocationManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Admin"] == null)
            {
                btnEditItem.Enabled = false;
            }
            if (Session["itemKey"] != null)
            {
                if (!IsPostBack)
                {
                    string itemType = Session["itemType"].ToString();
                    int itemSKU = Convert.ToInt32(Session["itemKey"].ToString());
                    lblTypeDisplay.Text = itemType;
                    if (itemType == "Clubs")
                    {
                        c = ssm.singleItemLookUp(itemSKU);
                        lblSKUDisplay.Text = c.sku.ToString();
                        lblCostDisplay.Text = c.cost.ToString();
                        lblBrandDisplay.Text = idu.brandType(c.brandID);
                        lblPriceDisplay.Text = c.price.ToString();
                        lblQuantityDisplay.Text = c.quantity.ToString();
                        lblLocationDisplay.Text = lm.locationName(c.itemlocation);

                        lblClubTypeDisplay.Text = c.clubType.ToString();
                        lblModelDisplay.Text = idu.modelType(c.modelID);
                        lblShaftDisplay.Text = c.shaft.ToString();
                        lblNumberofClubsDisplay.Text = c.numberOfClubs.ToString();
                        lblClubSpecDisplay.Text = c.clubSpec.ToString();
                        lblShaftSpecDisplay.Text = c.shaftSpec.ToString();
                        lblShaftFlexDisplay.Text = c.shaftFlex.ToString();
                        lblDexterityDisplay.Text = c.dexterity.ToString();
                        chkUsed.Checked = c.used;
                        lblCommentsDisplay.Text = c.comments.ToString();

                    }
                    else if (itemType == "Accessories")
                    {
                        a = ssm.getAccessory(itemSKU);
                        lblSKUDisplay.Text = a.sku.ToString();
                        lblCostDisplay.Text = a.cost.ToString();
                        lblBrandDisplay.Text = idu.brandType(a.brandID);
                        lblPriceDisplay.Text = a.price.ToString();
                        lblQuantityDisplay.Text = a.quantity.ToString();
                        lblLocationDisplay.Text = lm.locationName(a.locID);

                        lblClubType.Text = "Size: ";
                        lblClubTypeDisplay.Text = a.size.ToString();
                        lblModel.Visible = false;
                        lblModelDisplay.Visible = false;
                        lblShaft.Text = "Colour: ";
                        lblShaftDisplay.Text = a.colour.ToString();
                        lblNumberofClubs.Visible = false;
                        lblNumberofClubsDisplay.Visible = false;
                        lblClubSpec.Visible = false;
                        lblClubSpecDisplay.Visible = false;
                        lblShaftSpec.Visible = false;
                        lblShaftSpecDisplay.Visible = false;
                        lblShaftFlex.Visible = false;
                        lblShaftFlexDisplay.Visible = false;
                        lblDexterity.Visible = false;
                        lblDexterityDisplay.Visible = false;
                        lblComments.Visible = false;
                        lblCommentsDisplay.Visible = false;
                        chkUsed.Visible = false;

                    }
                    else if (itemType == "Clothing")
                    {
                        cl = ssm.getClothing(itemSKU);
                        lblSKUDisplay.Text = cl.sku.ToString();
                        lblCostDisplay.Text = cl.cost.ToString();
                        lblBrandDisplay.Text = idu.brandType(cl.brandID);
                        lblPriceDisplay.Text = cl.price.ToString();
                        lblQuantityDisplay.Text = cl.quantity.ToString();
                        lblLocationDisplay.Text = lm.locationName(cl.locID);

                        lblClubType.Text = "Size: ";
                        lblClubTypeDisplay.Text = cl.size.ToString();
                        lblModel.Visible = false;
                        lblModelDisplay.Visible = false;
                        lblShaft.Text = "Colour: ";
                        lblShaftDisplay.Text = cl.colour.ToString();
                        lblNumberofClubs.Visible = false;
                        lblNumberofClubsDisplay.Visible = false;
                        lblClubSpec.Text = "Gender: ";
                        lblClubSpecDisplay.Text = cl.gender.ToString();
                        lblShaftFlex.Text = "Style: ";
                        lblShaftFlexDisplay.Text = cl.style.ToString();
                        lblShaftSpec.Visible = false;
                        lblShaftSpecDisplay.Visible = false;
                        lblDexterity.Visible = false;
                        lblDexterityDisplay.Visible = false;
                        lblComments.Visible = false;
                        lblCommentsDisplay.Visible = false;
                        chkUsed.Visible = false;
                    }
                }
            }
            else
            {
                ddlType.Visible = true;
                lblTypeDisplay.Visible = false;

                txtCost.Visible = true;
                lblCostDisplay.Visible = false;

                ddlBrand.Visible = true;
                lblBrandDisplay.Visible = false;

                txtPrice.Visible = true;
                lblPriceDisplay.Visible = false;

                txtQuantity.Visible = true;
                lblQuantityDisplay.Visible = false;

                ddlLocation.Visible = true;
                lblLocationDisplay.Visible = false;

                lblClubTypeDisplay.Visible = false;
                lblModelDisplay.Visible = false;
                lblShaftDisplay.Visible = false;
                lblNumberofClubsDisplay.Visible = false;
                lblClubSpecDisplay.Visible = false;
                lblShaftSpecDisplay.Visible = false;
                lblShaftFlexDisplay.Visible = false;
                lblDexterityDisplay.Visible = false;
                lblCommentsDisplay.Visible = false;

                //Accessories
                if (ddlType.SelectedIndex <= 0)
                {
                    lblClubType.Text = "Size: ";
                    txtClubType.Visible = true;
                    lblShaft.Text = "Colour: ";
                    txtShaft.Visible = true;
                    txtClubSpec.Visible = false;
                    txtShaftFlex.Visible = false;
                    chkUsed.Visible = false;
                    lblModel.Visible = false;
                    ddlModel.Visible = false;
                    lblNumberofClubs.Visible = false;
                    txtNumberofClubs.Visible = false;
                    lblClubSpec.Visible = false;
                    lblShaftSpec.Visible = false;
                    txtShaftSpec.Visible = false;
                    lblShaftFlex.Visible = false;
                    lblDexterity.Visible = false;
                    txtDexterity.Visible = false;
                    lblComments.Visible = false;
                    txtComments.Visible = false;
                }
                //Clubs
                else if (ddlType.SelectedIndex == 2)
                {
                    txtClubType.Visible = true;
                    ddlModel.Visible = true;
                    txtShaft.Visible = true;
                    txtNumberofClubs.Visible = true;
                    txtClubSpec.Visible = true;
                    txtShaftSpec.Visible = true;
                    txtShaftFlex.Visible = true;
                    txtDexterity.Visible = true;
                    txtComments.Visible = true;
                    chkUsed.Enabled = true;
                    lblClubType.Text = "Club Type: ";
                    lblShaft.Text = "Shaft: ";
                    lblClubSpec.Text = "Club Spec: ";
                    lblClubSpec.Visible = true;
                    lblShaftFlex.Text = "Shaft Flex: ";
                    lblShaftFlex.Visible = true;
                    lblModel.Visible = true;
                    lblNumberofClubs.Visible = true;
                    lblShaftSpec.Visible = true;
                    lblDexterity.Visible = true;
                    lblComments.Visible = true;
                    chkUsed.Visible = true;
                }
                //Clothing
                else if (ddlType.SelectedIndex == 1)
                {
                    lblClubType.Text = "Size: ";
                    txtClubType.Visible = true;
                    lblShaft.Text = "Colour: ";
                    txtShaft.Visible = true;
                    lblClubSpec.Text = "Gender: ";
                    lblClubSpec.Visible = true;
                    txtClubSpec.Visible = true;
                    lblShaftFlex.Text = "Style: ";
                    lblShaftFlex.Visible = true;
                    txtShaftFlex.Visible = true;
                    chkUsed.Visible = false;
                    ddlModel.Visible = false;
                    txtNumberofClubs.Visible = false;
                    txtShaftSpec.Visible = false;
                    txtDexterity.Visible = false;
                    txtComments.Visible = false;
                    lblShaftSpec.Visible = false;
                    lblModel.Visible = false;
                    lblNumberofClubs.Visible = false;
                    lblDexterity.Visible = false;
                    lblComments.Visible = false;

                }


                btnSaveItem.Visible = false;
                btnAddItem.Visible = true;
                pnlDefaultButton.DefaultButton = "btnAddItem";
                btnEditItem.Visible = false;
                btnCancel.Visible = false;
                btnBackToSearch.Visible = true;
            }

        }
        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            int skuNum;
            string type = idu.typeName(Convert.ToInt32(ddlType.SelectedValue));
            Session["itemType"] = type;
            if (ddlType.SelectedIndex == 2)
            {
                c.cost = Convert.ToDouble(txtCost.Text);
                c.brandID = Convert.ToInt32(ddlBrand.SelectedValue);
                c.price = Convert.ToDouble(txtPrice.Text);
                c.quantity = Convert.ToInt32(txtQuantity.Text);
                c.itemlocation = Convert.ToInt32(ddlLocation.SelectedValue);
                c.clubType = txtClubType.Text;
                c.modelID = Convert.ToInt32(ddlModel.SelectedValue);
                c.shaft = txtShaft.Text;
                c.numberOfClubs = txtNumberofClubs.Text;
                c.clubSpec = txtClubSpec.Text;
                c.shaftSpec = txtShaftSpec.Text;
                c.shaftFlex = txtShaftFlex.Text;
                c.dexterity = txtDexterity.Text;
                c.used = chkUsed.Checked;
                c.comments = txtComments.Text;
                c.typeID = Convert.ToInt32(ddlType.SelectedValue);
                o = c as Object;
            }
            else if (ddlType.SelectedIndex == 0)
            {
                a.brandID = Convert.ToInt32(ddlBrand.SelectedValue);
                a.cost = Convert.ToDouble(txtCost.Text);
                a.price = Convert.ToDouble(txtPrice.Text);
                a.quantity = Convert.ToInt32(txtQuantity.Text);
                a.locID = Convert.ToInt32(ddlLocation.SelectedValue);
                a.typeID = Convert.ToInt32(ddlType.SelectedValue);
                a.size = txtClubType.Text;
                a.colour = txtShaft.Text;
                o = a as Object;
            }
            else if (ddlType.SelectedIndex == 1)
            {
                cl.brandID = Convert.ToInt32(ddlBrand.SelectedValue);
                cl.cost = Convert.ToDouble(txtCost.Text);
                cl.price = Convert.ToDouble(txtPrice.Text);
                cl.quantity = Convert.ToInt32(txtQuantity.Text);
                cl.locID = Convert.ToInt32(ddlLocation.SelectedValue);
                cl.typeID = Convert.ToInt32(ddlType.SelectedValue);
                cl.size = txtClubType.Text;
                cl.colour = txtShaft.Text;
                cl.gender = txtClubSpec.Text;
                cl.style = txtShaftFlex.Text;
                o = cl as Object;
            }
            skuNum = ssm.addItem(o);
            Session["itemKey"] = skuNum;
            Response.Redirect(Request.RawUrl);
        }
        protected void btnEditItem_Click(object sender, EventArgs e)
        {
            txtCost.Text = lblCostDisplay.Text;
            txtCost.Visible = true;
            lblCostDisplay.Visible = false;

            ddlBrand.SelectedValue = idu.brandName(lblBrandDisplay.Text).ToString();
            ddlBrand.Visible = true;
            lblBrandDisplay.Visible = false;

            txtPrice.Text = lblPriceDisplay.Text;
            txtPrice.Visible = true;
            lblPriceDisplay.Visible = false;

            txtQuantity.Text = lblQuantityDisplay.Text;
            txtQuantity.Visible = true;
            lblQuantityDisplay.Visible = false;

            ddlLocation.SelectedValue = lm.locationID(lblLocationDisplay.Text).ToString();
            ddlLocation.Visible = true;
            lblLocationDisplay.Visible = false;

            if (lblTypeDisplay.Text == "Clubs")
            {
                txtClubType.Text = lblClubTypeDisplay.Text;
                txtClubType.Visible = true;
                lblClubTypeDisplay.Visible = false;

                ddlModel.SelectedValue = idu.modelName(lblModelDisplay.Text).ToString();
                ddlModel.Visible = true;
                lblModelDisplay.Visible = false;

                txtShaft.Text = lblShaftDisplay.Text;
                txtShaft.Visible = true;
                lblShaftDisplay.Visible = false;

                txtNumberofClubs.Text = lblNumberofClubsDisplay.Text;
                txtNumberofClubs.Visible = true;
                lblNumberofClubsDisplay.Visible = false;

                txtClubSpec.Text = lblClubSpecDisplay.Text;
                txtClubSpec.Visible = true;
                lblClubSpecDisplay.Visible = false;

                txtShaftSpec.Text = lblShaftSpecDisplay.Text;
                txtShaftSpec.Visible = true;
                lblShaftSpecDisplay.Visible = false;

                txtShaftFlex.Text = lblShaftFlexDisplay.Text;
                txtShaftFlex.Visible = true;
                lblShaftFlexDisplay.Visible = false;

                txtDexterity.Text = lblDexterityDisplay.Text;
                txtDexterity.Visible = true;
                lblDexterityDisplay.Visible = false;

                txtComments.Text = lblCommentsDisplay.Text;
                txtComments.Visible = true;
                lblCommentsDisplay.Visible = false;

                chkUsed.Enabled = true;
            }
            else if (lblTypeDisplay.Text == "Accessories")
            {
                txtClubType.Text = lblClubTypeDisplay.Text;
                txtClubType.Visible = true;
                lblClubTypeDisplay.Visible = false;

                txtShaft.Text = lblShaftDisplay.Text;
                txtShaft.Visible = true;
                lblShaftDisplay.Visible = false;
            }
            else if (lblTypeDisplay.Text == "Clothing")
            {
                txtClubType.Text = lblClubTypeDisplay.Text;
                txtClubType.Visible = true;
                lblClubTypeDisplay.Visible = false;

                txtShaft.Text = lblShaftDisplay.Text;
                txtShaft.Visible = true;
                lblShaftDisplay.Visible = false;

                txtClubSpec.Text = lblClubSpecDisplay.Text;
                txtClubSpec.Visible = true;
                lblClubSpecDisplay.Visible = false;

                txtShaftFlex.Text = lblShaftFlexDisplay.Text;
                txtShaftFlex.Visible = true;
                lblShaftFlexDisplay.Visible = false;
            }
            btnSaveItem.Visible = true;
            pnlDefaultButton.DefaultButton = "btnSaveItem";
            btnEditItem.Visible = false;
            btnAddItem.Visible = false;
            btnCancel.Visible = true;
            btnBackToSearch.Visible = false;

        }
        protected void btnSaveItem_Click(object sender, EventArgs e)
        {

            if (lblTypeDisplay.Text == "Clubs")
            {

                c.sku = Convert.ToInt32(Session["itemKey"].ToString());
                c.cost = Convert.ToDouble(txtCost.Text);
                c.brandID = Convert.ToInt32(ddlBrand.SelectedValue);
                c.price = Convert.ToDouble(txtPrice.Text);
                c.quantity = Convert.ToInt32(txtQuantity.Text);
                c.itemlocation = Convert.ToInt32(ddlLocation.SelectedValue);
                c.clubType = txtClubType.Text;
                c.modelID = Convert.ToInt32(ddlModel.SelectedValue);
                c.shaft = txtShaft.Text;
                c.numberOfClubs = txtNumberofClubs.Text;
                c.clubSpec = txtClubSpec.Text;
                c.shaftSpec = txtShaftSpec.Text;
                c.shaftFlex = txtShaftFlex.Text;
                c.dexterity = txtDexterity.Text;
                c.comments = txtComments.Text;
                c.used = chkUsed.Checked;
                ssm.updateItem(c);

                txtCost.Visible = false;
                lblCostDisplay.Visible = true;
                ddlBrand.Visible = false;
                lblBrandDisplay.Visible = true;
                txtPrice.Visible = false;
                lblPriceDisplay.Visible = true;
                txtQuantity.Visible = false;
                lblQuantityDisplay.Visible = true;
                ddlLocation.Visible = false;
                lblLocationDisplay.Visible = true;
                txtClubType.Visible = false;
                lblClubTypeDisplay.Visible = true;
                ddlModel.Visible = false;
                lblModelDisplay.Visible = true;
                txtShaft.Visible = false;
                lblShaftDisplay.Visible = true;
                txtNumberofClubs.Visible = false;
                lblNumberofClubsDisplay.Visible = true;
                txtClubSpec.Visible = false;
                lblClubSpecDisplay.Visible = true;
                txtShaftSpec.Visible = false;
                lblShaftSpecDisplay.Visible = true;
                txtShaftFlex.Visible = false;
                lblShaftFlexDisplay.Visible = true;
                txtDexterity.Visible = false;
                lblDexterityDisplay.Visible = true;
                txtComments.Visible = false;
                lblCommentsDisplay.Visible = true;
                chkUsed.Enabled = false;
            }
            else if (lblTypeDisplay.Text == "Accessories")
            {
                a.sku = Convert.ToInt32(lblSKUDisplay.Text);
                a.brandID = Convert.ToInt32(ddlBrand.SelectedValue);
                a.cost = Convert.ToDouble(txtCost.Text);
                a.price = Convert.ToDouble(txtPrice.Text);
                a.quantity = Convert.ToInt32(txtQuantity.Text);
                a.locID = Convert.ToInt32(ddlLocation.SelectedValue);
                a.size = txtClubType.Text;
                a.colour = txtShaft.Text;
                ssm.updateAccessories(a);

                txtCost.Visible = false;
                lblCostDisplay.Visible = true;
                ddlBrand.Visible = false;
                lblBrandDisplay.Visible = true;
                txtPrice.Visible = false;
                lblPriceDisplay.Visible = true;
                txtQuantity.Visible = false;
                lblQuantityDisplay.Visible = true;
                ddlLocation.Visible = false;
                lblLocationDisplay.Visible = true;
                txtClubType.Visible = false;
                lblClubTypeDisplay.Visible = true;
                txtShaft.Visible = false;
                lblShaftDisplay.Visible = true;
            }
            else if (lblTypeDisplay.Text == "Clothing")
            {
                cl.sku = Convert.ToInt32(lblSKUDisplay.Text);
                cl.brandID = Convert.ToInt32(ddlBrand.SelectedValue);
                cl.cost = Convert.ToDouble(txtCost.Text);
                cl.price = Convert.ToDouble(txtPrice.Text);
                cl.quantity = Convert.ToInt32(txtQuantity.Text);
                cl.locID = Convert.ToInt32(ddlLocation.SelectedValue);
                cl.size = txtClubType.Text;
                cl.colour = txtShaft.Text;
                cl.gender = txtClubSpec.Text;
                cl.style = txtShaftFlex.Text;
                ssm.updateClothing(cl);

                txtCost.Visible = false;
                lblCostDisplay.Visible = true;
                ddlBrand.Visible = false;
                lblBrandDisplay.Visible = true;
                txtPrice.Visible = false;
                lblPriceDisplay.Visible = true;
                txtQuantity.Visible = false;
                lblQuantityDisplay.Visible = true;
                ddlLocation.Visible = false;
                lblLocationDisplay.Visible = true;
                txtClubType.Visible = false;
                lblClubTypeDisplay.Visible = true;
                txtShaft.Visible = false;
                lblShaftDisplay.Visible = true;
                txtClubSpec.Visible = false;
                lblClubSpecDisplay.Visible = true;
                txtShaftFlex.Visible = false;
                lblShaftFlexDisplay.Visible = true;
            }

            btnSaveItem.Visible = false;
            btnEditItem.Visible = true;
            pnlDefaultButton.DefaultButton = "btnEditItem";
            btnCancel.Visible = false;
            btnAddItem.Visible = false;
            btnBackToSearch.Visible = true;

            Response.Redirect(Request.RawUrl);

        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }
        protected void btnBackToSearch_Click(object sender, EventArgs e)
        {
            Session["itemType"] = null;
            Session["itemKey"] = null;
            Response.Redirect("InventoryHomePage.aspx");
        }
        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["itemType"] = ddlType.SelectedIndex;

        }
    }
}