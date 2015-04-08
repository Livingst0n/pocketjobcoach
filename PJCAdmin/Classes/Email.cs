using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace PJCAdmin.Classes
{
    public class Email
    {
        /// <summary>
        /// Sends an email via G-Mail with the provided email settings.
        /// </summary>
        /// <param name="fromEmailAddress"></param>
        /// <param name="toEmailAddress"></param>
        /// <param name="subject"></param>
        /// <param name="messageBody"></param>
        /// <param name="gmailPassword"></param>
        /// <returns></returns>
        public static Boolean viaGmail(string fromEmailAddress, string toEmailAddress, string subject, string messageBody, string gmailPassword)
        {
            try
            {
                SmtpClient smtp = new SmtpClient();
                smtp.Credentials = new NetworkCredential(fromEmailAddress, gmailPassword);
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;

                MailMessage message = new MailMessage();
                message.From = new MailAddress(fromEmailAddress);
                message.To.Add(toEmailAddress);
                message.Subject = subject;
                message.Body = messageBody;
                smtp.Send(message);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}