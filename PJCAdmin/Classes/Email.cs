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
        public static void send(PJCAdmin.Models.EmailOutbox outbox, string toEmailAddress, string subject, string messageBody)
        {
            send(outbox.emailAddress, outbox.emailName, toEmailAddress, subject, messageBody, outbox.emailPassword, outbox.smtpServerName, outbox.portNumber, outbox.smtpTimeout);
        }

        /// <summary>
        /// Sends an email via G-Mail with the provided email settings.
        /// </summary>
        /// <param name="fromEmailAddress"></param>
        /// <param name="toEmailAddress"></param>
        /// <param name="subject"></param>
        /// <param name="messageBody"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static void send(string fromEmailAddress, string fromName, string toEmailAddress, string subject, string messageBody, string password, string server, int port, int timeout)
        {
            CDO.Message smtp = new CDO.Message();
            CDO.Configuration smtpConfig = new CDO.Configuration();

            ADODB.Fields fieldCollection = smtpConfig.Fields;
            ADODB.Field serverField = fieldCollection["http://schemas.microsoft.com/cdo/configuration/smtpserver"];
            serverField.Value = server; 
            ADODB.Field usernameField = fieldCollection["http://schemas.microsoft.com/cdo/configuration/sendusername"];
            usernameField.Value = fromEmailAddress;
            ADODB.Field passwordField = fieldCollection["http://schemas.microsoft.com/cdo/configuration/sendpassword"];
            passwordField.Value = password;
            ADODB.Field authField = fieldCollection["http://schemas.microsoft.com/cdo/configuration/smtpauthenticate"];
            authField.Value = 1;
            ADODB.Field timeoutField = fieldCollection["http://schemas.microsoft.com/cdo/configuration/smtpconnectiontimeout"];
            timeoutField.Value = timeout;
            ADODB.Field serverPortField = fieldCollection["http://schemas.microsoft.com/cdo/configuration/smtpserverport"];
            serverPortField.Value = port;
            ADODB.Field sendUsingField = fieldCollection["http://schemas.microsoft.com/cdo/configuration/sendusing"];
            sendUsingField.Value = 2;
            ADODB.Field useSSLField = fieldCollection["http://schemas.microsoft.com/cdo/configuration/smtpusessl"];
            useSSLField.Value = 1;
            fieldCollection.Update();

            smtp.Configuration = smtpConfig;
            smtp.Subject = subject;
            smtp.From = fromEmailAddress;
            smtp.To = toEmailAddress;
            smtp.Sender = fromName;
            smtp.Organization = fromName;
            smtp.ReplyTo = fromEmailAddress;
            smtp.TextBody = messageBody;
            smtp.Send();
        }
    }
}