﻿using SweetShop;
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
        SweetShopManager ssm = new SweetShopManager();
        LocationManager lm = new LocationManager();
        List<Checkout> mopList = new List<Checkout>();
        List<Cart> cart = new List<Cart>();
        CheckoutManager ckm = new CheckoutManager();

        double dblAmountPaid;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(Session["loggedIn"]) == false)
            {
                Response.Redirect("LoginPage.aspx");
            }
            int custNum = (Convert.ToInt32(Session["key"].ToString()));
            Customer c = ssm.GetCustomerbyCustomerNumber(custNum);
            lblCustomerName.Text = c.firstName.ToString() + " " + c.lastName.ToString();
            lblStreetAddress.Text = c.primaryAddress.ToString();
            lblPostalAddress.Text = c.city.ToString() + ", " + lm.provinceName(c.province) + " " + c.postalCode.ToString();
            lblPhone.Text = c.primaryPhoneNumber.ToString();
            lblinvoiceNum.Text = Convert.ToString(Session["Invoice"]);
            lblDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            Location l = lm.returnLocationForInvoice(Convert.ToString(Session["Loc"]));
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

                cart = (List<Cart>)Session["ItemsInCart"];
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
        protected void btnHome_Click(object sender, EventArgs e)
        {
            Session["Invoice"] = null;
            Session["key"] = null;
            Session["ItemsInCart"] = null;
            Session["CheckOutTotals"] = null;
            Session["MethodsofPayment"] = null;
            Response.Redirect("HomePage.aspx");
        }
    }
}