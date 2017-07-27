using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SweetSpotDiscountGolfPOS.ClassLibrary
{
    public class Checkout
    {
        public string methodOfPayment { get; set; }
        public double amountPaid { get; set; }

        public Checkout() { }
        
        public Checkout(string m, double a)
        {
            methodOfPayment = m;
            amountPaid = a;
        }



    }
}