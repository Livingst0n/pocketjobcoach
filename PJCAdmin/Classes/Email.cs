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
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(fromEmailAddress, gmailPassword);
                smtp.Port = 587; //587 sends TLS error, 465 sends general failure error (implicit vs explicit ssl), 25 sends TLS error
                smtp.Host = "smtp.gmail.com";
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                MailMessage message = new MailMessage();
                message.From = new MailAddress(fromEmailAddress);
                message.To.Add(toEmailAddress);
                message.Subject = subject;
                message.Body = messageBody;
                smtp.EnableSsl = true;
                smtp.Send(message); //SmtpException is generated here...
                return true;
            }
            catch (SmtpException e) 
            {
                if (e.StatusCode == SmtpStatusCode.MustIssueStartTlsFirst)
                    return false;
                
                else
                    return false;
            }
            //catch (SmtpFailedRecipientsException) { return true;}
            //catch
            //{
               // return false;
            //}
        }
    }
}