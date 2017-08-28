using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.IO;
using System.Web.UI.WebControls;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using SweetShop;
using SweetSpotProShop;
using System.Threading;
using System.Diagnostics;
using System.Text;
using OfficeOpenXml;

namespace SweetSpotDiscountGolfPOS.ClassLibrary
{
    public class Reports
    {
        string connectionString;
        List<Cashout> cashout = new List<Cashout>();
        List<Cashout> remainingCashout = new List<Cashout>();
        Clubs c = new Clubs();
        Accessories a = new Accessories();
        Clothing cl = new Clothing();
        Customer cu = new Customer();
        SweetShopManager ssm = new SweetShopManager();
        ItemDataUtilities idu = new ItemDataUtilities();
        LocationManager lm = new LocationManager();
        Object o = new Object();
        private System.Data.DataTable exportTable;
        private System.Data.DataTable exportInvoiceTable;
        private System.Data.DataTable exportInvoiceItemTable;
        private System.Data.DataTable exportInvoiceMOPTable;

        public Reports()
        {
            connectionString = ConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString;
        }

        //*******************CASHOUT UTILITIES*******************************************************
        //Cashout
        public List<Cashout> cashoutAmounts(DateTime startDate, DateTime endDate, int locationID)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Select tbl_invoiceMOP.mopType, tbl_invoiceMOP.amountPaid, tbl_invoice.tradeinAmount, " +
                "tbl_invoice.subTotal, tbl_invoice.governmentTax, tbl_invoice.provincialTax from tbl_invoiceMOP " +
                "INNER JOIN tbl_invoice ON tbl_invoiceMOP.invoiceNum = tbl_invoice.invoiceNum " +
                " where tbl_invoice.invoiceDate between @startDate and @endDate and tbl_invoice.locationID = @locationID;";
            cmd.Parameters.AddWithValue("@startDate", startDate);
            cmd.Parameters.AddWithValue("@endDate", endDate);
            cmd.Parameters.AddWithValue("@locationID", locationID);

            cmd.Connection = con;
            con.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Cashout cs = new Cashout(
                    Convert.ToString(reader["mopType"]),
                    Convert.ToDouble(reader["amountPaid"]),
                    Convert.ToDouble(reader["tradeinAmount"]));
                    

                cashout.Add(cs);
            }
            con.Close();
            return cashout;
        }
        public List<Cashout> getRemainingCashout(DateTime startDate, DateTime endDate, int locationID)
        {

            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Select subTotal, governmentTax, provincialTax from  tbl_invoice" +                
                " where invoiceDate between @startDate and @endDate and locationID = @locationID;";
            cmd.Parameters.AddWithValue("@startDate", startDate);
            cmd.Parameters.AddWithValue("@endDate", endDate);
            cmd.Parameters.AddWithValue("@locationID", locationID);

            cmd.Connection = con;
            con.Open();

            SqlDataReader reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                Cashout cs = new Cashout(
                    Convert.ToDouble(reader["governmentTax"]),
                    Convert.ToDouble(reader["provincialTax"]),
                    Convert.ToDouble(reader["subTotal"]));


                remainingCashout.Add(cs);
            }
            con.Close();

            return remainingCashout;
        }
        public double getTradeInsCashout(DateTime startDate, DateTime endDate, int locationID)
        {
            double tradeintotal = 0;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Select tradeinAmount from  tbl_invoice" +
                " where invoiceDate between @startDate and @endDate and locationID = @locationID;";
            cmd.Parameters.AddWithValue("@startDate", startDate);
            cmd.Parameters.AddWithValue("@endDate", endDate);
            cmd.Parameters.AddWithValue("@locationID", locationID);

            cmd.Connection = con;
            con.Open();

            SqlDataReader reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                tradeintotal += Convert.ToDouble(reader["tradeinAmount"]);                
            }
            con.Close();

            return tradeintotal;
        }
        //Insert the cashout into the database
        public void insertCashout(Cashout cas)
        {
            int processed = 0;
            int finalized = 0;
            if (cas.processed == true)
            {
                processed = 1;
            }
            else
            {
                processed = 0;
            }
            if (cas.finalized == true)
            {
                finalized = 1;
            }
            else
            {
                finalized = 0;
            }

            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Insert into tbl_cashout values('" +
               cas.date + "', '" + cas.time + "', " + cas.saleTradeIn + ", " + cas.saleGiftCard + ", " +
               cas.saleCash + ", "  + cas.saleDebit + ", " + cas.saleMasterCard + ", " +
               cas.saleVisa + ", " +  cas.receiptTradeIn + ", " + cas.receiptGiftCard + ", " +
               cas.receiptCash + ", "  + cas.receiptDebit + ", " + cas.receiptMasterCard + ", " + cas.receiptVisa + ", " +
               cas.preTax + ", " + cas.saleGST + ", " +  cas.salePST + ", " + 
               cas.overShort + ", " + finalized + ", " + processed + ");";

            //" saleTradeIn = @saleTradeIn, " +
            //" saleGiftCard = @saleGiftCard, saleCash = @saleCash, saleCheque = @saleCheque, " +
            //" saleDebit = @saleDebit, saleMasterCard = @saleMasterCard, saleVisa = @saleVisa, " +
            //" saleAmex = @saleAmex, receiptTradeIn = @receiptTradeIn, receiptGiftCard = @receiptGiftCard, " +
            //" receiptCash = @receiptCash, receiptCheque = @receiptCheque, receiptDebit = @receiptDebit, " +
            //" receiptMasterCard = @receiptMasterCard, receiptVisa = @receiptVisa, receiptAmex = @receiptAmex, " +
            //" overShort = @overShort, finalized = @finalized, processed = @processed);";

            //cmd.Parameters.AddWithValue("@cashoutDate", cas.date);
            //cmd.Parameters.AddWithValue("@cashoutTime", cas.time);
            //cmd.Parameters.AddWithValue("@saleTradeIn", cas.saleTradeIn);

            //cmd.Parameters.AddWithValue("@saleGiftCard", cas.saleGiftCard);
            //cmd.Parameters.AddWithValue("@saleCash", cas.saleCash);
            //cmd.Parameters.AddWithValue("@saleCheque", cas.saleCheque);

            //cmd.Parameters.AddWithValue("@saleDebit", cas.saleDebit);
            //cmd.Parameters.AddWithValue("@saleMasterCard", cas.saleMasterCard);
            //cmd.Parameters.AddWithValue("@saleVisa", cas.saleVisa);

            //cmd.Parameters.AddWithValue("@saleAmex", cas.saleAmex);
            //cmd.Parameters.AddWithValue("@receiptTradeIn", cas.receiptTradeIn);
            //cmd.Parameters.AddWithValue("@receiptGiftCard", cas.receiptGiftCard);

            //cmd.Parameters.AddWithValue("@receiptCash", cas.receiptCash);
            //cmd.Parameters.AddWithValue("@receiptCheque", cas.receiptCheque);
            //cmd.Parameters.AddWithValue("@receiptDebit", cas.receiptDebit);

            //cmd.Parameters.AddWithValue("@receiptMasterCard", cas.receiptMasterCard);
            //cmd.Parameters.AddWithValue("@receiptVisa", cas.receiptVisa);
            //cmd.Parameters.AddWithValue("@receiptAmex", cas.receiptAmex);

            //cmd.Parameters.AddWithValue("@overShort", cas.overShort);
            //cmd.Parameters.AddWithValue("@finalized", finalized);
            //cmd.Parameters.AddWithValue("@processed", processed);

            cmd.Connection = con;
            con.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            con.Close();
        }

        //********************IMPORTING***************************************************************
        public void importItems(FileUpload fup)
        {
            List<Clubs> listClub = new List<Clubs>();
            List<Clothing> listClothing = new List<Clothing>();
            List<Accessories> listAccessories = new List<Accessories>();

            //check if there is actually a file being uploaded
            if (fup.HasFile)
            {
                //load the uploaded file into the memorystream
                using (MemoryStream stream = new MemoryStream(fup.FileBytes))


                using (ExcelPackage xlPackage = new ExcelPackage(stream))
                {

                    // get the first worksheet in the workbook
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[1];
                    var rowCnt = worksheet.Dimension.End.Row;
                    var colCnt = worksheet.Dimension.End.Column;


                    //Beginning the loop for data gathering
                    for (int i = 2; i < rowCnt; i++)
                    {
                        string itemType;
                        try
                        {
                            itemType = (worksheet.Cells[i, 5].Value).ToString();
                        }
                        catch (Exception ex)
                        {
                            itemType = "";
                        }


                        if (worksheet.Row(i) != null && worksheet.Cells[i, 5].Value != null)
                        {
                            if (itemType == null) { }
                            else if (itemType.Equals("")) { }
                            //***************ACCESSORIES*********
                            else if (itemType.Equals("Accessories") || itemType.Equals("accessories"))
                            {
                                //***************SKU***************
                                if (!Convert.ToInt32(worksheet.Cells[i, 3].Value).Equals(null))
                                {
                                    a.sku = Convert.ToInt32(worksheet.Cells[i, 3].Value);
                                }
                                else
                                {
                                    a.sku = 0;
                                }
                                //***************BRAND ID***************
                                int bName = idu.brandName(itemType.ToString());
                                if (!bName.Equals(null))
                                {
                                    a.brandID = bName;
                                }
                                else
                                {
                                    a.brandID = 1;
                                }
                                //***************MODEL ID***************                                
                                try
                                {
                                    string mName;
                                    mName = (worksheet.Cells[i, 6].Value).ToString();
                                    if (mName == null)
                                    {
                                        a.modelID = 1;
                                    }
                                    else
                                    {
                                        int mID = idu.modelName(mName);
                                        if (!mID.Equals(null))
                                            if (mName == "360") { a.modelID = 17; }
                                            else { a.modelID = mID; }

                                        else
                                            a.modelID = 1;
                                    }
                                }
                                catch (Exception e)
                                {
                                    a.modelID = 1427;
                                }
                                //***************ACCESSORY TYPE***************
                                try
                                {
                                    if ((string)(worksheet.Cells[i, 7].Value) != null)
                                    {
                                        a.accessoryType = (string)(worksheet.Cells[i, 7].Value);
                                    }
                                    else
                                    {
                                        a.accessoryType = "";
                                    }
                                }
                                catch(Exception ex)
                                {
                                    a.accessoryType = "";
                                }
                                //***************COST***************
                                try
                                {
                                    if (!Convert.ToDouble(worksheet.Cells[i, 12].Value).Equals(null))
                                    {
                                        a.cost = Convert.ToDouble(worksheet.Cells[i, 12].Value);
                                    }
                                    else
                                    {
                                        a.cost = 0;
                                    }
                                }
                                catch(Exception ex)
                                {
                                    a.cost = 0;
                                }
                                //***************PRICE***************
                                try
                                {
                                    if (!Convert.ToDouble(worksheet.Cells[i, 15].Value).Equals(null))
                                    {
                                        a.price = Convert.ToDouble(worksheet.Cells[i, 15].Value);
                                    }
                                    else
                                    {
                                        a.price = 0;
                                    }
                                }
                                catch(Exception ex)
                                {
                                    a.price = 0;
                                }
                                //***************QUANTITY***************
                                try
                                {
                                    if (!Convert.ToInt32(worksheet.Cells[i, 13].Value).Equals(null))
                                    {
                                        a.quantity = Convert.ToInt32(worksheet.Cells[i, 13].Value);
                                    }
                                    else
                                    {
                                        a.quantity = 0;
                                    }
                                }
                                catch(Exception ex)
                                {
                                    a.quantity = 0;
                                }
                                //***************COMMENTS***************
                                try
                                {
                                    if (!(worksheet.Cells[i, 16].Value).Equals(null))
                                    {
                                        a.comments = (string)(worksheet.Cells[i, 16].Value);
                                    }
                                    else
                                    {
                                        a.comments = "";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    a.comments = "";
                                }

                                a.locID = 1;//
                                a.typeID = 2;
                                a.size = "";
                                a.colour = "";

                                listAccessories.Add(a);
                                o = a as Object;
                            }
                            //***************APPAREL*************
                            else if (itemType.Equals("Apparel") || itemType.Equals("apparel"))
                            {
                                //***************SKU***************
                                if (!Convert.ToInt32(worksheet.Cells[i, 3].Value).Equals(null))
                                {
                                    cl.sku = Convert.ToInt32(worksheet.Cells[i, 3].Value);
                                }
                                else
                                {
                                    cl.sku = 0;
                                }
                                //***************BRAND ID***************                                
                                int bName = idu.brandName(itemType.ToString());
                                if (!bName.Equals(null))
                                {
                                    cl.brandID = bName;
                                }
                                else
                                {
                                    cl.brandID = 1;
                                }
                                //***************COST***************
                                try
                                {
                                    if (!Convert.ToDouble(worksheet.Cells[i, 12].Value).Equals(null))
                                    {
                                        cl.cost = Convert.ToDouble(worksheet.Cells[i, 12].Value);
                                    }
                                    else
                                    {
                                        cl.cost = 0;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    cl.cost = 0;
                                }
                                //***************PRICE***************
                                try
                                {
                                    if (!Convert.ToDouble(worksheet.Cells[i, 15].Value).Equals(null))
                                    {
                                        cl.price = Convert.ToDouble(worksheet.Cells[i, 15].Value);
                                    }
                                    else
                                    {
                                        cl.price = 0;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    cl.price = 0;
                                }
                                //***************QUANTITY***************
                                try
                                {
                                    if (!Convert.ToInt32(worksheet.Cells[i, 13].Value).Equals(null))
                                    {
                                        cl.quantity = Convert.ToInt32(worksheet.Cells[i, 13].Value);
                                    }
                                    else
                                    {
                                        cl.quantity = 0;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    cl.quantity = 0;
                                }
                                //***************GENDER***************
                                try
                                {
                                    if (!(worksheet.Cells[i, 6].Value).Equals(null))
                                    {
                                        cl.gender = (string)(worksheet.Cells[i, 6].Value);
                                    }
                                    else
                                    {
                                        cl.gender = "";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    cl.gender = "";
                                }
                                //***************STYLE***************
                                try
                                {
                                    if (!(worksheet.Cells[i, 7].Value).Equals(null))
                                    {
                                        cl.style = (string)(worksheet.Cells[i, 7].Value);
                                    }
                                    else
                                    {
                                        cl.style = "";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    cl.style = "";
                                }
                                //***************COMMENTS***************
                                try
                                {
                                    if (!(worksheet.Cells[i, 16].Value).Equals(null))
                                    {
                                        cl.comments = (string)(worksheet.Cells[i, 16].Value);
                                    }
                                    else
                                    {
                                        cl.comments = "";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    cl.comments = "";
                                }

                                cl.locID = 1;//
                                cl.typeID = 3; //Type can be directly assigned
                                cl.size = "";
                                cl.colour = "";

                                o = cl as Object;
                            }
                            //***************CLUBS***************
                            else
                            {
                                //***************SKU***************
                                if (!Convert.ToInt32(worksheet.Cells[i, 3].Value).Equals(null))
                                {
                                    c.sku = Convert.ToInt32(worksheet.Cells[i, 3].Value);
                                }
                                else
                                {
                                    c.sku = 0;
                                }
                                //***************BRAND ID***************
                                int bName = idu.brandName(itemType.ToString());
                                if (!bName.Equals(null))
                                {
                                    c.brandID = bName;
                                }
                                else
                                {
                                    c.brandID = 1;
                                }
                                //***************MODEL ID***************                                
                                try
                                {
                                    string mName;
                                    mName = (worksheet.Cells[i, 6].Value).ToString();
                                    if (mName == null)
                                    {
                                        c.modelID = 1;
                                    }
                                    else
                                    {
                                        int mID = idu.modelName(mName);
                                        if (!mID.Equals(null))
                                            if (mName == "360") { c.modelID = 17; }
                                            else { c.modelID = mID; }

                                        else
                                            c.modelID = 1;
                                    }
                                }
                                catch (Exception e)
                                {
                                    c.modelID = 1427;
                                }
                                //***************COST***************
                                try
                                {
                                    if (!Convert.ToDouble(worksheet.Cells[i, 12].Value).Equals(null))
                                    {
                                        c.cost = Convert.ToDouble(worksheet.Cells[i, 12].Value);
                                    }
                                    else
                                    {
                                        c.cost = 0;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    c.cost = 0;
                                }
                                //***************PRICE***************
                                try
                                {
                                    if (!Convert.ToDouble(worksheet.Cells[i, 15].Value).Equals(null))
                                    {
                                        c.price = Convert.ToDouble(worksheet.Cells[i, 15].Value);
                                    }
                                    else
                                    {
                                        c.price = 0;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    c.price = 0;
                                }
                                //***************QUANTITY***************
                                try
                                {
                                    if (!Convert.ToInt32(worksheet.Cells[i, 13].Value).Equals(null))
                                    {
                                        c.quantity = Convert.ToInt32(worksheet.Cells[i, 13].Value);
                                    }
                                    else
                                    {
                                        c.quantity = 0;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    c.quantity = 0;
                                }
                                //***************COMMENTS***************
                                try
                                {
                                    if (!(worksheet.Cells[i, 16].Value).Equals(null))
                                    {
                                        c.comments = (string)(worksheet.Cells[i, 16].Value);
                                    }
                                    else
                                    {
                                        c.comments = "";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    c.comments = "";
                                }
                                //***************PREMIUM***************
                                try
                                {
                                    if (!Convert.ToDouble(worksheet.Cells[i, 11].Value).Equals(null))
                                    {
                                        c.premium = Convert.ToDouble(worksheet.Cells[i, 11].Value);
                                    }
                                    else
                                    {
                                        c.premium = 0;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    c.premium = 0;
                                }
                                //***************CLUB TYPE***************
                                try
                                {
                                    if (!(worksheet.Cells[i, 7].Value).Equals(null))
                                    {
                                        c.clubType = (string)(worksheet.Cells[i, 7].Value);
                                    }
                                    else
                                    {
                                        c.clubType = "";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    c.clubType = "";
                                }
                                //***************SHAFT***************
                                try
                                {
                                    if (!(worksheet.Cells[i, 8].Value).Equals(null))
                                    {
                                        c.shaft = (string)(worksheet.Cells[i, 8].Value);
                                    }
                                    else
                                    {
                                        c.shaft = "";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    c.shaft = "";
                                }
                                //***************NUMBER OF CLUBS***************
                                try
                                {
                                    if (!(worksheet.Cells[i, 9].Value).Equals(null))
                                    {
                                        c.numberOfClubs = (string)(worksheet.Cells[i, 9].Value);
                                    }
                                    else
                                    {
                                        c.numberOfClubs = "";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    c.numberOfClubs = "";
                                }
                                //***************CLUB SPEC***************
                                try
                                {
                                    if (!(worksheet.Cells[i, 18].Value).Equals(null))
                                    {
                                        c.clubSpec = (string)(worksheet.Cells[i, 18].Value);
                                    }
                                    else
                                    {
                                        c.clubSpec = "";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    c.clubSpec = "";
                                }
                                //***************SHAFT SPEC***************
                                try
                                {
                                    if (!(worksheet.Cells[i, 19].Value).Equals(null))
                                    {
                                        c.shaftSpec = (string)(worksheet.Cells[i, 19].Value);
                                    }
                                    else
                                    {
                                        c.shaftSpec = "";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    c.shaftSpec = "";
                                }
                                //***************SHAFT FLEX***************
                                try
                                {
                                    if (!(worksheet.Cells[i, 20].Value).Equals(null))
                                    {
                                        c.shaftFlex = (string)(worksheet.Cells[i, 20].Value);
                                    }
                                    else
                                    {
                                        c.shaftFlex = "";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    c.shaftFlex = "";
                                }
                                //***************DEXTERITY***************
                                try
                                {
                                    if (!(worksheet.Cells[i, 21].Value).Equals(null))
                                    {
                                        c.dexterity = (string)(worksheet.Cells[i, 21].Value);
                                    }
                                    else
                                    {
                                        c.dexterity = "";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    c.dexterity = "";
                                }

                                c.typeID = 1;
                                c.itemlocation = 1;//
                                c.used = false;//

                                listClub.Add(c);
                                o = c as Object;
                            }
                            ssm.checkForItem(o);
                        }
                    }
                }
            }

        }
        public void importCustomers(FileUpload fup)
        {
            Excel.Application xlApp = new Excel.Application();
            //string path = fup.PostedFile.FileName;
            //System.Web.HttpContext.Current.Server.MapPath(fup.FileName)
            string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string path = Path.Combine(pathUser, "Downloads\\");
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(path + fup.FileName);
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            int rowCount = xlRange.Rows.Count;
            int colCount = xlRange.Columns.Count;

            for (int i = 2; i <= rowCount; i++)
            {
                string itemType = (string)((xlRange.Cells[i, 5] as Range).Value2);

                //Write the value to the console, and start gathering item info for insert
                if (xlRange.Cells[i] != null && xlRange.Cells[i].Value2 != null)
                {
                    //tbl_customers: custID, firstName, lastName, primaryAddress, secondaryAddress, primaryPhoneINT, secondaryPhoneINT
                    //billingAddress, email, city, provStateID, country, postZip                    
                    //First Name
                    if ((xlWorksheet.Cells[i, 2] as Range).Value2 != null)
                        cu.firstName = (xlWorksheet.Cells[i, 2] as Range).Value2;
                    else
                        cu.firstName = "";
                    //Last Name
                    if ((xlWorksheet.Cells[i, 3] as Range).Value2 != null)
                        cu.lastName = (xlWorksheet.Cells[i, 3] as Range).Value2;
                    else
                        cu.lastName = "";
                    //primaryAddress
                    if ((xlWorksheet.Cells[i, 5] as Range).Value2 != null)
                        cu.primaryAddress = (xlWorksheet.Cells[i, 5] as Range).Value2;
                    else
                        cu.primaryAddress = "";
                    //primaryPhoneINT
                    if ((xlWorksheet.Cells[i, 9] as Range).Value2 != null)
                        cu.primaryPhoneNumber = (xlWorksheet.Cells[i, 9] as Range).Value2;
                    else
                        cu.primaryPhoneNumber = "";
                    //secondaryPhoneINT
                    if ((xlWorksheet.Cells[i, 10] as Range).Value2 != null)
                        cu.secondaryPhoneNumber = (xlWorksheet.Cells[i, 10] as Range).Value2;
                    else
                        cu.secondaryPhoneNumber = "";
                    //email
                    if ((xlWorksheet.Cells[i, 11] as Range).Value2 != null)
                        cu.email = (xlWorksheet.Cells[i, 11] as Range).Value2;
                    else
                        cu.email = "";
                    //city
                    if ((xlWorksheet.Cells[i, 6] as Range).Value2 != null)
                        cu.city = (xlWorksheet.Cells[i, 6] as Range).Value2;
                    else
                        cu.city = "";
                    //provStateID
                    if ((xlWorksheet.Cells[i, 7] as Range).Value2 != null)
                    {
                        string provinceName = (xlWorksheet.Cells[i, 7] as Range).Value2;
                        cu.province = lm.pronvinceID(provinceName);
                    }
                    else
                        cu.province = 1;
                    //country                    
                    cu.country = lm.countryIDFromProvince(cu.province);
                    //postZip
                    if ((xlWorksheet.Cells[i, 8] as Range).Value2 != null)
                        cu.postalCode = (xlWorksheet.Cells[i, 8] as Range).Value2;
                    else
                        cu.postalCode = "";

                    cu.secondaryAddress = "";
                    cu.billingAddress = "";
                }
                ssm.addCustomer(cu);
            }

        }

        //********************EXPORTING***************************************************************
        //Export clubs table to excel file in users Downloads folder
        public void exportClubs()
        {
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_clubs", sqlCon);
            System.Data.DataTable dtMainSQLData = new System.Data.DataTable();
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
            ExcelApp.ActiveWorkbook.SaveCopyAs(pathDownload + "\\ClubsInventory-" + DateTime.Now.ToString("d MMM yyyy") + ".xlsx");
            ExcelApp.ActiveWorkbook.Saved = true;
            ExcelApp.Quit();
        }
        //Export clothing table to excel file in users Downloads folder
        public void exportClothing()
        {
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_clothing", sqlCon);
            System.Data.DataTable dtMainSQLData = new System.Data.DataTable();
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
            ExcelApp.ActiveWorkbook.SaveCopyAs(pathDownload + "\\ClothingInventory-" + DateTime.Now.ToString("d MMM yyyy") + ".xlsx");
            ExcelApp.ActiveWorkbook.Saved = true;
            ExcelApp.Quit();
        }
        //Export accessories table to excel file in users Downloads folder
        public void exportAccessories()
        {
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_accessories", sqlCon);
            System.Data.DataTable dtMainSQLData = new System.Data.DataTable();
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
            ExcelApp.ActiveWorkbook.SaveCopyAs(pathDownload + "\\AccessoriesInventory-" + DateTime.Now.ToString("d MMM yyyy") + ".xlsx");
            ExcelApp.ActiveWorkbook.Saved = true;
            ExcelApp.Quit();
        }
        //Export all items in inventory
        public System.Data.DataTable exportAllItems()
        {
            exportTable = new System.Data.DataTable();           

            exportTable.Columns.Add("Vendor", typeof(string));
            exportTable.Columns.Add("Store_ID", typeof(string));
            exportTable.Columns.Add("ItemNumber", typeof(string));
            exportTable.Columns.Add("Shipment_Date", typeof(string));
            exportTable.Columns.Add("Brand", typeof(string));
            exportTable.Columns.Add("Model", typeof(string));
            exportTable.Columns.Add("Club_Type", typeof(string));
            exportTable.Columns.Add("Shaft", typeof(string));
            exportTable.Columns.Add("Number_of_Clubs", typeof(string));
            exportTable.Columns.Add("Tradein_Price", typeof(double));
            exportTable.Columns.Add("Premium", typeof(double));
            exportTable.Columns.Add("WE PAY", typeof(double));
            exportTable.Columns.Add("QUANTITY", typeof(int));
            exportTable.Columns.Add("Ext'd Price", typeof(double));
            exportTable.Columns.Add("RetailPrice", typeof(double));
            exportTable.Columns.Add("Comments", typeof(string));
            exportTable.Columns.Add("Image", typeof(string));
            exportTable.Columns.Add("Club_Spec", typeof(string));
            exportTable.Columns.Add("Shaft_Spec", typeof(string));
            exportTable.Columns.Add("Shaft_Flex", typeof(string));
            exportTable.Columns.Add("Dexterity", typeof(string));
            exportTable.Columns.Add("Destination", typeof(string));
            exportTable.Columns.Add("Received", typeof(string));
            exportTable.Columns.Add("Paid", typeof(string));

            exportAllAdd_Clubs();
            exportAllAdd_Accessories();
            exportAllAdd_Clothing();           

            return exportTable;
        }
        //****NEED TO ADD SUB QUEREY
        //Puts the clubs in the export table
        public void exportAllAdd_Clubs()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select * from tbl_clubs";
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                exportTable.Rows.Add("", (lm.locationName(Convert.ToInt32(reader["locationID"]))).ToString(), (Convert.ToInt32(reader["sku"])).ToString(),
                    "", idu.brandType(Convert.ToInt32(reader["brandID"])), idu.modelType(Convert.ToInt32(reader["brandID"])), reader["clubType"].ToString(),
                    reader["shaft"].ToString(), reader["numberOfClubs"].ToString(), 0, Convert.ToDouble(reader["premium"]), Convert.ToDouble(reader["cost"]),
                    Convert.ToInt32(reader["quantity"]), 0, Convert.ToDouble(reader["price"]), reader["comments"].ToString(), "", reader["clubSpec"].ToString(),
                    reader["shaftSpec"].ToString(), reader["shaftFlex"].ToString(), reader["dexterity"].ToString(), "", "", "");
            }
            conn.Close();
        }
        //Puts the accessories in the export table
        public void exportAllAdd_Accessories()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select * from tbl_accessories";
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                exportTable.Rows.Add("", (lm.locationName(Convert.ToInt32(reader["locationID"]))).ToString(), (Convert.ToInt32(reader["sku"])).ToString(),
                    "", idu.brandType(Convert.ToInt32(reader["brandID"])), idu.modelType(Convert.ToInt32(reader["modelID"])), "",
                    "", "", 0, 0, Convert.ToDouble(reader["cost"]),
                    Convert.ToInt32(reader["quantity"]), 0, Convert.ToDouble(reader["price"]), "", "", "",
                   "", "", "", "", "", "");
            }
            conn.Close();
        }
        //Puts the clothing in the export table
        public void exportAllAdd_Clothing()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select * from tbl_clothing";
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                exportTable.Rows.Add("", (lm.locationName(Convert.ToInt32(reader["locationID"]))).ToString(), (Convert.ToInt32(reader["sku"])).ToString(),
                    "", idu.brandType(Convert.ToInt32(reader["brandID"])), reader["gender"].ToString(), reader["style"].ToString(),
                    "", "", 0, 0, Convert.ToDouble(reader["cost"]),
                    Convert.ToInt32(reader["quantity"]), 0, Convert.ToDouble(reader["price"]), "", "", "",
                   "", "", "", "", "", "");
            }
            conn.Close();
        }

        //Export sales/invoices to excel
        public void exportAllSalesToExcel()
        {
            //invoiceNum, invoiceSubNum, invoiceDate, invoiceTime, custID, empID,
            //locationID, subTotal, discountAmount, tradeinAmount, governmentTax,
            //provincialTax, balanceDue, transactionType, comments

            //invoiceNum, invoiceSubNum, sku, itemQuantity, itemCost,
            //itemPrice, itemDiscount, percentage

            //ID, invoiceNum, invoiceSubNum, mopType, amountPaid

            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            SqlDataAdapter im = new SqlDataAdapter("SELECT * FROM tbl_invoice", sqlCon);
            System.Data.DataTable dtim = new System.Data.DataTable();
            im.Fill(dtim);
            DataColumnCollection dcimHeaders = dtim.Columns;
            sqlCon.Close();

            sqlCon.Open();
            SqlDataAdapter ii = new SqlDataAdapter("SELECT * FROM tbl_invoice", sqlCon);
            System.Data.DataTable dtii = new System.Data.DataTable();
            ii.Fill(dtii);
            DataColumnCollection dciiHeaders = dtii.Columns;
            sqlCon.Close();

            sqlCon.Open();
            SqlDataAdapter imo = new SqlDataAdapter("SELECT * FROM tbl_invoice", sqlCon);
            System.Data.DataTable dtimo = new System.Data.DataTable();
            imo.Fill(dtimo);
            DataColumnCollection dcimoHeaders = dtimo.Columns;
            sqlCon.Close();

            //Initiating Everything
            initiateInvoiceTable();
            exportSales_Invoice();
            initiateInvoiceItemTable();
            exportSales_Items();
            initiateInvoiceMOPTable();
            exportSales_MOP();


            //// Export Data into EXCEL Sheet
            //Application ExcelApp = new Application();
            //ExcelApp.Application.Workbooks.Add(Type.Missing);
            //Sheets worksheets = ExcelApp.Worksheets;

            //var xlInvoiceMain = (Worksheet)worksheets.Add(worksheets[1], Type.Missing, Type.Missing, Type.Missing);
            //xlInvoiceMain.Name = "InvoiceMain";

            //var xlInvoiceItem = (Worksheet)worksheets.Add(worksheets[1], Type.Missing, Type.Missing, Type.Missing);
            //xlInvoiceItem.Name = "InvoiceItems";

            //var xlInvoiceMOPS = (Worksheet)worksheets.Add(worksheets[1], Type.Missing, Type.Missing, Type.Missing);
            //xlInvoiceMOPS.Name = "InvoiceMOPS";


            ////Export mop invoice
            //for (int i = 1; i < exportInvoiceMOPTable.Rows.Count + 2; i++)
            //{
            //    for (int j = 1; j < exportInvoiceMOPTable.Columns.Count + 1; j++)
            //    {
            //        if (i == 1)
            //        {
            //            xlInvoiceMOPS.Cells[i, j] = dcCollection[j - 1].ToString();
            //        }
            //        else
            //            xlInvoiceMOPS.Cells[i, j] = exportInvoiceMOPTable.Rows[i - 2][j - 1].ToString();
            //    }
            //}
            ////Export item invoice
            //for (int i = 1; i < exportInvoiceItemTable.Rows.Count + 2; i++)
            //{
            //    for (int j = 1; j < exportInvoiceItemTable.Columns.Count + 1; j++)
            //    {
            //        if (i == 1)
            //        {
            //            xlInvoiceItem.Cells[i, j] = dcCollection[j - 1].ToString();
            //        }
            //        else
            //            xlInvoiceItem.Cells[i, j] = exportInvoiceItemTable.Rows[i - 2][j - 1].ToString();
            //    }
            //}
            ////Export main invoice
            //for (int i = 1; i < exportInvoiceTable.Rows.Count + 2; i++)
            //{
            //    for (int j = 1; j < exportInvoiceTable.Columns.Count + 1; j++)
            //    {
            //        if (i == 1)
            //        {
            //            xlInvoiceMain.Cells[i, j] = dcCollection[j - 1].ToString();
            //        }
            //        else
            //            xlInvoiceMain.Cells[i, j] = exportInvoiceTable.Rows[i - 2][j - 1].ToString();
            //    }
            //}


            ////Get users profile, downloads folder path, and save to workstation
            //string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            //string pathDownload = Path.Combine(pathUser, "Downloads");
            //ExcelApp.ActiveWorkbook.SaveCopyAs(pathDownload + "\\AllSales-" + DateTime.Now.ToString("d MMM yyyy") + ".xlsx");
            //ExcelApp.ActiveWorkbook.Saved = true;
            //ExcelApp.Quit();
        }
        public System.Data.DataTable initiateInvoiceTable()
        {
            //invoiceNum, invoiceSubNum, invoiceDate, invoiceTime, custID, empID,
            //locationID, subTotal, discountAmount, tradeinAmount, governmentTax,
            //provincialTax, balanceDue, transactionType, comments
            exportInvoiceTable = new System.Data.DataTable();
            exportInvoiceTable.Columns.Add("invoiceNum", typeof(string));
            exportInvoiceTable.Columns.Add("invoiceSubNum", typeof(string));
            exportInvoiceTable.Columns.Add("invoiceDate", typeof(string));
            exportInvoiceTable.Columns.Add("invoiceTime", typeof(string));
            exportInvoiceTable.Columns.Add("custID", typeof(string));
            exportInvoiceTable.Columns.Add("empID", typeof(string));
            exportInvoiceTable.Columns.Add("locationID", typeof(string));
            exportInvoiceTable.Columns.Add("subTotal", typeof(string));
            exportInvoiceTable.Columns.Add("discountAmount", typeof(string));
            exportInvoiceTable.Columns.Add("tradeinAmount", typeof(string));
            exportInvoiceTable.Columns.Add("governmentTax", typeof(string));
            exportInvoiceTable.Columns.Add("provincialTax", typeof(string));
            exportInvoiceTable.Columns.Add("balanceDue", typeof(string));
            exportInvoiceTable.Columns.Add("transactionType", typeof(string));
            exportInvoiceTable.Columns.Add("comments", typeof(string));
            exportSales_Invoice();

            return exportInvoiceTable;
        }
        public System.Data.DataTable initiateInvoiceItemTable()
        {
            //invoiceNum, invoiceSubNum, sku, itemQuantity, itemCost,
            //itemPrice, itemDiscount, percentage
            exportInvoiceItemTable = new System.Data.DataTable();
            exportInvoiceItemTable.Columns.Add("invoiceNum", typeof(string));
            exportInvoiceItemTable.Columns.Add("invoiceSubNum", typeof(string));
            exportInvoiceItemTable.Columns.Add("sku", typeof(string));
            exportInvoiceItemTable.Columns.Add("itemQuantity", typeof(string));
            exportInvoiceItemTable.Columns.Add("itemCost", typeof(string));
            exportInvoiceItemTable.Columns.Add("itemPrice", typeof(string));
            exportInvoiceItemTable.Columns.Add("itemDiscount", typeof(string));
            exportInvoiceItemTable.Columns.Add("percentage", typeof(string));
            exportSales_Items();

            return exportInvoiceItemTable;
        }
        public System.Data.DataTable initiateInvoiceMOPTable()
        {
            //ID, invoiceNum, invoiceSubNum, mopType, amountPaid
            exportInvoiceMOPTable = new System.Data.DataTable();
            exportInvoiceMOPTable.Columns.Add("ID", typeof(string));
            exportInvoiceMOPTable.Columns.Add("invoiceNum", typeof(string));
            exportInvoiceMOPTable.Columns.Add("invoiceSubNum", typeof(string));
            exportInvoiceMOPTable.Columns.Add("mopType", typeof(string));
            exportInvoiceMOPTable.Columns.Add("amountPaid", typeof(string));
            exportSales_MOP();

            return exportInvoiceMOPTable;
        }
        public void exportSales_Invoice()
        {
            //invoiceNum, invoiceSubNum, invoiceDate, invoiceTime, custID, empID,
            //locationID, subTotal, discountAmount, tradeinAmount, governmentTax,
            //provincialTax, balanceDue, transactionType, comments
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select * from tbl_invoice";
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string invoiceNum = reader["invoiceNum"].ToString();
                string invoiceSubNum = reader["invoiceSubNum"].ToString();
                string invoiceDate = reader["invoiceDate"].ToString();
                string invioceTime = reader["invoiceTime"].ToString();
                string custID = reader["custID"].ToString();
                string empID = reader["empID"].ToString();
                string locationID = reader["locationID"].ToString();
                string subTotal = reader["subTotal"].ToString();
                string discountAmount = reader["discountAmount"].ToString();
                string tradeinAmount = reader["tradeinAmount"].ToString();
                string governmentTax = reader["governmentTax"].ToString();
                string provincialTax = reader["provincialTax"].ToString();
                string balanceDue = reader["balanceDue"].ToString();
                string transactionType = reader["transactionType"].ToString();
                string comments = reader["comments"].ToString();
                exportInvoiceTable.Rows.Add(invoiceNum, invoiceSubNum, invoiceDate, invioceTime,
                    custID, empID, locationID, subTotal, discountAmount,
                    tradeinAmount, governmentTax, provincialTax, balanceDue, transactionType, comments);
            }
            conn.Close();
        }
        public void exportSales_Items()
        {
            //invoiceNum, invoiceSubNum, sku, itemQuantity, itemCost,
            //itemPrice, itemDiscount, percentage
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select * from tbl_invoiceItem";
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string invoiceNum = reader["invoiceNum"].ToString();
                string invoiceSubNum = reader["invoiceSubNum"].ToString();
                string sku = reader["sku"].ToString();
                string itemQuantity = reader["itemQuantity"].ToString();
                string itemCost = reader["itemCost"].ToString();
                string itemPrice = reader["itemPrice"].ToString();
                string itemDisocunt = reader["itemDiscount"].ToString();
                string percentage = reader["percentage"].ToString();
                exportInvoiceItemTable.Rows.Add(invoiceNum, invoiceSubNum, sku, itemQuantity, itemCost,
                    itemPrice, itemDisocunt, percentage);
            }
            conn.Close();
        }
        public void exportSales_MOP()
        {
            //ID, invoiceNum, invoiceSubNum, mopType, amountPaid
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select * from tbl_invoiceMOP";
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string ID = reader["ID"].ToString();
                string invoiceNum = reader["invoiceNum"].ToString();
                string invoiceSubNum = reader["invoiceSubNum"].ToString();
                string mopType = reader["mopType"].ToString();
                string amountPaid = reader["amountPaid"].ToString();
                exportInvoiceMOPTable.Rows.Add(ID, invoiceNum, invoiceSubNum, mopType, amountPaid);
            }
            conn.Close();
        }



    }
}