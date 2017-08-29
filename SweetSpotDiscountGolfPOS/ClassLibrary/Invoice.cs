using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetShop
{
    public class Invoice
    {

        public int invoiceNum { get; set; }
        public int invoiceSub { get; set; }
        public DateTime invoiceDate { get; set; }
        public string invoiceTime { get; set; }
        public int customerID { get; set; }
        public int employeeID { get; set; }
        public int locationID { get; set; }
        public double subTotal { get; set; }
        public double discountAmount { get; set; }
        public double tradeinAmount { get; set; }
        public double governmentTax { get; set; }
        public double provincialTax { get; set; }
        public double balanceDue { get; set; }
        public int transactionType { get; set; }
        public string comments { get; set; }
        public string customerName { get; set; }
        public string employeeName { get; set; }
        public string locationName { get; set; }
        public double shippingAmount { get; set; }

        public Invoice() { }
        public Invoice(int I, double G, double P, double S, double B, int T)
        {
            invoiceNum = I;
            governmentTax = G;
            provincialTax = P;
            subTotal = S;
            balanceDue = B;
            transactionType = T;
        }
        public Invoice(int I, int S, DateTime D, string CN, double BD, string LN, string EN)
        {
            invoiceNum = I;
            invoiceSub = S;
            invoiceDate = D;
            customerName = CN;
            balanceDue = BD;
            locationName = LN;
            employeeName = EN;
        }
        public Invoice(int I, double TR, double S, double G, double P, double B, int T, DateTime D)
        {
            invoiceNum = I;
            tradeinAmount = TR;
            subTotal = S;
            governmentTax = G;
            provincialTax = P;
            balanceDue = B;
            transactionType = T;
            invoiceDate = D;
        }
        public Invoice(int I, int IS, DateTime ID, string IT, int CN, int EN, int L, double ST, double DA, double TA, double G, double P, double BD, int TT, string C)
        {
            invoiceNum = I;
            invoiceSub = IS;
            invoiceDate = ID;
            invoiceTime = IT;
            customerID = CN;
            employeeID = EN;
            locationID = L;
            subTotal = ST;
            discountAmount = DA;
            tradeinAmount = TA;
            governmentTax = G;
            provincialTax = P;
            balanceDue = BD;
            transactionType = TT;
            comments = C;
        }
        public Invoice(int I, int IS, DateTime ID, string IT, int CN, int EN, int L, double ST, double SA, double DA, double TA, double G, double P, double BD, int TT, string C)
        {
            invoiceNum = I;
            invoiceSub = IS;
            invoiceDate = ID;
            invoiceTime = IT;
            customerID = CN;
            employeeID = EN;
            locationID = L;
            subTotal = ST;
            shippingAmount = SA;
            discountAmount = DA;
            tradeinAmount = TA;
            governmentTax = G;
            provincialTax = P;
            balanceDue = BD;
            transactionType = TT;
            comments = C;
        }
        //public Invoice(int invoiceID)
        //{
        //    invoiceId = invoiceID;
        //}
        //     public Invoice(int InvoiceID, int CustomerID, double GST, double PST, double PaymentTotal,
        //         double SubTotal, double Total, double Discount, double TradeIn, int PaymentID, int StateProveID, bool Posted,
        //         bool InProcess, DateTime PostedDate, DateTime DateModified, DateTime SaleDate)
        //     {
        //         invoiceId = InvoiceID;
        //         customerId = CustomerID;
        //         gst = GST;
        //         pst = PST;
        //         paymentTotal = PaymentTotal;
        //         subTotal = SubTotal;
        //         total = Total;
        //         discount = Discount;
        //         tradeIn = TradeIn;
        //         paymentID = PaymentID;
        //stateprovID = StateProveID;
        //         posted = Posted;
        //         inProcess = InProcess;
        //         postedDate = PostedDate;
        //         dateModified = DateModified;
        //         saleDate = SaleDate;

        //     }
        //     public Invoice(int InvoiceID, int CustomerID, double GST, double PST,  double PaymentTotal, double SubTotal, double Total, DateTime SaleDate)
        //     {
        //         invoiceId = InvoiceID;
        //         customerId = CustomerID;
        //         gst = GST;
        //         pst = PST;
        //         paymentTotal = PaymentTotal;
        //         subTotal = SubTotal;
        //         total = Total;
        //         saleDate = SaleDate;
        //     }
        //public Invoice(int InvoiceID, double GST, double PST, double Total, int PaymentID, DateTime SaleDate)
        //{
        //    invoiceId = InvoiceID;
        //    gst = GST;
        //    pst = PST;
        //    total = Total;
        //    paymentID = PaymentID;
        //    saleDate = SaleDate;
        //}
        //public Invoice(int InvoiceID, double TradeIn, double SubTotal, double GST, double PST, double Total, int PaymentID, DateTime SaleDate)
        //{
        //    invoiceId = InvoiceID;
        //    tradeIn = TradeIn;
        //    subTotal = SubTotal;
        //    gst = GST;
        //    pst = PST;
        //    total = Total;
        //    paymentID = PaymentID;
        //    saleDate = SaleDate;
        //}
    }
}
