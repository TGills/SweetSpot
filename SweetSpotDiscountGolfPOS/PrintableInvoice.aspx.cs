using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
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
            string method = "Page_Load";
            Session["currPage"] = "PrintableInvoice.aspx";
            try
            {
                if (Convert.ToBoolean(Session["loggedIn"]) == false)
                {
                    Server.Transfer("LoginPage.aspx", false);
                }
                int custNum = (Convert.ToInt32(Session["key"].ToString()));
                Customer c = ssm.GetCustomerbyCustomerNumber(custNum);
                lblCustomerName.Text = c.firstName.ToString() + " " + c.lastName.ToString();
                lblStreetAddress.Text = c.primaryAddress.ToString();
                lblPostalAddress.Text = c.city.ToString() + ", " + lm.provinceName(c.province) + " " + c.postalCode.ToString();
                lblPhone.Text = c.primaryPhoneNumber.ToString();
                lblinvoiceNum.Text = Convert.ToString(Session["Invoice"]);
                lblDate.Text = DateTime.Today.ToString("yyyy-MM-dd");

                bool useInvoiceLocation = Convert.ToBoolean(Session["useInvoice"]);
                Location l = new Location();
                if (useInvoiceLocation == false)
                {
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
                    Boolean isDeleted = Convert.ToBoolean(Session["isDeleted"]);
                    if (isDeleted == false)
                    {
                        l = lm.returnLocationForInvoice(ssm.invoice_getLocation(invoiceNum, invoiceSubNum, "tbl_invoice"));
                        lbldeletedMessage.Visible = false;
                        lbldeletedMessageDisplay.Visible = false;
                    }
                    else if (isDeleted == true)
                    {
                        l = lm.returnLocationForInvoice(ssm.invoice_getLocation(invoiceNum, invoiceSubNum, "tbl_deletedInvoice"));
                        lbldeletedMessage.Visible = true;
                        lbldeletedMessageDisplay.Visible = true;
                        lbldeletedMessageDisplay.Text = ssm.deletedInvoice_getReason(invoiceNum, invoiceSubNum);
                    }
                }

                lblSweetShopName.Text = l.location.ToString();
                lblSweetShopStreetAddress.Text = l.address.ToString();
                lblSweetShopPostalAddress.Text = l.city.ToString() + ", " + lm.provinceName(l.provID) + " " + l.postal.ToString();
                lblSweetShopPhone.Text = l.phone.ToString();

                if (l.location.ToString().Equals("The Sweet Spot Discount Golf"))
                {
                    lblTaxNum.Text = "842165458RT0001";
                }
                else if (l.location.ToString().Equals("Golf Traders"))
                {
                    lblTaxNum.Text = "778164723";
                }

                tranType = Convert.ToInt32(Session["TranType"]);
                if (tranType == 1) { cart = (List<Cart>)Session["ItemsInCart"]; }
                else if (tranType == 2) { cart = (List<Cart>)Session["returnedCart"]; }
                else if (tranType == 3) { cart = (List<Cart>)Session["ItemsInCart"]; }
                ckm = (CheckoutManager)Session["CheckOutTotals"];
                mopList = (List<Checkout>)Session["MethodsofPayment"];

                double tax = 0;
                lblDiscountsDisplay.Text = "$ " + ckm.dblDiscounts.ToString("#0.00");
                lblTradeInsDisplay.Text = "$ " + ckm.dblTradeIn.ToString("#0.00");
                lblShippingDisplay.Text = "$ " + ckm.dblShipping.ToString("#0.00");
                if (ckm.blGst)
                {
                    tax = ckm.dblGst;
                }
                lblGSTDisplay.Text = "$ " + tax.ToString("#0.00");
                tax = 0;
                if (ckm.blPst)
                {
                    tax = ckm.dblPst;
                }
                lblPSTDisplay.Text = "$ " + tax.ToString("#0.00");
                lblSubtotalDisplay.Text = "$ " + ckm.dblSubTotal.ToString("#0.00");
                foreach (var mop in mopList)
                {
                    dblAmountPaid += mop.amountPaid;
                }
                grdItemsSoldList.DataSource = cart;
                grdItemsSoldList.DataBind();
                lblTotalPaidDisplay.Text = "$ " + dblAmountPaid.ToString("#0.00");
                grdMOPS.DataSource = mopList;
                grdMOPS.DataBind();
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
        protected void btnHome_Click(object sender, EventArgs e)
        {
            string method = "btnHome_Click";
            try
            {
                Session["useInvoice"] = null;
                Session["Invoice"] = null;
                Session["key"] = null;
                Session["ItemsInCart"] = null;
                Session["returnedCart"] = null;
                Session["TranType"] = null;
                Session["CheckOutTotals"] = null;
                Session["MethodsofPayment"] = null;
                Session["prevPage"] = Session["currPage"];
                Server.Transfer("HomePage.aspx", false);
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