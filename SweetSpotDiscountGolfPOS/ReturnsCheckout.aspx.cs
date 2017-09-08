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
    public partial class ReturnsCheckout : System.Web.UI.Page
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
        double amountPaid;
        double dblShippingAmount;
        int tranType;
        int gridID;

        protected void Page_Load(object sender, EventArgs e)
        {
            string method = "Page_Load";
            Session["currPage"] = "ReturnsCheckout.aspx";
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

                    cart = (List<Cart>)Session["returnedCart"];


                    DateTime recDate = Convert.ToDateTime(Session["strDate"]);
                    bool bolShipping = Convert.ToBoolean(Session["shipping"]);
                    if (bolShipping)
                    {
                        int custNum = (int)Convert.ToInt32(Session["key"].ToString());
                        Customer c = ssm.GetCustomerbyCustomerNumber(custNum);
                        t = ssm.getTaxes(c.province, recDate);
                        dblShippingAmount = Convert.ToDouble(Session["ShippingAmount"].ToString());
                    }
                    else
                    {
                        //**Will need to be enabled not shipping 
                        t = ssm.getTaxes(cm.returnLocationID(Convert.ToString(Session["Loc"])), recDate);
                        dblShippingAmount = 0;
                    }
                    int location = Convert.ToInt32(Session["locationID"]);
                    Invoice rInvoice = (Invoice)Session["searchReturnInvoices"];
                    ckm = new CheckoutManager(cm.returnRefundTotalAmount(cart), 0, 0, 0, false, false, 0, 0, 0);
                    foreach (var T in t)
                    {
                        switch (T.taxName)
                        {
                            case "GST":
                                lblGovernment.Visible = true;
                                lblGovernmentAmount.Visible = true;
                                if (rInvoice.governmentTax > 0)
                                {
                                    ckm.dblGst = cm.returnGSTAmount(T.taxRate, ckm.dblSubTotal);
                                    ckm.blGst = true;
                                }
                                lblGovernmentAmount.Text = "$ " + ckm.dblGst.ToString("#0.00");
                                break;
                            case "PST":
                                lblProvincial.Visible = true;
                                lblProvincialAmount.Visible = true;
                                if (rInvoice.provincialTax > 0)
                                {
                                    ckm.dblPst = cm.returnPSTAmount(T.taxRate, ckm.dblSubTotal);
                                    ckm.blPst = true;
                                }
                                lblProvincialAmount.Text = "$ " + pst.ToString("#0.00");
                                break;
                            case "HST":
                                lblProvincial.Visible = false;
                                lblGovernmentAmount.Visible = true;
                                lblGovernment.Text = "HST";
                                if (rInvoice.governmentTax > 0)
                                {
                                    ckm.dblGst = cm.returnGSTAmount(T.taxRate, ckm.dblSubTotal);
                                    ckm.blGst = true;
                                }
                                lblGovernmentAmount.Text = "$ " + gst.ToString("#0.00");
                                break;
                            case "RST":
                                lblProvincial.Visible = true;
                                lblProvincialAmount.Visible = true;
                                lblProvincial.Text = "RST";
                                if (rInvoice.provincialTax > 0)
                                {
                                    ckm.dblPst = cm.returnPSTAmount(T.taxRate, ckm.dblSubTotal);
                                    ckm.blPst = true;
                                }
                                lblProvincialAmount.Text = "$ " + pst.ToString("#0.00");
                                break;
                            case "QST":
                                lblProvincial.Visible = true;
                                lblProvincialAmount.Visible = true;
                                lblProvincial.Text = "QST";
                                if (rInvoice.provincialTax > 0)
                                {
                                    ckm.dblPst = cm.returnPSTAmount(T.taxRate, ckm.dblSubTotal);
                                    ckm.blPst = true;
                                }
                                lblProvincialAmount.Text = "$ " + pst.ToString("#0.00");
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
                    lblRefundSubTotalAmount.Text = "$ " + (ckm.dblSubTotal + ckm.dblShipping).ToString("#0.00");
                    lblGovernmentAmount.Text = "$ " + ckm.dblGst.ToString("#0.00");
                    lblProvincialAmount.Text = "$ " + ckm.dblPst.ToString("#0.00");
                    lblRefundBalanceAmount.Text = "$ " + ckm.dblBalanceDue.ToString("#0.00");
                    lblRemainingRefundDisplay.Text = "$ " + ckm.dblRemainingBalance.ToString("#0.00");
                    Session["CheckOutTotals"] = ckm;
                    txtAmountRefunding.Text = ckm.dblRemainingBalance.ToString("#0.00");
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
                string boxResult = txtAmountRefunding.Text;
                if (boxResult != "")
                {
                    amountPaid = Convert.ToDouble(boxResult);
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
                string boxResult = txtAmountRefunding.Text;
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
                string boxResult = txtAmountRefunding.Text;
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
                string boxResult = txtAmountRefunding.Text;
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
                string boxResult = txtAmountRefunding.Text;
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
                lblRemainingRefundDisplay.Text = "$ " + ckm.dblRemainingBalance.ToString("#0.00");
                txtAmountRefunding.Text = ckm.dblRemainingBalance.ToString("#0.00");
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
                lblRemainingRefundDisplay.Text = "$ " + ckm.dblRemainingBalance.ToString("#0.00");
                txtAmountRefunding.Text = ckm.dblRemainingBalance.ToString("#0.00");
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
                Server.Transfer("ReturnsCart.aspx", false);
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

        protected void btnFinalize_Click(object sender, EventArgs e)
        {//Transaction type 1

            string method = "btnFinalize_Click";
            try
            {
                if (!txtAmountRefunding.Text.Equals("0.00"))
                {
                    MessageBox.ShowMessage("Remaining Refund Does NOT Equal 0.", this);
                }
                else
                {
                    //Gathering needed information for the invoice
                    //Cart
                    tranType = Convert.ToInt32(Session["TranType"]);
                    List<Cart> cart = new List<Cart>();
                    if (tranType == 2) { cart = (List<Cart>)Session["returnedCart"]; }

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