﻿using OfficeOpenXml;
using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using SweetSpotProShop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSpotDiscountGolfPOS
{
    public partial class SettingsHomePage : System.Web.UI.Page
    {
        ErrorReporting er = new ErrorReporting();
        SweetShopManager ssm = new SweetShopManager();
        EmployeeManager em = new EmployeeManager();
        Reports r = new Reports();
        internal static readonly Page aspx;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "SettingsHomePage.aspx";
            try
            {
                //checks if the user has logged in
                if (Convert.ToBoolean(Session["loggedIn"]) == false)
                {
                    //Go back to Login to log in
                    Server.Transfer("LoginPage.aspx", false);
                }
                //Checks if the user is an Admin
                if (Session["Admin"] == null)
                {
                    //btnAddNewEmployee.Enabled = false;
                    //btnLoadCustomers.Enabled = false;
                    //btnLoadEmployees.Enabled = false;
                    //btnLoadItems.Enabled = false;
                }
            }
            //Exception catch
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = Convert.ToInt32(Session["loginEmployeeID"]);
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
            }
        }
        protected void btnAddNewEmployee_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnAddNewEmployee_Click";
            try
            {
                //Change to Employee add new page
                Server.Transfer("EmployeeAddNew.aspx", false);
            }
            //Exception catch
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = Convert.ToInt32(Session["loginEmployeeID"]);
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
            }
        }
        protected void grdEmployeesSearched_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method for error tracking
            string method = "grdEmployeesSearched_RowCommand";
            try
            {
                //Gets string from the command argument
                string key = e.CommandArgument.ToString();
                //Checks if the string is view profile
                if (e.CommandName == "ViewProfile")
                {
                    //Sets the employee id into a session
                    Session["empKey"] = key;
                    //Changes page to Employee Add New
                    Server.Transfer("EmployeeAddNew.aspx", false);
                }
            }
            //Exception catch
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = Convert.ToInt32(Session["loginEmployeeID"]);
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
            }
        }
        protected void btnEmployeeSearch_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnEmployeeSearch_Click";
            try
            {
                //Queries database and returns a list of emplyees that match the search criteria
                List<Employee> emp = em.GetEmployeefromSearch(txtSearch.Text);
                grdEmployeesSearched.Visible = true;
                //Binds the employee list to grid view
                grdEmployeesSearched.DataSource = emp;
                grdEmployeesSearched.DataBind();
            }
            //Exception catch
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = Convert.ToInt32(Session["loginEmployeeID"]);
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
            }

        }
        //Importing
        protected void btnLoadItems_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnLoadItems_Click";
            try
            {
                int error = 0;
                //Verifies file has been selected
                if (fupItemSheet.HasFile)
                {
                    //load the uploaded file into the memorystream
                    using (MemoryStream stream = new MemoryStream(fupItemSheet.FileBytes))
                    //Lets the server know to use the excel package
                    using (ExcelPackage xlPackage = new ExcelPackage(stream))
                    {
                        //Gets the first worksheet in the workbook
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[1];
                        //Gets the row count
                        var rowCnt = worksheet.Dimension.End.Row;
                        //Gets the column count
                        var colCnt = worksheet.Dimension.End.Column;
                        //Beginning the loop for data gathering
                        for (int i = 2; i <= rowCnt; i++) //Starts on 2 because excel starts at 1, and line 1 is headers
                        {
                            //Array of the cells that will need to be checked
                            int[] cells = { 3, 10, 11, 12, 13, 14, 15 };
                            foreach (int column in cells)
                            {
                                //If there is no value in the column, proceed
                                if (worksheet.Cells[i, column].Value == null)
                                {
                                    worksheet.Cells[i, column].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[i, column].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                    error = 1;
                                }
                            }
                        }
                        //xlPackage.SaveAs(new FileInfo(@"c:\temp\myFile.xls"));
                        if (error == 1)
                        {
                            //Sets the attributes and writes file
                            Response.Clear();
                            Response.AddHeader("content-disposition", "attachment; filename=" + fupItemSheet.FileName + "_ErrorsFound" + ".xlsx");
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.BinaryWrite(xlPackage.GetAsByteArray());
                            Response.End();
                        }
                        else
                        {
                            //Calls method to import the requested file
                            r.importItems(fupItemSheet);
                        }
                    }
                    //Show that it is done
                    MessageBox.ShowMessage("Importing Complete", this);
                }
            }
            //Exception catch
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = Convert.ToInt32(Session["loginEmployeeID"]);
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
            }
        }
        protected void btnImportCustomers_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnImportCustomers_Click";
            try
            {
                //Verifies file has been selected
                if (fupCustomers.HasFile)
                {
                    //Calls method to import the requested file
                    r.importCustomers(fupCustomers);
                }
                //Show that it is done
                MessageBox.ShowMessage("Importing Complete", this);
            }
            //Exception catch
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = Convert.ToInt32(Session["loginEmployeeID"]);
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
            }
        }
        //Exporting
        protected void btnExportAll_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnExportAll_Click";
            try
            {
                //Sets path and file name to save the export to 
                string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string pathDownload = (pathUser + "\\Downloads\\");
                FileInfo newFile = new FileInfo(pathDownload + "TotalInventory.xlsx");
                //With the craeted file do all intenal code
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    //Add page to the work book called inventory
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Inventory");
                    // write to sheet
                    DataTable exportTable = r.exportAllItems();
                    //Setting data collection as datatable
                    DataColumnCollection dcCollection = exportTable.Columns;
                    //Loops through each row in the datatable
                    for (int i = 1; i < exportTable.Rows.Count + 2; i++)
                    {
                        //Loops through each column in the data table
                        for (int j = 1; j < exportTable.Columns.Count + 1; j++)
                        {
                            //When the row equals 1 set the headers
                            if (i == 1)
                            {
                                worksheet.Cells[i, j].Value = dcCollection[j - 1].ToString();
                            }
                            else
                                //Set the values in the data table
                                worksheet.Cells[i, j].Value = exportTable.Rows[i - 2][j - 1].ToString();
                        }
                    }
                    //Sets the attributes and writes file
                    Response.Clear();
                    Response.AddHeader("content-disposition", "attachment; filename=TotalInventory.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.BinaryWrite(xlPackage.GetAsByteArray());
                    Response.End();
                }
            }
            //Exception catch
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = Convert.ToInt32(Session["loginEmployeeID"]);
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
            }
        }
        protected void btnExportClubs_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnExportClubs_Click";
            try
            {
                //Calls method to export clubs
                r.exportClubs();
                //Displays message
                MessageBox.ShowMessage("Export Complete", this);
            }
            //Exception catch
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = Convert.ToInt32(Session["loginEmployeeID"]);
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
            }
        }
        protected void btnExportClothing_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnExportClothing_Click";
            try
            {
                //Calls method to export all clothing
                r.exportClothing();
                //Displays message
                MessageBox.ShowMessage("Export Complete", this);
            }
            //Exception catch
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = Convert.ToInt32(Session["loginEmployeeID"]);
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
            }
        }
        protected void btnExportAccessories_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnExportAccessories_Click";
            try
            {
                //Calls method to export all accessories
                r.exportAccessories();
                //Displays message
                MessageBox.ShowMessage("Export Complete", this);
            }
            //Exception catch
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = Convert.ToInt32(Session["loginEmployeeID"]);
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
            }
        }
    }
}