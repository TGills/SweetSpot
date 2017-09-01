using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;

namespace SweetSpotDiscountGolfPOS
{
    public partial class EmployeeAddNew : System.Web.UI.Page
    {
        LocationManager lm = new LocationManager();
        EmployeeManager empM = new EmployeeManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(Session["loggedIn"]) == false)
            {
                Server.Transfer("LoginPage.aspx", false);
            }

            if (Session["Admin"] == null)
            {
                btnEditEmployee.Enabled = false;
            }
            if (Session["empKey"] != null)
            {
                if (!IsPostBack)
                {
                    int empNum = (Convert.ToInt32(Session["empKey"].ToString()));
                    Employee em = empM.getEmployeeByID(empNum);

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

                btnSaveEmployee.Visible = false;
                btnAddEmployee.Visible = true;
                pnlDefaultButton.DefaultButton = "btnAddEmployee";
                btnEditEmployee.Visible = false;
                btnCancel.Visible = false;
                btnBackToSearch.Visible = true;
            }

        }
        protected void btnAddEmployee_Click(object sender, EventArgs e)
        {

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

            Session["empKey"] = empM.addEmployee(em);
            Server.Transfer(Request.RawUrl, false);
        }
        protected void btnEditEmployee_Click(object sender, EventArgs e)
        {

            txtFirstName.Text = lblFirstNameDisplay.Text;
            txtFirstName.Visible = true;
            lblFirstNameDisplay.Visible = false;

            txtLastName.Text = lblLastNameDisplay.Text;
            txtLastName.Visible = true;
            lblLastNameDisplay.Visible = false;

            ddlJob.SelectedValue = empM.jobType(lblJobDisplay.Text).ToString();
            ddlJob.Visible = true;
            lblJobDisplay.Visible = false;

            ddlLocation.Text = lm.locationID(lblLocationDisplay.Text).ToString();
            ddlLocation.Visible = true;
            lblLocationDisplay.Visible = false;

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

            ddlProvince.Text = lm.pronvinceID(lblProvinceDisplay.Text).ToString();
            ddlProvince.Visible = true;
            lblProvinceDisplay.Visible = false;

            ddlCountry.Text = lm.countryID(lblCountryDisplay.Text).ToString();
            ddlCountry.Visible = true;
            lblCountryDisplay.Visible = false;

            btnSaveEmployee.Visible = true;
            pnlDefaultButton.DefaultButton = "btnSaveEmployee";
            btnEditEmployee.Visible = false;
            btnAddEmployee.Visible = false;
            btnCancel.Visible = true;
            btnBackToSearch.Visible = false;

        }
        protected void btnSaveEmployee_Click(object sender, EventArgs e)
        {

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

            empM.updateEmployee(em);

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

            btnSaveEmployee.Visible = false;
            btnEditEmployee.Visible = true;
            pnlDefaultButton.DefaultButton = "btnEditEmployee";
            btnCancel.Visible = false;
            btnAddEmployee.Visible = false;
            btnBackToSearch.Visible = true;

            Server.Transfer(Request.RawUrl, false);

        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Server.Transfer(Request.RawUrl, false);
        }
        protected void btnBackToSearch_Click(object sender, EventArgs e)
        {
            Session["empKey"] = null;
            Server.Transfer("SettingsHomePage.aspx", false);
        }
    }
}