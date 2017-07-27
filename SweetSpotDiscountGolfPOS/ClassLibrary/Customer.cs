using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetShop
{
    public class Customer
    {
        public int customerId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string primaryAddress { get; set; }
        public string secondaryAddress { get; set; }
        public string primaryPhoneNumber { get; set; }
        public string secondaryPhoneNumber { get; set; }
        public string billingAddress { get; set; }
        public string email { get; set; }
        public string city { get; set; }
        public int province { get; set; }
        public int country { get; set; }
        public string postalCode { get; set; }

        public Customer() { }

        public Customer(int CustomerID, string FirstName, string LastName, string pAddress,
           string sAddress, string pPhoneNumber, string sPhoneNumber, string billAddress, string Email,
           string City, int Province, int Country, string PostalCode)
        {
            customerId = CustomerID;
            firstName = FirstName;
            lastName = LastName;
            primaryAddress = pAddress;
            secondaryAddress = sAddress;
            primaryPhoneNumber = pPhoneNumber;
            secondaryPhoneNumber = sPhoneNumber;
            billingAddress = billAddress;
            email = Email;
            city = City;
            province = Province;
            country = Country;
            postalCode = PostalCode;
        }

        public Customer(string fname, string lname)
        {
            firstName = fname;
            lastName = lname;
        }


    }
}
