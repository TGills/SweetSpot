using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetShop
{
    //The clubs class is used to define and create easy to access information for clubs.
    public class Clubs
    {

        public int sku { get; set; }
        public DateTime ShipmentDate { get; set; }
        public int QuantityInOrder { get; set; }
        public double ItemSubTotal { get; set; }
        public int brandID { get; set; }
        public int modelID { get; set; }
        public string clubType { get; set; }
        public string shaft { get; set; }
        public string numberOfClubs { get; set; }
        public double TradeInPrice { get; set; }
        public double premium { get; set; }
        public double cost { get; set; }
        public double price { get; set; }
        public double WePay { get; set; }
        public int quantity { get; set; }
        public double ExtendedPrice { get; set; }
        public double RetailPrice { get; set; }
        public string comments { get; set; }
        public string clubSpec { get; set; }
        public string shaftSpec { get; set; }
        public string shaftFlex { get; set; }
        public string dexterity { get; set; }
        public string Destination { get; set; }
        public bool Received { get; set; }
        public bool Paid { get; set; }
        public bool Gst { get; set; }
        public bool Pst { get; set; }
        public double GstAmount { get; set; }
        public double PstAmount { get; set; }
        public int typeID { get; set; }
        public bool used { get; set; }
        public int itemlocation { get; set; }

        //*******
        public Clubs(int SKU, int brand, int model, int type, string ClubType, string Shaft, string NumberOfClubs,
            double Premium, double Cost, double Price, int Quantity, string ClubSpec, string ShaftSpec, string ShaftFlex,
            string Dexterity, bool Used, string Comments)
        {
            sku = SKU;
            brandID = brand;
            modelID = model;
            typeID = type;
            clubType = ClubType;
            shaft = Shaft;
            numberOfClubs = NumberOfClubs;
            premium = Premium;
            cost = Cost;
            price = Price;
            quantity = Quantity;
            clubSpec = ClubSpec;
            shaftSpec = ShaftSpec;
            shaftFlex = ShaftFlex;
            dexterity = Dexterity;
            used = Used;
            comments = Comments;
        }
        //This one has location
        public Clubs(int SKU, int brand, int model, int type, string ClubType, string Shaft, string NumberOfClubs,
            double Premium, double Cost, double Price, int Quantity, string ClubSpec, string ShaftSpec, string ShaftFlex,
            string Dexterity, int itemLocation, bool Used, string Comments)
        {
            sku = SKU;
            brandID = brand;
            modelID = model;
            typeID = type;
            clubType = ClubType;
            shaft = Shaft;
            numberOfClubs = NumberOfClubs;
            premium = 0;
            cost = Cost;
            price = Price;
            quantity = Quantity;
            clubSpec = ClubSpec;
            shaftSpec = ShaftSpec;
            shaftFlex = ShaftFlex;
            dexterity = Dexterity;
            itemlocation = itemLocation;
            used = Used;
            comments = Comments;
        }
        //********


        public Clubs() { }

        //public Clubs(int SKU, DateTime ShipDate, int brand,
        //    int model, string ClubType, string Shaft, string NumberOfClubs,
        //    double tradeInPrice, double Premium, double wePay, int Quantity,
        //    double extendedPrice, double retailPrice, string Comments,
        //    string ClubSpec, string ShaftSpec, string ShaftFlex, string Dexterity,
        //    string destination, bool received, bool paid, bool gST, bool pST)
        //{
        //    sku = SKU;
        //    ShipmentDate = ShipDate;
        //    brandID = brand;
        //    modelID = model;
        //    clubType = ClubType;
        //    shaft = Shaft;
        //    numberOfClubs = NumberOfClubs;
        //    TradeInPrice = tradeInPrice;
        //    premium = Premium;
        //    WePay = wePay;
        //    quantity = Quantity;
        //    ExtendedPrice = extendedPrice;
        //    RetailPrice = retailPrice;
        //    comments = Comments;
        //    clubSpec = ClubSpec;
        //    shaftSpec = ShaftSpec;
        //    shaftFlex = ShaftFlex;
        //    dexterity = Dexterity;
        //    Destination = destination;
        //    Received = received;
        //    Paid = paid;
        //    Gst = gST;
        //    Pst = pST;
        //    QuantityInOrder = 1;
        //    ItemSubTotal = RetailPrice * QuantityInOrder;

        //}

        //public Clubs(int sKU, int brand, int model, string ClubType, string Shaft, string NumberOfClubs,
        //    double tradeInPrice, double Premium, double wePay, int Quantity, double extendedPrice, double retailPrice,
        //    string ClubSpec, string ShaftSpec, string ShaftFlex, string Dexterity)
        //{
        //    sku = sKU;
        //    brandID = brand;
        //    modelID = model;
        //    clubType = ClubType;
        //    shaft = Shaft;
        //    numberOfClubs = NumberOfClubs;
        //    TradeInPrice = tradeInPrice;
        //    premium = Premium;
        //    WePay = wePay;
        //    quantity = Quantity;
        //    ExtendedPrice = extendedPrice;
        //    RetailPrice = retailPrice;
        //    clubSpec = ClubSpec;
        //    shaftSpec = ShaftSpec;
        //    shaftFlex = ShaftFlex;
        //    dexterity = Dexterity;
        //    QuantityInOrder = 1;
        //    ItemSubTotal = RetailPrice * QuantityInOrder;

        //}

        //public Clubs(int sKU,int quantityInOrder, int brand, int model, string ClubType, string Shaft, string NumberOfClubs,
        // double tradeInPrice, double Premium, double wePay, int Quantity, double extendedPrice, double retailPrice,
        // string ClubSpec, string ShaftSpec, string ShaftFlex, string Dexterity,bool gST,bool pST)
        //{
        //    sku = sKU;
        //    brandID = brand;
        //    modelID = model;
        //    clubType = clubType;
        //    shaft = shaft;
        //    numberOfClubs = numberOfClubs;
        //    TradeInPrice = tradeInPrice;
        //    premium = premium;
        //    WePay = wePay;
        //    quantity = quantity;
        //    ExtendedPrice = extendedPrice;
        //    RetailPrice = retailPrice;
        //    clubSpec = clubSpec;
        //    shaftSpec = shaftSpec;
        //    shaftFlex = shaftFlex;
        //    dexterity = dexterity;
        //    Gst = gST;
        //    Pst = pST;
        //    ItemSubTotal = RetailPrice * quantityInOrder;

        //}

        //public Clubs(int SKU, int brand, int model, string ClubType, string Shaft, string NumberOfClubs, double tradeInPrice,
        //    double Premium, double wePay, int Quantity, double extendedPrice, double retailPrice, string ClubSpec,
        //    string ShaftSpec, string ShaftFlex, string Dexterity, bool gST, bool pST)
        //{
        //    sku = SKU;
        //    brandID = brand;
        //    modelID = model;
        //    clubType = ClubType;
        //    shaft = Shaft;
        //    numberOfClubs = NumberOfClubs;
        //    TradeInPrice = tradeInPrice;
        //    premium = Premium;
        //    WePay = wePay;
        //    quantity = Quantity;
        //    ExtendedPrice = extendedPrice;
        //    RetailPrice = retailPrice;
        //    clubSpec = ClubSpec;
        //    shaftSpec = ShaftSpec;
        //    shaftFlex = ShaftFlex;
        //    dexterity = Dexterity;
        //    Gst = gST;
        //    Pst = pST;
        //}


    }
}
