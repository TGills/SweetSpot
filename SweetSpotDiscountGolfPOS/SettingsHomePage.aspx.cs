using OfficeOpenXml;
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
            string method = "Page_Load";
            Session["currPage"] = "InventoryHomePage.aspx";
            Session["prevPage"] = "HomePage.aspx";
            try
            {
                if (Convert.ToBoolean(Session["loggedIn"]) == false)
                {
                    Server.Transfer("LoginPage.aspx", false);
                }
                if (Session["Admin"] == null)
                {
                    //btnAddNewEmployee.Enabled = false;
                    //btnLoadCustomers.Enabled = false;
                    //btnLoadEmployees.Enabled = false;
                    //btnLoadItems.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                int employeeID = Convert.ToInt32(Session["loginEmployeeID"]);
                string currPage = Convert.ToString(Session["currPage"]);
                er.logError(ex, employeeID, currPage, method, this);
                string prevPage = Convert.ToString(Session["prevPage"]);
                Server.Transfer(prevPage, false);
            }
        }
        protected void btnAddNewEmployee_Click(object sender, EventArgs e)
        {
            string method = "btnAddNewEmployee_Click";
            try
            {
                Session["prevPage"] = Session["currPage"];
                Server.Transfer("EmployeeAddNew.aspx", false);
            }
            catch (Exception ex)
            {
                int employeeID = Convert.ToInt32(Session["loginEmployeeID"]);
                string currPage = Convert.ToString(Session["currPage"]);
                er.logError(ex, employeeID, currPage, method, this);
                string prevPage = Convert.ToString(Session["prevPage"]);
                Server.Transfer(prevPage, false);
            }
        }
        protected void grdEmployeesSearched_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string method = "grdEmployeesSearched_RowCommand";
            try
            {
                string key = e.CommandArgument.ToString();
                if (e.CommandName == "ViewProfile")
                {
                    Session["empKey"] = key;
                    Session["prevPage"] = Session["currPage"];
                    Server.Transfer("EmployeeAddNew.aspx", false);
                }
            }
            catch (Exception ex)
            {
                int employeeID = Convert.ToInt32(Session["loginEmployeeID"]);
                string currPage = Convert.ToString(Session["currPage"]);
                er.logError(ex, employeeID, currPage, method, this);
                string prevPage = Convert.ToString(Session["prevPage"]);
                Server.Transfer(prevPage, false);
            }
        }
        protected void btnEmployeeSearch_Click(object sender, EventArgs e)
        {
            string method = "btnEmployeeSearch_Click";
            try
            {
                List<Employee> emp = em.GetEmployeefromSearch(txtSearch.Text);
                grdEmployeesSearched.Visible = true;
                grdEmployeesSearched.DataSource = emp;
                grdEmployeesSearched.DataBind();
            }
            catch (Exception ex)
            {
                int employeeID = Convert.ToInt32(Session["loginEmployeeID"]);
                string currPage = Convert.ToString(Session["currPage"]);
                er.logError(ex, employeeID, currPage, method, this);
                string prevPage = Convert.ToString(Session["prevPage"]);
                Server.Transfer(prevPage, false);
            }

        }
        //Importing
        protected void btnLoadItems_Click(object sender, EventArgs e)
        {
            string method = "btnLoadItems_Click";
            try
            {
                if (fupItemSheet.HasFile)
                {
                    r.importItems(fupItemSheet);
                }
                //Show that it is done
                MessageBox.ShowMessage("Importing Complete", this);
            }
            catch (Exception ex)
            {
                int employeeID = Convert.ToInt32(Session["loginEmployeeID"]);
                string currPage = Convert.ToString(Session["currPage"]);
                er.logError(ex, employeeID, currPage, method, this);
                string prevPage = Convert.ToString(Session["prevPage"]);
                Server.Transfer(prevPage, false);
            }
        }
        protected void btnImportCustomers_Click(object sender, EventArgs e)
        {
            string method = "btnImportCustomers_Click";
            try
            {
                if (fupCustomers.HasFile)
                {
                    r.importCustomers(fupCustomers);
                }
                //Show that it is done
                MessageBox.ShowMessage("Importing Complete", this);
            }
            catch (Exception ex)
            {
                int employeeID = Convert.ToInt32(Session["loginEmployeeID"]);
                string currPage = Convert.ToString(Session["currPage"]);
                er.logError(ex, employeeID, currPage, method, this);
                string prevPage = Convert.ToString(Session["prevPage"]);
                Server.Transfer(prevPage, false);
            }
        }

        //Exporting
        protected void btnExportAll_Click(object sender, EventArgs e)
        {
            //r.exportAllItems();
            //MessageBox.ShowMessage("Export Complete", this);
            string method = "btnExportAll_Click";
            try
            {
                string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string pathDownload = (pathUser + "\\Downloads\\");
                FileInfo newFile = new FileInfo(pathDownload + "TotalInventory.xlsx");
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Inventory");
                    // write to sheet
                    DataTable exportTable = r.exportAllItems();

                    DataColumnCollection dcCollection = exportTable.Columns;

                    for (int i = 1; i < exportTable.Rows.Count + 2; i++)
                    {
                        for (int j = 1; j < exportTable.Columns.Count + 1; j++)
                        {
                            if (i == 1)
                            {
                                worksheet.Cells[i, j].Value = dcCollection[j - 1].ToString();
                            }
                            else
                                worksheet.Cells[i, j].Value = exportTable.Rows[i - 2][j - 1].ToString();
                        }
                    }

                    Response.Clear();
                    Response.AddHeader("content-disposition", "attachment; filename=TotalInventory.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.BinaryWrite(xlPackage.GetAsByteArray());
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                int employeeID = Convert.ToInt32(Session["loginEmployeeID"]);
                string currPage = Convert.ToString(Session["currPage"]);
                er.logError(ex, employeeID, currPage, method, this);
                string prevPage = Convert.ToString(Session["prevPage"]);
                Server.Transfer(prevPage, false);
            }
        }
        protected void btnExportClubs_Click(object sender, EventArgs e)
        {
            string method = "btnExportClubs_Click";
            try
            {
                r.exportClubs();
                MessageBox.ShowMessage("Export Complete", this);
            }
            catch (Exception ex)
            {
                int employeeID = Convert.ToInt32(Session["loginEmployeeID"]);
                string currPage = Convert.ToString(Session["currPage"]);
                er.logError(ex, employeeID, currPage, method, this);
                string prevPage = Convert.ToString(Session["prevPage"]);
                Server.Transfer(prevPage, false);
            }
        }
        protected void btnExportClothing_Click(object sender, EventArgs e)
        {
            string method = "btnExportClothing_Click";
            try
            {
                r.exportClothing();
                MessageBox.ShowMessage("Export Complete", this);
            }
            catch (Exception ex)
            {
                int employeeID = Convert.ToInt32(Session["loginEmployeeID"]);
                string currPage = Convert.ToString(Session["currPage"]);
                er.logError(ex, employeeID, currPage, method, this);
                string prevPage = Convert.ToString(Session["prevPage"]);
                Server.Transfer(prevPage, false);
            }
        }
        protected void btnExportAccessories_Click(object sender, EventArgs e)
        {
            string method = "btnExportAccessories_Click";
            try
            {
                r.exportAccessories();
                MessageBox.ShowMessage("Export Complete", this);
            }
            catch (Exception ex)
            {
                int employeeID = Convert.ToInt32(Session["loginEmployeeID"]);
                string currPage = Convert.ToString(Session["currPage"]);
                er.logError(ex, employeeID, currPage, method,  this);
                string prevPage = Convert.ToString(Session["prevPage"]);
                Server.Transfer(prevPage, false);
            }
        }


    }
}