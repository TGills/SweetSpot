using SweetShop;
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
    public partial class TradeINEntry : System.Web.UI.Page
    {
        Object o = new Object();
        SweetShopManager ssm = new SweetShopManager();
        Clubs c = new Clubs();
        LocationManager lm = new LocationManager();
        ItemDataUtilities idu = new ItemDataUtilities();
        public int tradeInSku;
        public int storeLocation;

        protected void Page_Load(object sender, EventArgs e)
        {

            storeLocation = lm.locationID(Convert.ToString(Session["Loc"]));
            lblSKUDisplay.Text = (idu.reserveTradeInSKu(storeLocation)).ToString();
            tradeInSku = Convert.ToInt32(lblSKUDisplay.Text);
            //lblSKUDisplay.Text = idu.tradeInSku(storeLocation).ToString();
        }
        //Cancelling the trade-in item
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            string redirect = "<script>window.close('TradeINEntry.aspx');</script>";
            Response.Write(redirect);
        }
        //Finalizing the trade-in item
        protected void btnAddTradeIN_Click(object sender, EventArgs e)
        {
            //Grabbing the values of the trade-in item
            int sku = idu.tradeInSku(storeLocation);
            double cost = Convert.ToDouble(txtCost.Text);
            int brandID = Convert.ToInt32(ddlBrand.SelectedValue);
            int modelID = Convert.ToInt32(ddlModel.SelectedValue);
            int quant = Convert.ToInt32(txtQuantity.Text);
            int clubTypeID = Convert.ToInt32(ddlClubType.SelectedValue);
            string clubType = idu.getClubTypeName(clubTypeID);
            string shaft = txtShaft.Text;
            string clubSpec = txtClubSpec.Text;
            string shaftFlex = txtShaftFlex.Text;
            string numOfClubs = txtNumberofClubs.Text;
            string shaftSpec = txtShaftSpec.Text;
            string dext = txtDexterity.Text;
            string comments = txtComments.Text;
            bool used = true; //Set to true because a trade-in item is used

            //Creating a new club
            Clubs tradeIN = new Clubs(sku, brandID, modelID, 1, clubType,
                shaft, numOfClubs, 0, cost, 0, quant, clubSpec, shaftSpec,
                shaftFlex, dext, used, comments);
            //Trade in club to be displayed
            Clubs tradeINDisplay = new Clubs(sku, brandID, modelID, 1, clubType,
                shaft, numOfClubs, 0, 0, (cost*(-1)), quant, clubSpec, shaftSpec,
                shaftFlex, dext, used, comments);
            //Adding the trade-in item to the trade-in storage
            idu.addTradeInItem(tradeIN, tradeInSku);
            //Adding trade-in item to cart
            List<Cart> itemsInCart;
            itemsInCart = (List<Cart>)Session["ItemsInCart"];
            o = tradeINDisplay as Object;
            itemsInCart.Add(idu.addingToCart(o));
            Session["UpdateTheCart"] = true;
            //Closing the trade in information window
            string redirect = "<script>window.close('TradeINEntry.aspx');</script>";
            Response.Write(redirect);
        }
    }
}