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

namespace SweetSpotDiscountGolfPOS
{
    public partial class ReportsHomePage : System.Web.UI.Page
    {

        SweetShopManager ssm = new SweetShopManager();
        Reports r = new Reports();


        //List<Invoice> invoice;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(Session["loggedIn"]) == false)
            {
                Response.Redirect("LoginPage.aspx");
            }

            if (Session["Admin"] == null)
            {
                lblReport.Text = "You are not authorized to view reports";
                lblReport.Visible = true;
                lblReport.ForeColor = System.Drawing.Color.Red;
                calStart.Visible = false;
                calEnd.Visible = false;
                btnRunReport.Visible = false;
                txtEndDate.Visible = false;
                txtStartDate.Visible = false;
                pnlDefaultButton.Visible = false;
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

            Response.Redirect("ReportsCashOut.aspx");
        }

        protected void btnExportInvoices_Click(object sender, EventArgs e)
        {
            r.exportAllSalesToExcel();
            MessageBox.ShowMessage("Report Completed. Check Downloads", this);


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


                System.Data.DataTable exportInvoiceTable =  r.initiateInvoiceTable(); 
                System.Data.DataTable exportInvoiceItemTable = r.initiateInvoiceItemTable();
                System.Data.DataTable exportInvoiceMOPTable = r.initiateInvoiceMOPTable();




                //Export main invoice
                for (int i = 1; i < exportInvoiceTable.Rows.Count + 2; i++)
                {
                    for (int j = 1; j < exportInvoiceTable.Columns.Count + 1; j++)
                    {
                        if (i == 1)
                        {
                            invoiceMain.Cells[i, j].Value = exportInvoiceTable.Rows[i - 2][j - 1].ToString();
                        }
                        else
                            invoiceMain.Cells[i, j].Value = exportInvoiceTable.Rows[i - 2][j - 1].ToString();
                    }
                }
                //Export item invoice
                for (int i = 1; i < exportInvoiceItemTable.Rows.Count + 2; i++)
                {
                    for (int j = 1; j < exportInvoiceItemTable.Columns.Count + 1; j++)
                    {
                        if (i == 1)
                        {
                            invoiceItems.Cells[i, j].Value = exportInvoiceItemTable.Rows[i - 2][j - 1].ToString();
                        }
                        else
                            invoiceItems.Cells[i, j].Value = exportInvoiceItemTable.Rows[i - 2][j - 1].ToString();
                    }
                }
                //Export mop invoice
                for (int i = 1; i < exportInvoiceMOPTable.Rows.Count + 2; i++)
                {
                    for (int j = 1; j < exportInvoiceMOPTable.Columns.Count + 1; j++)
                    {
                        if (i == 1)
                        {
                            invoiceMOPS.Cells[i, j].Value = exportInvoiceMOPTable.Rows[i - 2][j - 1].ToString();
                        }
                        else
                            invoiceMOPS.Cells[i, j].Value = exportInvoiceMOPTable.Rows[i - 2][j - 1].ToString();
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
            string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string pathDownload = (pathUser + "\\Downloads\\");
            FileInfo newFile = new FileInfo(pathDownload + "mynewfile.xlsx");
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Test Sheet");
                // write to sheet
                worksheet.Cells[1, 1].Value = "Test";
                //xlPackage.SaveAs(aFile);

                Response.Clear();
                Response.AddHeader("content-disposition", "attachment; filename=test.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.BinaryWrite(xlPackage.GetAsByteArray());
                Response.End();
            }
        }

    }
}