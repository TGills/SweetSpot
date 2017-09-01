using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using SweetSpotProShop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSpotDiscountGolfPOS
{
    public partial class ReportsCashOut : System.Web.UI.Page
    {
        SweetShopManager ssm = new SweetShopManager();
        ErrorReporting er = new ErrorReporting();
        DateTime startDate;
        DateTime endDate;
        Employee e;
        Reports reports = new Reports();
        ItemDataUtilities idu = new ItemDataUtilities();

        double cashoutTotal;
        double mcTotal = 0;
        double visaTotal = 0;        
        double giftCertTotal = 0;
        double cashTotal = 0;        
        double debitTotal = 0;
        double tradeinTotal = 0;
        double subtotalTotal;
        double pstTotal = 0;
        double gstTotal = 0;
        double receiptTotal = 0;
        double receiptMCTotal;// = 0;
        double receiptVisaTotal;// = 0;        
        double receiptGiftCertTotal;// = 0;
        double receiptCashTotal;// = 0;        
        double receiptDebitTotal;// = 0;
        double receiptSubTotalTotal;
        double receiptGSTTotal;
        double receiptPSTTotal;
        double receiptTradeinTotal;// = 0;
        double overShort = 0;
        bool finalized = false;
        bool processed = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            string method = "Page_Load";
            Session["currPage"] = "ReportsCashOut.aspx";
            try
            {
                if (Convert.ToBoolean(Session["loggedIn"]) == false)
                {
                    Server.Transfer("LoginPage.aspx", false);
                }


                //Gathering the start and end dates
                DateTime[] reportDates = (DateTime[])Session["reportDates"];
                startDate = reportDates[0];
                endDate = reportDates[1];
                //Creating a cashout list and calling a method that grabs all mops and amounts paid
                EmployeeManager em = new EmployeeManager();
                int empNum = idu.returnEmployeeIDfromPassword(Convert.ToInt32(Session["id"]));
                Employee emp = em.getEmployeeByID(empNum);

                int locationID = emp.locationID;
                List<Cashout> lc = reports.cashoutAmounts(startDate, endDate, locationID);
                List<Cashout> rc = reports.getRemainingCashout(startDate, endDate, locationID);
                int counter = 0;
                //Looping through the list and adding up the totals
                foreach (Cashout ch in lc)
                {
                    if (ch.mop == "Visa")
                    {
                        cashoutTotal += ch.amount;
                        visaTotal += ch.amount;
                    }
                    else if (ch.mop == "MasterCard")
                    {
                        cashoutTotal += ch.amount;
                        mcTotal += ch.amount;
                    }
                    else if (ch.mop == "Cash")
                    {
                        cashoutTotal += ch.amount;
                        cashTotal += ch.amount;
                    }
                    else if (ch.mop == "Gift Card")
                    {
                        cashoutTotal += ch.amount;
                        giftCertTotal += ch.amount;
                    }
                    else if (ch.mop == "Debit")
                    {
                        cashoutTotal += ch.amount;
                        debitTotal += ch.amount;
                    }

                    //if (ch.tradeIn != 0)
                    //{
                    //    cashoutTotal -= ch.tradeIn; //Adding because it is a negative value //Switched to subtracting
                    //    tradeinTotal += ch.tradeIn;

                    //}                
                }
                tradeinTotal = reports.getTradeInsCashout(startDate, endDate, locationID);
                cashoutTotal -= tradeinTotal;

                foreach (Cashout rch in rc)
                {
                    subtotalTotal += rch.saleSubTotal;
                    gstTotal += rch.saleGST;
                    pstTotal += rch.salePST;
                }

                tradeinTotal = tradeinTotal * -1;

                Cashout cas = new Cashout(tradeinTotal, giftCertTotal, cashTotal,
                    debitTotal, mcTotal, visaTotal, gstTotal, pstTotal, subtotalTotal);

                Session["saleCashout"] = cas;

                lblVisaDisplay.Text = visaTotal.ToString("#0.00");
                lblMasterCardDisplay.Text = mcTotal.ToString("#0.00");
                lblCashDisplay.Text = cashTotal.ToString("#0.00");
                lblGiftCardDisplay.Text = giftCertTotal.ToString("#0.00");
                lblDebitDisplay.Text = debitTotal.ToString("#0.00");
                lblTradeInDisplay.Text = tradeinTotal.ToString("#0.00");
                lblTotalDisplay.Text = cashoutTotal.ToString("#0.00");
                lblGSTDisplay.Text = gstTotal.ToString("#0.00");
                lblPSTDisplay.Text = pstTotal.ToString("#0.00");
                lblPreTaxDisplay.Text = (subtotalTotal + tradeinTotal).ToString("#0.00");
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
        //Calculating the cashout
        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            string method = "btnCalculate_Click";
            try
            {
                //If nothing is entered, setting text to 0.00 and the total to 0
                if (txtCash.Text == "") { txtCash.Text = "0.00"; receiptCashTotal = 0; }
                if (txtDebit.Text == "") { txtDebit.Text = "0.00"; receiptDebitTotal = 0; }
                if (txtGiftCard.Text == "") { txtGiftCard.Text = "0.00"; receiptGiftCertTotal = 0; }
                if (txtMasterCard.Text == "") { txtMasterCard.Text = "0.00"; receiptMCTotal = 0; }
                if (txtTradeIn.Text == "") { txtTradeIn.Text = "0.00"; receiptTradeinTotal = 0; }
                if (txtVisa.Text == "") { txtVisa.Text = "0.00"; receiptVisaTotal = 0; }

                //Giving values to the entered totals
                receiptCashTotal = Convert.ToDouble(txtCash.Text);
                receiptDebitTotal = Convert.ToDouble(txtDebit.Text);
                receiptGiftCertTotal = Convert.ToDouble(txtGiftCard.Text);
                receiptMCTotal = Convert.ToDouble(txtMasterCard.Text);
                receiptTradeinTotal = Convert.ToDouble(txtTradeIn.Text);
                receiptVisaTotal = Convert.ToDouble(txtVisa.Text);

                //The calculation of the receipt total
                receiptTotal = receiptCashTotal +
                    receiptDebitTotal + receiptGiftCertTotal + receiptMCTotal +
                    receiptTradeinTotal + receiptVisaTotal;

                //Setting the text for the receipt and sales totals
                lblReceiptsFinal.Text = receiptTotal.ToString("#0.00");
                lblTotalFinal.Text = cashoutTotal.ToString("#0.00");

                //Calculating overShort
                overShort = receiptTotal - cashoutTotal;

                //Checking over or under
                if (overShort == 0)
                {
                    lblOverShortFinal.Text = overShort.ToString("#0.00");
                }
                else if (overShort < 0)
                {
                    lblOverShortFinal.Text = "(" + overShort.ToString("#0.00") + ")";
                }
                else if (overShort > 0)
                {
                    lblOverShortFinal.Text = overShort.ToString("#0.00");
                }

                //Storing in session
                Cashout cas = new Cashout("lol", receiptTradeinTotal, receiptGiftCertTotal,
                    receiptCashTotal, receiptDebitTotal,
                    receiptMCTotal, receiptVisaTotal, receiptGSTTotal, receiptPSTTotal,
                    receiptSubTotalTotal, overShort);
                Session["receiptCashout"] = cas;
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
        //Clearing the entered amounts
        protected void btnClear_Click(object sender, EventArgs e)
        {
            string method = "btnClear_Click";
            try
            {
                //Blanking the textboxes
                txtCash.Text = "";
                txtDebit.Text = "";
                txtGiftCard.Text = "";
                txtMasterCard.Text = "";
                txtTradeIn.Text = "";
                txtVisa.Text = "";
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
        protected void printReport(object sender, EventArgs e)
        {
            string method = "printReport";
            try
            { }
            catch (Exception ex)
            {
                int employeeID = Convert.ToInt32(Session["loginEmployeeID"]);
                string currPage = Convert.ToString(Session["currPage"]);
                er.logError(ex, employeeID, currPage, method, this);
                string prevPage = Convert.ToString(Session["prevPage"]);
                Server.Transfer(prevPage, false);
            }
        }
        protected void btnProcessReport_Click(object sender, EventArgs e)
        {
            string method = "btnProcessReport_Click";
            try
            {
                string date = DateTime.Now.ToString("yyyy-MM-dd");
                string time = DateTime.Now.ToString("HH:mm:ss");

                Cashout s = (Cashout)Session["saleCashout"];
                Cashout r = (Cashout)Session["receiptCashout"];

                processed = true;

                Cashout cas = new Cashout(date, time, s.saleTradeIn, s.saleGiftCard,
                    s.saleCash, s.saleDebit, s.saleMasterCard, s.saleVisa, s.saleGST, s.salePST, s.saleSubTotal,
                    r.receiptTradeIn, r.receiptGiftCard, r.receiptCash,
                    r.receiptDebit, r.receiptMasterCard, r.receiptVisa, r.receiptGST, r.receiptPST, r.receiptSubTotal, r.overShort,
                    finalized, processed, Convert.ToDouble(lblPreTaxDisplay.Text));
                reports.insertCashout(cas);
                Session["saleCashout"] = null;
                Session["receiptCashout"] = null;
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