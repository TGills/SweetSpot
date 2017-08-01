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

namespace SweetSpotDiscountGolfPOS
{
    public partial class SalesCart : System.Web.UI.Page
    {
        public string skuString;
        public int skuInt;
        public int invNum;
        SweetShopManager ssm = new SweetShopManager();
        ItemDataUtilities idu = new ItemDataUtilities();
        List<Items> invoiceItems = new List<Items>();
        List<Cart> itemsInCart = new List<Cart>();
        List<Cart> returnedCart = new List<Cart>();
        Cart tempItemInCart;
        Object o = new Object();
        protected void Page_Load(object sender, EventArgs e)
        {
            lblInvalidQty.Visible = false;
            if (!Page.IsPostBack)
            {
                int tranType = Convert.ToInt32(Session["TranType"]);
                if (tranType == 1)
                {
                    txtSearch.Focus();
                    if (Session["key"] != null)
                    {
                        int custNum;
                        custNum = (int)(Convert.ToInt32(Session["key"].ToString()));

                        Customer c = ssm.GetCustomerbyCustomerNumber(custNum);
                        txtCustomer.Text = c.firstName + " " + c.lastName;
                    }
                    //display system time in Sales Page
                    DateTime today = DateTime.Today;
                    invNum = idu.getNextInvoiceNum();
                    string loc = Convert.ToString(Session["Loc"]);
                    lblInvoiceNumberDisplay.Text = loc + "-" + (invNum + "-" + idu.getNextInvoiceSubNum(invNum)).ToString();
                    //lblInvoiceNumberDisplay.Text = loc + "-" + invNum;
                    Session["Invoice"] = lblInvoiceNumberDisplay.Text;
                    lblDateDisplay.Text = today.ToString("yyyy-MM-dd");
                    if (Session["ItemsInCart"] != null)
                    {
                        grdCartItems.DataSource = Session["ItemsInCart"];
                        grdCartItems.DataBind();
                        lblSubtotalDisplay.Text = "$ " + ssm.returnSubtotalAmount((List<Cart>)Session["ItemsInCart"]).ToString();
                    }
                }
                else if (tranType == 2)
                {
                    Invoice rInvoice = (Invoice)Session["searchReturnInvoices"];
                    lblCustomerDisplay.Visible = true;
                    lblCustomerDisplay.Text = rInvoice.customerName.ToString();
                    txtCustomer.Visible = false;
                    btnCustomerSelect.Visible = false;
                    RadioButton1.Visible = false;
                    RadioButton2.Visible = false;
                    lblShipping.Visible = false;
                    txtShippingAmount.Visible = false;
                    lblInvoiceNumberDisplay.Text = rInvoice.invoiceNum.ToString() + "-" + idu.getNextInvoiceSubNum(rInvoice.invoiceNum).ToString();
                    lblDateDisplay.Text = rInvoice.invoiceDate.ToString();
                    txtSearch.Visible = false;
                    btnInventorySearch.Visible = false;
                    grdInventorySearched.Visible = false;
                    grdCartItems.Visible = false;
                    grdInvoicedItems.Visible = true;
                    grdReturningItems.Visible = true;
                    lblSubtotal.Text = "Return Total:";
                    btnCancelSale.Text = "Cancel Return";
                    btnProceedToCheckout.Text = "Reimburse Customer";
                    itemsInCart = ssm.returningItems(rInvoice.invoiceNum, rInvoice.invoiceSub);
                    Session["ItemsInCart"] = itemsInCart;
                    grdInvoicedItems.DataSource = itemsInCart;
                    grdInvoicedItems.DataBind();

                }
            }
            //else if (Convert.ToBoolean(Session["UpdateTheCart"]))
            //{
            //    grdCartItems.DataSource = Session["ItemsInCart"];
            //    grdCartItems.DataBind();
            //    Session["UpdateTheCart"] = null;
            //}
        }
        protected void btnCustomerSelect_Click(object sender, EventArgs e)
        {
            if (Session["ItemsInCart"] != null)
            {
                itemsInCart = (List<Cart>)Session["ItemsInCart"];
            }
            foreach (var cart in itemsInCart)
            {
                int remainingQTY = idu.getquantity(cart.sku, cart.typeID);
                idu.updateQuantity(cart.sku, cart.typeID, (remainingQTY + cart.quantity));
            }

            lblInvalidQty.Visible = false;
            Session["key"] = null;
            Session["shipping"] = null;
            Session["ItemsInCart"] = null;
            Session["CheckOutTotals"] = null;
            Session["MethodsofPayment"] = null;
            Session["Grid"] = null;
            Session["SKU"] = null;
            Session["Items"] = null;
            Response.Redirect("CustomerHomePage.aspx");
        }
        protected void btnInventorySearch_Click(object sender, EventArgs e)
        {
            lblInvalidQty.Visible = false;
            string loc = Convert.ToString(Session["Loc"]);
            if (!txtSearch.Text.Equals("") && !txtSearch.Text.Equals(null))
            {
                if (!int.TryParse(txtSearch.Text, out skuInt))
                {
                    skuString = txtSearch.Text;
                    invoiceItems = ssm.returnSearchFromAllThreeItemSets(skuString, loc);
                }
                else
                {
                    skuString = txtSearch.Text;
                    // this looks for the item in the database
                    List<Items> i = idu.getItemByID(Convert.ToInt32(skuInt), loc);// txtSearch.Text));

                    //if adding new item
                    if (i != null && i.Count >= 1)
                    {
                        invoiceItems.Add(i.ElementAt(0));
                    }
                }
            }

            grdInventorySearched.DataSource = invoiceItems;
            //pass invoice items to Session to delete item from first gridView- grdInventorySerach
            Session["Items"] = invoiceItems;
            grdInventorySearched.DataBind();
            txtSearch.Text = "";

        }
        //Currently used for Removing the row
        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            lblInvalidQty.Visible = false;
            int index = e.RowIndex;
            int sku = Convert.ToInt32(grdCartItems.Rows[index].Cells[2].Text);
            //int quantity = Convert.ToInt32(((Label)grdCartItems.Rows[index].Cells[3].Controls[0]).Text);
            int quantity = Convert.ToInt32(grdCartItems.Rows[index].Cells[3].Text);
            int itemType = Convert.ToInt32(((Label)grdCartItems.Rows[index].Cells[8].FindControl("lblTypeID")).Text);
            int remainingQTY = idu.getquantity(sku, itemType);

            List<Cart> duplicateCart = (List<Cart>)Session["ItemsInCart"];
            foreach (var cart in duplicateCart)
            {
                if (cart.sku != sku)
                { itemsInCart.Add(cart); }
                else { idu.removeQTYfromInventoryWithSKU(cart.sku, cart.typeID, (remainingQTY + quantity)); }
            }
            grdCartItems.EditIndex = -1;
            Session["ItemsInCart"] = itemsInCart;
            grdCartItems.DataSource = itemsInCart;
            grdCartItems.DataBind();
            lblSubtotalDisplay.Text = "$ " + ssm.returnSubtotalAmount(itemsInCart).ToString("#0.00");
            //DataTable sc = new DataTable();
            //if (Session["SKU"] != null)
            //{
            //    sc = (DataTable)Session["SKU"];
            //}

            //int index = Convert.ToInt32(e.RowIndex);
            //DataTable dt = (DataTable)Session["Grid"];
            //DataRow rowD = dt.Rows[index];
            //int j;
            //if (Session["count"] != null)
            //{
            //    j = (int)Session["count"];
            //}
            //else
            //{
            //    j = (sc.Rows.Count);
            //}

            //for (int i = (j - 1); i >= 0; i--)
            //{
            //    if (sc.Rows.Count >= 1)
            //    {

            //        if (((sc.Rows[i].ItemArray[0])).Equals(rowD.ItemArray[0]))
            //        {
            //            sc.Rows[i].Delete();
            //            int c = ((j) - 1);
            //            Session["count"] = c;

            //        }
            //    }
            //}
            //dt.Rows[index].Delete();

            //DataTable del = new DataTable();
            //del = createTable();

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    if (i != index)
            //    {
            //        DataRow rowSubtotal = dt.Rows[i];
            //        int subQty = Convert.ToInt32(rowSubtotal.ItemArray[1]);
            //        double subPrice = Convert.ToDouble(rowSubtotal.ItemArray[3]);
            //        subTotal += subQty * subPrice;
            //        del.Rows.Add(rowSubtotal.ItemArray);
            //    }

            //}
            //Session["Grid"] = del;

            //Session["SKU"] = sc;

            //lblTotalVal.Text = subTotal.ToString();
            //grdCartItems.DataSource = dt;
            //grdCartItems.DataBind();

        }
        //Currently used for Editing the row
        protected void OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            lblInvalidQty.Visible = false;
            int index = e.NewEditIndex;
            //Label discountAmount = (Label)grdCartItems.Rows[index].Cells[6].FindControl("lblAmountDisplay");
            //string discountOnItem = discountAmount.Text;
            //Label price = (Label)grdCartItems.Rows[index].Cells[5].FindControl("price");
            //string sPrice = price.Text;
            //Label cost = (Label)grdCartItems.Rows[index].Cells[5].FindControl("cost");
            //string sCost = cost.Text;

            //bool radioButtonSelected = false;
            //RadioButton rbOne = (RadioButton)grdCartItems.Rows[index].Cells[6].FindControl("rddiscountdisabled");
            //RadioButton rbTwo = (RadioButton)grdCartItems.Rows[index].Cells[6].FindControl("rdamountdisabled");
            //if (rbOne.Checked)
            //{
            //    radioButtonSelected = true;
            //}
            //else if (rbTwo.Checked)
            //{
            //    radioButtonSelected = false;
            //}

            //string sku = grdCartItems.Rows[index].Cells[2].Text;
            int quantity = Convert.ToInt32(grdCartItems.Rows[index].Cells[3].Text);
            Session["originalQTY"] = quantity;
            //string desc = grdCartItems.Rows[index].Cells[4].Text;

            //tempItemInCart = new Cart(Convert.ToInt32(sku), desc, Convert.ToInt32(quantity), Convert.ToDouble(sPrice), Convert.ToDouble(sCost),
            //    Convert.ToDouble(discountOnItem), radioButtonSelected);

            //var obj = itemsInCart.FirstOrDefault(x => x.sku == Convert.ToInt32(sku));
            //if (obj != null)
            //{
            //    obj.quantity = tempItemInCart.quantity;
            //    obj.price = tempItemInCart.price;
            //    obj.percentage = tempItemInCart.percentage;
            //    obj.discount = tempItemInCart.discount;
            //}

            grdCartItems.DataSource = (List<Cart>)Session["ItemsInCart"];
            grdCartItems.EditIndex = index;
            grdCartItems.DataBind();
            lblSubtotalDisplay.Text = "$ " + ssm.returnSubtotalAmount((List<Cart>)Session["ItemsInCart"]).ToString("#0.00");
        }
        //Currently used for cancelling the edit
        protected void ORowCanceling(object sender, GridViewCancelEditEventArgs e)
        {
            lblInvalidQty.Visible = false;
            grdCartItems.EditIndex = -1;
            grdCartItems.DataSource = Session["ItemsInCart"];
            grdCartItems.DataBind();
            lblSubtotalDisplay.Text = "$ " + ssm.returnSubtotalAmount((List<Cart>)Session["ItemsInCart"]).ToString("#0.00");
            Session["originalQTY"] = null;
        }
        //Currently used for updating the row
        protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            lblInvalidQty.Visible = false;
            int index = e.RowIndex;
            TextBox discountAmount = (TextBox)grdCartItems.Rows[index].Cells[6].FindControl("txtAmnt");
            string discountOnItem = discountAmount.Text;
            Label price = (Label)grdCartItems.Rows[index].Cells[5].FindControl("price");
            string sPrice = price.Text;
            Label cost = (Label)grdCartItems.Rows[index].Cells[5].FindControl("cost");
            string sCost = cost.Text;

            bool radioButtonSelected = false;
            CheckBox chkPerecent = (CheckBox)grdCartItems.Rows[index].Cells[6].FindControl("ckbPercentageEdit");
            //RadioButton rbOne = (RadioButton)grdCartItems.Rows[index].Cells[6].FindControl("rddiscount");
            //RadioButton rbTwo = (RadioButton)grdCartItems.Rows[index].Cells[6].FindControl("rdamount");
            if (chkPerecent.Checked)
            {
                radioButtonSelected = true;
            }
            else
            {
                radioButtonSelected = false;
            }

            bool tradeInItemInCart = ((CheckBox)grdCartItems.Rows[index].Cells[7].FindControl("chkTradeIn")).Checked;
            string itemType = ((Label)grdCartItems.Rows[index].Cells[8].FindControl("lblTypeID")).Text;
            string sku = grdCartItems.Rows[index].Cells[2].Text;
            string quantity = ((TextBox)grdCartItems.Rows[index].Cells[3].Controls[0]).Text;
            string desc = grdCartItems.Rows[index].Cells[4].Text;

            tempItemInCart = new Cart(Convert.ToInt32(sku), desc, Convert.ToInt32(quantity), Convert.ToDouble(sPrice), Convert.ToDouble(sCost),
                Convert.ToDouble(discountOnItem), radioButtonSelected, tradeInItemInCart, Convert.ToInt32(itemType));

            //var obj = itemsInCart.FirstOrDefault(x => x.sku == Convert.ToInt32(sku));
            //if (obj != null)
            //{
            //    obj.quantity = tempItemInCart.quantity;
            //    obj.price = tempItemInCart.price;
            //    obj.percentage = tempItemInCart.percentage;
            //    obj.discount = tempItemInCart.discount;
            //}

            List<Cart> duplicateCart = (List<Cart>)Session["ItemsInCart"];
            int remainingQTY = idu.getquantity(Convert.ToInt32(sku), Convert.ToInt32(itemType));
            int differenceInQTY = Convert.ToInt32(Session["originalQTY"]) - Convert.ToInt32(quantity);
            if ((remainingQTY + differenceInQTY) < 0)
            {
                itemsInCart = duplicateCart;
                lblInvalidQty.Visible = true;
                lblInvalidQty.Text = "Quantity Exceeds the Remaining Inventory";
                lblInvalidQty.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                foreach (var cart in duplicateCart)
                {
                    if (cart.sku == tempItemInCart.sku)
                    {
                        itemsInCart.Add(tempItemInCart);
                        idu.removeQTYfromInventoryWithSKU(cart.sku, cart.typeID, (remainingQTY + differenceInQTY));
                    }
                    else
                    {
                        itemsInCart.Add(cart);
                    }
                }
            }

            grdCartItems.EditIndex = -1;
            Session["ItemsInCart"] = itemsInCart;
            grdCartItems.DataSource = itemsInCart;
            grdCartItems.DataBind();
            lblSubtotalDisplay.Text = "$ " + ssm.returnSubtotalAmount(itemsInCart).ToString("#0.00");
            Session["originalQTY"] = null;
        }
        protected void btnCancelSale_Click(object sender, EventArgs e)
        {
            if (Session["ItemsInCart"] != null)
            {
                itemsInCart = (List<Cart>)Session["ItemsInCart"];
            }
            foreach(var cart in itemsInCart){
                int remainingQTY = idu.getquantity(cart.sku, cart.typeID);
                idu.updateQuantity(cart.sku, cart.typeID, (remainingQTY + cart.quantity));
            }


            lblInvalidQty.Visible = false;
            //* *update * *to null any new seesions btnCancelSale_Click;
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
            Response.Redirect("HomePage.aspx");
        }
        protected void btnProceedToCheckout_Click(object sender, EventArgs e)
        {
            lblInvalidQty.Visible = false;
            if (!RadioButton2.Checked)
            {
                Session["shipping"] = false;
                Session["ShippingAmount"] = 0;
            }
            else
            {
                Session["shipping"] = true;
                Session["ShippingAmount"] = txtShippingAmount.Text;
            }

            Response.Redirect("SalesCheckout.aspx");
        }
        protected void grdInventorySearched_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            lblInvalidQty.Visible = false;
            bool bolAdded = false;
            if (Session["ItemsInCart"] != null)
            {
                itemsInCart = (List<Cart>)Session["ItemsInCart"];
            }
            int itemKey = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "AddItem")
            {
                foreach (var cart in itemsInCart)
                {
                    if (cart.sku == itemKey && !bolAdded)
                    {
                        int remainingQTY = idu.getquantity(cart.sku, cart.typeID);
                        if ((cart.quantity + 1) > remainingQTY)
                        {
                            lblInvalidQty.Visible = true;
                            lblInvalidQty.Text = "Quantity Exceeds the Remaining Inventory";
                            lblInvalidQty.ForeColor = System.Drawing.Color.Red;
                        }
                        else
                        {
                            cart.quantity += 1;
                            idu.removeQTYfromInventoryWithSKU(cart.sku, cart.typeID, (remainingQTY - 1));
                        }
                        bolAdded = true;
                    }
                }

                //int locationID = Convert.ToInt32(lblLocationID.Text);
                int locationID = 1;
                //Finding the min and max range for trade ins
                int[] range = idu.tradeInSkuRange(locationID);

                //If the itemKey is between or equal to the ranges, do trade in
                if (itemKey >= range[0] && itemKey < range[1])
                {
                    
                    //Trade In Sku to add in SK
                    string redirect = "<script>window.open('TradeINEntry.aspx');</script>";
                    Response.Write(redirect);
                }
                else if (itemsInCart.Count == 0 || !bolAdded)
                {
                    Clubs c = ssm.singleItemLookUp(itemKey);
                    Clothing cl = ssm.getClothing(itemKey);
                    Accessories ac = ssm.getAccessory(itemKey);
                    int itemType = 0;
                    if (c.sku != 0)
                    {
                        itemType = c.typeID;
                        o = c as Object;
                    }
                    else if (cl.sku != 0)
                    {
                        itemType = cl.typeID;
                        o = cl as Object;
                    }
                    else if (ac.sku != 0)
                    {
                        itemType = ac.typeID;
                        o = ac as Object;
                    }
                    int remainingQTY = idu.getquantity(itemKey, itemType);
                    if (1 > remainingQTY)
                    {

                        MessageBox.ShowMessage("This item has 0 quantity", this);
                        lblInvalidQty.Visible = true;
                        //lblInvalidQty.Text = "Quantity Exceeds the Remaining Inventory";
                        //lblInvalidQty.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        itemsInCart.Add(idu.addingToCart(o));
                        idu.removeQTYfromInventoryWithSKU(itemKey, itemType, (remainingQTY - 1));
                    }
                }
            }
            Session["ItemsInCart"] = itemsInCart;
            grdCartItems.DataSource = itemsInCart;
            grdCartItems.DataBind();
            List<Items> nullGrid = new List<Items>();
            nullGrid = null;
            grdInventorySearched.DataSource = nullGrid;
            grdInventorySearched.DataBind();
            lblSubtotalDisplay.Text = "$ " + ssm.returnSubtotalAmount(itemsInCart).ToString("#.00");
        }

        protected void grdInvoicedItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            lblInvalidQty.Visible = false;
            int index = e.RowIndex;
            int sku = Convert.ToInt32(grdInvoicedItems.Rows[index].Cells[1].Text);
            int quantity = Convert.ToInt32(grdInvoicedItems.Rows[index].Cells[2].Text);
            int itemType = Convert.ToInt32(((Label)grdInvoicedItems.Rows[index].Cells[6].FindControl("lblReturnTypeID")).Text);
            int inStockQTY = idu.getquantity(sku, itemType);
            int remainingQTY = 0;
            List<Cart> duplicateCart = (List<Cart>)Session["ItemsInCart"];
            Cart returnedItem = new Cart();
            List<Cart> duplicateReturnedCart = new List<Cart>();
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
                                returnedItem = new Cart(retCart.sku, retCart.description, retCart.quantity + 1, retCart.price, retCart.cost, retCart.discount, retCart.percentage, retCart.tradeIn, retCart.typeID);
                            }
                            else
                            {
                                returnedItem = new Cart(cart.sku, cart.description, 1, cart.price, cart.cost, cart.discount, cart.percentage, cart.tradeIn, cart.typeID);
                            }
                            returnedCart.Add(returnedItem);
                        }
                    }
                    else
                    {
                        returnedItem = new Cart(cart.sku, cart.description, 1, cart.price, cart.cost, cart.discount, cart.percentage, cart.tradeIn, cart.typeID);
                        returnedCart.Add(returnedItem);
                    }
                }
                else if (cart.sku != sku)
                {
                    itemsInCart.Add(cart);
                }
            }
            idu.removeQTYfromInventoryWithSKU(returnedItem.sku, returnedItem.typeID, inStockQTY + 1);
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
            lblSubtotalDisplay.Text = "$ " + ssm.returnSubtotalAmount(returnedCart).ToString("#0.00");
        }

        protected void grdReturningItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            lblInvalidQty.Visible = false;
            int index = e.RowIndex;
            int sku = Convert.ToInt32(grdReturningItems.Rows[index].Cells[1].Text);
            int quantity = Convert.ToInt32(grdReturningItems.Rows[index].Cells[2].Text);
            int itemType = Convert.ToInt32(((Label)grdReturningItems.Rows[index].Cells[6].FindControl("lblReturnedTypeID")).Text);
            int inStockQTY = idu.getquantity(sku, itemType);
            int remainingQTY = 0;
            List<Cart> duplicateCart = (List<Cart>)Session["returnedCart"];
            Cart cancelReturnedItem = new Cart();
            List<Cart> duplicateReturnedCart = (List<Cart>)Session["ItemsInCart"];
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
                            }
                            else
                            {
                                cancelReturnedItem = new Cart(cart.sku, cart.description, 1, cart.price, cart.cost, cart.discount, cart.percentage, cart.tradeIn, cart.typeID);
                            }
                            itemsInCart.Add(cancelReturnedItem);
                        }
                    }
                    else
                    {
                        cancelReturnedItem = new Cart(cart.sku, cart.description, 1, cart.price, cart.cost, cart.discount, cart.percentage, cart.tradeIn, cart.typeID);
                        itemsInCart.Add(cancelReturnedItem);
                    }
                }
                else if (cart.sku != sku)
                {
                    returnedCart.Add(cart);
                }
            }
            idu.removeQTYfromInventoryWithSKU(cancelReturnedItem.sku, cancelReturnedItem.typeID, inStockQTY - 1);
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
            lblSubtotalDisplay.Text = "$ " + ssm.returnSubtotalAmount(returnedCart).ToString("#0.00");
        }

        protected void btnJumpToInventory_Click(object sender, EventArgs e)
        {
            //Inventory screen in new window/tab
            string redirect = "<script>window.open('InventoryHomePage.aspx');</script>";
            Response.Write(redirect);
        }
    }
}