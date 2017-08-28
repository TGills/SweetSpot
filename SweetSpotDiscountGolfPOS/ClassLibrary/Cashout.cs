using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SweetSpotDiscountGolfPOS.ClassLibrary
{
    public class Cashout
    {
        public string mop { get; set; }
        public double amount { get; set; }
        public double tradeIn { get; set; }

        public double saleTradeIn { get; set; }
        public double saleGiftCard { get; set; }
        public double saleCash { get; set; }        
        public double saleDebit { get; set; }
        public double saleMasterCard { get; set; }
        public double saleVisa { get; set; }
        public double saleGST { get; set; }
        public double salePST { get; set; }
        public double saleSubTotal { get; set; }
               

        public double receiptTradeIn { get; set; }
        public double receiptGiftCard { get; set; }
        public double receiptCash { get; set; }
        public double receiptDebit { get; set; }
        public double receiptMasterCard { get; set; }
        public double receiptVisa { get; set; }
        public double receiptGST { get; set; }
        public double receiptPST { get; set; }
        public double receiptSubTotal { get; set; }

        public double preTax { get; set; }

        public double overShort { get; set; }
        public bool finalized { get; set; }
        public bool processed { get; set; }

        public string date { get; set; }
        public string time { get; set; }


        public Cashout() { }


        public Cashout(string m, double a, double t)
        {
            mop = m;
            amount = a;
            tradeIn = t;
        }


        public Cashout(double sgt, double spt, double ssub)
        {
            saleGST = sgt;
            salePST = spt;
            saleSubTotal = ssub;
        }

        //Used to store values for Session["saleCashout"]
        public Cashout(double st, double sg, double sc, double sd, double smc,
            double sv,  double sgt, double spt, double ssub)
        {
            saleTradeIn = st;
            saleGiftCard = sg;
            saleCash = sc;
            saleDebit = sd;
            saleMasterCard = smc;
            saleVisa = sv;
            saleGST = sgt;
            salePST = spt;
            saleSubTotal = ssub;
        }

        //Used to store values for Session["receiptCashout"]
        public Cashout(string throwaway, double rt, double rg, double rc, double rd,
            double rmc, double rv, double os, double rgt, double rpt, double rsub)
        {
            receiptTradeIn = rt;
            receiptGiftCard = rg;
            receiptCash = rc;
            receiptDebit = rd;
            receiptMasterCard = rmc;
            receiptVisa = rv;
            receiptGST = rgt;
            receiptPST = rpt;
            receiptSubTotal = rsub;


            overShort = os;
        }

        //Used for storing the cashout
        public Cashout(string d, string t, double st, double sg, double sc,  double sd, double smc,
            double sv,  double sgt, double spt, double ssub, double rt, double rg, double rc,  double rd,
            double rmc, double rv,  double rgt, double rpt, double rsub, double os, bool f, bool p, double prt)
        {
            date = d;
            time = t;

            saleTradeIn = st;
            saleGiftCard = sg;
            saleCash = sc;
            saleDebit = sd;
            saleMasterCard = smc;
            saleVisa = sv;
            saleGST = sgt;
            salePST = spt;
            saleSubTotal = ssub;

            receiptTradeIn = rt;
            receiptGiftCard = rg;
            receiptCash = rc;
            receiptDebit = rd;
            receiptMasterCard = rmc;
            receiptVisa = rv;
            receiptGST = rgt;
            receiptPST = rpt;
            receiptSubTotal = rsub;

            preTax = prt;

            overShort = os;

            finalized = f;

            processed = p;
        }

    }
}