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
            DataRow row = table.Rows[0];
            brandID = Convert.ToInt32(row["brandID"]);
            
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
            SqlConnection con = new SqlConnection(connectionString);
            using (var cmd = new SqlCommand("getMaxSku", con))
            using (var da = new SqlDataAdapter(cmd))
            {
                //Adding the parameters
                cmd.Parameters.AddWithValue("@itemType", itemType);
                //Executing the SP
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(table);
                if (table.Rows.Count != 0)
                {
                    DataRow row = table.Rows[1];
                    maxSku = Convert.ToInt32(row["largestSku"]) + 1;
                    storeMaxSku(maxSku, itemType);

                }                
            }
            //Returns the new max sku
            return maxSku;
        }
        //Stores the max sku in the skuNumber table
        public void storeMaxSku(int sku, int itemType)
        {
            //This method stores the max sku along with its item type
            SqlConnection con = new SqlConnection(connectionString);
            using (var cmd = new SqlCommand("insertMaxSku", con))
            {
                //Adding the parameters
                cmd.Parameters.AddWithValue("@sku", sku);
                cmd.Parameters.AddWithValue("@itemType", itemType);
                //Executing the SP
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}