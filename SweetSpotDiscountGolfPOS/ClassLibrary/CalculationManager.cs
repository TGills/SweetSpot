using SweetShop;
using SweetSpotProShop;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SweetSpotDiscountGolfPOS.ClassLibrary
{
    public class CalculationManager
    {
        ItemDataUtilities idu = new ItemDataUtilities();
        public double returnDiscount(List<Cart> itemsSold)
        {
            double singleDiscoount = 0;
            double totalDiscount = 0;
            foreach (var cart in itemsSold)
            {
                if (cart.percentage)
                {
                    singleDiscoount = cart.quantity * (cart.price * (cart.discount / 100));
                }
                else
                {
                    singleDiscoount = cart.quantity * cart.discount;
                }
                totalDiscount += singleDiscoount;
            }
            return Math.Round(totalDiscount, 2);
        }
        public double returnTradeInAmount(List<Cart> itemsSold, int loc)
        {
            double singleTradeInAmount = 0;
            double totalTradeinAmount = 0;

            int[] range = idu.tradeInSkuRange(loc);
            foreach (var cart in itemsSold)
            {
                if (cart.sku <= range[1] && cart.sku >= range[0])
                {
                    singleTradeInAmount = cart.quantity * cart.price;
                    totalTradeinAmount += singleTradeInAmount;
                }
            }
            return totalTradeinAmount;
        }
        public double returnSubtotalAmount(List<Cart> itemsSold, int loc)
        {
            double totalSubtotalAmount = 0;
            double totalDiscountAmount = returnDiscount(itemsSold);
            double totalTradeInAmount = returnTradeInAmount(itemsSold, loc);
            double totalTotalAmount = returnTotalAmount(itemsSold, loc);
            totalSubtotalAmount = totalSubtotalAmount + totalTotalAmount;
            totalSubtotalAmount = totalSubtotalAmount - totalDiscountAmount;
            totalSubtotalAmount = totalSubtotalAmount - (totalTradeInAmount * (-1));
            return totalSubtotalAmount;
        }
        public double returnTotalAmount(List<Cart> itemsSold, int loc)
        {
            int[] range = idu.tradeInSkuRange(loc);
            double singleTotalAmount = 0;
            double totalTotalAmount = 0;
            foreach (var cart in itemsSold)
            {
                if (cart.sku >= range[1] || cart.sku <= range[0])
                {
                    singleTotalAmount = cart.quantity * cart.price;
                    totalTotalAmount += singleTotalAmount;
                }
            }
            return totalTotalAmount;
        }
        public double returnGSTAmount(double rate, double subtotal)
        {
            double GSTAmount = 0;
            GSTAmount = Math.Round((rate * subtotal), 2);
            return GSTAmount;
        }
        public double returnPSTAmount(double rate, double subtotal)
        {
            double PSTAmount = 0;
            PSTAmount = Math.Round((rate * subtotal), 2);
            return PSTAmount;
        }
        public int returnLocationID(string loc)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select provStateID from tbl_location where city = '" + loc + "'";
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            int locID = 0;
            while (reader.Read())
            {
                locID = Convert.ToInt32(reader["provStateID"]);
            }
            conn.Close();
            return locID;
        }
        
    }
}