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

namespace SweetSpotDiscountGolfPOS
{
    public partial class ReportsHomePage : System.Web.UI.Page
    {
        ErrorReporting er = new ErrorReporting();
        SweetShopManager ssm = new SweetShopManager();
        Reports r = new Reports();
        ItemDataUtilities idu = new ItemDataUtilities();
        CustomMessageBox cmb = new CustomMessageBox();

        //List<Invoice> invoice;
        protected void Page_Load(object sender, EventArgs e)
        {
            string method = "Page_Load";
            Session["currPage"] = "ReportsHomePage";
            Session["prevPage"] = "HomePage";
            try
            {
                if (Convert.ToBoolean(Session["loggedIn"]) == false)
                {
                    Server.Transfer("LoginPage.aspx", false);
                }
                if (!IsPostBack)
                {
                    calStartDate.SelectedDate = DateTime.Today;
                    calEndDate.SelectedDate = DateTime.Today;
                    txtStartDate.Text = DateTime.Today.ToShortDateString();
                    txtEndDate.Text = DateTime.Today.ToShortDateString();

                }
                if (Session["Admin"] == null)
                {
                    lblReport.Text = "You are not authorized to view reports";
                    lblReport.Visible = true;
                    lblReport.ForeColor = System.Drawing.Color.Red;
                    //calStart.Visible = false;
                    //calEnd.Visible = false;
                    btnRunReport.Visible = false;
                    txtEndDate.Visible = false;
                    txtStartDate.Visible = false;
                    pnlDefaultButton.Visible = false;
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

        protected void calStart_SelectionChanged(object sender, EventArgs e)
        {
            txtStartDate.Text = calStartDate.SelectedDate.ToShortDateString();
        }
        protected void calEnd_SelectionChanged(object sender, EventArgs e)
        {
            txtEndDate.Text = calEndDate.SelectedDate.ToShortDateString();
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtStartDate.Text == "" || txtEndDate.Text == "")
            {
                lbldate.Visible = true;
                lbldate.Text = "Please Select a Start and End date";
                lbldate.ForeColor = System.Drawing.Color.Red;

            }
            else
            {
                DateTime[] reportDates = new DateTime[2];
                reportDates[0] = calStartDate.SelectedDate;
                reportDates[1] = calEndDate.SelectedDate;
                Session["reportDates"] = reportDates;

                //  if(txtStartDate.Text == txtEndDate.Text)
                //  {

                // //invoice = ssm.selectAllInventorySalesBetweenDates(Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text).AddHours(23).AddMinutes(59));
                // Session["salesinvoice"] = invoice;

                //}
            }

            Server.Transfer("ReportsCashOut.aspx", false);
        }
        protected void btnExportInvoices_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString;
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            SqlDataAdapter im = new SqlDataAdapter("SELECT * FROM tbl_invoice", sqlCon);
            System.Data.DataTable dtim = new System.Data.DataTable();
            im.Fill(dtim);
            DataColumnCollection dcimHeaders = dtim.Columns;
            sqlCon.Close();

            sqlCon.Open();
            SqlDataAdapter ii = new SqlDataAdapter("SELECT * FROM tbl_invoiceItem", sqlCon);
            System.Data.DataTable dtii = new System.Data.DataTable();
            ii.Fill(dtii);
            DataColumnCollection dciiHeaders = dtii.Columns;
            sqlCon.Close();

            sqlCon.Open();
            SqlDataAdapter imo = new SqlDataAdapter("SELECT * FROM tbl_invoiceMOP", sqlCon);
            System.Data.DataTable dtimo = new System.Data.DataTable();
            imo.Fill(dtimo);
            DataColumnCollection dcimoHeaders = dtimo.Columns;
            sqlCon.Close();

            string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string pathDownload = (pathUser + "\\Downloads\\");
            FileInfo newFile = new FileInfo(pathDownload + "InvoiceReport.xlsx");
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet invoiceMain = xlPackage.Workbook.Worksheets.Add("Invoice Main");
                ExcelWorksheet invoiceItems = xlPackage.Workbook.Worksheets.Add("Invoice Items");
                ExcelWorksheet invoiceMOPS = xlPackage.Workbook.Worksheets.Add("Invoice MOPS");
                // write to sheet

                //Initiating Everything   
                System.Data.DataTable exportInvoiceTable = r.initiateInvoiceTable();
                System.Data.DataTable exportInvoiceItemTable = r.initiateInvoiceItemTable();
                System.Data.DataTable exportInvoiceMOPTable = r.initiateInvoiceMOPTable();

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
        protected void btnTesting_Click(object sender, EventArgs e)
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


        //Viewing invoices
        protected void btnInvoiceBetweenDates_Click(object sender, EventArgs e)
        {
            Session["isDeleted"] = false;
            DateTime startDate = calStartDate.SelectedDate;
            DateTime endDate = calEndDate.SelectedDate;
            List<Invoice> i = ssm.getInvoiceBetweenDates(startDate, endDate, "tbl_invoice");
            grdInvoicesBetweenDates.Columns[8].Visible = true;
            grdInvoicesBetweenDates.DataSource = i;
            grdInvoicesBetweenDates.DataBind();

        }
        protected void btnReturnInvoice_Click(object sender, EventArgs e)
        {
            Session["isDeleted"] = false;
            string invoiceNumber = txtInvoiceNum.Text;
            char[] splitchar = { '-' };
            string[] invoiceSplit = invoiceNumber.Split(splitchar);
            int invoiceNum = Convert.ToInt32(invoiceSplit[0]);
            int invoiceSubNum = Convert.ToInt32(invoiceSplit[1]);
            List<Invoice> i = ssm.getInvoice(invoiceNum);
            grdInvoicesBetweenDates.DataSource = i;
            grdInvoicesBetweenDates.DataBind();

        }
        protected void btnDeletedInvoiceBetweenDates_Click(object sender, EventArgs e)
        {
            Session["isDeleted"] = true;
            DateTime startDate = calStartDate.SelectedDate;
            DateTime endDate = calEndDate.SelectedDate;
            List<Invoice> i = ssm.getInvoiceBetweenDates(startDate, endDate, "tbl_deletedInvoice");
            grdInvoicesBetweenDates.Columns[8].Visible = false;
            grdInvoicesBetweenDates.DataSource = i;
            grdInvoicesBetweenDates.DataBind();

        }
        protected void lbtnInvoiceNumber_Click(object sender, EventArgs e)
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
            if (isDeleted == false)
            {
                Session["key"] = ssm.invoice_getCustID(invoiceNum, invoiceSubNum, "tbl_invoice");
                Session["Invoice"] = invoice;
                Session["useInvoice"] = true;
                Session["ItemsInCart"] = ssm.invoice_getItems(invoiceNum, invoiceSubNum, "tbl_invoiceItem");
                Session["CheckOutTotals"] = ssm.invoice_getCheckoutTotals(invoiceNum, invoiceSubNum, "tbl_invoice");
                Session["MethodsOfPayment"] = ssm.invoice_getMOP(invoiceNum, invoiceSubNum, "tbl_invoiceMOP");
            }
            else if (isDeleted == true)
            {
                Session["key"] = ssm.invoice_getCustID(invoiceNum, invoiceSubNum, "tbl_deletedInvoice");
                Session["Invoice"] = invoice;
                Session["useInvoice"] = true;
                Session["ItemsInCart"] = ssm.invoice_getItems(invoiceNum, invoiceSubNum, "tbl_deletedInvoiceItem");
                Session["CheckOutTotals"] = ssm.invoice_getCheckoutTotals(invoiceNum, invoiceSubNum, "tbl_deletedInvoice");
                Session["MethodsOfPayment"] = ssm.invoice_getMOP(invoiceNum, invoiceSubNum, "tbl_deletedInvoiceMOP");
            }
            Server.Transfer("PrintableInvoice.aspx", false);
        }
        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //string deletionReason = cmb.inputBoxV2("Reason", "Reason for deleting invoice:");
            string deletionReason = Microsoft.VisualBasic.Interaction.InputBox("Reason for deleting invioce", "", "", -1, -1);
            while (deletionReason == "")
            {
                deletionReason = cmb.InputBox("Reason", "Reason for deleting invoice:");
                //deletionReason = Microsoft.VisualBasic.Interaction.InputBox("Reason for deleting invioce", "", "", -1, -1);
            }
            //Label1.Text = deletionReason;
            int index = e.RowIndex;
            Label lblInvoice = (Label)grdInvoicesBetweenDates.Rows[index].FindControl("lblInvoiceNumber");
            string invoice = lblInvoice.Text;
            char[] splitchar = { '-' };
            string[] invoiceSplit = invoice.Split(splitchar);
            int invoiceNum = Convert.ToInt32(invoiceSplit[0]);
            int invoiceSubNum = Convert.ToInt32(invoiceSplit[1]);
            idu.deleteInvoice(invoiceNum, invoiceSubNum, deletionReason);
            MessageBox.ShowMessage("Invoice " + invoice + " has been deleted", this);
            Server.Transfer(Request.RawUrl, false);
        }
    }
}