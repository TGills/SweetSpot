using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;

namespace SweetSpotDiscountGolfPOS.ClassLibrary
{
    public class ErrorReporting
    {



        public void sendError(string errorMessage)
        {
            SmtpClient SmtpServer = new SmtpClient();
            MailMessage mail = new MailMessage();
            SmtpServer.UseDefaultCredentials = true;
            SmtpServer.Credentials = new System.Net.NetworkCredential("sweetspotgolfshop@outlook.com", "ARu23B101");
            SmtpServer.EnableSsl = true;
            SmtpServer.Port = 587;
            SmtpServer.Host = "smtp.gmail.com";
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;

            mail = new MailMessage();
            mail.From = new MailAddress("sweetspotgolfshop@outlook.com");
            mail.To.Add("sweetspotgolfshop@outlook.com");
            mail.Subject = "Error " + DateTime.Now.ToString();
            mail.Body = errorMessage;
            
            SmtpServer.Send(mail);
        }


    }
}