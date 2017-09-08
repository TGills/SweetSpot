using System;
using SweetShop;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SweetSpotProShop;
using System.Data;
using System.Threading.Tasks;
using SweetSpotDiscountGolfPOS.ClassLibrary;

namespace SweetSpotDiscountGolfPOS
{
    public partial class ReturnsCart : System.Web.UI.Page
    {
        public string skuString;
        public int skuInt;
        public int invNum;
        ErrorReporting er = new ErrorReporting();
        SweetShopManager ssm = new SweetShopManager();
        ItemDataUtilities idu = new ItemDataUtilities();
        List<Items> invoiceItems = new List<Items>();
        List<Cart> itemsInCart = new List<Cart>();
        List<Cart> returnedCart = new List<Cart>();
        List<Cart> temp = new List<Cart>();
        LocationManager lm = new LocationManager();
        Object o = new Object();
        protected void Page_Load(object sender, EventArgs e)
        {
            string method = "Page_Load";
            Session["currPage"] = "ReturnsCart.aspx";
            try
            {
                if (Convert.ToBoolean(Session["loggedIn"]) == false)
                {
                    Server.Transfer("LoginPage.aspx", false);
                }
                if (!Page.IsPostBack)
                {
                    int tranType = Convert.ToInt32(Session["TranType"]);
                    if (tranType == 2)
                    {
                        Invoice rInvoice = (Invoice)Session["searchReturnInvoices"];
                        if (Session["returnedCart"] != null)
                        {
                            itemsInCart = (List<Cart>)Session["ItemsInCart"];
                            returnedCart = (List<Cart>)Session["returnedCart"];
                            grdReturningItems.DataSource = returnedCart;
                            grdReturningItems.DataBind();
                            lblReturnSubtotalDisplay.Text = "$ " + ssm.returnRefundSubtotalAmount(returnedCart).ToString("#0.00");
                        }
                        else
                        {
                            temp = ssm.returningItems(rInvoice.invoiceNum, rInvoice.invoiceSub);
                            foreach (var item in temp)
                            {
                                if (item.typeID != 0)
                                {
                                    itemsInCart.Add(item);
                                }
                            }
                        }
                        lblCustomerDisplay.Text = rInvoice.customerName.ToString();
                        lblInvoiceNumberDisplay.Text = Convert.ToString(Session["Loc"]) + "-" + rInvoice.invoiceNum.ToString() + "-" + idu.getNextInvoiceSubNum(rInvoice.invoiceNum).ToString();
                        Session["Invoice"] = lblInvoiceNumberDisplay.Text;
                        lblDateDisplay.Text = rInvoice.invoiceDate.ToString("yyyy-MM-dd");
                        Session["ItemsInCart"] = itemsInCart;
                        grdInvoicedItems.DataSource = itemsInCart;
                        grdInvoicedItems.DataBind();
                    }
                }
                Session["strDate"] = lblDateDisplay.Text;
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
                //Server.Transfer(prevPage, false);
            }
        }
        protected void btnCancelReturn_Click(object sender, EventArgs e)
        {
            string method = "btnCancelSale_Click";
            try
            {
                int tranType = Convert.ToInt32(Session["TranType"]);
                if (tranType == 2)
                {
                    if (Session["returnedCart"] != null)
                    {
                        itemsInCart = (List<Cart>)Session["returnedCart"];
                    }
                    foreach (var cart in itemsInCart)
                    {
                        int remainingQTY = idu.getquantity(cart.sku, cart.typeID);
                        idu.updateQuantity(cart.sku, cart.typeID, (remainingQTY - cart.quantity));
                    }
                }
                //* *update * *to null any new seesions btnCancelReturn_Click;
                Session["returnedCart"] = null;
                Session["key"] = null;
                Session["shipping"] = null;
                Session["ItemsInCart"] = null;
                Session["CheckOutTotals"] = null;
                Session["MethodsofPayment"] = null;
                Session["Grid"] = null;
                Session["SKU"] = null;
                Session["Items"] = null;
                Session["Invoice"] = null;
                Session["searchReturnInvoices"] = null;
                Session["TranType"] = null;
                Session["ShippingAmount"] = null;
                Session["strDate"] = null;
                Server.Transfer("HomePage.aspx", false);
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
                //Server.Transfer(prevPage, false);
            }
        }
        protected void btnProceedToReturnCheckout_Click(object sender, EventArgs e)
        {
            string method = "btnProceedToCheckout_Click";
            try
            {
                Server.Transfer("ReturnsCheckout.aspx", false);
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
                //Server.Transfer(prevPage, false);
            }
        }
        protected void grdInvoicedItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string method = "grdInvoicedItems_RowDeleting";
            try
            {
                int index = e.RowIndex;
                int sku = Convert.ToInt32(grdInvoicedItems.Rows[index].Cells[1].Text);
                int quantity = Convert.ToInt32(grdInvoicedItems.Rows[index].Cells[2].Text);
                int itemType = Convert.ToInt32(((Label)grdInvoicedItems.Rows[index].Cells[6].FindControl("lblReturnTypeID")).Text);
                double returnAmount = Convert.ToDouble(((TextBox)grdInvoicedItems.Rows[index].Cells[7].FindControl("txtReturnAmount")).Text);
                int inStockQTY = idu.getquantity(sku, itemType);
                int remainingQTY = 0;
                List<Cart> duplicateCart = (List<Cart>)Session["ItemsInCart"];
                Cart returnedItem = new Cart();
                List<Cart> duplicateReturnedCart = new List<Cart>();
                bool bolAdded = false;
                bool bolCart = true;
                if (Session["returnedCart"] != null)
                {
                    duplicateReturnedCart = (List<Cart>)Session["returnedCart"];
                    bolCart = false;
                }
                foreach (var cart in duplicateCart)
                {
                    if (cart.sku == sku)
                    {
                        if (cart.quantity > 1)
                        {
                            remainingQTY = quantity - 1;
                            cart.quantity = remainingQTY;
                            itemsInCart.Add(cart);
                        }
                        if (!bolCart)
                        {
                            foreach (var retCart in duplicateReturnedCart)
                            {
                                if (retCart.sku == cart.sku)
                                {
                                    returnedItem = new Cart(retCart.sku, retCart.description, retCart.quantity + 1, retCart.price, retCart.cost, retCart.discount, retCart.percentage, retCart.returnAmount, retCart.tradeIn, retCart.typeID);
                                    idu.removeQTYfromInventoryWithSKU(returnedItem.sku, returnedItem.typeID, inStockQTY + 1);
                                    bolAdded = true;
                                }
                                else
                                {
                                    returnedItem = new Cart(retCart.sku, retCart.description, retCart.quantity, retCart.price, retCart.cost, retCart.discount, retCart.percentage, retCart.returnAmount, retCart.tradeIn, retCart.typeID);
                                }
                                returnedCart.Add(returnedItem);
                            }
                            if (!bolAdded)
                            {
                                int multi;
                                if (cart.percentage) { multi = 1; } else { multi = -1; }
                                returnedItem = new Cart(cart.sku, cart.description, 1, -1 * cart.price, cart.cost, multi * cart.discount, cart.percentage, -1 * returnAmount, cart.tradeIn, cart.typeID);
                                idu.removeQTYfromInventoryWithSKU(returnedItem.sku, returnedItem.typeID, inStockQTY + 1);
                                returnedCart.Add(returnedItem);
                            }
                        }
                        else
                        {
                            int multi;
                            if (cart.percentage) { multi = 1; } else { multi = -1; }
                            returnedItem = new Cart(cart.sku, cart.description, 1, -1 * cart.price, cart.cost, multi * cart.discount, cart.percentage, -1 * returnAmount, cart.tradeIn, cart.typeID);
                            idu.removeQTYfromInventoryWithSKU(returnedItem.sku, returnedItem.typeID, inStockQTY + 1);
                            returnedCart.Add(returnedItem);
                        }
                    }
                    else if (cart.sku != sku)
                    {
                        itemsInCart.Add(cart);
                    }
                }

                grdInvoicedItems.EditIndex = -1;
                Session["ItemsInCart"] = itemsInCart;
                grdInvoicedItems.DataSource = itemsInCart;
                grdInvoicedItems.DataBind();
                if (returnedCart.Count > 0)
                {
                    Session["returnedCart"] = returnedCart;
                }
                else
                {
                    Session["returnedCart"] = null;
                }
                grdReturningItems.DataSource = returnedCart;
                grdReturningItems.DataBind();
                lblReturnSubtotalDisplay.Text = "$ " + ssm.returnRefundSubtotalAmount(returnedCart).ToString("#0.00");
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
                //Server.Transfer(prevPage, false);
            }
        }

        protected void grdReturningItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string method = "grdReturningItems_RowDeleting";
            try
            {
                int index = e.RowIndex;
                int sku = Convert.ToInt32(grdReturningItems.Rows[index].Cells[1].Text);
                int quantity = Convert.ToInt32(grdReturningItems.Rows[index].Cells[2].Text);
                int itemType = Convert.ToInt32(((Label)grdReturningItems.Rows[index].Cells[6].FindControl("lblReturnedTypeID")).Text);
                int inStockQTY = idu.getquantity(sku, itemType);
                int remainingQTY = 0;
                List<Cart> duplicateCart = (List<Cart>)Session["returnedCart"];
                Cart cancelReturnedItem = new Cart();
                List<Cart> duplicateReturnedCart = (List<Cart>)Session["ItemsInCart"];
                bool bolAdded = false;
                bool bolCart = true;
                if (duplicateReturnedCart.Count > 0)
                {
                    bolCart = false;
                }
                foreach (var cart in duplicateCart)
                {
                    if (cart.sku == sku)
                    {
                        if (quantity > 1)
                        {
                            remainingQTY = quantity - 1;
                            cart.quantity = remainingQTY;
                            returnedCart.Add(cart);
                        }
                        if (!bolCart)
                        {
                            foreach (var retCart in duplicateReturnedCart)
                            {
                                if (retCart.sku == cart.sku)
                                {
                                    cancelReturnedItem = new Cart(retCart.sku, retCart.description, retCart.quantity + 1, retCart.price, retCart.cost, retCart.discount, retCart.percentage, retCart.tradeIn, retCart.typeID);
                                    idu.removeQTYfromInventoryWithSKU(cancelReturnedItem.sku, cancelReturnedItem.typeID, inStockQTY - 1);
                                    bolAdded = true;
                                }
                                else
                                {
                                    cancelReturnedItem = new Cart(retCart.sku, retCart.description, retCart.quantity, retCart.price, retCart.cost, retCart.discount, retCart.percentage, retCart.tradeIn, retCart.typeID);
                                }
                                itemsInCart.Add(cancelReturnedItem);
                            }
                            if (!bolAdded)
                            {
                                int multi;
                                if (cart.percentage) { multi = 1; } else { multi = -1; }
                                cancelReturnedItem = new Cart(cart.sku, cart.description, 1, -1 * cart.price, cart.cost, multi * cart.discount, cart.percentage, cart.tradeIn, cart.typeID);
                                idu.removeQTYfromInventoryWithSKU(cancelReturnedItem.sku, cancelReturnedItem.typeID, inStockQTY - 1);
                                itemsInCart.Add(cancelReturnedItem);
                            }
                        }
                        else
                        {
                            int multi;
                            if (cart.percentage) { multi = 1; } else { multi = -1; }
                            cancelReturnedItem = new Cart(cart.sku, cart.description, 1, -1 * cart.price, cart.cost, multi * cart.discount, cart.percentage, cart.tradeIn, cart.typeID);
                            idu.removeQTYfromInventoryWithSKU(cancelReturnedItem.sku, cancelReturnedItem.typeID, inStockQTY - 1);
                            itemsInCart.Add(cancelReturnedItem);
                        }
                    }
                    else if (cart.sku != sku)
                    {
                        returnedCart.Add(cart);
                    }
                }
                grdReturningItems.EditIndex = -1;
                if (returnedCart.Count > 0)
                {
                    Session["returnedCart"] = returnedCart;
                }
                else
                {
                    Session["returnedCart"] = null;
                }
                grdReturningItems.DataSource = returnedCart;
                grdReturningItems.DataBind();
                Session["ItemsInCart"] = itemsInCart;
                grdInvoicedItems.DataSource = itemsInCart;
                grdInvoicedItems.DataBind();
                lblReturnSubtotalDisplay.Text = "$ " + ssm.returnRefundSubtotalAmount(returnedCart).ToString("#0.00");
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
                //Server.Transfer(prevPage, false);
            }
        }
    }
}