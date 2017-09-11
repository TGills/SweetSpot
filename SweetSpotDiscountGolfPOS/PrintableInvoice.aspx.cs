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
    public partial class PrintableInvoice : System.Web.UI.Page
    {
        ErrorReporting er = new ErrorReporting();
        SweetShopManager ssm = new SweetShopManager();
        LocationManager lm = new LocationManager();
        List<Checkout> mopList = new List<Checkout>();
        List<Cart> cart = new List<Cart>();
        CheckoutManager ckm = new CheckoutManager();
        int tranType;
        double dblAmountPaid;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "PrintableInvoice.aspx";
            try
            {
                //checks if the user has logged in
                if (Convert.ToBoolean(Session["loggedIn"]) == false)
                {
                    //Go back to Login to log in
                    Server.Transfer("LoginPage.aspx", false);
                }
                //get current customer from customer number session
                int custNum = (Convert.ToInt32(Session["key"].ToString()));
                //Store in Customer class
                Customer c = ssm.GetCustomerbyCustomerNumber(custNum);
                //display information on receipt
                lblCustomerName.Text = c.firstName.ToString() + " " + c.lastName.ToString();
                lblStreetAddress.Text = c.primaryAddress.ToString();
                lblPostalAddress.Text = c.city.ToString() + ", " + lm.provinceName(c.province) + " " + c.postalCode.ToString();
                lblPhone.Text = c.primaryPhoneNumber.ToString();
                lblinvoiceNum.Text = Convert.ToString(Session["Invoice"]);
                lblDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                //Gathers location info from session
                bool useInvoiceLocation = Convert.ToBoolean(Session["useInvoice"]);
                Location l = new Location();
                if (useInvoiceLocation == false)
                {
                    //Use current location to display on invoice
                    l = lm.returnLocationForInvoice(Convert.ToString(Session["Loc"]));
                }
                else if (useInvoiceLocation == true)
                {
                    
                    string invoice = lblinvoiceNum.Text;
                    //Parsing into invoiceNum and invoiceSubNum
                    char[] splitchar = { '-' };
                    string[] invoiceSplit = invoice.Split(splitchar);
                    int invoiceNum = Convert.ToInt32(invoiceSplit[0]);
                    int invoiceSubNum = Convert.ToInt32(invoiceSplit[1]);
                    //Gathers deleted Session
                    Boolean isDeleted = Convert.ToBoolean(Session["isDeleted"]);
                    if (isDeleted == false)
                    {
                        //Returns location based on invoice table
                        l = lm.returnLocationForInvoice(ssm.invoice_getLocation(invoiceNum, invoiceSubNum, "tbl_invoice"));
                        lbldeletedMessage.Visible = false;
                        lbldeletedMessageDisplay.Visible = false;
                    }
                    else if (isDeleted == true)
                    {
                        //Returns location based on deleted invoice table
                        l = lm.returnLocationForInvoice(ssm.invoice_getLocation(invoiceNum, invoiceSubNum, "tbl_deletedInvoice"));
                        lbldeletedMessage.Visible = true;
                        lbldeletedMessageDisplay.Visible = true;
                        lbldeletedMessageDisplay.Text = ssm.deletedInvoice_getReason(invoiceNum, invoiceSubNum);
                    }
                }
                //Display the location information
                lblSweetShopName.Text = l.location.ToString();
                lblSweetShopStreetAddress.Text = l.address.ToString();
                lblSweetShopPostalAddress.Text = l.city.ToString() + ", " + lm.provinceName(l.provID) + " " + l.postal.ToString();
                lblSweetShopPhone.Text = l.phone.ToString();

                if (l.location.ToString().Equals("The Sweet Spot Discount Golf"))
                {
                    //Show tax number if in Moose Jaw
                    lblTaxNum.Text = "842165458RT0001";
                }
                else if (l.location.ToString().Equals("Golf Traders"))
                {
                    //Show tax number if in Calgary
                    lblTaxNum.Text = "778164723";
                }
                //Gather transaction type from Session
                tranType = Convert.ToInt32(Session["TranType"]);
                //Determins the session to get the cart items from
                if (tranType == 1) { cart = (List<Cart>)Session["ItemsInCart"]; }
                else if (tranType == 2) { cart = (List<Cart>)Session["returnedCart"]; }
                else if (tranType == 3) { cart = (List<Cart>)Session["ItemsInCart"]; }
                //Gathers stored totals
                ckm = (CheckoutManager)Session["CheckOutTotals"];
                //Gathers stored payment methods
                mopList = (List<Checkout>)Session["MethodsofPayment"];

                double tax = 0;
                //Display the totals
                lblDiscountsDisplay.Text = "$ " + ckm.dblDiscounts.ToString("#0.00");
                lblTradeInsDisplay.Text = "$ " + ckm.dblTradeIn.ToString("#0.00");
                lblShippingDisplay.Text = "$ " + ckm.dblShipping.ToString("#0.00");
                //Checks for GST paid
                if (ckm.blGst)
                {
                    tax = ckm.dblGst;
                }
                lblGSTDisplay.Text = "$ " + tax.ToString("#0.00");
                tax = 0;
                //Checks for PST paid
                if (ckm.blPst)
                {
                    tax = ckm.dblPst;
                }
                lblPSTDisplay.Text = "$ " + tax.ToString("#0.00");
                //Displays subtotal
                lblSubtotalDisplay.Text = "$ " + ckm.dblSubTotal.ToString("#0.00");
                //Loops through each payment method and totlas them
                foreach (var mop in mopList)
                {
                    dblAmountPaid += mop.amountPaid;
                }
                if (tranType == 2)
                {
                    //Changes headers if the invoice is return
                    grdItemsSoldList.Columns[3].HeaderText = "Sold At";
                    grdItemsSoldList.Columns[4].HeaderText = "Non Refundable";
                    grdItemsSoldList.Columns[5].HeaderText = "Returned At";
                }
                //Binds the cart to the grid view
                grdItemsSoldList.DataSource = cart;
                grdItemsSoldList.DataBind();
                //Displays the total amount ppaid
                lblTotalPaidDisplay.Text = "$ " + dblAmountPaid.ToString("#0.00");
                //Binds the payment methods to a gridview
                grdMOPS.DataSource = mopList;
                grdMOPS.DataBind();
            }
            //Exception catch
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = Convert.ToInt32(Session["loginEmployeeID"]);
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
            }
        }
        protected void btnHome_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnHome_Click";
            try
            {
                //Nulls used stored sessions 
                Session["useInvoice"] = null;
                Session["Invoice"] = null;
                Session["key"] = null;
                Session["ItemsInCart"] = null;
                Session["returnedCart"] = null;
                Session["TranType"] = null;
                Session["CheckOutTotals"] = null;
                Session["MethodsofPayment"] = null;
                //Change to the Home Page
                Server.Transfer("HomePage.aspx", false);
            }
            //Exception catch
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = Convert.ToInt32(Session["loginEmployeeID"]);
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
            }
        }
    }
}