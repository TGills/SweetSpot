using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetShop
{
    //The current user class is used to keep track of the current user's information
    class CurrentUser
    {
        private int password;
        private int empID { get; set; }
        private int locationID { get; set; }

        public CurrentUser() { }
        public CurrentUser(int id, int loc)
        {
            empID = id;
            locationID = loc;
        }
        public CurrentUser(int password)
        {
            this.password = password;
        }
        
       
        
    }
}
