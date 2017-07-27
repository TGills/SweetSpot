using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetShop
{
    public class Cart
    {
        public int sku { get; set; }
        public string description { get; set; }
        public int quantity { get; set; }
        public double price { get; set; }
        public double cost { get; set; }
        public double discount { get; set; }
        public bool percentage { get; set; }
        public bool tradeIn { get; set; }
        public int typeID { get; set; }

        public Cart() { }

        public Cart(int s, string de, int q, double pr, double c, double d, bool p, bool t, int id)
        {
            sku = s;
            description = de;
            quantity = q;
            price = pr;
            cost = c;
            discount = d;
            percentage = p;
            tradeIn = t;
            typeID = id;
        }
        //public List<Items> ItemsInCart(int sku)
        //{
        //}
    }
}
