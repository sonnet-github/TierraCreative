using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Net.Mail;
using System.Net.Mime;
using TierraCreative.ViewModels;
using System.Configuration;

namespace TierraCreative.Controllers
{
    public class SendGridHelper
    {
        public bool SendEmail(SendGridModel model)
        {
            string emailhost = "";
            int emailport = 0;
            string emailusername = "";
            string emailpassword = "";

            try
            {
                MailMessage mailMsg = new MailMessage();

                // From
                mailMsg.From = new MailAddress(model.From.Email, model.From.Name);

                // Multiple To
                foreach (var recipientTo in model.To)
                    mailMsg.To.Add(new MailAddress(recipientTo.Email, recipientTo.Name));

                // Multiple Cc
                foreach (var recipientCc in model.Cc)
                    mailMsg.CC.Add(new MailAddress(recipientCc.Email, recipientCc.Name));

                // Multiple Bcc
                foreach (var recipientBcc in model.Bcc)
                    mailMsg.Bcc.Add(new MailAddress(recipientBcc.Email, recipientBcc.Name));

                // Multiple Attachment
                foreach (var attachment in model.Attachment)
                    mailMsg.Attachments.Add(attachment);

                //Subject
                mailMsg.Subject = model.Subject;

                //Body
                if (model.IsBodyHtml)
                    mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(model.Body, null, MediaTypeNames.Text.Html));
                else
                    mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(model.Body, null, MediaTypeNames.Text.Plain));

                //SmtpClient smtpClient = new SmtpClient(ConfigurationManager.AppSettings["emailhost"], Convert.ToInt32(ConfigurationManager.AppSettings["emailport"]));
                //System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["emailusername"], ConfigurationManager.AppSettings["emailpassword"]);

                SmtpClient smtpClient = new SmtpClient(emailhost, emailport);
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(emailusername, emailpassword);

                smtpClient.Credentials = credentials;

                smtpClient.Send(mailMsg);
            }
            catch(Exception e) 
            {
                //Utilities.WriteEmailErrLog(model.Subject, model.Body + " | " + e.InnerException + " | " + e.Message);

                return false;
            }

            return true;
        }

    }
}