using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Configuration;
using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;

namespace SweetSpotProShop
{
    public class ItemDataUtilities
    {
        private String connectionString;
        LocationManager lm = new LocationManager();
        public ItemDataUtilities()
        {
            connectionString = ConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString;
        }
        //Return Model string created by Nathan and Tyler
        public string modelType(int modelID)
        {

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = "Select modelName from tbl_model where modelID = " + modelID;

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            string model = null;

            while (reader.Read())
            {
                string m = reader["modelName"].ToString();
                model = m;
            }
            conn.Close();
            return model;
        }
        //Return Brand string created by Nathan and Tyler
        public string brandType(int brandID)
        {

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = "Select brandName from tbl_brand where brandID = " + brandID;

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            string brand = null;

            while (reader.Read())
            {
                string b = reader["brandName"].ToString();
                brand = b;
            }
            conn.Close();
            return brand;
        }
        //Return Model Int created by Nathan and Tyler
        public int modelName(string modelN)
        {
            int model = 0;
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = "Select modelID from tbl_model where modelName = @modelName";
            cmd.Parameters.AddWithValue("modelName", modelN);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int m = Convert.ToInt32(reader["modelID"]);
                model = m;
            }
            conn.Close();
            return model;
        }
        //Return Brand Int created by Nathan and Tyler
        public int brandName(string brandN)
        {


            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = "Select brandID from tbl_brand where brandName = '" + brandN + "'";

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            int brand = 0;

            while (reader.Read())
            {
                int b = Convert.ToInt32(reader["brandID"]);
                brand = b;
            }
            conn.Close();
            return brand;
        }
        //Return Model string created by Nathan and Tyler
        public string typeName(int typeNum)
        {

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = "Select typeDescription from tbl_itemType where typeID = " + typeNum;

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            string type = null;

            while (reader.Read())
            {
                string t = reader["typeDescription"].ToString();
                type = t;
            }
            conn.Close();
            return type;
        }

        //Return Vendor ID
        public int getVendorID(string vendorName)
        {
            int vendorID = 0;
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select vendorID from tbl_vendor where vendorName = '" + vendorName + "'";
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int vID = Convert.ToInt32(reader["vendorID"]);
                vendorID = vID;
            }
            conn.Close();
            return vendorID;
        }
        //Return Vendor Name
        public string getVendorName(int vendorID)
        {
            string vendorName = null;
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = "Select vendorName from tbl_vendor where vendorID = " + vendorID;

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string vN = reader["vendorName"].ToString();
                vendorName = vN;
            }
            conn.Close();

            return vendorName;
        }


        //Return Club Type ID
        public int getClubTypeID(string typeName)
        {
            int typeID = 0;
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select typeID from tbl_clubType where typeName = '" + typeName + "'";
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int tID = Convert.ToInt32(reader["typeID"]);
                typeID = tID;
            }
            conn.Close();
            return typeID;
        }
        //Return Club Type Name
        public string getClubTypeName(int typeID)
        {
            string typeName = null;
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = "Select typeName from tbl_clubType where typeID = " + typeID;

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string tN = reader["typeName"].ToString();
                typeName = tN;
            }
            conn.Close();

            return typeName;
        }

        //Adding items to the Cart class. Totally not stolen by Tickles
        public Cart addingToCart(Object o)
        {
            Cart ca = new Cart();
            if (o is Clubs)
            {
                Clubs c = o as Clubs;
                ca.sku = c.sku;
                ca.description = brandType(c.brandID) + " " + modelType(c.modelID) + " " + c.clubType + " " + c.shaft + " " + c.numberOfClubs + " " + c.dexterity;
                ca.price = c.price;
                ca.cost = c.cost;
                ca.typeID = c.typeID;
            }
            else if (o is Accessories)
            {
                Accessories a = o as Accessories;
                ca.sku = a.sku;
                ca.description = brandType(a.brandID) + " " + a.size + " " + a.colour;
                ca.price = a.price;
                ca.cost = a.cost;
                ca.typeID = a.typeID;
            }
            else if (o is Clothing)
            {
                Clothing cl = o as Clothing;
                ca.sku = cl.sku;
                ca.description = brandType(cl.brandID) + " " + cl.size + " " + cl.colour + " " + cl.gender + " " + cl.style;
                ca.price = cl.price;
                ca.cost = cl.cost;
                ca.typeID = cl.typeID;
            }
            ca.quantity = 1;
            return ca;
        }
        public string ConvertDBNullToString(Object o)
        {
            if (o is DBNull)
                o = "";

            return o.ToString();

        }
        public double ConvertDBNullToDouble(Object o)
        {
            double dbl = 0.0;
            if (o is DBNull)
                dbl = 0.0;
            else
                dbl = Convert.ToDouble(o);

            return dbl;

        }
        public double GetTaxRates(int ProId)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select GST from StateProvLT where stateProvID = @ProId";
            cmd.Parameters.AddWithValue("@ProId", ProId);
            conn.Open();
            SqlDataReader read = cmd.ExecuteReader();

            read.Read();

            double t = Convert.ToDouble(read["gst"]);


            conn.Close();
            return t;
        }
        public double GetPSTTax(int ProId)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select PST from StateProvLT where stateProvID = @ProId";
            cmd.Parameters.AddWithValue("@ProId", ProId);
            conn.Open();
            SqlDataReader read = cmd.ExecuteReader();

            read.Read();
            double t = Convert.ToDouble(read["pst"]);

            conn.Close();
            return t;
        }
        //populating gridView on Inventory Search button in Sales Cart with location
        public List<Items> getItemByID(Int32 ItemNumber, string loc)
        {
            List<Items> items = new List<Items>();
            Items i = new Items();
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            int intLocation = lm.locationIDfromCity(loc);
            cmd.Connection = conn;
            conn.Open();
            //Removed location because client did not want
            cmd.CommandText = "Select sku, quantity, brandID, size, colour, price, cost From tbl_accessories Where SKU = @skuAcc";
            cmd.Parameters.AddWithValue("skuAcc", ItemNumber);
            //cmd.Parameters.AddWithValue("locationID", intLocation);

            SqlDataReader readerAcc = cmd.ExecuteReader();
            while (readerAcc.Read())
            {

                i = new Items(Convert.ToInt32(readerAcc["sku"]), brandType(Convert.ToInt32(readerAcc["brandID"]))
                    + " " + readerAcc["size"].ToString() + " " + readerAcc["colour"].ToString(),
                    Convert.ToInt32(readerAcc["quantity"]), Convert.ToDouble(readerAcc["price"]),
                    Convert.ToDouble(readerAcc["cost"]));

            }
            if (!readerAcc.HasRows)
            {
                readerAcc.Close();
                cmd.CommandText = "Select sku, brandID, modelID, clubType, shaft, numberOfClubs, dexterity, quantity, price, cost From tbl_clubs Where SKU = @skuClubs";
                cmd.Parameters.AddWithValue("skuClubs", ItemNumber);
                //cmd.Parameters.AddWithValue("locationIDclubs", intLocation);
                SqlDataReader readerClubs = cmd.ExecuteReader();
                while (readerClubs.Read())
                {
                    i = new Items(Convert.ToInt32(readerClubs["sku"]), brandType(Convert.ToInt32(readerClubs["brandID"]))
                        + " " + modelType(Convert.ToInt32(readerClubs["modelID"])) + " " + readerClubs["clubType"].ToString()
                        + " " + readerClubs["shaft"].ToString() + " " + readerClubs["numberOfClubs"].ToString() + " "
                        + readerClubs["dexterity"].ToString(), Convert.ToInt32(readerClubs["quantity"]),
                        Convert.ToDouble(readerClubs["price"]), Convert.ToDouble(readerClubs["cost"]));

                }
                if (!readerClubs.HasRows)
                {
                    readerClubs.Close();
                    cmd.CommandText = "Select sku, brandID, size, colour, gender, style, quantity, price, cost From tbl_clothing Where SKU = @skuClothing";
                    cmd.Parameters.AddWithValue("skuClothing", ItemNumber);
                    //cmd.Parameters.AddWithValue("locationIDclothing", intLocation);
                    SqlDataReader readerClothing = cmd.ExecuteReader();
                    while (readerClothing.Read())
                    {
                        i = new Items(Convert.ToInt32(readerClothing["sku"]), brandType(Convert.ToInt32(readerClothing["brandID"]))
                            + " " + readerClothing["size"].ToString() + " " + readerClothing["colour"].ToString()
                            + " " + readerClothing["gender"].ToString() + " " + readerClothing["style"].ToString(),
                            Convert.ToInt32(readerClothing["quantity"]), Convert.ToDouble(readerClothing["price"]),
                            Convert.ToDouble(readerClothing["cost"]));
                    }
                }
            }
            if (i.sku > 0)
            {
                items.Add(i);
            }
            conn.Close();

            return items;

        }
        //populating gridView on Inventory Search button for all locations
        public List<Items> getItemByID(Int32 ItemNumber)
        {
            List<Items> items = new List<Items>();
            Items i = new Items();
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            conn.Open();
            cmd.CommandText = "Select sku, quantity, brandID, size, colour, price, cost, locationID From tbl_accessories Where SKU = @skuAcc";
            cmd.Parameters.AddWithValue("skuAcc", ItemNumber);

            SqlDataReader readerAcc = cmd.ExecuteReader();
            while (readerAcc.Read())
            {

                i = new Items(Convert.ToInt32(readerAcc["sku"]), brandType(Convert.ToInt32(readerAcc["brandID"]))
                    + " " + readerAcc["size"].ToString() + " " + readerAcc["colour"].ToString(),
                    Convert.ToInt32(readerAcc["quantity"]), Convert.ToDouble(readerAcc["price"]),
                    Convert.ToDouble(readerAcc["cost"]), lm.locationName(Convert.ToInt32(readerAcc["locationID"])));

            }
            if (!readerAcc.HasRows)
            {
                readerAcc.Close();
                cmd.CommandText = "Select sku, brandID, modelID, clubType, shaft, numberOfClubs, dexterity, quantity, price, cost, locationID From tbl_clubs Where SKU = @skuClubs";
                cmd.Parameters.AddWithValue("skuClubs", ItemNumber);
                SqlDataReader readerClubs = cmd.ExecuteReader();
                while (readerClubs.Read())
                {
                    i = new Items(Convert.ToInt32(readerClubs["sku"]), brandType(Convert.ToInt32(readerClubs["brandID"]))
                        + " " + modelType(Convert.ToInt32(readerClubs["modelID"])) + " " + readerClubs["clubType"].ToString()
                        + " " + readerClubs["shaft"].ToString() + " " + readerClubs["numberOfClubs"].ToString() + " "
                        + readerClubs["dexterity"].ToString(), Convert.ToInt32(readerClubs["quantity"]),
                        Convert.ToDouble(readerClubs["price"]), Convert.ToDouble(readerClubs["cost"]), lm.locationName(Convert.ToInt32(readerClubs["locationID"])));

                }
                if (!readerClubs.HasRows)
                {
                    readerClubs.Close();
                    cmd.CommandText = "Select sku, brandID, size, colour, gender, style, quantity, price, cost, locationID From tbl_clothing Where SKU = @skuClothing";
                    cmd.Parameters.AddWithValue("skuClothing", ItemNumber);
                    SqlDataReader readerClothing = cmd.ExecuteReader();
                    while (readerClothing.Read())
                    {
                        i = new Items(Convert.ToInt32(readerClothing["sku"]), brandType(Convert.ToInt32(readerClothing["brandID"]))
                            + " " + readerClothing["size"].ToString() + " " + readerClothing["colour"].ToString()
                            + " " + readerClothing["gender"].ToString() + " " + readerClothing["style"].ToString(),
                            Convert.ToInt32(readerClothing["quantity"]), Convert.ToDouble(readerClothing["price"]),
                            Convert.ToDouble(readerClothing["cost"]), lm.locationName(Convert.ToInt32(readerClothing["locationID"])));
                    }
                }
            }
            if (i.sku > 0)
            {
                items.Add(i);
            }
            conn.Close();

            return items;

        }
        //Being used now to return qty to validate if sku can be added to cart.
        public int getquantity(int sku, int typeID)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select quantity from tbl_clubs where SKU = @sku";
            cmd.Parameters.AddWithValue("sku", sku);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            int itemQTY = 0;

            while (reader.Read())
            {
                itemQTY = Convert.ToInt32(reader["quantity"]);
            }
            if (!reader.HasRows)
            {
                reader.Close();
                cmd.CommandText = "Select quantity from tbl_accessories where SKU = @skuacces";
                cmd.Parameters.AddWithValue("skuacces", sku);
                SqlDataReader readerAccesories = cmd.ExecuteReader();

                while (readerAccesories.Read())
                {
                    itemQTY = Convert.ToInt32(readerAccesories["quantity"]);
                }
                if (!readerAccesories.HasRows)
                {
                    readerAccesories.Close();
                    cmd.CommandText = "Select quantity from tbl_clothing where SKU = @skucloth";
                    cmd.Parameters.AddWithValue("skucloth", sku);
                    SqlDataReader readerclothing = cmd.ExecuteReader();

                    while (readerclothing.Read())
                    {
                        itemQTY = Convert.ToInt32(readerclothing["quantity"]);
                    }
                }
            }
            conn.Close();
            return itemQTY;
        }
        public void removeQTYfromInventoryWithSKU(int sku, int typeID, int remainingQTY)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            string table = typeName(typeID);
            cmd.CommandText = "UPDATE tbl_" + table + " SET quantity = @quantity WHERE sku = @sku and typeID = @typeID";
            cmd.Parameters.AddWithValue("@sku", sku);
            cmd.Parameters.AddWithValue("@typeID", typeID);
            cmd.Parameters.AddWithValue("@quantity", remainingQTY);
            //Declare and open connection
            cmd.Connection = con;
            con.Open();
            //Execute Insert
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public void updateQuantity(int sku, int typeID, int quantity)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            string table = typeName(typeID);
            cmd.CommandText = "UPDATE tbl_" + table + " SET quantity = @quantity WHERE sku = @sku and typeID = @typeID";
            cmd.Parameters.AddWithValue("@sku", sku);
            cmd.Parameters.AddWithValue("@typeID", typeID);
            cmd.Parameters.AddWithValue("@quantity", quantity);
            //Declare and open connection
            cmd.Connection = con;
            con.Open();
            //Execute Insert
            cmd.ExecuteNonQuery();
            con.Close();
        }


        //Reserve trade-in sku
        public int reserveTradeInSKu(int loc)
        {
            int tradeInSkuDisplay = 0;
            tradeInSkuDisplay = tradeInSku(loc);
            int[] range = new int[2];
            range = tradeInSkuRange(loc);
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Insert into tbl_tempTradeInCartSkus (sku, locationID) values (@sku, @locationID);";
            cmd.Parameters.AddWithValue("sku", tradeInSkuDisplay);
            cmd.Parameters.AddWithValue("locationID", loc);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            conn.Close();
            return tradeInSkuDisplay;
        }

        //Grabbing trade-in sku
        public int tradeInSku(int location)
        {
            int sku = 0;
            int[] range = new int[2];
            range = tradeInSkuRange(location);
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select max(sku) as maxsku from tbl_tempTradeInCartSkus where locationID = " + location.ToString();
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            Clubs club = new Clubs();
            int maxSku = 0;
            while (reader.Read())
            {
                maxSku = (reader["maxsku"] as int?) ?? range[0]; //Setting it to 0
            }
            conn.Close();

            //If the maxSku returns as null, sets the sku as the min range
            if (maxSku.Equals(null))
            {
                sku = range[0];
            }
            //If the maxSku is less than the upper range, sku is the max sku + 1
            else if (maxSku < range[1])
            {
                sku = maxSku + 1;
            }
            //If the maxSku equals the upper range, sku equals the min range
            else if (maxSku == range[1])
            {
                sku = range[0];
            }


            return sku;
        }
        public int[] tradeInSkuRange(int location)
        {
            int[] range = new int[2];
            int upper = 0;
            int lower = 0;

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = "Select skuStartAt, skuStopAt from tbl_tradeInSkusForCart where locationID = " + location.ToString();
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                upper = Convert.ToInt32(reader["skuStopAt"].ToString());
                lower = Convert.ToInt32(reader["skuStartAt"].ToString());
            }
            range[0] = lower;
            range[1] = upper;


            conn.Close();
            return range;
        }
        //Adding tradein item 
        public void addTradeInItem(Clubs tradeInItem)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            int used = 0;
            if (tradeInItem.used == true)
            {
                used = 1;
            }
            else
            {
                used = 0;
            }
            if(tradeInItem.itemlocation == 0)
            {
                tradeInItem.itemlocation = 1;
            }


            cmd.Connection = conn;
            //cmd.CommandText = "insert into tbl_tempTradeInCartSkus values(" + tradeInItem.sku + ", " + tradeInItem.brandID + ", " +
            //    tradeInItem.modelID + ", '" + tradeInItem.clubType + "', '" + tradeInItem.shaft + "', '" + tradeInItem.numberOfClubs + "', " +
            //    tradeInItem.premium + ", " + tradeInItem.cost + ", " + tradeInItem.price + ", " + tradeInItem.quantity + ", '" +
            //    tradeInItem.clubSpec + "', '" + tradeInItem.shaftSpec + "', '" + tradeInItem.shaftFlex + "', '" +
            //    tradeInItem.dexterity + "', " + tradeInItem.typeID + ", " + tradeInItem.itemlocation + ", " +
            //    used + ", '" + tradeInItem.comments + "');";

            cmd.CommandText = "Update tbl_tempTradeInCartSkus set brandID = @brandID, modelID = @modelID, clubType = @clubType, shaft = @shaft," +
                "numberOfClubs = @numberOfClubs, premium = @premium, cost = @cost, price = @price, quantity = @quantity, clubSpec = @clubSpec," +
                "shaftSpec = @shaftSpec, shaftFlex = @shaftFlex, dexterity = @dexterity, typeID = @typeID, locationID = @locationID, used = @used," +
                "comments = @comments where sku = @sku;";

            cmd.Parameters.AddWithValue("sku", tradeInItem.sku);

            cmd.Parameters.AddWithValue("brandID", tradeInItem.brandID);
            cmd.Parameters.AddWithValue("modelID", tradeInItem.brandID);
            cmd.Parameters.AddWithValue("clubType", tradeInItem.brandID);
            cmd.Parameters.AddWithValue("shaft", tradeInItem.brandID);

            cmd.Parameters.AddWithValue("numberOfClubs", tradeInItem.brandID);
            cmd.Parameters.AddWithValue("premium", tradeInItem.brandID);
            cmd.Parameters.AddWithValue("cost", tradeInItem.brandID);
            cmd.Parameters.AddWithValue("price", tradeInItem.brandID);
            cmd.Parameters.AddWithValue("quantity", tradeInItem.brandID);
            cmd.Parameters.AddWithValue("clubSpec", tradeInItem.brandID);

            cmd.Parameters.AddWithValue("shaftSpec", tradeInItem.brandID);
            cmd.Parameters.AddWithValue("shaftFlex", tradeInItem.brandID);
            cmd.Parameters.AddWithValue("dexterity", tradeInItem.brandID);
            cmd.Parameters.AddWithValue("typeID", tradeInItem.brandID);
            cmd.Parameters.AddWithValue("locationID", tradeInItem.brandID);
            cmd.Parameters.AddWithValue("used", tradeInItem.brandID);

            cmd.Parameters.AddWithValue("comments", tradeInItem.brandID);

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            conn.Close();
        }

        //Sending all of the invoice information to the database 
        public void mainInvoice(CheckoutManager ckm, List<Cart> cart, List<Checkout> mops, Customer c, Employee e, int transactionType, string invoiceNumber, string comments)
        {

            string locationInitial = invoiceNumber.Split('-')[0];

            ////Step 1: Find next invoice number 
            //int nextInvoiceNum = getNextInvoiceNum();
            int nextInvoiceNum = Convert.ToInt32(invoiceNumber.Split('-')[1]);

            ////Step 2: Find the invoice sub number
            int nextInvoiceSubNum = getNextInvoiceSubNum(nextInvoiceNum);
            //int nextInvoiceSubNum = Convert.ToInt32(invoiceNumber.Split('-')[2]);


            //Step 3: Get date and time
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string time = DateTime.Now.ToString("HH:mm:ss");

            //Step 4: Insert all relevent info into the mainInvoice table
            //invoiceNum, invoiceSubNum, invoiceDate, invoiceTime, custID, empID, locationID, subTotal, discountAmount, tradeinAmount, governmentTax, provincialTax, balanceDue, transactionType, comments
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            //+ ", '" + date + "', '" + time + "', "
            cmd.CommandText =
                "update tbl_invoice set " +
                "invoiceSubNum = @invoiceSubNum, " +
                "custID = @custID, " +
                "empID = @empID, " +
                "locationID = @locationID, " +
                "subTotal = @subtotal, " +
                "discountAmount = @discountAmount, " +
                "tradeinAmount = @tradeinAmount, " +
                "governmentTax = @governmentTax, " +
                "provincialTax = @provincialTax, " +
                "balanceDue = @balanceDue, " +
                "transactionType = @transactionType, " +
                "comments = @comments" +
                " where invoiceNum = @invoiceNum;";

            cmd.Parameters.AddWithValue("invoiceNum", nextInvoiceNum);
            cmd.Parameters.AddWithValue("invoiceSubNum", nextInvoiceSubNum);
            cmd.Parameters.AddWithValue("custID", c.customerId);
            cmd.Parameters.AddWithValue("empID", e.employeeID);
            cmd.Parameters.AddWithValue("locationID", e.locationID);
            cmd.Parameters.AddWithValue("subTotal", ckm.dblSubTotal);
            cmd.Parameters.AddWithValue("discountAmount", ckm.dblDiscounts);
            cmd.Parameters.AddWithValue("tradeinAmount", ckm.dblTradeIn);
            cmd.Parameters.AddWithValue("governmentTax", ckm.dblGst);
            cmd.Parameters.AddWithValue("provincialTax", ckm.dblPst);
            cmd.Parameters.AddWithValue("balanceDue", ckm.dblBalanceDue);
            cmd.Parameters.AddWithValue("transactionType", transactionType);
            cmd.Parameters.AddWithValue("comments", comments);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                nextInvoiceNum = Convert.ToInt32(reader["invoiceNum"]) + 1;
            }
            conn.Close();
            //Step 5: Insert each item into the invoiceItem table
            //invoiceNum, invoiceSubNum, sku, itemQuantity, itemCost, itemPrice, itemDiscount, Percentage
            foreach (Cart item in cart)
            {
                int percentage = 0;
                if (item.percentage)
                {
                    percentage = 1;
                }
                else
                {
                    percentage = 0;
                }
                string insert = "insert into tbl_invoiceItem values(" + nextInvoiceNum + ", " + nextInvoiceSubNum + ", " + item.sku + ", " + item.quantity + ", " +
                    item.cost + ", " + item.price + ", " + item.discount + ", " + percentage + ");";

                invoiceItem(insert);
            }
            //Step 6: Insert each MOP into the invoiceMOP table
            //ID(autoincrementing), invoiceNum, invoiceSubNum, mopID, amountPaid
            foreach (Checkout mop in mops)
            {
                string insert = "insert into tbl_invoiceMOP values(" + nextInvoiceNum + ", " + nextInvoiceSubNum + ", '" + mop.methodOfPayment + "', " + mop.amountPaid + ");";
                invoiceMOP(insert);
            }
        }
        //Returns the max invoice num + 1
        public int getNextInvoiceNum()
        {
            int nextInvoiceNum = 0;
            int nextInvoiceSubNum = 0;
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select Max(invoiceNum) as invoiceNum from tbl_invoice";
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (reader["invoiceNum"] == DBNull.Value)
                {
                    nextInvoiceNum = 0;
                    nextInvoiceSubNum = getNextInvoiceSubNum(nextInvoiceNum);
                    createInvoiceNum(nextInvoiceNum, nextInvoiceSubNum);
                }
                else
                {
                    nextInvoiceNum = Convert.ToInt32(reader["invoiceNum"]) + 1;
                    nextInvoiceSubNum = getNextInvoiceSubNum(nextInvoiceNum);
                    createInvoiceNum(nextInvoiceNum, nextInvoiceSubNum);
                }
            }
            conn.Close();
            return nextInvoiceNum;
        }
        //Create  the newly found invoice number
        public void createInvoiceNum(int invNum, int subNum)
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string time = DateTime.Now.ToString("HH:mm:ss");

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            //invoiceNum, invoiceSubNum, invoiceDate, invoiceTime, custID, empID, locationID, subTotal, discountAmount, tradeinAmount
            //governmentTax, provincialTax, balanceDue, transactionType, comments
            cmd.CommandText = "Insert into tbl_invoice(invoiceNum, invoiceSubNum, invoiceDate, invoiceTime) values(@invNum, @invSubNum, @date, @time);";
            cmd.Parameters.AddWithValue("invNum", invNum);
            cmd.Parameters.AddWithValue("invSubNum", subNum);
            cmd.Parameters.AddWithValue("date", date);
            cmd.Parameters.AddWithValue("time", time);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            conn.Close();
        }
        //Returns the max invoice subNum
        public int getNextInvoiceSubNum(int invoiceNumber)
        {
            int invoiceSubNum = 0;
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select Max(invoiceSubNum) as invoiceSubNum from tbl_invoice Where invoiceNum = " + invoiceNumber + ";";
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (reader["invoiceSubNum"] == DBNull.Value)
                {
                    invoiceSubNum = 0;
                }
                else
                {
                    invoiceSubNum = Convert.ToInt32(reader["invoiceSubNum"]) + 1;
                }
            }
            conn.Close();
            return invoiceSubNum;
        }
        //Adding items to the invoice  
        public void invoiceItem(string insert)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = insert;
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            conn.Close();
        }
        //Adding mops to the invoice 
        public void invoiceMOP(string insert)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = insert;
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            conn.Close();
        }
        public int returnEmployeeIDfromPassword(int pWord)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select empID from tbl_userInfo where password = " + pWord;
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            int empID = 0;
            while (reader.Read())
            {
                empID = Convert.ToInt32(reader["empID"]);
            }
            conn.Close();
            return empID;
        }
        //Returns max sku
        public int maxSku(int sku, string table)
        {
            int maxSku = 0;
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select Max(sku) as largestSku from tbl_" + table + ";";
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (reader["largestSku"] == DBNull.Value)
                {
                    maxSku = 0;
                }
                else
                {
                    maxSku = Convert.ToInt32(reader["largestSku"]) + 1;
                }
            }
            conn.Close();
            return maxSku;
        }

        //************************Deleting invoices**********************************
        public void deleteInvoice(int invoiceNum, int invoiceSubNum)
        {
            //Step 1: Re-add the removed items back into inventory
            //Gathering items in the invoice
            List<Items> itemsToAdd = getItemsToReAdd(invoiceNum, invoiceSubNum);
            foreach(Items i in itemsToAdd)
            {
                //Checks what type of item the item from the invoice is
                bool isClub = checkInClub(i.sku);
                bool isClothing = checkInClothing(i.sku);
                bool isAccessorie = checkInAccessories(i.sku);
                int previousQuantity = 0;
                if(isClub == true)
                {
                    //Gets the current quantity in the inventory
                    previousQuantity = getQuantity(i.sku, "clubs");
                    //Updates the current inventory's quantity by adding the previous quantity with the quantity in the invoice
                    reAddingItems(i.sku, previousQuantity + i.quantity, "clubs");
                }
                else if(isClothing == true)
                {
                    previousQuantity = getQuantity(i.sku, "clothing");
                    reAddingItems(i.sku, previousQuantity + i.quantity, "clothing");
                }
                else if(isAccessorie == true)
                {
                    previousQuantity = getQuantity(i.sku, "accessories");
                    reAddingItems(i.sku, previousQuantity + i.quantity, "accessories");
                }
            }
            //Step 2: Remove MOPS 
            deleteInvoiceMOP(invoiceNum, invoiceSubNum);
            //Step 3: Remove Items
            deleteInvoiceItem(invoiceNum, invoiceSubNum);
            //Step 4: Remove the overall Invoice
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "delete tbl_invoice where invoiceNum = @invoiceNum and invoiceSubNum = @invoiceSubNum;";
            cmd.Parameters.AddWithValue("invoiceNum", invoiceNum);
            cmd.Parameters.AddWithValue("invoiceSubNum", invoiceSubNum);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            conn.Close();
        }
        public List<Items> getItemsToReAdd(int invoiceNum, int invoiceSubNum)
        {
            List<Items> items = new List<Items>();
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select sku, itemQuantity from tbl_invoiceItem where invoiceNum = @invoiceNum and invoiceSubNum = @invoiceSubNum;";
            cmd.Parameters.AddWithValue("invoiceNum", invoiceNum);
            cmd.Parameters.AddWithValue("invoiceSubNum", invoiceSubNum);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                items.Add(new Items(
                    Convert.ToInt32(reader["sku"]),
                    Convert.ToInt32(reader["itemQuantity"])));
            }
            conn.Close();
            return items;
        }
        public bool checkInClub(int sku)
        {
            bool isClub = false;
            //New command
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select count(*) from tbl_clubs where sku = " + sku;
            //Declare and open connection
            cmd.Connection = con;
            con.Open();
            //Execute search
            cmd.ExecuteNonQuery();
            int itemExists = (int)cmd.ExecuteScalar();

            //If item exists
            if (itemExists > 0)
            {
                isClub = true;
            }
            //If item doesn't exist
            else
            {
                isClub = false;
            }
            //Closing
            con.Close();
            return isClub;
        }
        public bool checkInClothing(int sku)
        {
            bool isClothing = false;
            //New command
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select count(*) from tbl_clothing where sku = " + sku;
            //Declare and open connection
            cmd.Connection = con;
            con.Open();
            //Execute search
            cmd.ExecuteNonQuery();
            int itemExists = (int)cmd.ExecuteScalar();

            //If item exists
            if (itemExists > 0)
            {
                isClothing = true;
            }
            //If item doesn't exist
            else
            {
                isClothing = false;
            }
            //Closing
            con.Close();
            return isClothing;
        }
        public bool checkInAccessories(int sku)
        {
            bool isAccessorie= false;
            //New command
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select count(*) from tbl_accessories where sku = " + sku;
            //Declare and open connection
            cmd.Connection = con;
            con.Open();
            //Execute search
            cmd.ExecuteNonQuery();
            int itemExists = (int)cmd.ExecuteScalar();

            //If item exists
            if (itemExists > 0)
            {
                isAccessorie = true;
            }
            //If item doesn't exist
            else
            {
                isAccessorie = false;
            }
            //Closing
            con.Close();
            return isAccessorie;
        }
        public int getQuantity(int sku, string table)
        {
            int quantity = 0;
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select Max(quantity) as itemQuantity from tbl_"+table+" Where sku = @sku;";
            cmd.Parameters.AddWithValue("sku", sku);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (reader["itemQuantity"] == DBNull.Value)
                {
                    quantity = 0;
                }
                else
                {
                    quantity = Convert.ToInt32(reader["itemQuantity"]);
                }
            }
            conn.Close();
            return quantity;
        }
        public void reAddingItems(int sku, int quantity, string table)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "update tbl_"+table+" set quantity = @quantity where sku = @sku;";
            cmd.Parameters.AddWithValue("sku", sku);
            cmd.Parameters.AddWithValue("quantity", quantity);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            conn.Close();
        }
        public void deleteInvoiceMOP(int invoiceNum, int invoiceSubNum)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "delete tbl_invoiceMOP where invoiceNum = @invoiceNum and invoiceSubNum = @invoiceSubNum;";
            cmd.Parameters.AddWithValue("invoiceNum", invoiceNum);
            cmd.Parameters.AddWithValue("invoiceSubNum", invoiceSubNum);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            conn.Close();
        }
        public void deleteInvoiceItem(int invoiceNum, int invoiceSubNum)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "delete tbl_invoiceItem where invoiceNum = @invoiceNum and invoiceSubNum = @invoiceSubNum;";
            cmd.Parameters.AddWithValue("invoiceNum", invoiceNum);
            cmd.Parameters.AddWithValue("invoiceSubNum", invoiceSubNum);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            conn.Close();


        }


    }
}