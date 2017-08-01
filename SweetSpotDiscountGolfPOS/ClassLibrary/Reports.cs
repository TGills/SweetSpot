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

namespace SweetSpotDiscountGolfPOS.ClassLibrary
{
    public class Reports
    {
        string connectionString;
        List<Cashout> cashout = new List<Cashout>();
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
            cmd.CommandText = "Select tbl_invoiceMOP.mopType, tbl_invoiceMOP.amountPaid, tbl_invoice.tradeinAmount from tbl_invoiceMOP " +
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
               cas.saleCash + ", " + cas.saleCheque + ", " + cas.saleDebit + ", " + cas.saleMasterCard + ", " +
               cas.saleVisa + ", " + cas.saleAmex + ", " + cas.receiptTradeIn + ", " + cas.receiptGiftCard + ", " +
               cas.receiptCash + ", " + cas.receiptCheque + ", " + cas.receiptDebit + ", " + cas.receiptMasterCard + ", " +
               cas.receiptVisa + ", " + cas.receiptAmex + ", " + cas.overShort + ", " + finalized + ", " + processed + ");";

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
            //try
            //{
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
                    Console.Write(xlRange.Cells[i].Value2.ToString() + "\t");
                    //UPDATE ALL WITH PROPER VALUES/POSITIONS
                    if (itemType == null) { }
                    else if (itemType.Equals("")) { }
                    else if (itemType.Equals("Accessories") || itemType.Equals("accessories") || itemType.Equals("Balls") ||
                        itemType.Equals("balls") || itemType.Equals("Grips") || itemType.Equals("grips"))
                    {
                        if (Convert.ToInt32((xlRange.Cells[i, 3] as Range).Value2) != null)
                            a.sku = Convert.ToInt32((xlRange.Cells[i, 3] as Range).Value2);
                        else
                            a.sku = 0;

                        int bName = idu.brandName((xlWorksheet.Cells[i, 5] as Range).Value2);
                        if (!bName.Equals(null))
                            a.brandID = bName;
                        else
                            a.brandID = 1;

                        string mName;
                        try
                        {
                            mName = ((xlRange.Cells[i, 6] as Range).Value2).ToString();
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

                        if (Convert.ToDouble((xlWorksheet.Cells[i, 12] as Range).Value2) != null)
                            a.cost = Convert.ToDouble((xlWorksheet.Cells[i, 12] as Range).Value2);
                        else
                            a.cost = 0;

                        if (Convert.ToDouble((xlRange.Cells[i, 15] as Range).Value2) != null)
                            a.price = Convert.ToDouble((xlRange.Cells[i, 15] as Range).Value2);
                        else
                            a.price = 0;

                        if (Convert.ToInt32((xlRange.Cells[i, 13] as Range).Value2) != null)
                            a.quantity = Convert.ToInt32((xlRange.Cells[i, 13] as Range).Value2);
                        else
                            a.quantity = 0;

                        if ((string)((xlRange.Cells[i, 19] as Range).Value2) != null)
                            a.comments = (string)((xlRange.Cells[i, 19] as Range).Value2);
                        else
                            a.comments = "";

                        a.locID = 1;//
                        a.typeID = 2;
                        a.size = "";
                        a.colour = "";

                        listAccessories.Add(a);
                        o = a as Object;
                    }
                    else if (itemType.Equals("Apparel") || itemType.Equals("apparel") || itemType.Equals("Shoes") || itemType.Equals("shoes"))
                    {
                        if (Convert.ToInt32((xlRange.Cells[i, 3] as Range).Value2) != null)
                            cl.sku = Convert.ToInt32((xlRange.Cells[i, 3] as Range).Value2);
                        else
                            cl.sku = 0;

                        int bName = idu.brandName(itemType.ToString());
                        if (!bName.Equals(null))
                            cl.brandID = bName;
                        else
                            cl.brandID = 1;

                        if (Convert.ToDouble((xlWorksheet.Cells[i, 12] as Range).Value2) != null)
                            cl.cost = Convert.ToDouble((xlWorksheet.Cells[i, 12] as Range).Value2);
                        else
                            cl.cost = 0;

                        if (Convert.ToDouble((xlRange.Cells[i, 15] as Range).Value2) != null)
                            cl.price = Convert.ToDouble((xlRange.Cells[i, 15] as Range).Value2);
                        else
                            cl.price = 0;

                        if (Convert.ToInt32((xlRange.Cells[i, 13] as Range).Value2) != null)
                            cl.quantity = Convert.ToInt32((xlRange.Cells[i, 13] as Range).Value2);
                        else
                            cl.quantity = 0;

                        if ((string)((xlRange.Cells[i, 6] as Range).Value2) != null)
                            cl.gender = (string)((xlRange.Cells[i, 6] as Range).Value2);
                        else
                            cl.gender = "";

                        if ((string)((xlRange.Cells[i, 7] as Range).Value2) != null)
                            cl.style = (string)((xlRange.Cells[i, 7] as Range).Value2);
                        else
                            cl.style = "";

                        if ((string)((xlRange.Cells[i, 19] as Range).Value2) != null)
                            cl.comments = (string)((xlRange.Cells[i, 19] as Range).Value2);
                        else
                            cl.comments = "";

                        cl.locID = 1;//
                        cl.typeID = 3; //Type can be directly assigned
                        cl.size = "";
                        cl.colour = "";




                        o = cl as Object;
                    }
                    else if (itemType.CompareTo("Miscellaneous") != 0)
                    {
                        if (Convert.ToInt32((xlRange.Cells[i, 3] as Range).Value2) != null)
                            c.sku = Convert.ToInt32((xlRange.Cells[i, 3] as Range).Value2);
                        else
                            c.sku = 1;

                        if (Convert.ToDouble((xlRange.Cells[i, 12] as Range).Value2) != null)
                            c.cost = Convert.ToDouble((xlRange.Cells[i, 12] as Range).Value2);
                        else
                            c.cost = 0;


                        int bName = idu.brandName((xlWorksheet.Cells[i, 5] as Range).Value2);
                        string test = ((xlWorksheet.Cells[i, 5] as Range).Value2);
                        if (!bName.Equals(null))
                        {
                            c.brandID = bName;
                            if (c.brandID == 0) { c.brandID = 1; }
                        }
                        else
                            c.brandID = 1;

                        if (Convert.ToDouble((xlRange.Cells[i, 15] as Range).Value2) != null)
                            c.price = Convert.ToDouble((xlRange.Cells[i, 15] as Range).Value2);
                        else
                            c.price = 0;

                        if (Convert.ToDouble((xlRange.Cells[i, 11] as Range).Value2) != null)
                            c.premium = Convert.ToDouble((xlRange.Cells[i, 11] as Range).Value2);
                        else
                            c.premium = 0;

                        if (Convert.ToInt32((xlRange.Cells[i, 13] as Range).Value2) != null)
                            c.quantity = Convert.ToInt32((xlRange.Cells[i, 13] as Range).Value2);
                        else
                            c.quantity = 0;

                        if ((string)((xlRange.Cells[i, 7] as Range).Value2) != null)
                            c.clubType = (string)((xlRange.Cells[i, 7] as Range).Value2);
                        else
                            c.clubType = "";


                        string mName;
                        try
                        {
                            mName = ((xlRange.Cells[i, 6] as Range).Value2).ToString();
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



                        if ((string)((xlRange.Cells[i, 8] as Range).Value2) != null)
                            c.shaft = (string)((xlRange.Cells[i, 8] as Range).Value2);
                        else
                            c.shaft = "";

                        if ((string)((xlRange.Cells[i, 9] as Range).Value2) != null)
                            c.numberOfClubs = (string)((xlRange.Cells[i, 9] as Range).Value2);
                        else
                            c.numberOfClubs = "";

                        if ((string)((xlRange.Cells[i, 18] as Range).Value2) != null)
                            c.clubSpec = (string)((xlRange.Cells[i, 18] as Range).Value2);
                        else
                            c.clubSpec = "";

                        if ((string)((xlRange.Cells[i, 19] as Range).Value2) != null)
                            c.shaftSpec = (string)((xlRange.Cells[i, 19] as Range).Value2);
                        else
                            c.shaftSpec = "";

                        if ((string)((xlRange.Cells[i, 20] as Range).Value2) != null)
                            c.shaftFlex = (string)((xlRange.Cells[i, 20] as Range).Value2);
                        else
                            c.shaftFlex = "";

                        if ((string)((xlRange.Cells[i, 21] as Range).Value2) != null)
                            c.dexterity = (string)((xlRange.Cells[i, 21] as Range).Value2);
                        else
                            c.dexterity = "";

                        if ((string)((xlRange.Cells[i, 19] as Range).Value2) != null)
                            c.comments = (string)((xlRange.Cells[i, 19] as Range).Value2);
                        else
                            c.comments = "";

                        c.typeID = 1;
                        c.itemlocation = 1;//
                        c.used = false;//

                        listClub.Add(c);
                        o = c as Object;
                    }
                    ssm.checkForItem(o);
                }

            }


            //cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();

            //release com objects to fully kill excel process from running in the background
            Marshal.ReleaseComObject(xlRange);
            Marshal.ReleaseComObject(xlWorksheet);

            //close and release
            xlWorkbook.Close();
            Marshal.ReleaseComObject(xlWorkbook);

            //quit and release
            xlApp.Quit();
            Marshal.ReleaseComObject(xlApp);


            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex);
            //}

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
        public void exportAllItems()
        {
            exportTable = new System.Data.DataTable();
            //SqlConnection sqlCon = new SqlConnection(connectionString);
            //sqlCon.Open();
            //This statement is going to have to go through each table and merge the data
            //SqlDataAdapter club = new SqlDataAdapter("SELECT * FROM tbl_clubs", sqlCon);
            //SqlDataAdapter cl = new SqlDataAdapter("SELECT * FROM tbl_clothing", sqlCon);
            //SqlDataAdapter ac = new SqlDataAdapter("SELECT * FROM tbl_accessories", sqlCon);

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

            DataColumnCollection dcCollection = exportTable.Columns;

            // Export Data into EXCEL Sheet
            Application ExcelApp = new Application();
            ExcelApp.Application.Workbooks.Add(Type.Missing);

            for (int i = 1; i < exportTable.Rows.Count + 2; i++)
            {
                for (int j = 1; j < exportTable.Columns.Count + 1; j++)
                {
                    if (i == 1)
                    {
                        ExcelApp.Cells[i, j] = dcCollection[j - 1].ToString();
                    }
                    else
                        ExcelApp.Cells[i, j] = exportTable.Rows[i - 2][j - 1].ToString();
                }
            }
            //Get users profile, downloads folder path, and save to workstation
            string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string pathDownload = Path.Combine(pathUser, "Downloads");
            ExcelApp.ActiveWorkbook.SaveCopyAs(pathDownload + "\\TotalInventory-" + DateTime.Now.ToString("d MMM yyyy") + ".xlsx");
            ExcelApp.ActiveWorkbook.Saved = true;
            ExcelApp.Quit();
        }
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

        //Export sales to excel
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
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_invoice", sqlCon);
            System.Data.DataTable dtMainSQLData = new System.Data.DataTable();
            da.Fill(dtMainSQLData);
            DataColumnCollection dcCollection = dtMainSQLData.Columns;

            //Initiating Everything
            initiateInvoiceTable();
            exportSales_Invoice();
            initiateInvoiceItemTable();
            exportSales_Items();
            initiateInvoiceMOPTable();
            exportSales_MOP();


            // Export Data into EXCEL Sheet
            Application ExcelApp = new Application();
            ExcelApp.Application.Workbooks.Add(Type.Missing);
            Sheets worksheets = ExcelApp.Worksheets;

            var xlInvoiceMain = (Worksheet)worksheets.Add(worksheets[1], Type.Missing, Type.Missing, Type.Missing);
            xlInvoiceMain.Name = "InvoiceMain";

            var xlInvoiceItem = (Worksheet)worksheets.Add(worksheets[1], Type.Missing, Type.Missing, Type.Missing);
            xlInvoiceItem.Name = "InvoiceItems";

            var xlInvoiceMOPS = (Worksheet)worksheets.Add(worksheets[1], Type.Missing, Type.Missing, Type.Missing);
            xlInvoiceMOPS.Name = "InvoiceMOPS";


            //Export mop invoice
            for (int i = 1; i < exportInvoiceMOPTable.Rows.Count + 2; i++)
            {
                for (int j = 1; j < exportInvoiceMOPTable.Columns.Count + 1; j++)
                {
                    if (i == 1)
                    {
                        xlInvoiceMOPS.Cells[i, j] = dcCollection[j - 1].ToString();
                    }
                    else
                        xlInvoiceMOPS.Cells[i, j] = exportInvoiceMOPTable.Rows[i - 2][j - 1].ToString();
                }
            }
            //Export item invoice
            for (int i = 1; i < exportInvoiceItemTable.Rows.Count + 2; i++)
            {
                for (int j = 1; j < exportInvoiceItemTable.Columns.Count + 1; j++)
                {
                    if (i == 1)
                    {
                        xlInvoiceItem.Cells[i, j] = dcCollection[j - 1].ToString();
                    }
                    else
                        xlInvoiceItem.Cells[i, j] = exportInvoiceItemTable.Rows[i - 2][j - 1].ToString();
                }
            }
            //Export main invoice
            for (int i = 1; i < exportInvoiceTable.Rows.Count + 2; i++)
            {
                for (int j = 1; j < exportInvoiceTable.Columns.Count + 1; j++)
                {
                    if (i == 1)
                    {
                        xlInvoiceMain.Cells[i, j] = dcCollection[j - 1].ToString();
                    }
                    else
                        xlInvoiceMain.Cells[i, j] = exportInvoiceTable.Rows[i - 2][j - 1].ToString();
                }
            }
            
            
            //Get users profile, downloads folder path, and save to workstation
            string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string pathDownload = Path.Combine(pathUser, "Downloads");
            ExcelApp.ActiveWorkbook.SaveCopyAs(pathDownload + "\\AllSales-" + DateTime.Now.ToString("d MMM yyyy") + ".xlsx");
            ExcelApp.ActiveWorkbook.Saved = true;
            ExcelApp.Quit();
        }
        public void initiateInvoiceTable()
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
        }
        public void initiateInvoiceItemTable()
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
        }
        public void initiateInvoiceMOPTable()
        {
            //ID, invoiceNum, invoiceSubNum, mopType, amountPaid
            exportInvoiceMOPTable = new System.Data.DataTable();
            exportInvoiceMOPTable.Columns.Add("ID", typeof(string));
            exportInvoiceMOPTable.Columns.Add("invoiceNum", typeof(string));
            exportInvoiceMOPTable.Columns.Add("invoiceSubNum", typeof(string));
            exportInvoiceMOPTable.Columns.Add("mopType", typeof(string));
            exportInvoiceMOPTable.Columns.Add("amountPaid", typeof(string));
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