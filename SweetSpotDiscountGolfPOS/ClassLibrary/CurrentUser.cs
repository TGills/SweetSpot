using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetShop
{
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
