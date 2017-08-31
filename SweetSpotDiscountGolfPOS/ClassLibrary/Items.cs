using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetShop
{
    public class Items
    {
        public int sku { get; set; }
        public string description { get; set; }
        public int quantity { get; set; }
        public string size { get; set; }
        public string colour { get; set; }
        public string gender { get; set; }
        public string style { get; set; }
        public string clubType { get; set; }
        public string shaft { get; set; }
        public string numberOfClubs { get; set; }
        public int quantityInOrder { get; set; }
        public double price { get; set; }
        public double cost { get; set; }
        public string location { get; set; }
        public int typeID { get; set; }

        public Items() { }

        public Items(int q)
        {
            quantity = q;
        }

        public Items(int s, int q)
        {
            sku = s;
            quantity = q;
        }

        public Items(int s, string d, int q, double p, double c)
        {
            sku = s;
            description = d;
            quantity = q;
            price = p;
            cost = c;
        }
        public Items(int s, string d, int q, double p, double c, string l)
        {
            sku = s;
            description = d;
            quantity = q;
            price = p;
            cost = c;
            location = l;
        }
        public Items(int s, string d, int q, double p, double c, int t, string l)
        {
            sku = s;
            description = d;
            quantity = q;
            price = p;
            cost = c;
            typeID = t;
            location = l;
        }
        //Accessories table constructor
        public Items(int sk, string sz, string clr, double prc, double cst)
        {
            sku = sk;
            size = sz;
            quantityInOrder = 1;
            colour = clr;
            price = prc;
            cost = cst;
            description = size + " " + colour + " " + price;
        }
        //Clubs table constructor
        public Items(int sk, string ct, string shft, string noc, double prc, double cst)
        {
            sku = sk;
            clubType = ct;
            shaft = shft;
            numberOfClubs = noc;
            price = prc;
            quantityInOrder = 1;
            cost = cst;
            description = clubType + " " + shaft + " " + numberOfClubs;
        }

        //Clothing table constructor
        public Items(int sk, string sz, string clr, string gn, string st, double prc, double cst)
        {
            sku = sk;
            size = sz;
            colour = clr;
            gender = gn;
            style = st;
            price = prc;
            quantityInOrder = 1;            
            description = size + " " + colour + " " + gender + " " + style;
            cost = cst;
        }

        //public Items(int sku, int typeID)
        //{
        //    string description;
        //    string qry;

        //    connectionString = "SweetSpotSBConnectionString";

        //    SqlConnection con = new SqlConnection(connectionString);
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.Connection = con;
        //    cmd.CommandText = "Select typeDescription from tbl_itemType Where typeID = @typeID";
        //    cmd.Parameters.AddWithValue("typeID", typeID);
        //    con.Open();
        //    SqlDataReader read = cmd.ExecuteReader();

        //    string table = "tbl_";

        //    qry = "Select * from " + table + " Where sku = " + sku;
        //    switch (typeID) { 
        //        case 1: //clubs
                    
        //            break;
        //        case 2: //accessories
                    
        //            break;
        //        case 3: //clothing
                    
        //            break;
        //    }
        //}
    }
}
