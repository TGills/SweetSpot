using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetShop
{
    public class Accessories
    {
        public int sku { get; set; }
        public int brandID { get; set; }
        public string size { get; set; }
        public string colour { get; set; }
        public double price { get; set; }
        public double cost { get; set; }
        public int quantity { get; set; }
        public int typeID { get; set; }
        public int locID { get; set; }
        public string comments { get; set; }
        public int modelID { get; set; }
        public string accessoryType { get; set; }

        public Accessories() { }
        public Accessories(int s, int b, string z, string clr, double p, double c, int q, int t, int l)
        {
            sku = s;
            brandID = b;
            size = z;
            colour = clr;
            price = p;
            cost = c;
            quantity = q;
            typeID = t;
            locID = l;
        }

        public Accessories(int s, int b, string z, string clr, double p, double c, int q, int t, int l, string com)
        {
            sku = s;
            brandID = b;
            size = z;
            colour = clr;
            price = p;
            cost = c;
            quantity = q;
            typeID = t;
            locID = l;
            comments = com;
        }

        public Accessories(int s, int b, int m, string a, string z, string clr, double p, double c, int q, int t, int l, string com)
        {
            sku = s;
            brandID = b;
            modelID = m;
            accessoryType = a;
            size = z;
            colour = clr;
            price = p;
            cost = c;
            quantity = q;
            typeID = t;
            locID = l;
            comments = com;
        }
    }
}
