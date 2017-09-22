using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Configuration;
using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using System.Data;

namespace SweetSpotProShop
{
    //This class is used for way too much...
    public class ItemDataUtilities
    {
        private String connectionString;
        LocationManager lm = new LocationManager();
        DataTable table = new DataTable();

        //Connection String
        public ItemDataUtilities()
        {
            connectionString = ConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString;
        }
        //Return Model string created by Nathan and Tyler **getModelName
        public string modelType(int modelID)
        {
            //Variable to store the modelName
            string model = null;
            SqlConnection con = new SqlConnection(connectionString);
            using (var cmd = new SqlCommand("getModelName", con)) //Calling the SP   
            using (var da = new SqlDataAdapter(cmd))
            {
                //Adding the parameter
                cmd.Parameters.AddWithValue("@modelID", modelID);
                //Executing the SP
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(table);
            }
            foreach (DataRow row in table.Rows)
            {
                model = row["modelName"].ToString();
            }
            //Returns the model name
            return model;
        }
        //Return Brand string created by Nathan and Tyler **getBrandName
        public string brandType(int brandID)
        {
            //Variable to store the brandName
            string brandName = null;
            SqlConnection con = new SqlConnection(connectionString);
            using (var cmd = new SqlCommand("getBrandName", con)) //Calling the SP
            using (var da = new SqlDataAdapter(cmd))
            {
                //Adding the parameter
                cmd.Parameters.AddWithValue("@brandID", brandID);
                //Executing the SP
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(table);
            }
            foreach (DataRow row in table.Rows)
            {
                brandName = row["brandName"].ToString();
            }
            //Returns the brand name
            return brandName;
        }
        //Return Model Int created by Nathan and Tyler **getModelID
        public int modelName(string modelN)
        {
            //Variable to store the modelID
            int model = 0;
            SqlConnection con = new SqlConnection(connectionString);
            using (var cmd = new SqlCommand("getModelID", con)) //Calling the SP
            using (var da = new SqlDataAdapter(cmd))
            {
                //Adding the parameter
                cmd.Parameters.AddWithValue("@modelName", modelN);
                //Executing the SP
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(table);
            }
            foreach (DataRow row in table.Rows)
            {
                model = Convert.ToInt32(row["modelID"]);
            }
            //Returns the modelID 
            return model;
        }
        //Return Brand Int created by Nathan and Tyler **getBrandID
        public int brandName(string brandN)
        {
            //Variable to store the brandID
            int brandID = 0;
            SqlConnection con = new SqlConnection(connectionString);
            using (var cmd = new SqlCommand("getBrandID", con)) //Calling the SP
            using (var da = new SqlDataAdapter(cmd))
            {
                //Adding the parameter
                cmd.Parameters.AddWithValue("@brandName", brandN);
                //Executing teh SP
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(table);
            }
            foreach (DataRow row in table.Rows)
            {
                brandID = Convert.ToInt32(row["brandID"]);
            }
            //Returns the brandID
            return brandID;
        }
        //Return Model string created by Nathan and Tyler **getItemTypeDescritpion
        public string typeName(int typeNum)
        {
            //Creating a variable to store the typeDescription
            string typeDesc = null;
            SqlConnection con = new SqlConnection(connectionString);
            using (var cmd = new SqlCommand("getItemTypeDescription", con))
            using (var da = new SqlDataAdapter(cmd))
            {
                //Adding the parameter
                cmd.Parameters.AddWithValue("@typeID", typeNum);
                //Executing the SP
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(table);
            }
            foreach (DataRow row in table.Rows)
            {
                typeDesc = row["typeDescription"].ToString();
            }
            //Returns the item type description
            return typeDesc;
        }
        //Insert new brand name. Returns new brandID
        public int insertBrand(string brandName)
        {
            //Creating a variable to store the new brandID
            int brandID = 0;
            SqlConnection con = new SqlConnection(connectionString);
            using (var cmd = new SqlCommand("insertBrand", con))
            using (var da = new SqlDataAdapter(cmd))
            {
                //Adding the parameter
                cmd.Parameters.AddWithValue("@brandName", brandName);
                //Executing the SP
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(table);
            }
            foreach (DataRow row in table.Rows)
            {
                brandID = Convert.ToInt32(row["brandID"]);
            }
            //Returns the brandID of the newly added brand
            return brandID;
        }
        //Insert new model name. return new modelID
        public int insertModel(string modelName)
        {
            //Creating a variable to store the new modelID
            int modelID = 0;
            SqlConnection con = new SqlConnection(connectionString);
            using (var cmd = new SqlCommand("insertModel", con))
            using (var da = new SqlDataAdapter(cmd))
            {
                //Adding the paramter
                cmd.Parameters.AddWithValue("@brandName", modelName);
                //Executing the SP
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(table);
            }
            foreach (DataRow row in table.Rows)
            {
                modelID = Convert.ToInt32(row["brandID"]);
            }
            //Returns the modelID of the newly added model
            return modelID;
        }


        //***NOT USED
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
        //NOT USED***


        //Return Club Type ID
        public int getClubTypeID(string typeName)
        {
            //Creating a variable to store the typeID
            int typeID = 0;
            SqlConnection con = new SqlConnection(connectionString);
            using (var cmd = new SqlCommand("getClubTypeID", con))
            using (var da = new SqlDataAdapter(cmd))
            {
                //Adding the parameter
                cmd.Parameters.AddWithValue("@typeName", typeName);
                //Executing the SP
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(table);
            }
            foreach (DataRow row in table.Rows)
            {
                typeID = Convert.ToInt32(row["typeID"]);
            }
            return typeID;
        }
        //Return Club Type Name
        public string getClubTypeName(int typeID)
        {
            //Creating a variable to store the results
            string typeName = null;
            SqlConnection con = new SqlConnection(connectionString);
            using (var cmd = new SqlCommand("getClubTypeDesc", con))
            using (var da = new SqlDataAdapter(cmd))
            {
                //Adding the parameter
                cmd.Parameters.AddWithValue("@typeID", typeID);
                //Executing the SP
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(table);
            }
            foreach (DataRow row in table.Rows)
            {
                typeName = row["typeName"].ToString();
            }
            //Returns the name of the club type
            return typeName;
        }
        //Adding items to the Cart class. Totally not stolen by Tickles
        public Cart addingToCart(Object o)
        {
            Cart ca = new Cart();
            //Checks if the item is a club
            if (o is Clubs)
            {
                //Converts the object to type club
                Clubs c = o as Clubs;
                ca.sku = c.sku;
                //Creates the description of the item for the cart
                ca.description = brandType(c.brandID) + " " + modelType(c.modelID) + " " +
                    c.clubSpec + " " + c.clubType + " " + c.shaftSpec + " " + c.shaftFlex + " " + c.dexterity;
                ca.price = c.price;
                ca.cost = c.cost;
                ca.returnAmount = 0;
                ca.typeID = c.typeID;
            }
            //Checks if the item is an accessory
            else if (o is Accessories)
            {
                Accessories a = o as Accessories;
                ca.sku = a.sku;
                ca.description = brandType(a.brandID) + " " + a.accessoryType + " " + a.size + " " + a.colour;
                ca.price = a.price;
                ca.cost = a.cost;
                ca.returnAmount = 0;
                ca.typeID = a.typeID;
            }
            //Checks if the item is clothing
            else if (o is Clothing)
            {
                Clothing cl = o as Clothing;
                ca.sku = cl.sku;
                ca.description = brandType(cl.brandID) + " " + cl.size + " " + cl.colour + " " + cl.gender + " " + cl.style;
                ca.price = cl.price;
                ca.cost = cl.cost;
                ca.returnAmount = 0;
                ca.typeID = cl.typeID;
            }
            ca.quantity = 1;
            //Returns the item in the form of a cart
            return ca;
        }
        //Converts a dbnull value to a string
        public string ConvertDBNullToString(Object o)
        {
            if (o is DBNull)
                o = "";

            return o.ToString();

        }
        //Converts a dbnull value to a double
        public double ConvertDBNullToDouble(Object o)
        {
            double dbl = 0.0;
            if (o is DBNull)
                dbl = 0.0;
            else
                dbl = Convert.ToDouble(o);

            return dbl;

        }
        //Returns the GST of a province
        public double GetTaxRates(int ProId)
        {
            //Create variable to store the results
            double tax = 0;
            SqlConnection con = new SqlConnection(connectionString);
            using (var cmd = new SqlCommand("getGSTTaxRates", con))
            using (var da = new SqlDataAdapter(cmd))
            {
                //Adding the parameters
                cmd.Parameters.AddWithValue("@ProID", ProId);
                //Executing the SP
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(table);
            }
            foreach (DataRow row in table.Rows)
            {
                tax = Convert.ToDouble(row["GST"]);
            }
            //Returns the GST rate
            return tax;
        }
        //Returns the PST of a province
        public double GetPSTTax(int ProId)
        {
            //Create variable to store the results
            double tax = 0;
            SqlConnection con = new SqlConnection(connectionString);
            using (var cmd = new SqlCommand("getPSTTaxRates", con))
            using (var da = new SqlDataAdapter(cmd))
            {
                //Adding the parameters
                cmd.Parameters.AddWithValue("@ProID", ProId);
                //Executing the SP
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(table);
            }
            foreach (DataRow row in table.Rows)
            {
                tax = Convert.ToDouble(row["PST"]);
            }
            //Returns the GST rate
            return tax;
        }
        //Populating gridView on Inventory Search button in Sales Cart with location **NOT USED
        public List<Items> getItemByID(Int32 ItemNumber, string loc)
        {
            //Loops through the database and adds items with the matching sku to a list of type item
            List<Items> items = new List<Items>();
            Items i = new Items();
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            int intLocation = lm.locationIDfromCity(loc);
            cmd.Connection = conn;
            conn.Open();
            //Removed location because client did not want
            cmd.CommandText = "Select sku, quantity, brandID, accessoryType, size, colour, price, cost From tbl_accessories Where SKU = @skuAcc";
            cmd.Parameters.AddWithValue("skuAcc", ItemNumber);
            //cmd.Parameters.AddWithValue("locationID", intLocation);

            SqlDataReader readerAcc = cmd.ExecuteReader();
            while (readerAcc.Read())
            {

                i = new Items(Convert.ToInt32(readerAcc["sku"]), brandType(Convert.ToInt32(readerAcc["brandID"])) + " " + readerAcc["accessoryType"].ToString()
                    + " " + readerAcc["size"].ToString() + " " + readerAcc["colour"].ToString(),
                    Convert.ToInt32(readerAcc["quantity"]), Convert.ToDouble(readerAcc["price"]),
                    Convert.ToDouble(readerAcc["cost"]));

            }
            if (!readerAcc.HasRows)
            {
                readerAcc.Close();
                cmd.CommandText = "Select sku, brandID, modelID, clubType, clubSpec, shaftSpec, shaftFlex," +
                    " numberOfClubs, dexterity, quantity, price, cost From tbl_clubs Where SKU = @skuClubs";
                cmd.Parameters.AddWithValue("skuClubs", ItemNumber);
                //cmd.Parameters.AddWithValue("locationIDclubs", intLocation);
                SqlDataReader readerClubs = cmd.ExecuteReader();
                while (readerClubs.Read())
                {
                    i = new Items(Convert.ToInt32(readerClubs["sku"]),

                        brandType(Convert.ToInt32(readerClubs["brandID"]))
                        + " " + modelType(Convert.ToInt32(readerClubs["modelID"])) + " " + readerClubs["clubSpec"].ToString()
                        + " " + readerClubs["clubType"].ToString() + " " + readerClubs["shaftSpec"].ToString() + " "
                        + readerClubs["shaftFlex"].ToString() + " " + readerClubs["dexterity"],

                        Convert.ToInt32(readerClubs["quantity"]), Convert.ToDouble(readerClubs["price"]),
                        Convert.ToDouble(readerClubs["cost"]));

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
        //Populating gridView on Inventory Search button for all locations
        public List<Items> getItemByID(Int32 ItemNumber)
        {
            //Loops through the database searching for items that match sku's with the search

            //Adds the items that are found to a list of type item
            List<Items> items = new List<Items>();
            Items i = new Items();

            SqlConnection con = new SqlConnection(connectionString);

            //Accessories
            using (var cmd = new SqlCommand("getAccessoryFromSku", con))
            using (var da = new SqlDataAdapter(cmd))
            {
                //Adding the parameters
                cmd.Parameters.AddWithValue("@skuAcc", ItemNumber);
                //Executing the SP
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(table);
            }
            if (table.Rows.Count != 0)
            {
                DataRow row = table.Rows[0];
                //If an item is found, creating a new "item" with the accessory information
                i = new Items(Convert.ToInt32(row["sku"]),
                        //Description
                        brandType(Convert.ToInt32(row["brandID"])) + " " +
                        row["size"].ToString() + " " +
                        row["colour"].ToString(),
                        //End of Description
                        Convert.ToInt32(row["quantity"]), Convert.ToDouble(row["price"]),
                        Convert.ToDouble(row["cost"]), Convert.ToInt32(row["typeID"]),
                        lm.locationName(Convert.ToInt32(row["locationID"])));

            }
            else
            {
                using (var cmd = new SqlCommand("getClothingFromSku", con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    //Adding the parameters
                    cmd.Parameters.AddWithValue("@skuClothing", ItemNumber);
                    //Executing the SP
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(table);
                }
                if (table.Rows.Count != 0)
                {
                    DataRow row = table.Rows[0];
                    //If an item is found, creating a new "item" with the clothing information
                    i = new Items(Convert.ToInt32(row["sku"]),
                            //Description
                            brandType(Convert.ToInt32(row["brandID"])) + " " +
                            row["size"].ToString() + " " +
                            row["colour"].ToString() + " " +
                            row["gender"].ToString() + " " +
                            row["style"].ToString(),
                            //End of Description
                            Convert.ToInt32(row["quantity"]), Convert.ToDouble(row["price"]),
                            Convert.ToDouble(row["cost"]), Convert.ToInt32(row["typeID"]),
                            lm.locationName(Convert.ToInt32(row["locationID"])));
                }
                else
                {
                    using (var cmd = new SqlCommand("getClubFromSku", con))
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        //Adding the parameters
                        cmd.Parameters.AddWithValue("@skuClubs", ItemNumber);
                        //Executing the SP
                        cmd.CommandType = CommandType.StoredProcedure;
                        da.Fill(table);
                    }
                    if (table.Rows.Count != 0)
                    {
                        DataRow row = table.Rows[0];
                        //If an item is found, creating a new "item" with the club information
                        i = new Items(Convert.ToInt32(row["sku"]),
                            //Description
                            brandType(Convert.ToInt32(row["brandID"])) + " " +
                            modelType(Convert.ToInt32(row["modelID"])) + " " +
                            row["clubSpec"].ToString() + " " +
                            row["clubType"].ToString() + " " +
                            row["shaftSpec"].ToString() + " " +
                            row["shaftFlex"].ToString() + " " +
                            row["dexterity"].ToString(),
                            //End of Description
                            Convert.ToInt32(row["quantity"]),
                            Convert.ToDouble(row["price"]), Convert.ToDouble(row["cost"]),
                            Convert.ToInt32(row["typeID"]), lm.locationName(Convert.ToInt32(row["locationID"])));
                    }
                }
            }
            //If the sku is greater than 0, add the item to the list
            if (i.sku > 0)
            {
                //Adding the item to the list
                items.Add(i);
            }
            //conn.Close();
            //Returns a list of any items that are found
            return items;
        }
        //Being used now to return qty to validate if sku can be added to cart.
        public int getquantity(int sku, int typeID)
        {
            //Variable to store quantity 
            int itemQTY = 0;
            SqlConnection con = new SqlConnection(connectionString);
            //Accessories
            using (var cmd = new SqlCommand("getAccessoryQuantity", con))
            using (var da = new SqlDataAdapter(cmd))
            {
                //Adding the parameters
                cmd.Parameters.AddWithValue("@skuacces", sku);
                //Executing the SP
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(table);
            }
            if (table.Rows.Count != 0)
            {
                DataRow row = table.Rows[0];
                itemQTY = Convert.ToInt32(row["quantity"]);
            }
            else
            {
                //Clothing
                using (var cmd = new SqlCommand("getClothingQuantity", con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    //Adding the parameters
                    cmd.Parameters.AddWithValue("@skucloth", sku);
                    //Executing the SP
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(table);
                }
                if (table.Rows.Count != 0)
                {
                    DataRow row = table.Rows[0];
                    itemQTY = Convert.ToInt32(row["quantity"]);
                }
                else
                {
                    //Clubs
                    using (var cmd = new SqlCommand("getClubQuantity", con))
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        //Adding the parameters
                        cmd.Parameters.AddWithValue("@sku", sku);
                        //Executing the SP
                        cmd.CommandType = CommandType.StoredProcedure;
                        da.Fill(table);
                    }
                    if (table.Rows.Count != 0)
                    {
                        DataRow row = table.Rows[0];
                        itemQTY = Convert.ToInt32(row["quantity"]);
                    }
                }
            }
            //Returns the quantity of the searched item
            return itemQTY;
        }
        //This method updates an item with its new quantity
        public void removeQTYfromInventoryWithSKU(int sku, int typeID, int remainingQTY)
        {
            //Connection string
            SqlConnection con = new SqlConnection(connectionString);
            if (typeID == 1) //Clubs
            {
                using (var cmd = new SqlCommand("updateClubQuantity", con))
                {
                    //Adding the parameters
                    cmd.Parameters.AddWithValue("@sku", sku);
                    cmd.Parameters.AddWithValue("@quantity", remainingQTY);
                    cmd.Parameters.AddWithValue("@typeID", typeID);
                    //Executing the SP
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            else if (typeID == 2) //Accessories
            {
                using (var cmd = new SqlCommand("updateAccessoryQuantity", con))
                {
                    //Adding the parameters
                    cmd.Parameters.AddWithValue("@sku", sku);
                    cmd.Parameters.AddWithValue("@quantity", remainingQTY);
                    cmd.Parameters.AddWithValue("@typeID", typeID);
                    //Executing the SP
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            else if (typeID == 3) //Clothing
            {
                using (var cmd = new SqlCommand("updateClothingQuantity", con))
                {
                    //Adding the parameters
                    cmd.Parameters.AddWithValue("@sku", sku);
                    cmd.Parameters.AddWithValue("@quantity", remainingQTY);
                    cmd.Parameters.AddWithValue("@typeID", typeID);
                    //Executing the SP
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        //This method updates an item with its new quantity
        public void updateQuantity(int sku, int typeID, int quantity)
        {
            //Connection string
            SqlConnection con = new SqlConnection(connectionString);
            if (typeID == 1) //Clubs
            {
                using (var cmd = new SqlCommand("updateClubQuantity", con))
                {
                    //Adding the parameters
                    cmd.Parameters.AddWithValue("@sku", sku);
                    cmd.Parameters.AddWithValue("@quantity", quantity);
                    cmd.Parameters.AddWithValue("@typeID", typeID);
                    //Executing the SP
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            else if (typeID == 2) //Accessories
            {
                using (var cmd = new SqlCommand("updateAccessoryQuantity", con))
                {
                    //Adding the parameters
                    cmd.Parameters.AddWithValue("@sku", sku);
                    cmd.Parameters.AddWithValue("@quantity", quantity);
                    cmd.Parameters.AddWithValue("@typeID", typeID);
                    //Executing the SP
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            else if (typeID == 3) //Clothing
            {
                using (var cmd = new SqlCommand("updateClothingQuantity", con))
                {
                    //Adding the parameters
                    cmd.Parameters.AddWithValue("@sku", sku);
                    cmd.Parameters.AddWithValue("@quantity", quantity);
                    cmd.Parameters.AddWithValue("@typeID", typeID);
                    //Executing the SP
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        //Reserve trade-in sku
        public int reserveTradeInSKu(int loc)
        {
            int tradeInSkuDisplay = 0;
            //Grabs the trade in sku
            tradeInSkuDisplay = tradeInSku(loc);
            int[] range = new int[2];
            //Returns the range for the trade in sku
            range = tradeInSkuRange(loc);
            SqlConnection con = new SqlConnection(connectionString);
            using (var cmd = new SqlCommand("reserveTradeInSku", con))
            {
                //Adding the parameters
                cmd.Parameters.AddWithValue("@sku", tradeInSkuDisplay);
                cmd.Parameters.AddWithValue("@locationID", loc);
                //Executing the SP
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            //Returns the trade in items display sku
            return tradeInSkuDisplay;
        }
        //Grabbing trade-in sku
        public int tradeInSku(int location)
        {
            int sku = 0;
            int[] range = new int[2];
            //Variable for the max sku
            int maxSku = 0;
            //Returns the range for the trade in sku
            range = tradeInSkuRange(location);
            SqlConnection con = new SqlConnection(connectionString);
            using (var cmd = new SqlCommand("getTradeInMaxSku", con))
            using (var da = new SqlDataAdapter(cmd))
            {
                //Adding the parameters
                cmd.Parameters.AddWithValue("@locationID", location);
                //Executing the SP
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(table);
                DataRow row = table.Rows[1];
                maxSku = Convert.ToInt32(row["maxsku"]);
            }

            //if (table.Rows.Count != 0)
            //{

            //}
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

            //Returns the sku that will be used
            return sku;
        }
        //Finds and returns an array containing the upper and lower range for the trade in skus
        public int[] tradeInSkuRange(int location)
        {
            int[] range = new int[2];
            int upper = 0;
            int lower = 0;

            SqlConnection con = new SqlConnection(connectionString);
            using (var cmd = new SqlCommand("getTradeInSkuRange", con))
            using (var da = new SqlDataAdapter(cmd))
            {
                //Adding the parameters
                cmd.Parameters.AddWithValue("@locationID", location);
                //Executing the SP
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(table);
            }
            if (table.Rows.Count != 0)
            {
                DataRow row = table.Rows[0];
                upper = Convert.ToInt32(row["skuStopAt"]);
                lower = Convert.ToInt32(row["skuStartAt"]);
            }
            range[0] = lower;
            range[1] = upper;
            //Returns the range
            return range;
        }
        //Adding tradein item 
        public void addTradeInItem(Clubs tradeInItem, int sku, int loc)
        {
            //This method addes the trade in item to the tempTradeInCartSKus table
            SqlConnection con = new SqlConnection(connectionString);
            if (tradeInItem.itemlocation == 0)
            { tradeInItem.itemlocation = 1; }
            using (var cmd = new SqlCommand("updateTradeInItem", con))
            {
                //Adding the parameters
                cmd.Parameters.AddWithValue("sku", sku);
                cmd.Parameters.AddWithValue("brandID", tradeInItem.brandID);
                cmd.Parameters.AddWithValue("modelID", tradeInItem.modelID);
                cmd.Parameters.AddWithValue("clubType", tradeInItem.clubType);
                cmd.Parameters.AddWithValue("shaft", tradeInItem.shaft);
                cmd.Parameters.AddWithValue("numberOfClubs", tradeInItem.numberOfClubs);
                cmd.Parameters.AddWithValue("premium", tradeInItem.premium);
                cmd.Parameters.AddWithValue("cost", tradeInItem.cost);
                cmd.Parameters.AddWithValue("price", tradeInItem.price);
                cmd.Parameters.AddWithValue("quantity", tradeInItem.quantity);
                cmd.Parameters.AddWithValue("clubSpec", tradeInItem.clubSpec);
                cmd.Parameters.AddWithValue("shaftSpec", tradeInItem.shaftSpec);
                cmd.Parameters.AddWithValue("shaftFlex", tradeInItem.shaftFlex);
                cmd.Parameters.AddWithValue("dexterity", tradeInItem.dexterity);
                cmd.Parameters.AddWithValue("typeID", tradeInItem.typeID);
                cmd.Parameters.AddWithValue("locationID", loc);
                cmd.Parameters.AddWithValue("used", tradeInItem.used);
                cmd.Parameters.AddWithValue("comments", tradeInItem.comments);
                //Executing the SP
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        //Sending all of the invoice information to the database 
        public void mainInvoice(CheckoutManager ckm, List<Cart> cart, List<Checkout> mops, Customer c, Employee e, int transactionType, string invoiceNumber, string comments)
        {

            string locationInitial = invoiceNumber.Split('-')[0];

            ////Step 1: Find next invoice number 
            //int nextInvoiceNum = getNextInvoiceNum();
            int nextInvoiceNum = Convert.ToInt32(invoiceNumber.Split('-')[1]);

            ////Step 2: Find the invoice sub number
            int nextInvoiceSubNum = 0;
            //If the transaction is a sale
            if (transactionType == 1)
            {
                nextInvoiceSubNum = Convert.ToInt32(invoiceNumber.Split('-')[2]);
            }
            //If the transaction is a return
            else if (transactionType == 2)
            {
                //Gets the next sub num
                nextInvoiceSubNum = getNextInvoiceSubNum(nextInvoiceNum);
            }

            //Step 3: Get date and time
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string time = DateTime.Now.ToString("HH:mm:ss");

            //Step 4: Insert all relevent info into the mainInvoice table
            //invoiceNum, invoiceSubNum, invoiceDate, invoiceTime, custID, empID, locationID, subTotal, discountAmount, tradeinAmount, governmentTax, provincialTax, balanceDue, transactionType, comments
            SqlConnection con = new SqlConnection(connectionString);
            using (var cmd = new SqlCommand("insertInvoice", con))
            {
                //Adding the parameters
                cmd.Parameters.AddWithValue("invoiceNum", nextInvoiceNum);
                cmd.Parameters.AddWithValue("invoiceSubNum", nextInvoiceSubNum);
                cmd.Parameters.AddWithValue("invoiceDate", date);
                cmd.Parameters.AddWithValue("invoiceTime", time);
                cmd.Parameters.AddWithValue("custID", c.customerId);
                cmd.Parameters.AddWithValue("empID", e.employeeID);
                cmd.Parameters.AddWithValue("locationID", e.locationID);
                cmd.Parameters.AddWithValue("subTotal", ckm.dblSubTotal);
                cmd.Parameters.AddWithValue("shippingAmount", ckm.dblShipping);
                cmd.Parameters.AddWithValue("discountAmount", ckm.dblDiscounts);
                cmd.Parameters.AddWithValue("tradeinAmount", ckm.dblTradeIn);
                double gTax = 0;
                //If the GST is included, set it's value to the checkoutmanager GST value otherwise it stays at 0
                if (ckm.blGst) { gTax = ckm.dblGst; }
                cmd.Parameters.AddWithValue("governmentTax", gTax);
                double pTax = 0;
                //If the GST is included, set it's value to the checkoutmanager GST value otherwise it stays at 0
                if (ckm.blPst) { pTax = ckm.dblPst; }
                cmd.Parameters.AddWithValue("provincialTax", pTax);
                cmd.Parameters.AddWithValue("balanceDue", ckm.dblBalanceDue);
                cmd.Parameters.AddWithValue("transactionType", transactionType);
                cmd.Parameters.AddWithValue("comments", comments);
                //Executing the SP
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            //Step 5: Insert each item into the invoiceItem table
            //invoiceNum, invoiceSubNum, sku, itemQuantity, itemCost, itemPrice, itemDiscount, Percentage
            string tbl = "";
            //If it is a sale, use tbl_invoiceItem
            if (transactionType == 1)
            {
                tbl = "tbl_invoiceItem";
            }
            //If it is a return, use tbl_invoiceItemReturns
            else if (transactionType == 2)
            {
                tbl = "tbl_invoiceItemReturns";
            }
            //Loops through the cart to look at the items
            foreach (Cart item in cart)
            {
                int percentage = 0;
                if (item.percentage)
                { percentage = 1; }
                else
                { percentage = 0; }
                using (var cmd = new SqlCommand("insertInvoiceItem", con))
                {
                    //Adding the parameters
                    cmd.Parameters.AddWithValue("invoiceNum", nextInvoiceNum);
                    cmd.Parameters.AddWithValue("invoiceSubNum", nextInvoiceSubNum);
                    cmd.Parameters.AddWithValue("sku", item.sku);
                    cmd.Parameters.AddWithValue("quantity", item.quantity);
                    cmd.Parameters.AddWithValue("cost", item.cost);
                    cmd.Parameters.AddWithValue("price", item.price);
                    cmd.Parameters.AddWithValue("discount", item.discount);
                    cmd.Parameters.AddWithValue("returnAmount", item.returnAmount);
                    cmd.Parameters.AddWithValue("percentage", percentage);
                    //Executing the SP
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            //Step 6: Insert each MOP into the invoiceMOP table
            //ID(autoincrementing), invoiceNum, invoiceSubNum, mopID, amountPaid
            //Loops through the checkout to get the mops
            foreach (Checkout mop in mops)
            {
                using (var cmd = new SqlCommand("insertInvoiceMop", con))
                {
                    //Adding the parameters
                    cmd.Parameters.AddWithValue("invoiceNum", nextInvoiceNum);
                    cmd.Parameters.AddWithValue("invoiceSubNum", nextInvoiceSubNum);
                    cmd.Parameters.AddWithValue("methodOfPayment", mop.methodOfPayment);
                    cmd.Parameters.AddWithValue("amountPaid", mop.amountPaid);
                    //Executing the SP
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        //Returns the max invoice num + 1
        public int getNextInvoiceNum()
        {
            int nextInvoiceNum = 0;
            SqlConnection con = new SqlConnection(connectionString);
            using (var cmd = new SqlCommand("getInvoiceMaxNum", con))
            using (var da = new SqlDataAdapter(cmd))
            {
                //Executing the SP
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(table);
                DataRow row = table.Rows[0];
                nextInvoiceNum = Convert.ToInt32(row["invoiceNum"]);
            }
            if (table.Rows.Count != 0)
            {
                //Take the found invoiceNum, and increment by 1 so there won't be a duplicate
                nextInvoiceNum = nextInvoiceNum + 1;
                //Creates the invoice with the next invoice num
                createInvoiceNum(nextInvoiceNum);
            }
            //Returns the next invoiceNum
            return nextInvoiceNum;
        }
        //Create  the newly found invoice number
        public void createInvoiceNum(int invNum)
        {
            SqlConnection con = new SqlConnection(connectionString);
            using (var cmd = new SqlCommand("reserveInvoiceNum", con))
            {
                //Adding the parameters
                cmd.Parameters.AddWithValue("invNum", invNum);
                //Executing the SP
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        //Returns the max invoice subNum
        public int getNextInvoiceSubNum(int invoiceNumber)
        {
            int invoiceSubNum = 0;
            SqlConnection con = new SqlConnection(connectionString);
            using (var cmd = new SqlCommand("getInvoiceMaxSubNum", con))
            using (var da = new SqlDataAdapter(cmd))
            {
                //Executing the SP
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(table);
                DataRow row = table.Rows[0];
                invoiceSubNum = Convert.ToInt32(row["invoiceNum"]);
            }
            if (table.Rows.Count != 0)
            {
                //Take the found invoiceNum, and increment by 1 so there won't be a duplicate
                invoiceSubNum = invoiceSubNum + 1;
            }
            //Return the invoice sub num
            return invoiceSubNum;
        }

        //**Recent changes made these methods not used
        //Adding items to the invoice 
        public void invoiceItem(string insert)
        {
            //This method works by executing the string that is passed in as a database query
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
            //This method works by executing the string that is passed in as a database query
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = insert;
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            conn.Close();
        }
        //Recent changes made these methods not used**

        //Returns the employee ID from a password
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
            //Returns the employee ID
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
                //If no sku is found, set it to 0
                if (reader["largestSku"] == DBNull.Value)
                {
                    maxSku = 0;
                }
                else
                {
                    //If a sku is found, increment by 1 to get the next largest sku
                    maxSku = Convert.ToInt32(reader["largestSku"]) + 1;
                }
            }
            conn.Close();
            //Returns the new largest sku
            return maxSku;
        }
        //Returns max sku from the skuNumber table based on itemType and directs code to store it
        public int maxSku(int itemType)
        {
            int maxSku = 0;
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select Max(sku) as largestSku from tbl_skuNumbers where itemType = @itemType;";
            cmd.Parameters.AddWithValue("@itemType", itemType);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                //If there is no sku found, set it to 0
                if (reader["largestSku"] == DBNull.Value)
                {
                    maxSku = 0;
                    //Stores the max sku along with its itemType
                    storeMaxSku(maxSku, itemType);
                }
                else
                {
                    //If a sku is found, increment it by 1
                    maxSku = Convert.ToInt32(reader["largestSku"]) + 1;
                    //Stores the new max sku along with its itemType
                    storeMaxSku(maxSku, itemType);
                }
            }
            conn.Close();
            //Returns the new max sku
            return maxSku;
        }
        //Stores the max sku in the skuNumber table
        public void storeMaxSku(int sku, int itemType)
        {
            //This method stores the max sku along with its item type
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "insert into tbl_skuNumbers values(@sku, @itemType);";
            cmd.Parameters.AddWithValue("@sku", sku);
            cmd.Parameters.AddWithValue("@itemType", itemType);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            conn.Close();
        }
        //************************Deleting invoices**********************************
        public void deleteInvoice(int invoiceNum, int invoiceSubNum, string deletionReason)
        {
            //Step 1: Re-add the removed items back into inventory
            //Gathering items in the invoice
            List<Items> itemsToAdd = getItemsToReAdd(invoiceNum, invoiceSubNum);
            foreach (Items i in itemsToAdd)
            {
                //Checks what type of item the item from the invoice is
                bool isClub = checkInClub(i.sku);
                bool isClothing = checkInClothing(i.sku);
                bool isAccessorie = checkInAccessories(i.sku);
                int previousQuantity = 0;
                if (isClub == true)
                {
                    //Gets the current quantity in the inventory
                    previousQuantity = getQuantity(i.sku, "clubs");
                    //Updates the current inventory's quantity by adding the previous quantity with the quantity in the invoice
                    reAddingItems(i.sku, previousQuantity + i.quantity, "clubs");
                }
                else if (isClothing == true)
                {
                    previousQuantity = getQuantity(i.sku, "clothing");
                    reAddingItems(i.sku, previousQuantity + i.quantity, "clothing");
                }
                else if (isAccessorie == true)
                {
                    previousQuantity = getQuantity(i.sku, "accessories");
                    reAddingItems(i.sku, previousQuantity + i.quantity, "accessories");
                }
            }
            //Step 2: Get invoice Data and transfer to the deleted invoice table
            getInvoiceData(invoiceNum, invoiceSubNum, deletionReason);
            getInvoiceItems(invoiceNum, invoiceSubNum);
            getInvoiceMOPs(invoiceNum, invoiceSubNum);


            //Step 3: Remove MOPS 
            deleteInvoiceMOP(invoiceNum, invoiceSubNum);
            //Step 4: Remove Items
            deleteInvoiceItem(invoiceNum, invoiceSubNum);
            //Step 5: Remove the overall Invoice
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
        //Returns a list of the items in the invoice to update their quantities
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
            //Returns the list of items
            return items;
        }
        //Looks in the club table for the sku
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
            //Returns a boolean to signal if it is or isn't a club
            return isClub;
        }
        //Looks in the clothing table for the sku
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
            //Returns a boolean to signal if it is or isn't clothing
            return isClothing;
        }
        //Looks in the accessories table for the sku
        public bool checkInAccessories(int sku)
        {
            bool isAccessorie = false;
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
            //Returns a boolean to signal if it is or isn't an accessory
            return isAccessorie;
        }
        //Gets the quantity of the item being returned
        public int getQuantity(int sku, string table)
        {
            int quantity = 0;
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select Max(quantity) as itemQuantity from tbl_" + table + " Where sku = @sku;";
            cmd.Parameters.AddWithValue("sku", sku);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                //If the item doesn't have a quantity, set to 0
                if (reader["itemQuantity"] == DBNull.Value)
                {
                    quantity = 0;
                }
                else
                {
                    //If the item has a quantity
                    quantity = Convert.ToInt32(reader["itemQuantity"]);
                }
            }
            conn.Close();
            //Returns the item's quantity
            return quantity;
        }
        //This method updates the item's quantity in the databse(clubs, accessories, clothing) by adding the returned items
        public void reAddingItems(int sku, int quantity, string table)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "update tbl_" + table + " set quantity = @quantity where sku = @sku;";
            cmd.Parameters.AddWithValue("sku", sku);
            cmd.Parameters.AddWithValue("quantity", quantity);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            conn.Close();
        }
        //This method gets the data from tbl_invoice and transfers it to the deletedInvoice table
        public void getInvoiceData(int invoiceNum, int invoiceSubNum, string deletionReason)
        {
            Invoice i = new Invoice();
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select * from tbl_invoice where invoiceNum = @invoiceNum and invoiceSubNum = @invoiceSubNum;";
            cmd.Parameters.AddWithValue("invoiceNum", invoiceNum);
            cmd.Parameters.AddWithValue("invoiceSubNum", invoiceSubNum);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                i = new Invoice(Convert.ToInt32(reader["invoiceNum"]), Convert.ToInt32(reader["invoiceSubNum"]), Convert.ToDateTime(reader["invoiceDate"]),
                    Convert.ToString(reader["invoiceTime"]), Convert.ToInt32(reader["custID"]), Convert.ToInt32(reader["empID"]),
                    Convert.ToInt32(reader["locationID"]), Convert.ToDouble(reader["subTotal"]), Convert.ToDouble(reader["shippingAmount"]),
                    Convert.ToDouble(reader["discountAmount"]), Convert.ToDouble(reader["tradeinAmount"]), Convert.ToDouble(reader["governmentTax"]),
                    Convert.ToDouble(reader["provincialTax"]), Convert.ToDouble(reader["balanceDue"]), Convert.ToInt32(reader["transactionType"]),
                    Convert.ToString(reader["comments"]));
            }
            conn.Close();
            //Sends the invoice and the deletion reason to the transfer method for invoices
            transferMainInvoice(i, deletionReason);
        }
        //This method gets the items in the invoice that is being deleted and transfers each item to the deleted invoice item table
        public void getInvoiceItems(int invoiceNum, int invoiceSubNum)
        {
            List<InvoiceItems> it = new List<InvoiceItems>();
            InvoiceItems inItem = new InvoiceItems();
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select * from tbl_invoiceItem where invoiceNum = @invoiceNum and invoiceSubNum = @invoiceSubNum;";
            cmd.Parameters.AddWithValue("invoiceNum", invoiceNum);
            cmd.Parameters.AddWithValue("invoiceSubNum", invoiceSubNum);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                inItem = new InvoiceItems(Convert.ToInt32(reader["invoiceNum"]), Convert.ToInt32(reader["invoiceSubNum"]),
                    Convert.ToInt32(reader["sku"]), Convert.ToInt32(reader["itemQuantity"]), Convert.ToDouble(reader["itemCost"]),
                    Convert.ToDouble(reader["itemPrice"]), Convert.ToDouble(reader["itemDiscount"]), Convert.ToBoolean(reader["percentage"]));
                //Adds the found item to a list of type InvoiceItems
                it.Add(inItem);
            }
            conn.Close();
            //Loops through each item found and transfers them to the deleted invoice item table
            foreach (InvoiceItems iItems in it)
            {
                //Sends the item to the transfer method for items
                transferInvoiceItem(iItems);
            }
        }
        //This method get the mops in the invoice that is being deleted and transfers each mop to the deleted invoice mop table
        public void getInvoiceMOPs(int invoiceNum, int invoiceSubNum)
        {
            List<InvoiceMOPs> im = new List<InvoiceMOPs>();
            InvoiceMOPs inMOPS = new InvoiceMOPs();
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select * from tbl_invoiceMOP where invoiceNum = @invoiceNum and invoiceSubNum = @invoiceSubNum;";
            cmd.Parameters.AddWithValue("invoiceNum", invoiceNum);
            cmd.Parameters.AddWithValue("invoiceSubNum", invoiceSubNum);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                inMOPS = new InvoiceMOPs(Convert.ToInt32(reader["ID"]), Convert.ToInt32(reader["invoiceNum"]), Convert.ToInt32(reader["invoiceSubNum"]),
                    Convert.ToString(reader["mopType"]), Convert.ToDouble(reader["amountPaid"]));
                //Adds the found mop to a list of type InvoiceMOPs
                im.Add(inMOPS);
            }
            conn.Close();
            //Loops through each mop found and transfers them to the deleted invoice mop table
            foreach (InvoiceMOPs iMOPS in im)
            {
                //Sends the mop to the transfer method for items
                transferInoviceMOP(iMOPS);
            }
        }
        //This method transfers the invoice that is being deleted to the deleted invoice table
        public void transferMainInvoice(Invoice i, string deletionReason)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText =
                "insert into tbl_deletedInvoice values( " +
                "@invoiceNum, " +
                "@invoiceSubNum, " +
                "@invoiceDate, " +
                "@invoiceTime, " +
                "@custID, " +
                "@empID, " +
                "@locationID, " +
                "@subtotal, " +
                "@shippingAmount, " +
                "@discountAmount, " +
                "@tradeinAmount, " +
                "@governmentTax, " +
                "@provincialTax, " +
                "@balanceDue, " +
                "@transactionType, " +
                "@comments, " +
                "@deletionReason);";
            cmd.Parameters.AddWithValue("invoiceNum", i.invoiceNum);
            cmd.Parameters.AddWithValue("invoiceSubNum", i.invoiceSub);
            cmd.Parameters.AddWithValue("invoiceDate", i.invoiceDate);
            cmd.Parameters.AddWithValue("invoiceTime", i.invoiceTime);
            cmd.Parameters.AddWithValue("custID", i.customerID);
            cmd.Parameters.AddWithValue("empID", i.employeeID);
            cmd.Parameters.AddWithValue("locationID", i.locationID);
            cmd.Parameters.AddWithValue("subTotal", i.subTotal);
            cmd.Parameters.AddWithValue("shippingAmount", i.shippingAmount);
            cmd.Parameters.AddWithValue("discountAmount", i.discountAmount);
            cmd.Parameters.AddWithValue("tradeinAmount", i.tradeinAmount);
            cmd.Parameters.AddWithValue("governmentTax", i.governmentTax);
            cmd.Parameters.AddWithValue("provincialTax", i.provincialTax);
            cmd.Parameters.AddWithValue("balanceDue", i.balanceDue);
            cmd.Parameters.AddWithValue("transactionType", i.transactionType);
            cmd.Parameters.AddWithValue("comments", i.comments);
            cmd.Parameters.AddWithValue("deletionReason", deletionReason);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            conn.Close();
        }
        //This method transfers the item from the deleted invoice to the deleted invoice items table
        public void transferInvoiceItem(InvoiceItems im)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText =
                "insert into tbl_deletedInvoiceItem values( " +
                "@invoiceNum, " +
                "@invoiceSubNum, " +
                "@sku, " +
                "@itemQuantity, " +
                "@itemCost, " +
                "@itemPrice, " +
                "@itemDiscount, " +
                "@percentage);";
            cmd.Parameters.AddWithValue("invoiceNum", im.invoiceNum);
            cmd.Parameters.AddWithValue("invoiceSubNum", im.invoiceSubNum);
            cmd.Parameters.AddWithValue("sku", im.sku);
            cmd.Parameters.AddWithValue("itemQuantity", im.itemQuantity);
            cmd.Parameters.AddWithValue("itemCost", im.itemCost);
            cmd.Parameters.AddWithValue("itemPrice", im.itemPrice);
            cmd.Parameters.AddWithValue("itemDiscount", im.itemDiscount);
            cmd.Parameters.AddWithValue("percentage", im.percentage);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            conn.Close();

        }
        //This method inserts the mops from the deleted invoice into the deleted invoice mops table
        public void transferInoviceMOP(InvoiceMOPs im)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText =
                "insert into tbl_deletedInvoiceMOP values( " +
                "@ID, " +
                "@invoiceNum, " +
                "@invoiceSubNum, " +
                "@mopType, " +
                "@amountPaid);";
            cmd.Parameters.AddWithValue("ID", im.id);
            cmd.Parameters.AddWithValue("invoiceNum", im.invoiceNum);
            cmd.Parameters.AddWithValue("invoiceSubNum", im.invoiceSubNum);
            cmd.Parameters.AddWithValue("mopType", im.mopType);
            cmd.Parameters.AddWithValue("amountPaid", im.amountPaid);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            conn.Close();
        }
        //This method deletes the mops that have an invoiceNum and subNum that belongs to the deleted invoice
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
        //This method deletes the items that have an invoiceNum and subNum that belongs to the deleted invoice
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