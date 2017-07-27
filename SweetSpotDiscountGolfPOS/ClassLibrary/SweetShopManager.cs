using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Web;
using System.Collections;
using System.Configuration;
using SweetSpotProShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;

namespace SweetShop
{
    public class SweetShopManager
    {
        private string connectionString;
        ItemDataUtilities idu = new ItemDataUtilities();
        LocationManager lm = new LocationManager();

        public SweetShopManager()
        {

            connectionString = ConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString;
        }

        /*******Customer Utilities************************************************************************************/

        //Retrieves Customer from search parameters Nathan and Tyler created
        public List<Customer> GetCustomerfromSearch(string searchField)
        {
            try
            {
                //Declares space for connection string and new command
                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                //cmd.CommandText = "Select * From tbl_customers Where (firstName + ' ' + lastName) Like '%@searchField1%' or (primaryPhoneINT + ' ' + secondaryPhoneINT) like '%@searchField2%' order by firstName asc";
                cmd.CommandText = "Select * From tbl_customers Where Concat(firstName,lastName) Like '%" + searchField + "%' or Concat(primaryPhoneINT,secondaryPhoneINT) like '%" + searchField + "%' order by firstName asc";
                //cmd.Parameters.AddWithValue("searchField1", searchField);
                //cmd.Parameters.AddWithValue("searchField2", searchField);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                //New List for Customer

                List<Customer> customer = new List<Customer>();

                //Begin reading
                while (reader.Read())
                {
                    Customer c = new Customer(Convert.ToInt32(reader["custID"]),
                        reader["firstName"].ToString(),
                        reader["lastName"].ToString(),
                        reader["primaryAddress"].ToString(),
                        reader["secondaryAddress"].ToString(),
                        reader["primaryPhoneINT"].ToString(),
                        reader["secondaryPhoneINT"].ToString(),
                        reader["billingAddress"].ToString(),
                        reader["email"].ToString(),
                        reader["city"].ToString(),
                        Convert.ToInt32(reader["provStateID"]),
                        Convert.ToInt32(reader["country"]),
                        reader["postZip"].ToString());

                    customer.Add(c);

                }

                con.Close();
                return customer;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        //Retreive Customer from custID Nathan and Tyler created
        public Customer GetCustomerbyCustomerNumber(int custNum)
        {
            try
            {
                //Declares space for connection string and new command
                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "Select * From tbl_customers Where custID = @custNum";
                cmd.Parameters.AddWithValue("custNum", custNum);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                //New Customer
                Customer customer = new Customer();

                //Begin reading
                while (reader.Read())
                {
                    Customer c = new Customer(Convert.ToInt32(reader["custID"]),
                        reader["firstName"].ToString(),
                        reader["lastName"].ToString(),
                        reader["primaryAddress"].ToString(),
                        reader["secondaryAddress"].ToString(),
                        reader["primaryPhoneINT"].ToString(),
                        reader["secondaryPhoneINT"].ToString(),
                        reader["billingAddress"].ToString(),
                        reader["email"].ToString(),
                        reader["city"].ToString(),
                        Convert.ToInt32(reader["provStateID"]),
                        Convert.ToInt32(reader["country"]),
                        reader["postZip"].ToString());

                    customer = c;
                }

                con.Close();
                return customer;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        //Add Customer Nathan and Tyler created
        public int addCustomer(Customer c)
        {
            //New command
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "Insert Into tbl_customers (firstName, lastName, primaryAddress,"
                + " secondaryAddress, primaryPhoneINT, secondaryPhoneINT, billingAddress, email,"
                + " city, provStateID, country, postZip) Values (@FirstName, @LastName, @primaryAddress,"
                + " @secondaryAddress, @primaryPhoneNumber, @secondaryPhoneNumber, @billingAddress,"
                + " @Email, @City, @Province, @Country, @PostalCode)";
            cmd.Parameters.AddWithValue("FirstName", c.firstName);
            cmd.Parameters.AddWithValue("LastName", c.lastName);
            cmd.Parameters.AddWithValue("primaryAddress", c.primaryAddress);
            cmd.Parameters.AddWithValue("secondaryAddress", c.secondaryAddress);
            cmd.Parameters.AddWithValue("billingAddress", c.billingAddress);
            cmd.Parameters.AddWithValue("City", c.city);
            cmd.Parameters.AddWithValue("PostalCode", c.postalCode);
            cmd.Parameters.AddWithValue("Province", c.province);
            cmd.Parameters.AddWithValue("Country", c.country);
            cmd.Parameters.AddWithValue("primaryPhoneNumber", c.primaryPhoneNumber);
            cmd.Parameters.AddWithValue("secondaryPhoneNumber", c.secondaryPhoneNumber);
            cmd.Parameters.AddWithValue("Email", c.email);
            //Declare and open connection
            cmd.Connection = con;
            con.Open();
            //Execute Insert
            cmd.ExecuteNonQuery();
            con.Close();

            return returnCustomerNumber(c);

        }

        //Nathan and Tyler created
        public int returnCustomerNumber(Customer c)
        {

            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "Select custID From tbl_customers Where firstName = @FirstName and lastName = @LastName and primaryAddress = @primaryAddress and secondaryAddress = @secondaryAddress and primaryPhoneINT = @primaryPhoneNumber and secondaryPhoneINT = @secondaryPhoneNumber and billingAddress = @BillingAddress and email = @Email and city = @City and provStateID = @Province and country = @Country and postZip = @PostalCode;";
            cmd.Parameters.AddWithValue("FirstName", c.firstName);
            cmd.Parameters.AddWithValue("LastName", c.lastName);
            cmd.Parameters.AddWithValue("primaryAddress", c.primaryAddress);
            cmd.Parameters.AddWithValue("secondaryAddress", c.secondaryAddress);
            cmd.Parameters.AddWithValue("primaryPhoneNumber", c.primaryPhoneNumber);
            cmd.Parameters.AddWithValue("secondaryPhoneNumber", c.secondaryPhoneNumber);
            cmd.Parameters.AddWithValue("BillingAddress", c.billingAddress);
            cmd.Parameters.AddWithValue("Email", c.email);
            cmd.Parameters.AddWithValue("City", c.city);
            cmd.Parameters.AddWithValue("Province", c.province);
            cmd.Parameters.AddWithValue("Country", c.country);
            cmd.Parameters.AddWithValue("PostalCode", c.postalCode);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            //New Customer number
            int custNum = 0;

            //Begin reading
            while (reader.Read())
            {
                custNum = Convert.ToInt32(reader["custID"]);
            }
            con.Close();
            return custNum;
        }

        //Update Customer Nathan and Tyler Created
        public void updateCustomer(Customer c)
        {
            //New command
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "Update tbl_customers Set firstName = @FirstName, lastName = @LastName, primaryAddress = @primaryAddress,"
                + " secondaryAddress = @secondaryAddress, primaryPhoneINT = @primaryPhoneNumber, secondaryPhoneINT = @secondaryPhoneNumber,"
                + " billingAddress = @BillingAddress, email = @Email, city = @City, provStateID = @Province, country = @Country,"
                + " postZip = @PostalCode Where custID = @CustomerID";
            cmd.Parameters.AddWithValue("CustomerID", c.customerId);
            cmd.Parameters.AddWithValue("FirstName", c.firstName);
            cmd.Parameters.AddWithValue("LastName", c.lastName);
            cmd.Parameters.AddWithValue("primaryAddress", c.primaryAddress);
            cmd.Parameters.AddWithValue("secondaryAddress", c.secondaryAddress);
            cmd.Parameters.AddWithValue("primaryPhoneNumber", c.primaryPhoneNumber);
            cmd.Parameters.AddWithValue("secondaryPhoneNumber", c.secondaryPhoneNumber);
            cmd.Parameters.AddWithValue("BillingAddress", c.billingAddress);
            cmd.Parameters.AddWithValue("Email", c.email);
            cmd.Parameters.AddWithValue("City", c.city);
            cmd.Parameters.AddWithValue("Province", c.province);
            cmd.Parameters.AddWithValue("Country", c.country);
            cmd.Parameters.AddWithValue("PostalCode", c.postalCode);

            //Declare and open connection
            cmd.Connection = con;
            con.Open();

            //Execute Update
            cmd.ExecuteNonQuery();
            con.Close();
        }

        /*******Item Utilities************************************************************************************/
        public List<Items> returnSearchFromAllThreeItemSets(string searchedText, string loc)
        {
            List<Items> searchClubs = new List<Items>();
            List<Items> searchClothing = new List<Items>();
            List<Items> searchAccessories = new List<Items>();
            List<Items> searchedItems = new List<Items>();
            searchClubs = GetItemfromSearch(searchedText, "Clubs", loc);
            searchClothing = GetItemfromSearch(searchedText, "Accessories", loc);
            searchAccessories = GetItemfromSearch(searchedText, "Clothing", loc);
            foreach (var item in searchClubs)
            {
                searchedItems.Add(item);
            }
            foreach (var item in searchClothing)
            {
                searchedItems.Add(item);
            }
            foreach (var item in searchAccessories)
            {
                searchedItems.Add(item);
            }
            return searchedItems;

        }

        //Robust search through inventory Nathan and Tyler created for specific location
        public List<Items> GetItemfromSearch(string itemSearched, string itemType, string loc)
        {
            ArrayList strText = new ArrayList();
            int intLocation = lm.locationIDfromCity(loc);
            int numFields = itemSearched.Split(' ').Length;

            if (numFields > 1)
            {
                for (int i = 0; i < numFields; i++)
                {
                    strText.Add(itemSearched.Split(' ')[i]);
                }
            }
            else
            {
                strText.Add(itemSearched);
            }

            //New Command
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            //If type is clubs perform this search

            //Item List

            List<Items> item = new List<Items>();
            //Open Database Connection
            cmd.Connection = con;
            con.Open();
            for (int i = 0; i < numFields; i++)
            {
                //removed itemLocation because client does not want
                if (itemType == "Clubs")
                {
                    if (i == 0)
                    {
                        cmd.CommandText = "Select * from tbl_" + itemType + " where (sku like '%" + strText[i] + "%' or "
                                        + " modelID in (Select modelID from tbl_model where modelName like '%" + strText[i] + "%') or "
                                        + " brandID in (Select brandID from tbl_brand where brandName like '%" + strText[i] + "%') or "
                                        + " concat(clubType, shaft, dexterity) like '%" + strText[i] + "%')";
                    }
                    else
                    {
                        cmd.CommandText = cmd.CommandText + " Intersect (Select * from tbl_" + itemType + " where (sku like '%" + strText[i] + "%' or "
                                        + " modelID in (Select modelID from tbl_model where modelName like '%" + strText[i] + "%') or "
                                        + " brandID in (Select brandID from tbl_brand where brandName like '%" + strText[i] + "%') or "
                                        + " concat(clubType, shaft, dexterity) like '%" + strText[i] + "%'))";
                    }
                }
                // if type is accessories perform this search
                else if (itemType == "Accessories")
                {
                    if (i == 0)
                    {
                        cmd.CommandText = "Select * from tbl_" + itemType + " where (sku like '%" + strText[i] + "%' or "
                                    + " brandID in (Select brandID from tbl_brand where brandName like '%" + strText[i] + "%') or "
                                    + " concat(size, colour) like '%" + strText[i] + "%')";
                    }
                    else
                    {
                        cmd.CommandText = cmd.CommandText + "Intersect (Select * from tbl_" + itemType + " where (sku like '%" + strText[i] + "%' or "
                                    + " brandID in (Select brandID from tbl_brand where brandName like '%" + strText[i] + "%') or "
                                    + " concat(size, colour) like '%" + strText[i] + "%'))";
                    }
                }
                //if type is clothing perform this search
                else if (itemType == "Clothing")
                {
                    if (i == 0)
                    {
                        cmd.CommandText = "Select * from tbl_" + itemType + " where (sku like '%" + strText[i] + "%' or "
                                    + " brandID in (Select brandID from tbl_brand where brandName like '%" + strText[i] + "%') or "
                                    + " concat(size, colour, gender, style) like '%" + strText[i] + "%')";
                    }
                    else
                    {
                        cmd.CommandText = cmd.CommandText + "Intersect (Select * from tbl_" + itemType + " where (sku like '%" + strText[i] + "%' or "
                                    + " brandID in (Select brandID from tbl_brand where brandName like '%" + strText[i] + "%') or "
                                    + " concat(size, colour, gender, style) like '%" + strText[i] + "%'))";
                    }
                }
            }
            cmd.CommandText = cmd.CommandText + ";";
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string brand = null;
                string model = null;
                string description = null;

                ItemDataUtilities idu = new ItemDataUtilities();
                //if brand is not null return brand description
                if (reader["brandID"] != null)
                {
                    brand = idu.brandType(Convert.ToInt32(reader["brandID"]));
                }
                //if search type is clubs enter here for model and description builder
                if (itemType == "Clubs")
                {
                    //if model is not null return model description
                    if (reader["modelID"] != null)
                    {
                        model = idu.modelType(Convert.ToInt32(reader["modelID"]));
                    }
                    //create string for a club description
                    description = brand + " " + model + " " + reader["clubType"].ToString() + " " + reader["shaft"].ToString() + " "
                    + reader["numberOfClubs"].ToString() + " " + reader["dexterity"].ToString();
                }
                //if search type is accessories create accessories description
                else if (itemType == "Accessories")
                {
                    description = brand + " " + reader["size"].ToString() + " " + reader["colour"].ToString();
                }
                //if search type is clothing create clothing description
                else if (itemType == "Clothing")
                {
                    description = brand + " " + reader["size"].ToString() + " " + reader["colour"].ToString() + " "
                    + reader["gender"].ToString() + " " + reader["style"].ToString();
                }
                //enter all returned items into item list for display
                Items j = new Items(Convert.ToInt32(reader["sku"]),
                    description,
                    Convert.ToInt32(reader["quantity"]),
                    Convert.ToDouble(reader["price"]),
                    Convert.ToDouble(reader["cost"]));
                item.Add(j);
            }
            con.Close();
            return item;
        }

        //Robust search through inventory Nathan and Tyler created
        public List<Items> GetItemfromSearch(string itemSearched, string itemType)
        {
            ArrayList strText = new ArrayList();
            int numFields = itemSearched.Split(' ').Length;

            if (numFields > 1)
            {
                for (int i = 0; i < numFields; i++)
                {
                    strText.Add(itemSearched.Split(' ')[i]);
                }
            }
            else
            {
                strText.Add(itemSearched);
            }

            //New Command
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            //If type is clubs perform this search

            //Item List

            List<Items> item = new List<Items>();
            //Open Database Connection
            cmd.Connection = con;
            con.Open();
            for (int i = 0; i < numFields; i++)
            {

                if (itemType == "Clubs")
                {
                    if (i == 0)
                    {
                        cmd.CommandText = "Select * from tbl_" + itemType + " where sku like '%" + strText[i] + "%' or "
                                        + " modelID in (Select modelID from tbl_model where modelName like '%" + strText[i] + "%') or "
                                        + " brandID in (Select brandID from tbl_brand where brandName like '%" + strText[i] + "%') or "
                                        + " concat(clubType, shaft, dexterity) like '%" + strText[i] + "%'";
                    }
                    else
                    {
                        cmd.CommandText = cmd.CommandText + " Intersect (Select * from tbl_" + itemType + " where sku like '%" + strText[i] + "%' or "
                                        + " modelID in (Select modelID from tbl_model where modelName like '%" + strText[i] + "%') or "
                                        + " brandID in (Select brandID from tbl_brand where brandName like '%" + strText[i] + "%') or "
                                        + " concat(clubType, shaft, dexterity) like '%" + strText[i] + "%')";
                    }
                }
                // if type is accessories perform this search
                else if (itemType == "Accessories")
                {
                    if (i == 0)
                    {
                        cmd.CommandText = "Select * from tbl_" + itemType + " where sku like '%" + strText[i] + "%' or "
                                    + " brandID in (Select brandID from tbl_brand where brandName like '%" + strText[i] + "%') or "
                                    + " concat(size, colour) like '%" + strText[i] + "%'";
                    }
                    else
                    {
                        cmd.CommandText = cmd.CommandText + "Intersect (Select * from tbl_" + itemType + " where sku like '%" + strText[i] + "%' or "
                                    + " brandID in (Select brandID from tbl_brand where brandName like '%" + strText[i] + "%') or "
                                    + " concat(size, colour) like '%" + strText[i] + "%')";
                    }
                }
                //if type is clothing perform this search
                else if (itemType == "Clothing")
                {
                    if (i == 0)
                    {
                        cmd.CommandText = "Select * from tbl_" + itemType + " where sku like '%" + strText[i] + "%' or "
                                    + " brandID in (Select brandID from tbl_brand where brandName like '%" + strText[i] + "%') or "
                                    + " concat(size, colour, gender, style) like '%" + strText[i] + "%'";
                    }
                    else
                    {
                        cmd.CommandText = cmd.CommandText + "Intersect (Select * from tbl_" + itemType + " where sku like '%" + strText[i] + "%' or "
                                    + " brandID in (Select brandID from tbl_brand where brandName like '%" + strText[i] + "%') or "
                                    + " concat(size, colour, gender, style) like '%" + strText[i] + "%')";
                    }
                }
            }
            cmd.CommandText = cmd.CommandText + ";";
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string brand = null;
                string model = null;
                string description = null;

                ItemDataUtilities idu = new ItemDataUtilities();
                //if brand is not null return brand description
                if (reader["brandID"] != null)
                {
                    brand = idu.brandType(Convert.ToInt32(reader["brandID"]));
                }
                //if search type is clubs enter here for model and description builder
                if (itemType == "Clubs")
                {
                    //if model is not null return model description
                    if (reader["modelID"] != null)
                    {
                        model = idu.modelType(Convert.ToInt32(reader["modelID"]));
                    }
                    //create string for a club description
                    description = brand + " " + model + " " + reader["clubType"].ToString() + " " + reader["shaft"].ToString() + " "
                    + reader["numberOfClubs"].ToString() + " " + reader["dexterity"].ToString();
                }
                //if search type is accessories create accessories description
                else if (itemType == "Accessories")
                {
                    description = brand + " " + reader["size"].ToString() + " " + reader["colour"].ToString();
                }
                //if search type is clothing create clothing description
                else if (itemType == "Clothing")
                {
                    description = brand + " " + reader["size"].ToString() + " " + reader["colour"].ToString() + " "
                    + reader["gender"].ToString() + " " + reader["style"].ToString();
                }
                //enter all returned items into item list for display
                Items j = new Items(Convert.ToInt32(reader["sku"]),
                    description,
                    Convert.ToInt32(reader["quantity"]),
                    Convert.ToDouble(reader["price"]),
                    Convert.ToDouble(reader["cost"]),
                    lm.locationName(Convert.ToInt32(reader["locationID"])));
                item.Add(j);
            }
            con.Close();
            return item;
        }

        //Trade-in specific club from inventory lookup
        public Clubs tradeInItemLookUp(int sku)
        {
            //New Command
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Select sku, brandID, modelID, clubType, shaft, numberOfClubs, premium, cost, price, quantity, clubSpec,"
                + " shaftSpec, shaftFlex, dexterity, used, comments From tbl_tempTradeInCartSkus Where sku = @sku";
            cmd.Parameters.AddWithValue("sku", sku);

            //Open Database Connection
            cmd.Connection = con;
            con.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            //Item List

            Clubs clubs = new Clubs();

            while (reader.Read())
            {
                clubs.sku = Convert.ToInt32(reader["sku"]);
                clubs.brandID = Convert.ToInt32(reader["brandID"]);
                clubs.modelID = Convert.ToInt32(reader["modelID"]);
                clubs.clubType = reader["clubType"].ToString();
                clubs.shaft = reader["shaft"].ToString();
                clubs.numberOfClubs = reader["numberOfClubs"].ToString();
                clubs.premium = Convert.ToDouble(reader["premium"]);
                clubs.cost = Convert.ToDouble(reader["cost"]);
                clubs.price = Convert.ToDouble(reader["price"]);
                clubs.quantity = Convert.ToInt32(reader["quantity"]);
                clubs.clubSpec = reader["clubSpec"].ToString();
                clubs.shaftSpec = reader["shaftSpec"].ToString();
                clubs.shaftFlex = reader["shaftFlex"].ToString();
                clubs.dexterity = reader["dexterity"].ToString();
                clubs.used = Convert.ToBoolean(reader["used"]);
                clubs.comments = reader["comments"].ToString();
            }
            con.Close();
            return clubs;
        }

        //Select specific club from inventory Nathan and Tyler created
        public Clubs singleItemLookUp(int sku)
        {
            //New Command
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Select sku, brandID, modelID, typeID, clubType, shaft, numberOfClubs, premium, cost, price, quantity, clubSpec,"
                + " shaftSpec, shaftFlex, dexterity, locationID, used, comments From tbl_clubs Where sku = @sku";
            cmd.Parameters.AddWithValue("sku", sku);

            //Open Database Connection
            cmd.Connection = con;
            con.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            //Item List

            Clubs clubs = new Clubs();

            while (reader.Read())
            {
                clubs.sku = Convert.ToInt32(reader["sku"]);
                clubs.brandID = Convert.ToInt32(reader["brandID"]);
                clubs.modelID = Convert.ToInt32(reader["modelID"]);
                clubs.typeID = Convert.ToInt32(reader["typeID"]);
                clubs.clubType = reader["clubType"].ToString();
                clubs.shaft = reader["shaft"].ToString();
                clubs.numberOfClubs = reader["numberOfClubs"].ToString();
                clubs.premium = Convert.ToDouble(reader["premium"]);
                clubs.cost = Convert.ToDouble(reader["cost"]);
                clubs.price = Convert.ToDouble(reader["price"]);
                clubs.quantity = Convert.ToInt32(reader["quantity"]);
                clubs.clubSpec = reader["clubSpec"].ToString();
                clubs.shaftSpec = reader["shaftSpec"].ToString();
                clubs.shaftFlex = reader["shaftFlex"].ToString();
                clubs.dexterity = reader["dexterity"].ToString();
                clubs.itemlocation = Convert.ToInt32(reader["locationID"]);
                clubs.used = Convert.ToBoolean(reader["used"]);
                clubs.comments = reader["comments"].ToString();
            }
            con.Close();
            return clubs;
        }

        //Adds new Item to tables Nathan created
        public int addItem(Object o)
        {
            if (o is Clubs)
            {
                Clubs c = o as Clubs;
                addClub(c);
            }
            else if (o is Accessories)
            {
                Accessories a = o as Accessories;
                addAccessory(a);
            }
            else if (o is Clothing)
            {
                Clothing cl = o as Clothing;
                addClothing(cl);
            }
            return returnItemNumber(o);
        }

        //Looks to see if the item already exists
        public void checkForItem(Object o)
        {
            if (o is Clubs)
            {
                Clubs c = o as Clubs;
                checkClub(c);
            }
            else if (o is Accessories)
            {
                Accessories a = o as Accessories;
                checkAccessory(a);
            }
            else if (o is Clothing)
            {
                Clothing cl = o as Clothing;
                checkClothing(cl);
            }
        }

        //returns sku number after adding to reload page with new item Nathan created
        public int returnItemNumber(Object o)
        {
            //New command
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            if (o is Clubs)
            {
                Clubs c = o as Clubs;
                cmd.CommandText = "Select sku From tbl_clubs Where brandID = @brandID and modelID = @modelID and clubType = @clubType and "
                    + " shaft = @shaft and numberOfClubs = @numberOfClubs and premium = @premium and cost = @cost and price = @price and "
                    + " quantity = @quantity and clubSpec = @clubSpec and shaftSpec = @shaftSpec and shaftFlex = @shaftFlex and "
                    + " dexterity = @dexterity and  used = @used and typeID = @typeID and comments = @comments";
                cmd.Parameters.AddWithValue("brandID", c.brandID);
                cmd.Parameters.AddWithValue("modelID", c.modelID);
                cmd.Parameters.AddWithValue("clubType", c.clubType);
                cmd.Parameters.AddWithValue("shaft", c.shaft);
                cmd.Parameters.AddWithValue("numberOfClubs", c.numberOfClubs);
                cmd.Parameters.AddWithValue("premium", c.premium);
                cmd.Parameters.AddWithValue("cost", c.cost);
                cmd.Parameters.AddWithValue("price", c.price);
                cmd.Parameters.AddWithValue("quantity", c.quantity);
                cmd.Parameters.AddWithValue("clubSpec", c.clubSpec);
                cmd.Parameters.AddWithValue("shaftSpec", c.shaftSpec);
                cmd.Parameters.AddWithValue("shaftFlex", c.shaftFlex);
                cmd.Parameters.AddWithValue("dexterity", c.dexterity);
                cmd.Parameters.AddWithValue("used", c.used);
                cmd.Parameters.AddWithValue("typeID", c.typeID);
                cmd.Parameters.AddWithValue("comments", c.comments);
            }
            else if (o is Accessories)
            {
                Accessories a = o as Accessories;
                cmd.CommandText = "Select sku From tbl_accessories Where brandID = @brandID and size = @size and colour = @colour and"
                    + " price = @price and cost = @cost and quantity = @quantity";
                cmd.Parameters.AddWithValue("brandID", a.brandID);
                cmd.Parameters.AddWithValue("size", a.size);
                cmd.Parameters.AddWithValue("colour", a.colour);
                cmd.Parameters.AddWithValue("price", a.price);
                cmd.Parameters.AddWithValue("cost", a.cost);
                cmd.Parameters.AddWithValue("quantity", a.quantity);
            }
            else if (o is Clothing)
            {
                Clothing c = o as Clothing;
                cmd.CommandText = "Select sku From tbl_clothing Where brandID = @brandID and size = @size and colour = @colour and"
                    + " gender = @gender and style = @style and price = @price and cost = @cost and quantity = @quantity";
                cmd.Parameters.AddWithValue("brandID", c.brandID);
                cmd.Parameters.AddWithValue("size", c.size);
                cmd.Parameters.AddWithValue("colour", c.colour);
                cmd.Parameters.AddWithValue("gender", c.gender);
                cmd.Parameters.AddWithValue("style", c.style);
                cmd.Parameters.AddWithValue("price", c.price);
                cmd.Parameters.AddWithValue("cost", c.cost);
                cmd.Parameters.AddWithValue("quantity", c.quantity);
            }

            //Declare and open connection
            cmd.Connection = con;
            con.Open();
            //Execute Insert
            SqlDataReader reader = cmd.ExecuteReader();
            //New sku number
            int sku = 0;
            //Begin reading
            while (reader.Read())
            {
                sku = Convert.ToInt32(reader["sku"]);
            }
            con.Close();
            return sku;
        }

        //These three actully add the item to specific tables Nathan created
        public void addClub(Clubs c)
        {
            //New command
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            if (c.sku < 10000000)
            {
                cmd.CommandText = "Insert Into tbl_clubs (sku, brandID, modelID, clubType, shaft, numberOfClubs,"
                    + " premium, cost, price, quantity, clubSpec, shaftSpec, shaftFlex, dexterity, typeID, locationID, used, comments)"
                    + " Values (@sku, @brandID, @modelID, @clubType, @shaft, @numberOfClubs, @premium, @cost, @price,"
                    + " @quantity, @clubSpec, @shaftSpec, @shaftFlex, @dexterity, @typeID, @locationID, @used, @comments)";
                cmd.Parameters.AddWithValue("sku", c.sku);
            }
            else
            {
                int nextSku = idu.maxSku(c.sku, "clubs");
                cmd.CommandText = "Insert Into tbl_clubs (sku, brandID, modelID, clubType, shaft, numberOfClubs,"
                    + " premium, cost, price, quantity, clubSpec, shaftSpec, shaftFlex, dexterity, typeID, locationID, used, comments)"
                    + " Values (" + nextSku + ", @brandID, @modelID, @clubType, @shaft, @numberOfClubs, @premium, @cost, @price,"
                    + " @quantity, @clubSpec, @shaftSpec, @shaftFlex, @dexterity, @typeID, @locationID, @used, @comments)";
            }

            cmd.Parameters.AddWithValue("brandID", c.brandID);
            cmd.Parameters.AddWithValue("modelID", c.modelID);
            cmd.Parameters.AddWithValue("clubType", c.clubType);
            cmd.Parameters.AddWithValue("shaft", c.shaft);
            cmd.Parameters.AddWithValue("numberOfClubs", c.numberOfClubs);
            cmd.Parameters.AddWithValue("premium", c.premium);
            cmd.Parameters.AddWithValue("cost", c.cost);
            cmd.Parameters.AddWithValue("price", c.price);
            cmd.Parameters.AddWithValue("quantity", c.quantity);
            cmd.Parameters.AddWithValue("clubSpec", c.clubSpec);
            cmd.Parameters.AddWithValue("shaftSpec", c.shaftSpec);
            cmd.Parameters.AddWithValue("shaftFlex", c.shaftFlex);
            cmd.Parameters.AddWithValue("dexterity", c.dexterity);
            cmd.Parameters.AddWithValue("typeID", c.typeID);
            cmd.Parameters.AddWithValue("locationID", c.itemlocation);
            cmd.Parameters.AddWithValue("used", c.used);
            cmd.Parameters.AddWithValue("comments", c.comments);
            //Declare and open connection
            cmd.Connection = con;
            con.Open();
            //Execute Insert
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public void addAccessory(Accessories a)
        {
            //New command
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();


            if (a.sku < 30000000)
            {
                cmd.CommandText = "Insert Into tbl_accessories (sku, size, colour, price, cost, brandID, quantity, typeID, locationID)"
            + " Values (@sku, @size, @colour, @price, @cost, @brandID, @quantity, @typeID, @locationID)";
                cmd.Parameters.AddWithValue("sku", a.sku);
            }
            else
            {
                int nextSku = idu.maxSku(a.sku, "accessories");
                cmd.CommandText = "Insert Into tbl_accessories (sku, size, colour, price, cost, brandID, quantity, typeID, locationID)"
            + " Values (" + nextSku + ", @size, @colour, @price, @cost, @brandID, @quantity, @typeID, @locationID)";
            }
                        
            cmd.Parameters.AddWithValue("size", a.size);
            cmd.Parameters.AddWithValue("colour", a.colour);
            cmd.Parameters.AddWithValue("price", a.price);
            cmd.Parameters.AddWithValue("cost", a.cost);
            cmd.Parameters.AddWithValue("brandID", a.brandID);
            cmd.Parameters.AddWithValue("quantity", a.quantity);
            cmd.Parameters.AddWithValue("typeID", a.typeID);
            cmd.Parameters.AddWithValue("locationID", a.locID);
            //Declare and open connection
            cmd.Connection = con;
            con.Open();
            //Execute Insert
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public void addClothing(Clothing c)
        {
            //New command
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            if (c.sku < 50000000)
            {
                cmd.CommandText = "Insert Into tbl_clothing (sku, size, colour, gender, style, price, cost, brandID, quantity, typeID, locationID)"
                + " Values (@sku, @size, @colour, @gender, @style, @price, @cost, @brandID, @quantity, @typeID, @locationID)";
                cmd.Parameters.AddWithValue("sku", c.sku);
            }
            else
            {
                int nextSku = idu.maxSku(c.sku, "clothing");
                cmd.CommandText = "Insert Into tbl_clothing (sku, size, colour, gender, style, price, cost, brandID, quantity, typeID, locationID)"
                + " Values (" + nextSku + ",, @size, @colour, @gender, @style, @price, @cost, @brandID, @quantity, @typeID, @locationID)";
            }
            
            cmd.Parameters.AddWithValue("size", c.size);
            cmd.Parameters.AddWithValue("colour", c.colour);
            cmd.Parameters.AddWithValue("gender", c.gender);
            cmd.Parameters.AddWithValue("style", c.style);
            cmd.Parameters.AddWithValue("price", c.price);
            cmd.Parameters.AddWithValue("cost", c.cost);
            cmd.Parameters.AddWithValue("brandID", c.brandID);
            cmd.Parameters.AddWithValue("quantity", c.quantity);
            cmd.Parameters.AddWithValue("typeID", c.typeID);
            cmd.Parameters.AddWithValue("locationID", c.locID);
            //Declare and open connection
            cmd.Connection = con;
            con.Open();
            //Execute Insert
            cmd.ExecuteNonQuery();
            con.Close();
        }

        //The check methods check if an item already exists or does not exist. It will update if it exists, and add if not.
        public void checkClub(Clubs c)
        {
            //New command
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select count(*) from tbl_clubs where sku = " + c.sku;
            //Declare and open connection
            cmd.Connection = con;
            con.Open();
            //Execute search
            cmd.ExecuteNonQuery();
            int itemExists = (int)cmd.ExecuteScalar();

            //If item exists
            if (itemExists > 0)
            {
                //update
                updateItem(c);
            }
            //If item doesn't exist
            else
            {
                //add
                addClub(c);
            }
            //Closing
            con.Close();
        }
        public void checkAccessory(Accessories a)
        {
            //New command
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select count(*) from tbl_accessories where sku = " + a.sku;
            //Declare and open connection
            cmd.Connection = con;
            con.Open();
            //Execute search
            cmd.ExecuteNonQuery();
            int itemExists = (int)cmd.ExecuteScalar();

            //If item exists
            if (itemExists > 0)
            {
                //update
                updateAccessories(a);
            }
            //If item doesn't exist
            else
            {
                //add
                addAccessory(a);
            }
            //Closing
            con.Close();
        }
        public void checkClothing(Clothing cl)
        {
            //New command
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select count(*) from tbl_clothing where sku = " + cl.sku;
            //Declare and open connection
            cmd.Connection = con;
            con.Open();
            //Execute search
            cmd.ExecuteNonQuery();
            int itemExists = (int)cmd.ExecuteScalar();

            //If item exists
            if (itemExists > 0)
            {
                //update
                updateClothing(cl);
            }
            //If item doesn't exist
            else
            {
                //add
                addClothing(cl);
            }
            //Closing
            con.Close();
        }

        public string convertDBNullToString(Object o)
        {
            if (o is DBNull)
                o = "";

            return o.ToString();

        }
        public double convertDBNullToDouble(Object o)
        {
            double dbl = 0.0;
            if (o is DBNull)
                dbl = 0.0;
            else
                dbl = Convert.ToDouble(o);

            return dbl;

        }

        //Returns single accessory Nathan created
        public Accessories getAccessory(int sku)
        {
            //New Command
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Select sku, brandID, size, colour, price, cost, quantity, typeID, locationID From tbl_accessories Where sku = @sku";
            cmd.Parameters.AddWithValue("sku", sku);

            //Open Database Connection
            cmd.Connection = con;
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            //Item List
            Accessories a = new Accessories();

            while (reader.Read())
            {
                a.sku = Convert.ToInt32(reader["sku"]);
                a.brandID = Convert.ToInt32(reader["brandID"]);
                a.size = reader["size"].ToString();
                a.colour = reader["colour"].ToString();
                a.cost = Convert.ToDouble(reader["cost"]);
                a.price = Convert.ToDouble(reader["price"]);
                a.quantity = Convert.ToInt32(reader["quantity"]);
                a.typeID = Convert.ToInt32(reader["typeID"]);
                a.locID = Convert.ToInt32(reader["locationID"]);
            }
            con.Close();
            return a;
        }
        //Returns single clothing Nathan created
        public Clothing getClothing(int sku)
        {
            //New Command
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Select sku, brandID, size, colour, gender, style, price, cost, quantity, typeID, locationID From tbl_clothing Where sku = @sku";
            cmd.Parameters.AddWithValue("sku", sku);

            //Open Database Connection
            cmd.Connection = con;
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            //Item List
            Clothing c = new Clothing();

            while (reader.Read())
            {
                c.sku = Convert.ToInt32(reader["sku"]);
                c.brandID = Convert.ToInt32(reader["brandID"]);
                c.size = reader["size"].ToString();
                c.colour = reader["colour"].ToString();
                c.gender = reader["gender"].ToString();
                c.style = reader["style"].ToString();
                c.cost = Convert.ToDouble(reader["cost"]);
                c.price = Convert.ToDouble(reader["price"]);
                c.quantity = Convert.ToInt32(reader["quantity"]);
                c.typeID = Convert.ToInt32(reader["typeID"]);
                c.locID = Convert.ToInt32(reader["locationID"]);
            }
            con.Close();
            return c;
        }

        public void addItem(Clubs c)
        {
            //New command
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "SET IDENTITY_INSERT Item ON; INSERT INTO Item(sku,shipmentDate, brand, model, clubType, shaft, numberOfClubs, tradeInPrice," +
                "premium, wePay, quantity, extendedPrice, retailPrice, comments, clubSpec, shaftSpec," +
                "shaftFlex, dexterity, destination, received, paid, gst, pst)" +
                "VALUES(@sku, @shipmentDate, @brand, @model, @clubType, @shaft, @numberOfClubs, @tradeInPrice, " +
                "@premium, @wePay, @quantity, @extendedPrice, @retailPrice, @comments, @clubSpec, @shaftSpec," +
                "@shaftFlex, @dexterity, @destination, @received, @paid, @gst, @pst); SET IDENTITY_INSERT Item OFF;";
            cmd.Parameters.AddWithValue("sku", c.sku);
            cmd.Parameters.AddWithValue("shipmentDate", c.ShipmentDate);
            cmd.Parameters.AddWithValue("brand", c.brandID);
            cmd.Parameters.AddWithValue("model", c.modelID);
            cmd.Parameters.AddWithValue("clubType", c.clubType);
            cmd.Parameters.AddWithValue("shaft", c.shaft);
            cmd.Parameters.AddWithValue("numberOfClubs", c.numberOfClubs);
            cmd.Parameters.AddWithValue("tradeInPrice", c.TradeInPrice);
            cmd.Parameters.AddWithValue("premium", c.premium);
            cmd.Parameters.AddWithValue("wePay", c.WePay);
            cmd.Parameters.AddWithValue("quantity", c.quantity);
            cmd.Parameters.AddWithValue("extendedPrice", c.ExtendedPrice);
            cmd.Parameters.AddWithValue("retailPrice", c.RetailPrice);
            cmd.Parameters.AddWithValue("comments", c.comments);
            cmd.Parameters.AddWithValue("clubSpec", c.clubSpec);
            cmd.Parameters.AddWithValue("shaftSpec", c.shaftSpec);
            cmd.Parameters.AddWithValue("shaftFlex", c.shaftFlex);
            cmd.Parameters.AddWithValue("dexterity", c.dexterity);
            cmd.Parameters.AddWithValue("destination", c.Destination);
            cmd.Parameters.AddWithValue("received", c.Received);
            cmd.Parameters.AddWithValue("paid", c.Paid);
            cmd.Parameters.AddWithValue("gst", c.Gst);
            cmd.Parameters.AddWithValue("pst", c.Pst);
            //Declare and open connection
            cmd.Connection = con;
            con.Open();
            //Execute Insert
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public void updateItemInvoice(Clubs c)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "UPDATE Item SET  quantityInOrder = @quantityInOrder,brand = @brand, model = @model," +
                "clubType = @clubType, shaft = @shaft, numberOfClubs = @numberOfClubs, tradeInPrice = @tradeInPrice," +
                "premium = @premium, wePay = @wePay, quantity = @quantity, extendedPrice = @extendedPrice," +
                "retailPrice = @retailPrice, clubSpec = @clubSpec, shaftSpec = @shaftSpec," +
                "shaftFlex = @shaftFlex, dexterity = @dexterity, gst = @gst, pst = @pst WHERE sku = @sku";

            cmd.Parameters.AddWithValue("@sku", c.sku);
            cmd.Parameters.AddWithValue("@quantityInOrder", c.QuantityInOrder);
            cmd.Parameters.AddWithValue("@brand", c.brandID);
            cmd.Parameters.AddWithValue("@model", c.modelID);
            cmd.Parameters.AddWithValue("@clubType", c.clubType);
            cmd.Parameters.AddWithValue("@shaft", c.shaft);
            cmd.Parameters.AddWithValue("@numberOfClubs", c.numberOfClubs);
            cmd.Parameters.AddWithValue("@tradeInPrice", c.TradeInPrice);
            cmd.Parameters.AddWithValue("@premium", c.premium);
            cmd.Parameters.AddWithValue("@wePay", c.WePay);
            cmd.Parameters.AddWithValue("@quantity", c.quantity);
            cmd.Parameters.AddWithValue("@extendedPrice", c.ExtendedPrice);
            cmd.Parameters.AddWithValue("@retailPrice", c.RetailPrice);
            cmd.Parameters.AddWithValue("@clubSpec", c.clubSpec);
            cmd.Parameters.AddWithValue("@shaftSpec", c.shaftSpec);
            cmd.Parameters.AddWithValue("@shaftFlex", c.shaftFlex);
            cmd.Parameters.AddWithValue("@dexterity", c.dexterity);
            cmd.Parameters.AddWithValue("@gst", c.Gst);
            cmd.Parameters.AddWithValue("@pst", c.Pst);
            //Declare and open connection
            cmd.Connection = con;
            con.Open();
            //Execute Insert
            cmd.ExecuteNonQuery();
            con.Close();
        }

        //Update item in inventory updated Nathan
        public void updateItem(Clubs c)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "UPDATE tbl_clubs SET brandID = @brandID, modelID = @modelID, clubType = @clubType, shaft = @shaft,"
                + " numberOfClubs = @numberOfClubs, premium = @premium, cost = @cost, price = @price, quantity = @quantity,"
                + " clubSpec = @clubSpec, shaftSpec = @shaftSpec, shaftFlex = @shaftFlex, dexterity = @dexterity,"
                + " locationID = @locationID, comments = @comments WHERE sku = @sku";
            cmd.Parameters.AddWithValue("@sku", c.sku);
            cmd.Parameters.AddWithValue("@brandID", c.brandID);
            cmd.Parameters.AddWithValue("@modelID", c.modelID);
            cmd.Parameters.AddWithValue("@clubType", c.clubType);
            cmd.Parameters.AddWithValue("@shaft", c.shaft);
            cmd.Parameters.AddWithValue("@numberOfClubs", c.numberOfClubs);
            cmd.Parameters.AddWithValue("@premium", c.premium);
            cmd.Parameters.AddWithValue("@cost", c.cost);
            cmd.Parameters.AddWithValue("@quantity", c.quantity);
            cmd.Parameters.AddWithValue("@price", c.price);
            cmd.Parameters.AddWithValue("@comments", c.comments);
            cmd.Parameters.AddWithValue("@locationID", c.itemlocation);
            cmd.Parameters.AddWithValue("@clubSpec", c.clubSpec);
            cmd.Parameters.AddWithValue("@shaftSpec", c.shaftSpec);
            cmd.Parameters.AddWithValue("@shaftFlex", c.shaftFlex);
            cmd.Parameters.AddWithValue("@dexterity", c.dexterity);
            //Declare and open connection
            cmd.Connection = con;
            con.Open();
            //Execute Insert
            cmd.ExecuteNonQuery();
            con.Close();
        }
        //Update item in inventory created Nathan
        public void updateAccessories(Accessories a)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "UPDATE tbl_accessories SET size = @size, colour = @colour, price = @price, cost = @cost, brandID = @brandID,"
                + " quantity = @quantity, locationID = @locationID WHERE sku = @sku";
            cmd.Parameters.AddWithValue("@sku", a.sku);
            cmd.Parameters.AddWithValue("@size", a.size);
            cmd.Parameters.AddWithValue("@colour", a.colour);
            cmd.Parameters.AddWithValue("@price", a.price);
            cmd.Parameters.AddWithValue("@cost", a.cost);
            cmd.Parameters.AddWithValue("@brandID", a.brandID);
            cmd.Parameters.AddWithValue("@locationID", a.locID);
            cmd.Parameters.AddWithValue("@quantity", a.quantity);
            //Declare and open connection
            cmd.Connection = con;
            con.Open();
            //Execute Insert
            cmd.ExecuteNonQuery();
            con.Close();
        }
        //Update item in inventory created Nathan
        public void updateClothing(Clothing cl)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "UPDATE tbl_clothing SET size = @size, colour = @colour, gender = @gender, style = @style,"
                + " price = @price, cost = @cost, brandID = @brandID, quantity = @quantity, locationID = @locationID WHERE sku = @sku";
            cmd.Parameters.AddWithValue("@sku", cl.sku);
            cmd.Parameters.AddWithValue("@size", cl.size);
            cmd.Parameters.AddWithValue("@colour", cl.colour);
            cmd.Parameters.AddWithValue("@gender", cl.gender);
            cmd.Parameters.AddWithValue("@style", cl.style);
            cmd.Parameters.AddWithValue("@price", cl.price);
            cmd.Parameters.AddWithValue("@cost", cl.cost);
            cmd.Parameters.AddWithValue("@brandID", cl.brandID);
            cmd.Parameters.AddWithValue("@quantity", cl.quantity);
            cmd.Parameters.AddWithValue("@locationID", cl.locID);
            //Declare and open connection
            cmd.Connection = con;
            con.Open();
            //Execute Insert
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public bool isTradein(int sku)
        {
            bool trade = false;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT used FROM tbl_clubs WHERE sku = @sku";
            cmd.Parameters.AddWithValue("sku", sku);
            cmd.Connection = con;
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    trade = Convert.ToBoolean(reader["used"]);
                }
            }
            con.Close();
            return trade;
        }
        public int whatTypeIsItem(int sku)
        {

            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT typeID FROM tbl_clubs WHERE sku = @sku";
            cmd.Parameters.AddWithValue("sku", sku);
            cmd.Connection = con;
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            int intType = 0;
            while (reader.Read())
            {
                intType = Convert.ToInt32(reader["typeID"]);
            }
            if (!reader.HasRows)
            {
                reader.Close();
                cmd.CommandText = "Select typeID from tbl_accessories where SKU = @skuacces";
                cmd.Parameters.AddWithValue("skuacces", sku);
                SqlDataReader readerAccesories = cmd.ExecuteReader();

                while (readerAccesories.Read())
                {
                    intType = Convert.ToInt32(readerAccesories["typeID"]);
                }
                if (!readerAccesories.HasRows)
                {
                    readerAccesories.Close();
                    cmd.CommandText = "Select typeID from tbl_clothing where SKU = @skucloth";
                    cmd.Parameters.AddWithValue("skucloth", sku);
                    SqlDataReader readerclothing = cmd.ExecuteReader();

                    while (readerclothing.Read())
                    {
                        intType = Convert.ToInt32(readerclothing["typeID"]);
                    }
                }
            }
            return intType;
        }
        public string getDescription(int sku, int type)
        {
            string desc = "";
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();

            if (type == 1)
            {
                cmd.CommandText = "Select brandID, modelID, clubType, shaft, numberOfClubs, dexterity from tbl_clubs where sku = @skuCl";
                cmd.Parameters.AddWithValue("@skuCl", sku);
            }
            else if (type == 2)
            {
                cmd.CommandText = "Select brandID, size, color from tbl_accessories where sku = @skuAc";
                cmd.Parameters.AddWithValue("@skuAc", sku);
            }
            else if (type == 3)
            {
                cmd.CommandText = "Select brandID, size, color, gender, style from tbl_clothing where sku = @skuClo";
                cmd.Parameters.AddWithValue("@skuClo", sku);
            }
            cmd.CommandText = cmd.CommandText + ";";
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                if (type == 1)
                {
                    desc = idu.brandType(Convert.ToInt32(reader["brandID"])).ToString() + " " + idu.modelType(Convert.ToInt32(reader["modelID"])).ToString() + " "
                        + reader["clubType"].ToString() + " " + reader["shaft"].ToString() + " "
                        + reader["numberOfClubs"].ToString() + " " + reader["dexterity"].ToString();
                }
                else if (type == 2)
                {
                    desc = idu.brandType(Convert.ToInt32(reader["brandID"])).ToString() + " " + reader["size"].ToString() + " " + reader["colour"].ToString();
                }
                else if (type == 3)
                {
                    desc = idu.brandType(Convert.ToInt32(reader["brandID"])).ToString() + " " + reader["size"].ToString() + " " + reader["colour"].ToString() + " "
                        + reader["gender"].ToString() + " " + reader["style"].ToString();
                }
            }
            return desc;
        }
        public void updateItemQuantity(int quantity, int sku)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "UPDATE Item SET quantity = quantity + @quantity WHERE sku = @sku";
            cmd.Parameters.AddWithValue("@sku", sku);
            cmd.Parameters.AddWithValue("@quantity", quantity);
            //Declare and open connection
            cmd.Connection = con;
            con.Open();
            //Execute Insert
            cmd.ExecuteNonQuery();
            con.Close();
        }
        //Delete item from inventory
        public void deleteItem(int sku)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "DELETE FROM Item WHERE sku=@sku";
            cmd.Parameters.AddWithValue("sku", sku);
            cmd.Connection = con;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        /*******Invoice Utilities************************************************************************************/

        public int getInvoiceID(DateTime saleDate)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select invoiceID FROM Invoice Where saleDate = @saleDate";
            cmd.Parameters.AddWithValue("saleDate", saleDate);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            // Invoice invoice = new Invoice();
            int invoiceID = 0;
            while (reader.Read())
            {

                invoiceID = Convert.ToInt32(reader["invoiceID"]);

            }
            conn.Close();
            return invoiceID;
        }
        //Get Invoice by invoiceID and return the invoice object
        public List<Invoice> getInvoice(int invoiceID)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT invoiceNum, invoiceSubNum, invoiceDate, invoiceTime, custID, empID, locationID, subTotal, discountAmount, "
                + "tradeinAmount, governmentTax, provincialTax, balanceDue, transactionType, comments FROM tbl_invoice "
                + "WHERE invoiceNum = @invoiceNum";
            cmd.Parameters.AddWithValue("invoiceNum", invoiceID);
            cmd.Connection = con;
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Invoice> i = new List<Invoice>();

            while (reader.Read())
            {
                i.Add(new Invoice(Convert.ToInt32(reader["invoiceNum"]), Convert.ToInt32(reader["invoiceSubNum"]), Convert.ToDateTime(reader["invoiceDate"]),
                    reader["invoiceTime"].ToString(), Convert.ToInt32(reader["custID"]), Convert.ToInt32(reader["empID"]), Convert.ToInt32(reader["locationID"]),
                    Convert.ToDouble(reader["subTotal"]), Convert.ToDouble(reader["discountAmount"]), Convert.ToDouble(reader["tradeinAmount"]), Convert.ToDouble(reader["governmentTax"]),
                    Convert.ToDouble(reader["provincialTax"]), Convert.ToDouble(reader["balanceDue"]), Convert.ToInt32(reader["transactionType"]), reader["comments"].ToString()));
            }
            con.Close();
            return i;
        }
        //Nathan built for home page sales display
        public List<Invoice> getInvoiceBySaleDate(DateTime givenDate, int locationID)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT invoiceNum, invoiceSubNum, custID, empID, subTotal, discountAmount, "
                + "tradeinAmount, governmentTax, provincialTax, balanceDue FROM tbl_invoice "
                + "WHERE invoiceDate = @givenDate AND locationID = @locationID AND transactionType = 1";
            cmd.Parameters.AddWithValue("givenDate", givenDate);
            cmd.Parameters.AddWithValue("locationID", locationID);
            cmd.Connection = con;
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Invoice> i = new List<Invoice>();
            while (reader.Read())
            {
                Invoice inv = new Invoice();
                inv.invoiceNum = Convert.ToInt32(reader["invoiceNum"]);
                inv.invoiceSub = Convert.ToInt32(reader["invoiceSubNum"]);
                inv.customerID = Convert.ToInt32(reader["custID"]);
                inv.employeeID = Convert.ToInt32(reader["empID"]);
                inv.subTotal = Convert.ToDouble(reader["subTotal"]);
                inv.discountAmount = Convert.ToDouble(reader["discountAmount"]);
                inv.tradeinAmount = Convert.ToDouble(reader["tradeinAmount"]);
                inv.governmentTax = Convert.ToDouble(reader["governmentTax"]);
                inv.provincialTax = Convert.ToDouble(reader["provincialTax"]);
                inv.balanceDue = Convert.ToDouble(reader["balanceDue"]);

                i.Add(inv);
            }
            con.Close();
            return i;
        }
        public List<Invoice> multiTypeSearchInvoices(string searchCriteria, Nullable<DateTime> searchDate)
        {
            List<Customer> c = GetCustomerfromSearch(searchCriteria);
            List<int> customerNum = new List<int>();
            foreach (var cust in c)
            {
                customerNum.Add(cust.customerId);
            }

            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            string selectStatement = "SELECT invoiceNum, invoiceSubNum, invoiceDate, invoiceTime, custID, empID, locationID, subTotal, discountAmount, "
                + "tradeinAmount, governmentTax, provincialTax, balanceDue, transactionType, comments FROM tbl_invoice "
                + "WHERE (";
            int firstCust = Enumerable.First(customerNum);
            foreach (var num in customerNum)
            {
                if (num.Equals(firstCust))
                {
                    selectStatement += "custID = " + num;
                }
                else
                {
                    selectStatement += "or custID = " + num;
                }
            }
            if (searchDate.HasValue)
            {
                selectStatement += ") and invoiceDate = @searchDate";
                cmd.Parameters.AddWithValue("@searchDate", searchDate);
            }
            else
            {
                selectStatement += ")";
            }

            cmd.CommandText = selectStatement;
            cmd.Connection = con;
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Invoice> i = new List<Invoice>();
            while (reader.Read())
            {
                Invoice inv = new Invoice();
                inv.invoiceNum = Convert.ToInt32(reader["invoiceNum"]);
                inv.invoiceSub = Convert.ToInt32(reader["invoiceSubNum"]);
                inv.invoiceDate = Convert.ToDateTime(reader["invoiceDate"]);
                inv.invoiceTime = reader["invoiceTime"].ToString();
                inv.customerID = Convert.ToInt32(reader["custID"]);
                inv.employeeID = Convert.ToInt32(reader["empID"]);
                inv.locationID = Convert.ToInt32(reader["locationID"]);
                inv.subTotal = Convert.ToDouble(reader["subTotal"]);
                inv.discountAmount = Convert.ToDouble(reader["discountAmount"]);
                inv.tradeinAmount = Convert.ToDouble(reader["tradeinAmount"]);
                inv.governmentTax = Convert.ToDouble(reader["governmentTax"]);
                inv.provincialTax = Convert.ToDouble(reader["provincialTax"]);
                inv.balanceDue = Convert.ToDouble(reader["balanceDue"]);
                inv.transactionType = Convert.ToInt32(reader["transactionType"]);
                inv.comments = reader["comments"].ToString();

                i.Add(inv);
            }
            con.Close();
            return i;

        }
        public List<Cart> returningItems(int invoiceNumber, int invoiceSub)
        {
            List<Cart> retItems = new List<Cart>();

            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT sku, itemQuantity, itemCost, itemPrice, itemDiscount, percentage"
                + " FROM tbl_invoiceItem WHERE invoiceNum = @invoiceNum and invoiceSubNum = @invoiceSubNum";
            cmd.Parameters.AddWithValue("invoiceNum", invoiceNumber);
            cmd.Parameters.AddWithValue("invoiceSubNum", invoiceSub);

            cmd.Connection = con;
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            Cart cartItem;
            while (reader.Read())
            {
                int intType = whatTypeIsItem(Convert.ToInt32(reader["sku"]));
                bool bolTrade = false;
                if (intType == 1)
                    bolTrade = isTradein(Convert.ToInt32(reader["sku"]));

                cartItem = new Cart(Convert.ToInt32(reader["sku"]),
                getDescription(Convert.ToInt32(reader["sku"]), intType),
                Convert.ToInt32(reader["itemQuantity"]),
                Convert.ToDouble(reader["itemPrice"]),
                Convert.ToDouble(reader["itemCost"]),
                Convert.ToDouble(reader["itemDiscount"]),
                Convert.ToBoolean(reader["percentage"]),
                bolTrade,
                intType);

                retItems.Add(cartItem);
            }
            con.Close();
            return retItems;
        }
        //public void updateReturnToInvoice(int invoiceID, double gstRefund, double pstRefund, double retailPrice, double totalRefund)
        //{
        //    SqlConnection con = new SqlConnection(connectionString);
        //    SqlCommand cmd = new SqlCommand();

        //    cmd.CommandText = "UPDATE Invoice SET gst = gst - @gst, pst = pst - @pst, subtotal = subtotal - @retailPrice,"+
        //        "total = total - @totalRefund, paymentTotal = paymentTotal - @totalRefund WHERE invoiceID = @invoiceID";
        //    cmd.Parameters.AddWithValue("invoiceID", invoiceID);
        //    cmd.Parameters.AddWithValue("gst", gstRefund);
        //    cmd.Parameters.AddWithValue("pst", pstRefund);
        //    cmd.Parameters.AddWithValue("retailPrice", retailPrice);
        //    cmd.Parameters.AddWithValue("totalRefund", totalRefund);
        //    //Declare and open connection
        //    cmd.Connection = con;
        //    con.Open();
        //    //Execute Insert
        //    cmd.ExecuteNonQuery();
        //    con.Close();
        //}
        public void addInvoice(Invoice i)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "INSERT INTO Invoice( customerID, gst, pst, paymentTotal, balance, subTotal, total, discount, tradeIn, paymentID, stateprovID, posted, inProcess, saleDate) VALUES ( @customerID, @gst, @pst, @paymentTotal, @balance, @subTotal, @total, @discount, @tradeIn, @paymentID, @stateprovID, @posted, @inProcess, @saleDate)";

            //cmd.Parameters.AddWithValue("customerID", i.customerId);
            //cmd.Parameters.AddWithValue("gst", i.gst);
            //cmd.Parameters.AddWithValue("pst", i.pst);
            //cmd.Parameters.AddWithValue("paymentTotal", i.paymentTotal);
            //cmd.Parameters.AddWithValue("balance", i.balance);
            //cmd.Parameters.AddWithValue("subTotal", i.subTotal);
            //cmd.Parameters.AddWithValue("total", i.total);
            //cmd.Parameters.AddWithValue("discount", i.discount);
            //cmd.Parameters.AddWithValue("tradeIn", i.tradeIn);
            //cmd.Parameters.AddWithValue("paymentID", i.paymentID);
            //cmd.Parameters.AddWithValue("stateprovID", i.stateprovID);
            //cmd.Parameters.AddWithValue("posted", i.posted);
            //cmd.Parameters.AddWithValue("inProcess", i.inProcess);
            //cmd.Parameters.AddWithValue("saleDate", i.saleDate);
            cmd.Connection = con;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
        //Sets affected invoices to posted and replaces the null value with a posted date.
        public void postInvoices(int invoiceID, DateTime PostedDate)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Update Invoice SET posted = 1, postedDate = @PostedDate WHERE invoiceID = @invoiceID";
            cmd.Parameters.AddWithValue("invoiceID", invoiceID);
            cmd.Parameters.AddWithValue("postedDate", PostedDate);
            //Declare and open connection
            cmd.Connection = con;
            con.Open();
            //Execute update
            cmd.ExecuteNonQuery();
            con.Close();
        }

        /*******Sale Utilities************************************************************************************/

        //Get sale by invoiceID
        public void getSaleByInvoiceIDAndSKU(int invoiceID, int SKU)
        {
            //New command
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM Sale WHERE invoiceID = @invoiceID AND SKU = @SKU";
            cmd.Parameters.AddWithValue("invoiceID", invoiceID);
            //Declare and open connection
            cmd.Connection = con;
            con.Open();
            //Execute Update
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void addSale(Sale s)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "INSERT INTO Sale( invoiceID, sku, quantity) VALUES (@invoiceID, @sku, @quantity)";

            cmd.Parameters.AddWithValue("invoiceID", s.invoiceId);
            cmd.Parameters.AddWithValue("sku", s.sku);
            cmd.Parameters.AddWithValue("quantity", s.quantity);
            cmd.Connection = con;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void returnUpdatedQuantity(int sku, int quantityInOrder)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "UPDATE Item SET quantity = quantity - @quantityInOrder WHERE sku = @sku";
            cmd.Parameters.AddWithValue("sku", sku);
            cmd.Parameters.AddWithValue("quantityInOrder", quantityInOrder);

            cmd.Connection = con;
            con.Open();

            cmd.ExecuteNonQuery();
            con.Close();
        }

        //Delete sale of specific item on an invoice
        public void deleteSale(int invoiceID, int sku)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            //Delete item from sale table
            cmd.CommandText = "DELETE FROM Sale WHERE invoiceID = @invoiceID AND sku=@sku";
            cmd.Parameters.AddWithValue("sku", sku);
            cmd.Parameters.AddWithValue("invoiceID", invoiceID);

            //Declare and open connection
            cmd.Connection = con;
            con.Open();

            //Execute Update
            cmd.ExecuteNonQuery();
            con.Close();
        }

        //Get Sale object for a specific item on an invoice
        public List<Sale> getSale(int invoiceID, int sku)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            //Delete item from sale table
            cmd.CommandText = "Select * FROM Sale WHERE invoiceID = @invoiceID AND sku=@sku";
            cmd.Parameters.AddWithValue("sku", sku);
            cmd.Parameters.AddWithValue("invoiceID", invoiceID);

            //Declare and open connection
            cmd.Connection = con;
            con.Open();
            List<Sale> s = new List<Sale>();
            //Initialize SQLdatareader and initialize new sale object with results
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                s.Add(new Sale(Convert.ToInt32(reader["invoiceID"]), Convert.ToInt32(reader["sku"]),
                Convert.ToInt32(reader["quantity"])));
            }
            //Close Connection
            con.Close();

            //return sale object
            return s;
        }

        //Get Sale object for a specific item on an invoice
        public Sale getSaleByInvAndSKU(int invoiceID, int sku)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            //Delete item from sale table
            cmd.CommandText = "Select * FROM Sale WHERE invoiceID = @invoiceID AND sku=@sku";
            cmd.Parameters.AddWithValue("sku", sku);
            cmd.Parameters.AddWithValue("invoiceID", invoiceID);

            //Declare and open connection
            cmd.Connection = con;
            con.Open();
            Sale s = new Sale();
            //Initialize SQLdatareader and initialize new sale object with results
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                s = (new Sale(Convert.ToInt32(reader["invoiceID"]), Convert.ToInt32(reader["sku"]),
                Convert.ToInt32(reader["quantity"])));
            }
            //Close Connection
            con.Close();

            //return sale object
            return s;
        }

        /*******Tax Utilities************************************************************************************/

        public List<Tax> getTaxes(int provStateID)
        {
            //New command
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select tr.taxRate, tbl_taxType.taxName from tbl_taxRate tr"
                            + " inner Join tbl_taxType on tr.taxID = tbl_taxType.taxID"
                            + " inner join (select taxID, max(taxDate) as MTD from tbl_taxRate where provStateID = @provStateID Group By taxID) td"
                            + " on tr.taxID = td.taxID and tr.taxDate = td.MTD where provStateID = @provStateID;";
            cmd.Parameters.AddWithValue("provStateID", provStateID);
            //Declare and open connection
            cmd.Connection = con;
            con.Open();
            List<Tax> tax = new List<Tax>();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Tax t = new Tax(reader["taxName"].ToString(),
                            Convert.ToDouble(reader["taxRate"]));
                tax.Add(t);
            }
            con.Close();
            return tax;
        }

        //    public void updateTax(int regionID, double gst, double pst)
        //{
        //    SqlConnection con = new SqlConnection(connectionString);
        //    SqlCommand cmd = new SqlCommand();

        //    cmd.CommandText = "UPDATE StateProvLT SET gst = @gst, pst = @pst WHERE stateProvID = @stateProvID";
        //    cmd.Parameters.AddWithValue("@stateProvID", regionID);
        //    cmd.Parameters.AddWithValue("@gst", gst);
        //    cmd.Parameters.AddWithValue("@pst", pst);

        //    //Declare and open connection
        //    cmd.Connection = con;
        //    con.Open();

        //    //Execute Insert
        //    cmd.ExecuteNonQuery();
        //    con.Close();
        //}

        //public Tax getTaxByStateProvID(int stateProvID)
        //{
        //    //New command
        //    SqlConnection con = new SqlConnection(connectionString);
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandText = "SELECT gst, pst FROM StateProvLT WHERE stateProvID = @stateProvID";
        //    cmd.Parameters.AddWithValue("stateProvID", stateProvID);

        //    //Declare and open connection
        //    cmd.Connection = con;
        //    con.Open();
        //    Tax t = new Tax();
        //    SqlDataReader read = cmd.ExecuteReader();
        //    while (read.Read())
        //    {
        //            t = new Tax(Convert.ToDouble(read["gst"]),
        //            Convert.ToDouble(read["pst"]));
        //    }
        //    con.Close();
        //    return t;
        //}

        //*******Report Utilities************************************************************************************/

        //Export customer table to excel file in users Downloads folder
        public void exportCustomers()
        {
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Customer", sqlCon);
            DataTable dtMainSQLData = new DataTable();
            da.Fill(dtMainSQLData);
            DataColumnCollection dcCollection = dtMainSQLData.Columns;

            // Export Data into EXCEL Sheet
            Microsoft.Office.Interop.Excel.Application ExcelApp = new
            Microsoft.Office.Interop.Excel.Application();
            ExcelApp.Application.Workbooks.Add(Type.Missing);

            for (int i = 1; i < dtMainSQLData.Rows.Count + 2; i++)
            {
                for (int j = 1; j < dtMainSQLData.Columns.Count + 1; j++)
                {
                    if (i == 1)
                    {
                        ExcelApp.Cells[i, j] = dcCollection[j - 1].ToString();
                    }
                    else
                        ExcelApp.Cells[i, j] = dtMainSQLData.Rows[i - 2][j - 1].ToString();
                }
            }
            //Get users profile, downloads folder path, and save to workstation
            string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string pathDownload = Path.Combine(pathUser, "Downloads");
            ExcelApp.ActiveWorkbook.SaveCopyAs(pathDownload + "\\Customers-" + DateTime.Now.ToString("d MMM yyyy") + ".xlsx");
            ExcelApp.ActiveWorkbook.Saved = true;
            ExcelApp.Quit();
        }

        //Export Inventory Sales to excel file in users Downloads folder
        public void exportInvoices(DateTime startDate, DateTime endDate)
        {
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            string sqlStatement = "SELECT Invoice.invoiceID, Item.SKU, Sale.quantity, Item.brand, Item.model, Item.clubType, Item.shaft, Item.numberOfClubs, Item.clubSpec, Item.shaftSpec, Item.shaftFlex, Item.dexterity, Item.wePay, Item.retailPrice FROM Invoice INNER JOIN Customer ON Invoice.customerID = Customer.customerId INNER JOIN Sale ON Invoice.invoiceID = Sale.invoiceID INNER JOIN Item ON Sale.SKU = Item.SKU WHERE (Invoice.saleDate BETWEEN '" + startDate + "' AND '" + endDate + "')";
            SqlDataAdapter da = new SqlDataAdapter(sqlStatement, sqlCon);
            DataTable dtMainSQLData = new DataTable();
            da.Fill(dtMainSQLData);
            DataColumnCollection dcCollection = dtMainSQLData.Columns;

            // Export Data into EXCEL Sheet
            Microsoft.Office.Interop.Excel.Application ExcelApp = new
            Microsoft.Office.Interop.Excel.Application();
            ExcelApp.Application.Workbooks.Add(Type.Missing);

            for (int i = 1; i < dtMainSQLData.Rows.Count + 2; i++)
            {
                for (int j = 1; j < dtMainSQLData.Columns.Count + 1; j++)
                {
                    if (i == 1)
                    {
                        ExcelApp.Cells[i, j] = dcCollection[j - 1].ToString();
                    }
                    else
                        ExcelApp.Cells[i, j] = dtMainSQLData.Rows[i - 2][j - 1].ToString();
                }
            }
            //Get users profile, downloads folder path, and save to workstation
            string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string pathDownload = Path.Combine(pathUser, "Downloads");
            ExcelApp.ActiveWorkbook.SaveCopyAs(pathDownload + "\\Inventory Sales-" + DateTime.Now.ToString("d MMM yyyy") + ".xlsx");
            ExcelApp.ActiveWorkbook.Saved = true;
            ExcelApp.Quit();
        }

        //Export items table to excel file in users Downloads folder
        public void exportItems()
        {
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM item", sqlCon);
            DataTable dtMainSQLData = new DataTable();
            da.Fill(dtMainSQLData);
            DataColumnCollection dcCollection = dtMainSQLData.Columns;

            // Export Data into EXCEL Sheet
            Microsoft.Office.Interop.Excel.Application ExcelApp = new
            Microsoft.Office.Interop.Excel.Application();
            ExcelApp.Application.Workbooks.Add(Type.Missing);

            for (int i = 1; i < dtMainSQLData.Rows.Count + 2; i++)
            {
                for (int j = 1; j < dtMainSQLData.Columns.Count + 1; j++)
                {
                    if (i == 1)
                    {
                        ExcelApp.Cells[i, j] = dcCollection[j - 1].ToString();
                    }
                    else
                        ExcelApp.Cells[i, j] = dtMainSQLData.Rows[i - 2][j - 1].ToString();
                }
            }
            //Get users profile, downloads folder path, and save to workstation
            string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string pathDownload = Path.Combine(pathUser, "Downloads");
            ExcelApp.ActiveWorkbook.SaveCopyAs(pathDownload + "\\Inventory-" + DateTime.Now.ToString("d MMM yyyy") + ".xlsx");
            ExcelApp.ActiveWorkbook.Saved = true;
            ExcelApp.Quit();
        }

        //Export Invoice table to excel 
        public void exportInvoices()
        {
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            string sqlStatement = "SELECT * FROM Invoice";
            SqlDataAdapter da = new SqlDataAdapter(sqlStatement, sqlCon);
            DataTable dtMainSQLData = new DataTable();
            da.Fill(dtMainSQLData);
            DataColumnCollection dcCollection = dtMainSQLData.Columns;

            // Export Data into EXCEL Sheet
            Microsoft.Office.Interop.Excel.Application ExcelApp = new
            Microsoft.Office.Interop.Excel.Application();
            ExcelApp.Application.Workbooks.Add(Type.Missing);

            for (int i = 1; i < dtMainSQLData.Rows.Count + 2; i++)
            {
                for (int j = 1; j < dtMainSQLData.Columns.Count + 1; j++)
                {
                    if (i == 1)
                    {
                        ExcelApp.Cells[i, j] = dcCollection[j - 1].ToString();
                    }
                    else
                        ExcelApp.Cells[i, j] = dtMainSQLData.Rows[i - 2][j - 1].ToString();
                }
            }
            //Get users profile, downloads folder path, and save to workstation
            string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string pathDownload = Path.Combine(pathUser, "Downloads");
            ExcelApp.ActiveWorkbook.SaveCopyAs(pathDownload + "\\Inventory-" + DateTime.Now.ToString("d MMM yyyy") + ".xlsx");
            ExcelApp.ActiveWorkbook.Saved = true;
            ExcelApp.Quit();
        }

        //************************************Inventory Sales Utilities********************************************/

        //This method will select all invoices involved in a sale between the specified dates and encapsulated them in a list.
        public List<InvSale> selectAllInventorySales(DateTime startDate, DateTime endDate)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT Invoice.invoiceID, Item.SKU, Sale.quantity, Item.brand, Item.model, Item.clubType, Item.shaft, Item.numberOfClubs, Item.clubSpec, Item.shaftSpec, Item.shaftFlex, Item.dexterity, Item.wePay, Item.retailPrice FROM Invoice INNER JOIN Customer ON Invoice.customerID = Customer.customerId INNER JOIN Sale ON Invoice.invoiceID = Sale.invoiceID INNER JOIN Item ON Sale.SKU = Item.SKU WHERE (Invoice.saleDate BETWEEN @startDate AND @endDate)";
            cmd.Parameters.AddWithValue("startDate", startDate);
            cmd.Parameters.AddWithValue("endDate", endDate);

            cmd.Connection = con;
            con.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            List<InvSale> ivs = new List<InvSale>();
            while (reader.Read())
            {
                InvSale isl = new InvSale(Convert.ToInt32(reader["invoiceID"]),
                    Convert.ToInt32(reader["sku"]),
                    Convert.ToInt32(reader["quantity"]),
                    Convert.ToString(reader["brand"]),
                    Convert.ToString(reader["model"]),
                    Convert.ToString(reader["clubType"]),
                    Convert.ToString(reader["shaft"]),
                    Convert.ToString(reader["numberOfClubs"]),
                    Convert.ToString(reader["clubSpec"]),
                    Convert.ToString(reader["shaftSpec"]),
                    Convert.ToString(reader["shaftFlex"]),
                    Convert.ToString(reader["dexterity"]),
                    Convert.ToDouble(reader["wePay"]),
                    Convert.ToDouble(reader["retailPrice"]));

                ivs.Add(isl);
            }

            con.Close();
            return ivs;

        }

        public double returnDiscount(List<Cart> itemsSold)
        {
            double singleDiscoount = 0;
            double totalDiscount = 0;
            foreach (var cart in itemsSold)
            {
                if (cart.percentage)
                {
                    singleDiscoount = cart.quantity * (cart.price * (cart.discount / 100));
                }
                else
                {
                    singleDiscoount = cart.quantity * cart.discount;
                }
                totalDiscount += singleDiscoount;
            }
            return Math.Round(totalDiscount, 2);
        }
        public double returnTradeInAmount(List<Cart> itemsSold)
        {
            double singleTradeInAmount = 0;
            double totalTradeinAmount = 0;
            ItemDataUtilities idu = new ItemDataUtilities();
            int[] range = idu.tradeInSkuRange(0);
            foreach (var cart in itemsSold)
            {
                if (cart.sku <= range[1] && cart.sku >= range[0])
                {
                    singleTradeInAmount = cart.quantity * cart.price;
                    totalTradeinAmount += singleTradeInAmount;
                }
            }
            return totalTradeinAmount;
        }
        public double returnSubtotalAmount(List<Cart> itemsSold)
        {
            double totalSubtotalAmount = 0;
            double totalDiscountAmount = returnDiscount(itemsSold);
            double totalTradeInAmount = returnTradeInAmount(itemsSold);
            double totalTotalAmount = returnTotalAmount(itemsSold);

            totalSubtotalAmount = totalSubtotalAmount + totalTotalAmount;
            totalSubtotalAmount = totalSubtotalAmount - totalDiscountAmount;
            totalSubtotalAmount = totalSubtotalAmount - (totalTradeInAmount * (-1));
            return totalSubtotalAmount;
        }
        public double returnTotalAmount(List<Cart> itemsSold)
        {
            ItemDataUtilities idu = new ItemDataUtilities();
            int[] range = idu.tradeInSkuRange(0);
            double singleTotalAmount = 0;
            double totalTotalAmount = 0;
            foreach (var cart in itemsSold)
            {
                if (cart.sku >= range[1] || cart.sku <= range[0])
                {
                    singleTotalAmount = cart.quantity * cart.price;
                    totalTotalAmount += singleTotalAmount;
                }
            }
            return totalTotalAmount;
        }

        //Transfering the trade in item to the clubs table
        public void transferTradeInStart(List<Cart> Clubs)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            int[] range = idu.tradeInSkuRange(0);
            Object o;

            foreach (Cart ti in Clubs)
            {
                if (ti.sku <= range[1] && ti.sku >= range[0])
                {
                    cmd.CommandText = "Select * from tbl_tempTradeInCartSkus Where sku = " + ti.sku;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();


                    while (reader.Read())
                    {
                        Clubs transferClub = new Clubs
                        (Convert.ToInt32(reader["sku"]),
                        Convert.ToInt32(reader["brandID"].ToString()),
                        Convert.ToInt32(reader["modelID"].ToString()),
                        Convert.ToInt32(reader["typeID"].ToString()),
                        reader["clubType"].ToString(),
                        reader["shaft"].ToString(),
                        reader["numberOfClubs"].ToString(),
                        Convert.ToDouble(reader["premium"]),
                        Convert.ToDouble(reader["cost"]),
                        Convert.ToDouble(reader["price"]),
                        Convert.ToInt32(reader["quantity"]),
                        reader["clubSpec"].ToString(),
                        reader["shaftSpec"].ToString(),
                        reader["shaftFlex"].ToString(),
                        reader["dexterity"].ToString(),
                        Convert.ToInt32(reader["locationID"].ToString()),
                        Convert.ToBoolean(reader["used"].ToString()),
                        reader["comments"].ToString());



                        o = transferClub as Object;
                        addItem(o);
                        transferTradeInDeleteOld(transferClub.sku);
                    }
                    conn.Close();
                }
            }
        }
        public void transferTradeInDeleteOld(int sku)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;

            cmd.CommandText = "Delete from tbl_tempTradeInCartSkus Where sku = " + sku;
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            conn.Close();
        }
    }
}