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
        public double saleCheque { get; set; }
        public double saleDebit { get; set; }
        public double saleMasterCard { get; set; }
        public double saleVisa { get; set; }
        public double saleAmex { get; set; }
        public double receiptTradeIn { get; set; }
        public double receiptGiftCard { get; set; }
        public double receiptCash { get; set; }
        public double receiptCheque { get; set; }
        public double receiptDebit { get; set; }
        public double receiptMasterCard { get; set; }
        public double receiptVisa { get; set; }
        public double receiptAmex { get; set; }
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

        //Used to store values for Session["saleCashout"]
        public Cashout(double st, double sg, double sc, double sch, double sd, double smc,
            double sv, double sam)
        {
            saleTradeIn = st;
            saleGiftCard = sg;
            saleCash = sc;
            saleCheque = sch;
            saleDebit = sd;
            saleMasterCard = smc;
            saleVisa = sv;
            saleAmex = sam;
        }

        //Used to store values for Session["receiptCashout"]
        public Cashout(string throwaway, double rt, double rg, double rc, double rch, double rd,
            double rmc, double rv, double ram, double os)
        {
            receiptTradeIn = rt;
            receiptGiftCard = rg;
            receiptCash = rc;
            receiptCheque = rch;
            receiptDebit = rd;
            receiptMasterCard = rmc;
            receiptVisa = rv;
            receiptAmex = ram;
            overShort = os;
        }

        //Used for storing the cashout
        public Cashout(string d, string t, double st, double sg, double sc, double sch, double sd, double smc,
            double sv, double sam, double rt, double rg, double rc, double rch, double rd,
            double rmc, double rv, double ram, double os, bool f, bool p)
        {
            date = d;
            time = t;

            saleTradeIn = st;
            saleGiftCard = sg;
            saleCash = sc;
            saleCheque = sch;
            saleDebit = sd;
            saleMasterCard = smc;
            saleVisa = sv;
            saleAmex = sam;

            receiptTradeIn = rt;
            receiptGiftCard = rg;
            receiptCash = rc;
            receiptCheque = rch;
            receiptDebit = rd;
            receiptMasterCard = rmc;
            receiptVisa = rv;
            receiptAmex = ram;

            overShort = os;

            finalized = f;

            processed = p;
        }

    }
}