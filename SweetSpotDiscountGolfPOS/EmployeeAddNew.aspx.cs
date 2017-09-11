using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using SweetSpotProShop;

namespace SweetSpotDiscountGolfPOS
{
    public partial class EmployeeAddNew : System.Web.UI.Page
    {
        ErrorReporting er = new ErrorReporting();
        LocationManager lm = new LocationManager();
        EmployeeManager empM = new EmployeeManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "EmployeeAddNew.aspx";
            try
            {
                //checks if the user has logged in
                if (Convert.ToBoolean(Session["loggedIn"]) == false)
                {
                    //Go back to Login to log in
                    Server.Transfer("LoginPage.aspx", false);
                }

                if (Session["Admin"] == null)
                {
                    //If user is not an admin then disable the edit employee button
                    btnEditEmployee.Enabled = false;
                }
                //Check to see if an employee was selected
                if (Session["empKey"] != null)
                {
                    if (!IsPostBack)
                    {
                        //Using the employee number
                        int empNum = (Convert.ToInt32(Session["empKey"].ToString()));
                        //Create an employee class
                        Employee em = empM.getEmployeeByID(empNum);
                        //Fill asll lables with current selected employee info
                        lblFirstNameDisplay.Text = em.firstName.ToString();
                        lblLastNameDisplay.Text = em.lastName.ToString();
                        lblJobDisplay.Text = empM.jobName(em.jobID);
                        lblLocationDisplay.Text = lm.locationName(em.locationID);
                        lblEmailDisplay.Text = em.emailAddress.ToString();
                        lblPrimaryPhoneNumberDisplay.Text = em.primaryContactNumber.ToString();
                        lblSecondaryPhoneNumberDisplay.Text = em.secondaryContactNumber.ToString();
                        lblPrimaryAddressDisplay.Text = em.primaryAddress.ToString();
                        lblSecondaryAddressDisplay.Text = em.secondaryAddress.ToString();
                        lblCityDisplay.Text = em.city.ToString();
                        lblPostalCodeDisplay.Text = em.postZip.ToString();
                        lblProvinceDisplay.Text = lm.provinceName(em.provState);
                        lblCountryDisplay.Text = lm.countryName(em.country);
                    }
                }
                else
                {
                    //With no employee selected display text boxes and drop downs to add employee
                    txtFirstName.Visible = true;
                    lblFirstNameDisplay.Visible = false;
                    txtLastName.Visible = true;
                    lblLastNameDisplay.Visible = false;
                    ddlJob.Visible = true;
                    lblJobDisplay.Visible = false;
                    ddlLocation.Visible = true;
                    lblLocationDisplay.Visible = false;
                    txtEmail.Visible = true;
                    lblEmailDisplay.Visible = false;
                    txtPrimaryPhoneNumber.Visible = true;
                    lblPrimaryPhoneNumberDisplay.Visible = false;
                    txtSecondaryPhoneNumber.Visible = true;
                    lblSecondaryPhoneNumberDisplay.Visible = false;
                    txtPrimaryAddress.Visible = true;
                    lblPrimaryAddressDisplay.Visible = false;
                    txtSecondaryAddress.Visible = true;
                    lblSecondaryAddressDisplay.Visible = false;
                    txtCity.Visible = true;
                    lblCityDisplay.Visible = false;
                    txtPostalCode.Visible = true;
                    lblPostalCodeDisplay.Visible = false;
                    ddlProvince.Visible = true;
                    lblProvinceDisplay.Visible = false;
                    ddlCountry.Visible = true;
                    lblCountryDisplay.Visible = false;

                    //hides and displays the proper buttons for access
                    btnSaveEmployee.Visible = false;
                    btnAddEmployee.Visible = true;
                    pnlDefaultButton.DefaultButton = "btnAddEmployee";
                    btnEditEmployee.Visible = false;
                    btnCancel.Visible = false;
                    btnBackToSearch.Visible = true;
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
        protected void btnAddEmployee_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnAddEmployee_Click";
            try
            {
                //Collects new employee data to add to database
                Employee em = new Employee();
                em.firstName = txtFirstName.Text;
                em.lastName = txtLastName.Text;
                em.jobID = Convert.ToInt32(ddlJob.SelectedValue);
                em.locationID = Convert.ToInt32(ddlLocation.SelectedValue);
                em.emailAddress = txtEmail.Text;
                em.primaryContactNumber = txtPrimaryPhoneNumber.Text;
                em.secondaryContactNumber = txtSecondaryPhoneNumber.Text;
                em.primaryAddress = txtPrimaryAddress.Text;
                em.secondaryAddress = txtSecondaryAddress.Text;
                em.city = txtCity.Text;
                em.postZip = txtPostalCode.Text;
                em.provState = Convert.ToInt32(ddlProvince.SelectedValue);
                em.country = Convert.ToInt32(ddlCountry.SelectedValue);

                //Process the add and saves the employee into the key.
                Session["empKey"] = empM.addEmployee(em);
                Server.Transfer(Request.RawUrl, false);
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
        protected void btnEditEmployee_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnEditEmployee_Click";
            try
            {
                //transfers data from label into textbox for editing
                txtFirstName.Text = lblFirstNameDisplay.Text;
                txtFirstName.Visible = true;
                lblFirstNameDisplay.Visible = false;

                txtLastName.Text = lblLastNameDisplay.Text;
                txtLastName.Visible = true;
                lblLastNameDisplay.Visible = false;

                txtEmail.Text = lblEmailDisplay.Text;
                txtEmail.Visible = true;
                lblEmailDisplay.Visible = false;

                txtPrimaryPhoneNumber.Text = lblPrimaryPhoneNumberDisplay.Text;
                txtPrimaryPhoneNumber.Visible = true;
                lblPrimaryPhoneNumberDisplay.Visible = false;

                txtSecondaryPhoneNumber.Text = lblSecondaryPhoneNumberDisplay.Text;
                txtSecondaryPhoneNumber.Visible = true;
                lblSecondaryPhoneNumberDisplay.Visible = false;

                txtPrimaryAddress.Text = lblPrimaryAddressDisplay.Text;
                txtPrimaryAddress.Visible = true;
                lblPrimaryAddressDisplay.Visible = false;

                txtSecondaryAddress.Text = lblSecondaryAddressDisplay.Text;
                txtSecondaryAddress.Visible = true;
                lblSecondaryAddressDisplay.Visible = false;

                txtCity.Text = lblCityDisplay.Text;
                txtCity.Visible = true;
                lblCityDisplay.Visible = false;

                txtPostalCode.Text = lblPostalCodeDisplay.Text;
                txtPostalCode.Visible = true;
                lblPostalCodeDisplay.Visible = false;

                //transfers data from label into dropdown for editing
                ddlJob.SelectedValue = empM.jobType(lblJobDisplay.Text).ToString();
                ddlJob.Visible = true;
                lblJobDisplay.Visible = false;

                ddlLocation.Text = lm.locationID(lblLocationDisplay.Text).ToString();
                ddlLocation.Visible = true;
                lblLocationDisplay.Visible = false;


                ddlProvince.Text = lm.pronvinceID(lblProvinceDisplay.Text).ToString();
                ddlProvince.Visible = true;
                lblProvinceDisplay.Visible = false;

                ddlCountry.Text = lm.countryID(lblCountryDisplay.Text).ToString();
                ddlCountry.Visible = true;
                lblCountryDisplay.Visible = false;

                //hides and displays the proper buttons for access
                btnSaveEmployee.Visible = true;
                pnlDefaultButton.DefaultButton = "btnSaveEmployee";
                btnEditEmployee.Visible = false;
                btnAddEmployee.Visible = false;
                btnCancel.Visible = true;
                btnBackToSearch.Visible = false;
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
        protected void btnSaveEmployee_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnSaveEmployee_Click";
            try
            {
                //Collects employee data to add to database
                Employee em = new Employee();
                em.employeeID = Convert.ToInt32(Session["empKey"].ToString());
                em.firstName = txtFirstName.Text;
                em.lastName = txtLastName.Text;
                em.jobID = Convert.ToInt32(ddlJob.SelectedValue);
                em.locationID = Convert.ToInt32(ddlLocation.SelectedValue);
                em.emailAddress = txtEmail.Text;
                em.primaryContactNumber = txtPrimaryPhoneNumber.Text;
                em.secondaryContactNumber = txtSecondaryPhoneNumber.Text;
                em.primaryAddress = txtPrimaryAddress.Text;
                em.secondaryAddress = txtSecondaryAddress.Text;
                em.city = txtCity.Text;
                em.postZip = txtPostalCode.Text;
                em.provState = Convert.ToInt32(ddlProvince.SelectedValue);
                em.country = Convert.ToInt32(ddlCountry.SelectedValue);
                //updates the customer info in tables
                empM.updateEmployee(em);
                //changes all text boxes and dropdowns to labels
                txtFirstName.Visible = false;
                lblFirstNameDisplay.Visible = true;
                txtLastName.Visible = false;
                lblLastNameDisplay.Visible = true;
                ddlJob.Visible = false;
                lblJobDisplay.Visible = true;
                ddlLocation.Visible = false;
                lblLocationDisplay.Visible = true;
                txtEmail.Visible = false;
                lblEmailDisplay.Visible = true;
                txtPrimaryPhoneNumber.Visible = false;
                lblPrimaryPhoneNumberDisplay.Visible = true;
                txtSecondaryPhoneNumber.Visible = false;
                lblSecondaryPhoneNumberDisplay.Visible = true;
                txtPrimaryAddress.Visible = false;
                lblPrimaryAddressDisplay.Visible = true;
                txtSecondaryAddress.Visible = false;
                lblSecondaryAddressDisplay.Visible = true;
                txtCity.Visible = false;
                lblCityDisplay.Visible = true;
                txtPostalCode.Visible = false;
                lblPostalCodeDisplay.Visible = true;
                ddlProvince.Visible = false;
                lblProvinceDisplay.Visible = true;
                ddlCountry.Visible = false;
                lblCountryDisplay.Visible = true;

                //hides and displays the proper buttons for access
                btnSaveEmployee.Visible = false;
                btnEditEmployee.Visible = true;
                pnlDefaultButton.DefaultButton = "btnEditEmployee";
                btnCancel.Visible = false;
                btnAddEmployee.Visible = false;
                btnBackToSearch.Visible = true;
                //reloads current page
                Server.Transfer(Request.RawUrl, false);
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
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnCancel_Click";
            try
            {
                //no changes saved, refreshes current page
                Server.Transfer(Request.RawUrl, false);
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
        protected void btnBackToSearch_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnBackToSearch_Click";
            try
            {
                //removes employee key that was set so no employee is currently selected
                Session["empKey"] = null;
                //Changes page to the settings page
                Server.Transfer("SettingsHomePage.aspx", false);
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