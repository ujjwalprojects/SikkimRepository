using SikkimRepository.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;

namespace SikkimRepository.Web.Utility
{
    public class SendMail
    {
        public bool SendEmail(utblTrnMessage Message)
        {
            System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration(System.Web.HttpContext.Current.Request.ApplicationPath);
            MailSettingsSectionGroup settings = (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");
            System.Net.NetworkCredential credential = new System.Net.NetworkCredential(settings.Smtp.Network.UserName, settings.Smtp.Network.Password);
            //Create the SMTP Client
            SmtpClient client = new SmtpClient();
            client.Host = settings.Smtp.Network.Host;
            client.Credentials = credential;
            client.Timeout = 300000;
            client.EnableSsl = false;

            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(settings.Smtp.Network.UserName, "Sikkim Shagun ");
            //SmtpClient client = new SmtpClient();
            //client.Host = "smtp.zoho.com";
            //client.Port = 587;
            //client.DeliveryMethod = SmtpDeliveryMethod.Network;

            ////client.Credentials = new System.Net.NetworkCredential("ssquota@gmail.com", "spacebar321");// Enter seders User name and password
            //client.UseDefaultCredentials = false;
            //client.Credentials = new System.Net.NetworkCredential("messenger@sikkimservice.in", "SSDG@123");// Enter seders User name and password
            //client.Timeout = 300000;
            //client.EnableSsl = true;
            //MailMessage msg = new MailMessage();
            //msg.From = new MailAddress("messenger@sikkimservice.in", "Sikkim Service");
            msg.Body = Message.MessageBody;
            msg.Subject = Message.MessageSubject;
            msg.To.Add(Message.SentTo);
            try
            {
                client.Send(msg);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}