using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using SweetSpotProShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.FormulaParsing;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Threading;

namespace SweetSpotDiscountGolfPOS
{
    public partial class ReportsHomePage : System.Web.UI.Page
    {
        ErrorReporting er = new ErrorReporting();
        SweetShopManager ssm = new SweetShopManager();
        Reports r = new Reports();
        ItemDataUtilities idu = new ItemDataUtilities();
        CustomMessageBox cmb = new CustomMessageBox();
        CurrentUser cu = new CurrentUser();
        //List<Invoice> invoice;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "ReportsHomePage";
            try
            {
                cu = (CurrentUser)Session["currentUser"];
                //checks if the user has logged in
                if (Session["currentUser"] == null)
                {
                    //Go back to Login to log in
                    Server.Transfer("LoginPage.aspx", false);
                }
                if (!IsPostBack)
                {
                    //Sets the calendar and text boxes start and end dates
                    calStartDate.SelectedDate = DateTime.Today;
                    calEndDate.SelectedDate = DateTime.Today;
                    txtStartDate.Text = DateTime.Today.ToShortDateString();
                    txtEndDate.Text = DateTime.Today.ToShortDateString();

                }
                if (cu.jobID != 0)
                {
                    //User is not an admin
                    lblReport.Text = "You are not authorized to view reports";
                    lblReport.Visible = true;
                    lblReport.ForeColor = System.Drawing.Color.Red;
                    //calStart.Visible = false;
                    //calEnd.Visible = false;
                    //Disables buttons
                    btnRunReport.Visible = false;
                    txtEndDate.Visible = false;
                    txtStartDate.Visible = false;
                    pnlDefaultButton.Visible = false;
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                er.logError(ex, cu.empID, Convert.ToString(Session["currPage"]), method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
            }
        }
        protected void calStart_SelectionChanged(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "calStart_SelectionChanged";
            try
            {
                //Resets date in text box to match the calendar
                txtStartDate.Text = calStartDate.SelectedDate.ToShortDateString();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = cu.empID;
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
        protected void calEnd_SelectionChanged(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "calEnd_SelectionChanged";
            try
            {
                //Resets date in text box to match the calendar
                txtEndDate.Text = calEndDate.SelectedDate.ToShortDateString();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = cu.empID;
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
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnSubmit_Click";
            try
            {
                if (txtStartDate.Text == "" || txtEndDate.Text == "")
                {
                    //One of the date boxes is empty
                    lbldate.Visible = true;
                    lbldate.Text = "Please Select a Start and End date";
                    lbldate.ForeColor = System.Drawing.Color.Red;

                }
                else
                {
                    //Stores report dates into Session
                    Session["reportDates"] = new DateTime[2] { calStartDate.SelectedDate, calEndDate.SelectedDate };
                    //DateTime[] reportDates = new DateTime[2];
                    //reportDates[0] = calStartDate.SelectedDate;
                    //reportDates[1] = calEndDate.SelectedDate;
                    //Session["reportDates"] = reportDates;

                    //  if(txtStartDate.Text == txtEndDate.Text)
                    //  {

                    // //invoice = ssm.selectAllInventorySalesBetweenDates(Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text).AddHours(23).AddMinutes(59));
                    // Session["salesinvoice"] = invoice;

                    //}
                }
                //Changes to the Reports Cash Out page
                Server.Transfer("ReportsCashOut.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = cu.empID;
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
        protected void btnExportInvoices_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnExportInvoices_Click";
            try
            {
                //Sets up database connection
                string connectionString = ConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString;
                SqlConnection sqlCon = new SqlConnection(connectionString);
                //Selects everything form the invoice table
                sqlCon.Open();
                SqlDataAdapter im = new SqlDataAdapter("SELECT * FROM tbl_invoice", sqlCon);
                DataTable dtim = new DataTable();
                im.Fill(dtim);
                DataColumnCollection dcimHeaders = dtim.Columns;
                sqlCon.Close();
                //Selects everything form the invoice item table
                sqlCon.Open();
                SqlDataAdapter ii = new SqlDataAdapter("SELECT * FROM tbl_invoiceItem", sqlCon);
                DataTable dtii = new DataTable();
                ii.Fill(dtii);
                DataColumnCollection dciiHeaders = dtii.Columns;
                sqlCon.Close();
                //Selects everything form the invoice mop table
                sqlCon.Open();
                SqlDataAdapter imo = new SqlDataAdapter("SELECT * FROM tbl_invoiceMOP", sqlCon);
                DataTable dtimo = new DataTable();
                imo.Fill(dtimo);
                DataColumnCollection dcimoHeaders = dtimo.Columns;
                sqlCon.Close();
                //Sets path and file name to download report to
                string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string pathDownload = (pathUser + "\\Downloads\\");
                FileInfo newFile = new FileInfo(pathDownload + "InvoiceReport.xlsx");
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    //Creates a seperate sheet for each data table
                    ExcelWorksheet invoiceMain = xlPackage.Workbook.Worksheets.Add("Invoice Main");
                    ExcelWorksheet invoiceItems = xlPackage.Workbook.Worksheets.Add("Invoice Items");
                    ExcelWorksheet invoiceMOPS = xlPackage.Workbook.Worksheets.Add("Invoice MOPS");
                    // write to sheet

                    //Initiating Everything   
                    DataTable exportInvoiceTable = r.initiateInvoiceTable();
                    DataTable exportInvoiceItemTable = r.initiateInvoiceItemTable();
                    DataTable exportInvoiceMOPTable = r.initiateInvoiceMOPTable();

                    //Export main invoice
                    for (int i = 1; i < exportInvoiceTable.Rows.Count; i++)
                    {
                        for (int j = 1; j < exportInvoiceTable.Columns.Count + 1; j++)
                        {
                            if (i == 1)
                            {
                                invoiceMain.Cells[i, j].Value = dcimHeaders[j - 1].ToString();
                            }
                            else
                            {
                                invoiceMain.Cells[i, j].Value = exportInvoiceTable.Rows[i - 1][j - 1];
                            }
                        }
                    }
                    //Export item invoice
                    for (int i = 1; i < exportInvoiceItemTable.Rows.Count; i++)
                    {
                        for (int j = 1; j < exportInvoiceItemTable.Columns.Count + 1; j++)
                        {
                            if (i == 1)
                            {
                                invoiceItems.Cells[i, j].Value = dciiHeaders[j - 1].ToString();
                            }
                            else
                            {
                                invoiceItems.Cells[i, j].Value = exportInvoiceItemTable.Rows[i - 1][j - 1];
                            }
                        }
                    }
                    //Export mop invoice
                    for (int i = 1; i < exportInvoiceMOPTable.Rows.Count; i++)
                    {
                        for (int j = 1; j < exportInvoiceMOPTable.Columns.Count + 1; j++)
                        {
                            if (i == 1)
                            {
                                invoiceMOPS.Cells[i, j].Value = dcimoHeaders[j - 1].ToString();
                            }
                            else
                            {
                                invoiceMOPS.Cells[i, j].Value = exportInvoiceMOPTable.Rows[i - 1][j - 1];
                            }
                        }
                    }
                    Response.Clear();
                    Response.AddHeader("content-disposition", "attachment; filename=InvoiceReport.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.BinaryWrite(xlPackage.GetAsByteArray());
                    Response.End();
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = cu.empID;
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
        protected void btnTesting_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnTesting_Click";
            //Method currently not used
            try
            {
                //string variable = " ";
                //Response.Write("<script>Request.QueryString("variable")</script>");
                //Label1.Text = variable;
                //ErrorReporting er = new ErrorReporting();
                //er.sendError("This is a test");        

                //string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                //string pathDownload = (pathUser + "\\Downloads\\");
                //FileInfo newFile = new FileInfo(pathDownload + "mynewfile.xlsx");
                //using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                //{
                //    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Test Sheet");
                //    // write to sheet
                //    worksheet.Cells[1, 1].Value = "Test";
                //    //xlPackage.SaveAs(aFile);

                //    Response.Clear();
                //    Response.AddHeader("content-disposition", "attachment; filename=test.xlsx");
                //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //    Response.BinaryWrite(xlPackage.GetAsByteArray());
                //    Response.End();
                //}
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = cu.empID;
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
        
        //Viewing invoices
        protected void btnInvoiceBetweenDates_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnInvoiceBetweenDates_Click";
            try
            {
                Session["isDeleted"] = false;
                //Gathers start and end dates
                DateTime startDate = calStartDate.SelectedDate;
                DateTime endDate = calEndDate.SelectedDate;
                //Gathers selected location
                string locationID = ddlLocation.SelectedValue;
                //Calls query to return list of all invoices between dates
                List<Invoice> i = ssm.getInvoiceBetweenDates(startDate, endDate, "tbl_invoice", locationID);
                grdInvoicesBetweenDates.Columns[8].Visible = true;
                //Binds invoice list to gridview
                grdInvoicesBetweenDates.DataSource = i;
                grdInvoicesBetweenDates.DataBind();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = cu.empID;
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
        protected void btnReturnInvoice_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnReturnInvoice_Click";
            try
            {
                Session["isDeleted"] = false;
                //Retreives invoice number and splits it
                string invoiceNumber = txtInvoiceNum.Text;
                char[] splitchar = { '-' };
                string[] invoiceSplit = invoiceNumber.Split(splitchar);
                int invoiceNum = Convert.ToInt32(invoiceSplit[0]);
                int invoiceSubNum = Convert.ToInt32(invoiceSplit[1]);
                //Calls query to return invoice based on number
                List<Invoice> i = ssm.getInvoice(invoiceNum);
                //Binds the invoice to grid view
                grdInvoicesBetweenDates.DataSource = i;
                grdInvoicesBetweenDates.DataBind();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = cu.empID;
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
        protected void btnDeletedInvoiceBetweenDates_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnDeletedInvoiceBetweenDates_Click";
            try
            {
                Session["isDeleted"] = true;
                //Gathers start and end dates
                DateTime startDate = calStartDate.SelectedDate;
                DateTime endDate = calEndDate.SelectedDate;
                //Gathers selected location
                string locationID = ddlLocation.SelectedValue;
                //Calls query to return list of all invoices between dates
                List<Invoice> i = ssm.getInvoiceBetweenDates(startDate, endDate, "tbl_deletedInvoice", locationID);
                grdInvoicesBetweenDates.Columns[8].Visible = false;
                //Binds invoice list to gridview
                grdInvoicesBetweenDates.DataSource = i;
                grdInvoicesBetweenDates.DataBind();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = cu.empID;
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
        protected void lbtnInvoiceNumber_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "lbtnInvoiceNumber_Click";
            try
            {
                //Text of the linkbutton
                LinkButton btn = sender as LinkButton;
                string invoice = btn.Text;
                //Parsing into invoiceNum and invoiceSubNum
                char[] splitchar = { '-' };
                string[] invoiceSplit = invoice.Split(splitchar);
                int invoiceNum = Convert.ToInt32(invoiceSplit[0]);
                int invoiceSubNum = Convert.ToInt32(invoiceSplit[1]);
                Boolean isDeleted = Convert.ToBoolean(Session["isDeleted"]);
                //determines the table to use for queries
                string table = "i";
                if (isDeleted)
                {
                    table = "deletedI";
                }
                //Stores required info into Sessions

                Invoice rInvoice = ssm.getSingleInvoice(invoiceNum, invoiceSubNum);
                Session["key"] = rInvoice.customerID;
                Session["Invoice"] = invoice;
                Session["useInvoice"] = true;
                Session["strDate"] = rInvoice.invoiceDate;
                Session["ItemsInCart"] = ssm.invoice_getItems(invoiceNum, invoiceSubNum, "tbl_" + table + "nvoiceItem");
                Session["CheckOutTotals"] = ssm.invoice_getCheckoutTotals(invoiceNum, invoiceSubNum, "tbl_" + table + "nvoice");
                Session["MethodsOfPayment"] = ssm.invoice_getMOP(invoiceNum, invoiceSubNum, "tbl_" + table + "nvoiceMOP");
                Session["TranType"] = 1;
                //Changes to printable invoice page
                Server.Transfer("PrintableInvoice.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = cu.empID;
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
        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //Collects current method for error tracking
            string method = "OnRowDeleting";
            try
            {
                string deleteReason = hidden.Value;
                //Checks deleted reason
                if (deleteReason.Equals("Code:CancelDelete"))
                {

                }
                else if (!deleteReason.Equals("Code:CancelDelete") && !deleteReason.Equals(""))
                {
                    //Gathers current row index
                    int index = e.RowIndex;
                    //determins the invoice
                    Label lblInvoice = (Label)grdInvoicesBetweenDates.Rows[index].FindControl("lblInvoiceNumber");
                    string invoice = lblInvoice.Text;
                    //splits the invoice number
                    char[] splitchar = { '-' };
                    string[] invoiceSplit = invoice.Split(splitchar);
                    int invoiceNum = Convert.ToInt32(invoiceSplit[0]);
                    int invoiceSubNum = Convert.ToInt32(invoiceSplit[1]);
                    //Calls query to delete the selected invoice
                    string deletionReason = deleteReason;
                    idu.deleteInvoice(invoiceNum, invoiceSubNum, deletionReason);
                    MessageBox.ShowMessage("Invoice " + invoice + " has been deleted", this);
                    //Refreshes current page
                    Server.Transfer(Request.RawUrl);
                }
                //Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "userInput()", true);
                ////string deletionReason = cmb.inputBoxV2("Reason", "Reason for deleting invoice:");
                //string deletionReason = deletionReason = cmb.InputBox("Reason", "Reason for deleting invoice:");
                //while (deletionReason == "")
                //{
                //    deletionReason = cmb.InputBox("Reason", "Reason for deleting invoice:");
                //    //deletionReason = Microsoft.VisualBasic.Interaction.InputBox("Reason for deleting invioce", "", "", -1, -1);
                //}
                //Label1.Text = deletionReason;
                //int index = e.RowIndex;
                //Label lblInvoice = (Label)grdInvoicesBetweenDates.Rows[index].FindControl("lblInvoiceNumber");
                //string invoice = lblInvoice.Text;
                //char[] splitchar = { '-' };
                //string[] invoiceSplit = invoice.Split(splitchar);
                //int invoiceNum = Convert.ToInt32(invoiceSplit[0]);
                //int invoiceSubNum = Convert.ToInt32(invoiceSplit[1]);
                //idu.deleteInvoice(invoiceNum, invoiceSubNum, deletionReason);
                //MessageBox.ShowMessage("Invoice " + invoice + " has been deleted", this);
                //Server.Transfer(Request.RawUrl);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = cu.empID;
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