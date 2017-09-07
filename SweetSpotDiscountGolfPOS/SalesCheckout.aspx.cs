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

    public partial class SalesCheckout : System.Web.UI.Page
    {
        ErrorReporting er = new ErrorReporting();
        SweetShopManager ssm = new SweetShopManager();
        List<Checkout> mopList = new List<Checkout>();
        List<Cart> itemsInCart = new List<Cart>();
        ItemDataUtilities idu = new ItemDataUtilities();
        CheckoutManager ckm;

        public double dblRemaining;
        public double subtotal;
        public double gst;
        public double pst;
        public double balancedue;
        public double dblAmountPaid;
        public double tradeInCost;
        public double taxAmount;
        //Remove Prov or Gov Tax
        bool chargeGST;
        bool chargePST;
        double amountPaid;
        double dblShippingAmount;
        int tranType;
        int gridID;

        protected void Page_Load(object sender, EventArgs e)
        {
            string method = "Page_Load";
            Session["currPage"] = "SalesCheckout.aspx";
            try
            {
                if (Convert.ToBoolean(Session["loggedIn"]) == false)
                {
                    Server.Transfer("LoginPage.aspx", false);
                }
                if (!Page.IsPostBack)
                {

                    List<Tax> t = new List<Tax>();
                    List<Cart> cart = new List<Cart>();
                    CalculationManager cm = new CalculationManager();
                    if (Convert.ToInt32(Session["TranType"]) == 2)
                    {
                        btnRemoveGov.Enabled = false;
                        btnRemoveProv.Enabled = false;
                        cart = (List<Cart>)Session["returnedCart"];
                        lblBalance.Text = "Due to Customer:";
                        lblAmountPaid.Text = "Owing to Customer:";
                        lblRemainingBalanceDue.Text = "Remaining Due to Customer:";
                    }
                    else
                    {
                        cart = (List<Cart>)Session["ItemsInCart"];
                    }
                    DateTime recDate = Convert.ToDateTime(Session["strDate"]);
                    bool bolShipping = Convert.ToBoolean(Session["shipping"]);
                    if (bolShipping)
                    {
                        int custNum = (int)Convert.ToInt32(Session["key"].ToString());
                        Customer c = ssm.GetCustomerbyCustomerNumber(custNum);
                        t = ssm.getTaxes(c.province, recDate);
                        lblShipping.Visible = true;
                        lblShippingAmount.Visible = true;
                        dblShippingAmount = Convert.ToDouble(Session["ShippingAmount"].ToString());
                    }
                    else
                    {
                        //**Will need to be enabled not shipping 
                        t = ssm.getTaxes(cm.returnLocationID(Convert.ToString(Session["Loc"])), recDate);
                        lblShipping.Visible = false;
                        lblShippingAmount.Visible = false;
                        dblShippingAmount = 0;
                    }
                    int location = Convert.ToInt32(Session["locationID"]);

                    ckm = new CheckoutManager(cm.returnTotalAmount(cart, location), cm.returnDiscount(cart), cm.returnTradeInAmount(cart, location), dblShippingAmount, true, true, 0, 0, 0);
                    foreach (var T in t)
                    {
                        switch (T.taxName)
                        {
                            case "GST":
                                lblGovernment.Visible = true;
                                ckm.dblGst = cm.returnGSTAmount(T.taxRate, ckm.dblSubTotal + ckm.dblShipping);
                                lblGovernmentAmount.Text = "$ " + ckm.dblGst.ToString("#0.00");
                                lblGovernmentAmount.Visible = true;
                                btnRemoveGov.Visible = true;
                                break;
                            case "PST":
                                lblProvincial.Visible = true;
                                ckm.dblPst = cm.returnPSTAmount(T.taxRate, ckm.dblSubTotal);
                                lblProvincialAmount.Text = "$ " + pst.ToString("#0.00");
                                lblProvincialAmount.Visible = true;
                                btnRemoveProv.Visible = true;
                                break;
                            case "HST":
                                lblProvincial.Visible = false;
                                lblGovernment.Text = "HST";
                                ckm.dblGst = cm.returnGSTAmount(T.taxRate, ckm.dblSubTotal);
                                lblGovernmentAmount.Text = "$ " + gst.ToString("#0.00");
                                lblGovernmentAmount.Visible = true;
                                btnRemoveProv.Visible = false;
                                btnRemoveGov.Text = "HST";
                                break;
                            case "RST":
                                lblProvincial.Visible = true;
                                lblProvincial.Text = "RST";
                                ckm.dblPst = cm.returnPSTAmount(T.taxRate, ckm.dblSubTotal);
                                lblProvincialAmount.Text = "$ " + pst.ToString("#0.00");
                                lblProvincialAmount.Visible = true;
                                btnRemoveProv.Visible = true;
                                btnRemoveProv.Text = "RST";
                                break;
                            case "QST":
                                lblProvincial.Visible = true;
                                lblProvincial.Text = "QST";
                                ckm.dblPst = cm.returnPSTAmount(T.taxRate, ckm.dblSubTotal);
                                lblProvincialAmount.Text = "$ " + pst.ToString("#0.00");
                                lblProvincialAmount.Visible = true;
                                btnRemoveProv.Visible = true;
                                btnRemoveProv.Text = "QST";
                                break;
                        }
                    }
                    ckm.dblBalanceDue += ckm.dblGst + ckm.dblPst;
                    ckm.dblRemainingBalance += ckm.dblGst + ckm.dblPst;

                    if (Session["MethodsofPayment"] != null)
                    {
                        mopList = (List<Checkout>)Session["MethodsofPayment"];
                        foreach (var mop in mopList)
                        {
                            dblAmountPaid += mop.amountPaid;
                        }
                        gvCurrentMOPs.DataSource = mopList;
                        gvCurrentMOPs.DataBind();
                        ckm.dblAmountPaid = dblAmountPaid;
                        ckm.dblRemainingBalance = ckm.dblBalanceDue - ckm.dblAmountPaid;
                    }

                    //***Assign each item to its Label.

                    lblTotalInCartAmount.Text = "$ " + ckm.dblTotal.ToString("#0.00");
                    lblTotalInDiscountsAmount.Text = "$ " + ckm.dblDiscounts.ToString("#0.00");
                    lblTradeInsAmount.Text = "$ " + ckm.dblTradeIn.ToString("#0.00");
                    lblSubTotalAmount.Text = "$ " + (ckm.dblSubTotal + ckm.dblShipping).ToString("#0.00");
                    lblShippingAmount.Text = "$ " + ckm.dblShipping.ToString("#0.00");
                    lblGovernmentAmount.Text = "$ " + ckm.dblGst.ToString("#0.00");
                    lblProvincialAmount.Text = "$ " + ckm.dblPst.ToString("#0.00");
                    lblBalanceAmount.Text = "$ " + ckm.dblBalanceDue.ToString("#0.00");
                    lblRemainingBalanceDueDisplay.Text = "$ " + ckm.dblRemainingBalance.ToString("#0.00");
                    Session["CheckOutTotals"] = ckm;
                    txtAmountPaying.Text = ckm.dblRemainingBalance.ToString("#0.00");
                    //    //Assigning session brought from the cart to variables
                    //    int custNum = (int)(Convert.ToInt32(Session["key"].ToString()));
                    //    bool shipping = Convert.ToBoolean(Session["shipping"]);
                    //    cart = (List<Cart>)Session["ItemsInCart"];
                    //    double dblShippingAmount = Convert.ToDouble(Session["ShippingAmount"].ToString());
                    //    Customer c = ssm.GetCustomerbyCustomerNumber(custNum);
                    //    //End of assigning

                    //    if (shipping == false)
                    //    {
                    //        //**Need to change to location for taxes
                    //        t = ssm.getTaxes(c.province);
                    //        lblShipping.Visible = false;
                    //        lblShippingAmount.Visible = false;
                    //        //txtShippingAmount.Visible = false;
                    //    }
                    //    else if (shipping == true)
                    //    {
                    //        t = ssm.getTaxes(c.province);
                    //        lblShipping.Visible = true;
                    //        lblShippingAmount.Visible = true;
                    //        //txtShippingAmount.Visible = true;
                    //    }

                    //    lblTotalInCartAmount.Text = "$ " + cm.returnTotalAmount(cart).ToString("#0.00");
                    //    lblTotalInDiscountsAmount.Text = "$ " + cm.returnDiscount(cart).ToString("#0.00");
                    //    lblTradeInsAmount.Text = "$ " + cm.returnTradeInAmount(cart).ToString("#0.00");
                    //    subtotal = cm.returnSubtotalAmount(cart);
                    //    lblSubTotalAmount.Text = "$ " + subtotal.ToString("#0.00");                
                    //    lblShippingAmount.Text = "$ " + dblShippingAmount.ToString("#0.00");

                    //    foreach (var T in t)
                    //    {
                    //        switch (T.taxName)
                    //        {
                    //            case "GST":
                    //                lblGovernment.Visible = true;
                    //                gst = cm.returnGSTAmount(T.taxRate, cart);
                    //                lblGovernmentAmount.Text = "$ " + gst.ToString("#0.00");
                    //                btnRemoveGov.Visible = true;
                    //                break;
                    //            case "PST":
                    //                lblProvincial.Visible = true;
                    //                pst = Math.Round((T.taxRate * subtotal), 2);
                    //                lblProvincialAmount.Text = "$ " + pst.ToString("#0.00");
                    //                btnRemoveProv.Visible = true;
                    //                break;
                    //            case "HST":
                    //                lblProvincial.Visible = false;
                    //                lblGovernment.Text = "HST";
                    //                gst = Math.Round((T.taxRate * subtotal), 2);
                    //                lblGovernmentAmount.Text = "$ " + gst.ToString("#0.00");
                    //                btnRemoveProv.Visible = false;
                    //                btnRemoveGov.Text = "HST";
                    //                break;
                    //            case "RST":
                    //                lblProvincial.Visible = true;
                    //                lblProvincial.Text = "RST";
                    //                pst = Math.Round((T.taxRate * subtotal), 2);
                    //                lblProvincialAmount.Text = "$ " + pst.ToString("#0.00");
                    //                btnRemoveProv.Visible = true;
                    //                btnRemoveProv.Text = "RST";
                    //                break;
                    //            case "QST":
                    //                lblProvincial.Visible = true;
                    //                lblProvincial.Text = "QST";
                    //                pst = Math.Round((T.taxRate * subtotal), 2);
                    //                lblProvincialAmount.Text = "$ " + pst.ToString("#0.00");
                    //                btnRemoveProv.Visible = true;
                    //                btnRemoveProv.Text = "QST";
                    //                break;
                    //        }
                    //    }
                    //    dblAmountPaid = 0;
                    //    //Checking if there are MOP's
                    //    if (Session["MethodsofPayment"] != null)
                    //    {
                    //        ck = (List<Checkout>)Session["MethodsofPayment"];
                    //        foreach (var mop in ck)
                    //        {
                    //            dblAmountPaid += mop.amountPaid;
                    //        }
                    //        gvCurrentMOPs.DataSource = ck;
                    //        gvCurrentMOPs.DataBind();
                    //    }
                    //    //End of checking MOP's

                    //    ckm = new CheckoutManager(cm.returnTotalAmount(cart), cm.returnDiscount(cart), cm.returnTradeInAmount(cart), dblShippingAmount, noGST, noPST, gst, pst,(cm.returnSubtotalAmount(cart)-dblAmountPaid));
                    //    balancedue = ckm.dblGst + ckm.dblPst + ckm.dblShipping + ckm.dblTotal - (ckm.dblDiscounts + ckm.dblTradeIn);
                    //    lblBalanceAmount.Text = "$ " + balancedue.ToString("#0.00");
                    //    lblRemainingBalanceDueDisplay.Text = "$ " + balancedue.ToString("#0.00");
                    //    Session["CheckOutTotals"] = ckm;
                    //}
                    //if (txtShippingAmount.Text == null || txtShippingAmount.Text == "")
                    //{
                    //    txtShippingAmount.Text = "0";
                    //}
                    //shippingCost = Convert.ToDouble(txtShippingAmount.Text);
                    //ckm = (CheckoutManager)Session["CheckOutTotals"];
                    //ckm.dblShipping = shippingCost;
                    //balancedue = ckm.dblGst + ckm.dblPst + ckm.dblShipping + ckm.dblTotal - (ckm.dblDiscounts + ckm.dblTradeIn);
                    //lblBalanceAmount.Text = "$ " + balancedue.ToString("#0.00");
                    //Session["CheckOutTotals"] = ckm;
                    //if (Session["MethodsofPayment"] == null)
                    //{
                    //    lblRemainingBalanceDueDisplay.Text = "$ " + balancedue.ToString("#0.00");
                    //}
                    //else
                    //{
                    //    ck = (List<Checkout>)Session["MethodsofPayment"];
                    //    gvCurrentMOPs.DataSource = ck;
                    //    gvCurrentMOPs.DataBind();
                    //    foreach (var mop in ck)
                    //    {
                    //        dblAmountPaid += mop.amountPaid;
                    //    }
                    //    if (!ckm.blGst)
                    //    {
                    //        dblRemaining += ckm.dblGst;
                    //    }
                    //    if (!ckm.blPst)
                    //    {
                    //        dblRemaining += ckm.dblPst;
                    //    }
                    //    dblRemaining += ckm.dblTotal + ckm.dblShipping - (ckm.dblDiscounts + ckm.dblTradeIn);
                    //    lblRemainingBalanceDueDisplay.Text = "$ " + (dblRemaining - dblAmountPaid).ToString("#0.00");
                }
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

        //American Express
        //protected void mopAmericanExpress_Click(object sender, EventArgs e)
        //{
        //    ckm = (CheckoutManager)Session["CheckOutTotals"];
        //    //string boxResult = Microsoft.VisualBasic.Interaction.InputBox("Enter Amount Paid", "Cash", ckm.dblRemainingBalance.ToString("#0.00"), -1, -1);
        //    string boxResult = txtAmountPaying.Text;
        //    if (boxResult != "")
        //    {
        //        amountPaid = Convert.ToDouble(boxResult);
        //        string methodOfPayment = "American Express";
        //        populateGridviewMOP(amountPaid, methodOfPayment);
        //    }
            
        //}
        //Cash
        protected void mopCash_Click(object sender, EventArgs e)
        {
            string method = "mopCash_Click";
            try
            {                              
                ckm = (CheckoutManager)Session["CheckOutTotals"];
                
                
                //string boxResult = Microsoft.VisualBasic.Interaction.InputBox("Enter Amount Paid", "Cash", ckm.dblRemainingBalance.ToString("#0.00"), -1, -1);
                string boxResult = txtAmountPaying.Text;
                if (boxResult != "")
                {
                    amountPaid = Convert.ToDouble(boxResult);
                    ClientScript.RegisterStartupScript(GetType(), "hwa", "userInput(" + amountPaid + ")", true);
                    string methodOfPayment = "Cash";
                    populateGridviewMOP(amountPaid, methodOfPayment);
                }
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
        //Account
        //protected void mopOnAccount_Click(object sender, EventArgs e)
        //{
        //    amountPaid = Convert.ToDouble(Microsoft.VisualBasic.Interaction.InputBox("Enter Amount Paid", "Account", "", -1, -1));
        //    String methodOfPayment = "Account";

        //    populateGridviewMOP(amountPaid, methodOfPayment);
        //}

        //Cheque
        //protected void mopCheque_Click(object sender, EventArgs e)
        //{
        //    ckm = (CheckoutManager)Session["CheckOutTotals"];
        //    //string boxResult = Microsoft.VisualBasic.Interaction.InputBox("Enter Amount Paid", "Cash", ckm.dblRemainingBalance.ToString("#0.00"), -1, -1);
        //    string boxResult = txtAmountPaying.Text;
        //    if (boxResult != "")
        //    {
        //        amountPaid = Convert.ToDouble(boxResult);
        //        string methodOfPayment = "Cheque";
        //        populateGridviewMOP(amountPaid, methodOfPayment);
        //    }
        //}
        //MasterCard
        protected void mopMasterCard_Click(object sender, EventArgs e)
        {
            string method = "mopMasterCard_Click";
            try
            {
                ckm = (CheckoutManager)Session["CheckOutTotals"];
                //string boxResult = Microsoft.VisualBasic.Interaction.InputBox("Enter Amount Paid", "Cash", ckm.dblRemainingBalance.ToString("#0.00"), -1, -1);
                string boxResult = txtAmountPaying.Text;
                if (boxResult != "")
                {
                    amountPaid = Convert.ToDouble(boxResult);
                    string methodOfPayment = "MasterCard";
                    populateGridviewMOP(amountPaid, methodOfPayment);
                }
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
        //Debit
        protected void mopDebit_Click(object sender, EventArgs e)
        {
            string method = "mopDebit_Click";
            try
            {
                ckm = (CheckoutManager)Session["CheckOutTotals"];
                //string boxResult = Microsoft.VisualBasic.Interaction.InputBox("Enter Amount Paid", "Cash", ckm.dblRemainingBalance.ToString("#0.00"), -1, -1);
                string boxResult = txtAmountPaying.Text;
                if (boxResult != "")
                {
                    amountPaid = Convert.ToDouble(boxResult);
                    string methodOfPayment = "Debit";
                    populateGridviewMOP(amountPaid, methodOfPayment);
                }
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
        //Visa
        protected void mopVisa_Click(object sender, EventArgs e)
        {
            string method = "mopVisa_Click";
            try
            {
                ckm = (CheckoutManager)Session["CheckOutTotals"];
                //string boxResult = Microsoft.VisualBasic.Interaction.InputBox("Enter Amount Paid", "Cash", ckm.dblRemainingBalance.ToString("#0.00"), -1, -1);
                string boxResult = txtAmountPaying.Text;
                if (boxResult != "")
                {
                    amountPaid = Convert.ToDouble(boxResult);
                    string methodOfPayment = "Visa";
                    populateGridviewMOP(amountPaid, methodOfPayment);
                }
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
        //Gift Card
        protected void mopGiftCard_Click(object sender, EventArgs e)
        {
            string method = "mopGiftCard_Click";
            try
            {
                ckm = (CheckoutManager)Session["CheckOutTotals"];
                //string boxResult = Microsoft.VisualBasic.Interaction.InputBox("Enter Amount Paid", "Cash", ckm.dblRemainingBalance.ToString("#0.00"), -1, -1);
                string boxResult = txtAmountPaying.Text;
                if (boxResult != "")
                {
                    amountPaid = Convert.ToDouble(boxResult);
                    string methodOfPayment = "Gift Card";
                    populateGridviewMOP(amountPaid, methodOfPayment);
                }
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

        //Populating gridview with MOPs
        protected void populateGridviewMOP(double amountPaid, string methodOfPayment)
        {
            string method = "populateGridviewMOP";
            try
            {
                gridID = 0;
                if (Session["MethodsofPayment"] != null)
                {
                    mopList = (List<Checkout>)Session["MethodsofPayment"];
                    foreach (var mop in mopList)
                    {
                        if (mop.tableID > gridID)
                            gridID = mop.tableID;
                    }

                }
                Checkout tempCK = new Checkout(methodOfPayment, amountPaid, gridID + 1);
                ckm = (CheckoutManager)Session["CheckOutTotals"];
                //ck = ckm.methodsOfPayment(methodOfPayment, amountPaid, ck);
                mopList.Add(tempCK);
                foreach (var mop in mopList)
                {
                    dblAmountPaid += mop.amountPaid;
                }
                ckm.dblAmountPaid = dblAmountPaid;
                ckm.dblRemainingBalance -= amountPaid;


                foreach (GridViewRow row in gvCurrentMOPs.Rows)
                {
                    foreach (TableCell cell in row.Cells)
                    {
                        cell.Attributes.CssStyle["text-align"] = "center";
                    }
                }
                gvCurrentMOPs.DataSource = mopList;
                gvCurrentMOPs.DataBind();
                Session["MethodsofPayment"] = mopList;
                lblRemainingBalanceDueDisplay.Text = "$ " + ckm.dblRemainingBalance.ToString("#0.00");
                txtAmountPaying.Text = ckm.dblRemainingBalance.ToString("#0.00");
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

        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string method = "OnRowDeleting";
            try
            {
                int index = e.RowIndex;
                ckm = (CheckoutManager)Session["CheckOutTotals"];
                int mopRemovingID = Convert.ToInt32(((Label)gvCurrentMOPs.Rows[index].Cells[3].FindControl("lblTableID")).Text);
                double paidAmount = Convert.ToDouble(gvCurrentMOPs.Rows[index].Cells[2].Text);

                List<Checkout> tempMopList = (List<Checkout>)Session["MethodsofPayment"];
                foreach (var mop in tempMopList)
                {
                    if (mop.tableID != mopRemovingID)
                    {
                        mopList.Add(mop);
                        dblAmountPaid += mop.amountPaid;
                    }
                    else
                    {
                        ckm.dblRemainingBalance += paidAmount;
                    }
                    ckm.dblAmountPaid = dblAmountPaid;
                }
                gvCurrentMOPs.EditIndex = -1;
                Session["MethodsofPayment"] = mopList;
                gvCurrentMOPs.DataSource = mopList;
                gvCurrentMOPs.DataBind();
                lblRemainingBalanceDueDisplay.Text = "$ " + ckm.dblRemainingBalance.ToString("#0.00");
                txtAmountPaying.Text = ckm.dblRemainingBalance.ToString("#0.00");
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

        //For some reason, its only subtracting one, and not the other
        protected void btnRemoveGovTax(object sender, EventArgs e)
        {
            string method = "btnRemoveGovTax";
            try
            {
                ckm = (CheckoutManager)Session["CheckOutTotals"];
                chargeGST = ckm.blGst;
                taxAmount = 0;
                if (chargeGST)
                {
                    //chargeGST is now false so remove GST
                    chargeGST = false;
                    ckm.blGst = chargeGST;
                    lblGovernmentAmount.Text = "$ 0.00";
                    taxAmount = -(ckm.dblGst);
                    btnRemoveGov.Text = "Add GST"; //*** Need to figure out proper name of tax
                }
                else
                {
                    //chargeGST is now True so add GST
                    chargeGST = true;
                    ckm.blGst = chargeGST;
                    lblGovernmentAmount.Text = "$ " + ckm.dblGst.ToString("#0.00");
                    taxAmount = ckm.dblGst;
                    btnRemoveGov.Text = "Remove GST";
                }

                ckm.dblBalanceDue += taxAmount;
                lblBalanceAmount.Text = "$ " + ckm.dblBalanceDue.ToString("#0.00");
                ckm.dblRemainingBalance += taxAmount;
                lblRemainingBalanceDueDisplay.Text = "$ " + ckm.dblRemainingBalance.ToString("#0.00");
                txtAmountPaying.Text = ckm.dblRemainingBalance.ToString("#0.00");

                Session["CheckOutTotals"] = ckm;
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
        protected void btnRemoveProvTax(object sender, EventArgs e)
        {
            string method = "btnRemoveProvTax";
            try
            {
                ckm = (CheckoutManager)Session["CheckOutTotals"];
                chargePST = ckm.blPst;
                taxAmount = 0;
                if (chargePST)
                {
                    //chargePST is now false so remove PST
                    chargePST = false;
                    ckm.blPst = chargePST;
                    lblProvincialAmount.Text = "$ 0.00";
                    taxAmount = -(ckm.dblPst);
                    btnRemoveProv.Text = "Add PST"; //*** Need to figure out proper name of tax
                }
                else
                {
                    //chargePST is now True so add PST
                    chargePST = true;
                    ckm.blPst = chargePST;
                    lblProvincialAmount.Text = "$ " + ckm.dblPst.ToString("#0.00");
                    taxAmount = ckm.dblPst;
                    btnRemoveProv.Text = "Remove PST";
                }

                ckm.dblBalanceDue += taxAmount;
                lblBalanceAmount.Text = "$ " + ckm.dblBalanceDue.ToString("#0.00");
                ckm.dblRemainingBalance += taxAmount;
                lblRemainingBalanceDueDisplay.Text = "$ " + ckm.dblRemainingBalance.ToString("#0.00");
                txtAmountPaying.Text = ckm.dblRemainingBalance.ToString("#0.00");
                Session["CheckOutTotals"] = ckm;
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
        //Other functionality
        protected void btnCancelSale_Click(object sender, EventArgs e)
        {
            string method = "btnCancelSale_Click";
            try
            {
                int tranType = Convert.ToInt32(Session["TranType"]);
                if (tranType == 1)
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
                }
                else if (tranType == 2)
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
                Session["returnedCart"] = null;
                Session["key"] = null;
                Session["shipping"] = null;
                Session["ShippingAmount"] = null;
                Session["ItemsInCart"] = null;
                Session["CheckOutTotals"] = null;
                Session["MethodsofPayment"] = null;
                Session["Grid"] = null;
                Session["SKU"] = null;
                Session["Items"] = null;
                Session["Invoice"] = null;
                Session["TranType"] = null;
                Session["searchReturnInvoices"] = null;
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

        protected void btnReturnToCart_Click(object sender, EventArgs e)
        {
            string method = "btnReturnToCart_Click";
            try
            {
                Session["returnedToCart"] = true;
                Server.Transfer("SalesCart.aspx", false);
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

        protected void btnLayaway_Click(object sender, EventArgs e)
        {
            string method = "btnLayaway_Click";
            try
            { }
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

        protected void btnFinalize_Click(object sender, EventArgs e)
        {//Transaction type 1

            string method = "btnFinalize_Click";
            try
            {
                if (!txtAmountPaying.Text.Equals("0.00") && chbxDoesNotEqualZero.Checked == false)
                {
                    MessageBox.ShowMessage("Remaining Balance Does NOT Equal 0. Check box to proceed", this);
                }
                else
                {
                    //Gathering needed information for the invoice
                    //Cart
                    tranType = Convert.ToInt32(Session["TranType"]);
                    List<Cart> cart = new List<Cart>();
                    if (tranType == 1) { cart = (List<Cart>)Session["ItemsInCart"]; }
                    else if (tranType == 2) { cart = (List<Cart>)Session["returnedCart"]; }

                    //Customer
                    int custNum = Convert.ToInt32(Session["key"]);
                    Customer c = ssm.GetCustomerbyCustomerNumber(custNum);
                    //Employee
                    //******Need to get the employee somehow
                    EmployeeManager em = new EmployeeManager();
                    int empNum = idu.returnEmployeeIDfromPassword(Convert.ToInt32(Session["id"]));
                    Employee emp = em.getEmployeeByID(empNum);
                    //CheckoutTotals
                    ckm = (CheckoutManager)Session["CheckOutTotals"];
                    //MOP
                    mopList = (List<Checkout>)Session["MethodsofPayment"];


                    //CheckoutManager ckm, List<Cart> cart, List<Checkout> mops, Customer c, Employee e, int transactionType, string invoiceNumber, string comments)
                    idu.mainInvoice(ckm, cart, mopList, c, emp, tranType, (Session["Invoice"]).ToString(), txtComments.Text);

                    //ssm.transferTradeInStart((List<Cart>)Session["ItemsInCart"]);
                    Session["useInvoice"] = false;
                    Session["returnedToCart"] = null;
                    Session["shipping"] = null;
                    Session["Grid"] = null;
                    Session["SKU"] = null;
                    Session["Items"] = null;
                    Session["ShippingAmount"] = null;
                    Session["searchReturnInvoices"] = null;
                    Session["strDate"] = null;
                    Server.Transfer("PrintableInvoice.aspx", false);
                }
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