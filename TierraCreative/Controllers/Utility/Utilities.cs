﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TierraCreative.ViewModels;

namespace TierraCreative.Controllers.Utility
{
    public class Utilities
    {
        public bool SendFormEmails(
            string TransactionID,
            string FromCSNValue, string ToCSNValue,
            string ISINValue,
            string AmountValue,
            string Timestamp,
            string FullName, string UserName,
            string FormName,
            string fromEmail,
            string ApprovalEmail,
            string computershareEmail,
            string submitUserEmail)
        {
            SendGridHelper emailhelper = new SendGridHelper();
            SendGridModel emailmodel = new SendGridModel();

            var From = new Recipient
            {
                Email = fromEmail,
                Name = fromEmail
            };
            emailmodel.From = From;

            var To = new Recipient
            {
                Email = ApprovalEmail,
                Name = ApprovalEmail
            };
            emailmodel.To.Add(To);

            foreach (var emailshare in computershareEmail.Split('|'))
            {
                To = new Recipient
                {
                    Email = emailshare,
                    Name = emailshare
                };
                emailmodel.To.Add(To);
            }

            To = new Recipient
            {
                Email = submitUserEmail,
                Name = submitUserEmail
            };
            emailmodel.To.Add(To);

            /*Uncomment if need to attach single or multiple file*/
            //Attachment attachment = new Attachment(@"C:\2BInteractive\Docs\FTPAcct.txt");
            //emailmodel.Attachment.Add(attachment);

            var subject = "[TransactionID] - [FormName] Transaction Approved - [Timestamp]";
            subject = subject
                      .Replace("[TransactionID]", TransactionID)
                      .Replace("[FormName]", FormName)
                      .Replace("[Timestamp]", System.DateTime.Now.ToString("yyyy-MM-dd"));
            //string body = "<style type=\"text/css\">body{font-family:Verdana, Geneva, sans-serif;font-size:13px;padding-top:0;margin-top:0;line-height:19px;}table {border: solid #666 1px;font-size:13px;line-height:19px;}td {border: solid #666 1px;padding:5px;}tr td:nth-child(2){font-weigth:bold;}</ style >";
            string body = "<p style='font-family:Verdana, Geneva, sans-serif;font-size:13px;padding-top:0;margin-top:0;padding-bottom:0;margin-bottom:0;line-height:19px;'>The following <strong>[FormName]</strong> transaction was approved by  <strong>[FullName]</strong>  (  <strong>[Username]</strong>  ):</p><br/>";
            body += @"<table  border='1' cellspacing='0' cellpadding='0' style='border: solid #666 1px;font-size:13px;line-height:19px;'>
                                    <tr>
                                        <td width='160' valign='top' style='border: solid #666 1px;padding:5px;'><span style='font-family:arial,Helvetica,sans-serif;'>ID</span></td>
                                        <td width='151' valign='top' style='font-weigth:bold;border: solid #666 1px;padding:5px;font-weight:bold;'><span style='font-family:arial,Helvetica,sans-serif;'>[TransactionID]</span></td>
                                    </tr>
                                    <tr>
                                        <td width='160' valign='top' style='border: solid #666 1px;padding:5px;'><span style='font-family:arial,Helvetica,sans-serif;'>CSN</span></td>
                                        <td width='151' valign='top' style='font-weigth:bold;border: solid #666 1px;padding:5px;font-weight:bold;'><span style='font-family:arial,Helvetica,sans-serif;'>[FromCSNValue]</span></td>
                                    </tr>
                                    <tr>
                                        <td width='160' valign='top' style='border: solid #666 1px;padding:5px;'><span style='font-family:arial,Helvetica,sans-serif;'>ISIN</span></td>
                                        <td width='151' valign='top' style='font-weigth:bold;border: solid #666 1px;padding:5px;font-weight:bold;'><span style='font-family:arial,Helvetica,sans-serif;'>[ISINValue]</td>
                                    </tr>
                                    <tr>
                                        <td width='160' valign='top' style='border: solid #666 1px;padding:5px;'><span style='font-family:arial,Helvetica,sans-serif;'>DRP Amount</span></td>
                                        <td width='151' valign='top' style='font-weigth:bold;border: solid #666 1px;padding:5px;font-weight:bold;'><span style='font-family:arial,Helvetica,sans-serif;'>[AmountValue]</span></td>
                                    </tr>
                                    <tr>
                                        <td width='160' valign='top' style='border: solid #666 1px;padding:5px;'><span style='font-family:arial,Helvetica,sans-serif;'>Date</span></td>
                                        <td width='151' valign='top' style='font-weigth:bold;border: solid #666 1px;padding:5px;font-weight:bold;'><span style='font-family:arial,Helvetica,sans-serif;'>[Timestamp]</span></td>
                                    </tr>
                                </table>";

            if (FormName == "AIL" || FormName == "Supplementary Dividend")
            {
                body = "<p style='font-family:Verdana, Geneva, sans-serif;font-size:13px;padding-top:0;margin-top:0;line-height:19px;margin-bottom:10px;'><span style='font-family:arial,Helvetica,sans-serif;'> The following <strong>[FormName]</strong> transaction was approved by  <strong>[FullName]</strong>  (<strong>[Username]</strong>):</span></p>";
                body += @"<table border='1' cellspacing='0' cellpadding='0'>
                                    <tbody>
                                    <tr>
                                        <td width='160' valign='top' style='border: solid #666 1px;padding:5px;' ><span style='font-family:arial,Helvetica,sans-serif;'>ID</span></td>
                                        <td width='151' valign='top' style='font-weigth:bold;border: solid #666 1px;padding:5px;font-weight:bold;'><span style='font-family:arial,Helvetica,sans-serif;'>
<strong>[TransactionID]</strong></span></td>
                                    </tr>
                                    <tr>
                                        <td width='160' valign='top' style='border: solid #666 1px;padding:5px;' ><span style='font-family:arial,Helvetica,sans-serif;'>
From CSN</span></td>
                                        <td width='151' valign='top' style='font-weigth:bold;border: solid #666 1px;padding:5px;font-weight:bold;'><span style='font-family:arial,Helvetica,sans-serif;'>
<strong>[FromCSNValue]</strong></span></td>
                                    </tr>
                                    <tr>
                                        <td width='160' valign='top' style='border: solid #666 1px;padding:5px;' ><span style='font-family:arial,Helvetica,sans-serif;'>
To CSN</span></td>
                                        <td width='151' valign='top' style='font-weigth:bold;border: solid #666 1px;padding:5px;font-weight:bold;'><span style='font-family:arial,Helvetica,sans-serif;'>
<strong>[ToCSNValue]</strong></span></td>
                                    </tr>
                                    <tr>
                                        <td width='160' valign='top' style='border: solid #666 1px;padding:5px;' ><span style='font-family:arial,Helvetica,sans-serif;'>
ISIN</span></td>
                                        <td width='151' valign='top' style='font-weigth:bold;border: solid #666 1px;padding:5px;font-weight:bold;'><span style='font-family:arial,Helvetica,sans-serif;'>
<strong>[ISINValue]</strong></span></td>
                                    </tr>
                                    <tr>
                                        <td width='160' valign='top' style='border: solid #666 1px;padding:5px;' ><span style='font-family:arial,Helvetica,sans-serif;'>
Transfer Amount</span></td>
                                        <td width='151' valign='top' style='font-weigth:bold;border: solid #666 1px;padding:5px;font-weight:bold;'><span style='font-family:arial,Helvetica,sans-serif;'>
<strong>[AmountValue]</strong></span></td>
                                    </tr>
                                    <tr>
                                        <td width='160' valign='top' style='border: solid #666 1px;padding:5px;font-weight:bold;' ><span style='font-family:arial,Helvetica,sans-serif;'>
Date</span></td>
                                        <td width='151' valign='top' style='font-weigth:bold;border: solid #666 1px;padding:5px;font-weight:bold;'><span style='font-family:arial,Helvetica,sans-serif;'><strong>[Timestamp]</strong></span></td>
                                    </tr>
                                  </tbody>
                                </table>";
            }

            body = body
                    .Replace("[TransactionID]", TransactionID)
                    .Replace("[FormName]", FormName)
                    .Replace("[FullName]", FullName)
                    .Replace("[Username]", UserName)
                    .Replace("[FromCSNValue]", FromCSNValue)
                    .Replace("[ToCSNValue]", ToCSNValue)
                    .Replace("[ISINValue]", ISINValue)
                    .Replace("[AmountValue]", AmountValue)
                    .Replace("[Timestamp]", Convert.ToDateTime(Timestamp).ToString("yyyy-MM-dd"));

            emailmodel.Subject = subject ;
            emailmodel.Body = body + "<p style='font-family:Verdana, Geneva, sans-serif;font-size:13px;line-height:19px;'>  <em>This notification was sent on  <strong style=\"color:#f30;\">" + System.DateTime.Now.ToString("yyyy-MM-dd") + "</strong> by the CISELECT application.</em></p>";
            emailmodel.Body += "<p style='font-family:Verdana, Geneva, sans-serif;font-size:13px;line-height:19px;'><img src=\"http://tierracreative.clientpreview.agency/Images/cis-logo.png\" width=\"240\" height=\"60\"></p>";
            //body += " This e-mail was sent on by the CISELECT application.";
            emailmodel.IsBodyHtml = true;

            var success = emailhelper.SendEmail(emailmodel);

            return true;
        }

        public bool SendForgotPasswordEmail(
            string guid,
            string fromEmail,
            string toEmail,
            string username)
        {
            SendGridHelper emailhelper = new SendGridHelper();
            SendGridModel emailmodel = new SendGridModel();

            var From = new Recipient
            {
                Email = fromEmail,
                Name = fromEmail
            };
            emailmodel.From = From;

            var To = new Recipient
            {
                Email = toEmail,
                Name = toEmail
            };
            emailmodel.To.Add(To);

            /*Uncomment if need to attach single or multiple file*/
            //Attachment attachment = new Attachment(@"C:\2BInteractive\Docs\FTPAcct.txt");
            //emailmodel.Attachment.Add(attachment);

            var subject = @"CISELECT – Forgotten Password";

            //if (HttpContext.Current.Session["Layout"].ToString() != "admin")
            if (HttpContext.Current.Request.Url.AbsoluteUri.ToLower().ToString().IndexOf("/admin") != -1)

            if (HttpContext.Current.Session["Layout"].ToString() == "admin")
            {
                subject = "CISELECT – Admin Forgotten Password";
            }
            var body = "<p style='font-family:Verdana, Geneva, sans-serif;font-size:13px;padding-top:0;margin-top:0;line-height:19px;'><span style='font-family:arial,Helvetica,sans-serif;'>The following user <strong>" + username + "</strong> initiated a forgotten password request on <strong>" + System.DateTime.Now.ToString("yyyy-MM-dd") + "</strong>.</span></p>";
            body += " <p style='font-family:Verdana, Geneva, sans-serif;font-size:13px;padding-top:0;margin-top:0;line-height:19px;'> <span style='font-family:arial,Helvetica,sans-serif;'> Please use the following link to reset your password:</span></p>";
            body += " <p style='font-family:Verdana, Geneva, sans-serif;font-size:13px;padding-top:0;margin-top:0;line-height:19px;'> <span style='font-family:arial,Helvetica,sans-serif;'><a href='" + string.Format("{0}forgotpasswordchange?guid={1}&email={2}", HttpContext.Current.Request.Url.AbsoluteUri.Replace("forgotpassword", ""), guid.ToString(), toEmail) + "'>Click here</a></span></p> ";
            body += " <p style='font-family:Verdana, Geneva, sans-serif;font-size:13px;padding-top:0;margin-top:0;line-height:19px;'> <span style='font-family:arial,Helvetica,sans-serif;'> If you didn&rsquo;t initiate this request please use the link  below to invalidate it and contact <strong>" + fromEmail+ "</strong>.</span></p> ";
            body += " <p style='font-family:Verdana, Geneva, sans-serif;font-size:13px;padding-top:0;margin-top:0;line-height:19px;'> <span style='font-family:arial,Helvetica,sans-serif;'>  <strong>InvalidateLink</strong> </span></p>";
            body += " <p style='font-family:Verdana, Geneva, sans-serif;font-size:13px;padding-top:0;margin-top:0;line-height:19px;'><span style='font-family:arial,Helvetica,sans-serif;'><em>This e-mail was sent on by the CISELECT application.  </em></span></p>";
            body += "<p style='font-family:Verdana, Geneva, sans-serif;font-size:13px;padding-top:0;margin-top:0;line-height:19px;'><img src=\"http://tierracreative.clientpreview.agency/Images/cis-logo.png\" width=\"240\" height=\"60\"></p>";

            emailmodel.Subject = subject;
            emailmodel.Body = body;
            emailmodel.IsBodyHtml = true;

            var success = emailhelper.SendEmail(emailmodel);

            return true;
        }

        public bool SendChangePasswordEmail(
          string fromEmail,
          string toEmail,
          string username)
        {
            SendGridHelper emailhelper = new SendGridHelper();
            SendGridModel emailmodel = new SendGridModel();

            var From = new Recipient
            {
                Email = fromEmail,
                Name = fromEmail
            };
            emailmodel.From = From;

            var To = new Recipient
            {
                Email = toEmail,
                Name = toEmail
            };
            emailmodel.To.Add(To);

            /*Uncomment if need to attach single or multiple file*/
            //Attachment attachment = new Attachment(@"C:\2BInteractive\Docs\FTPAcct.txt");
            //emailmodel.Attachment.Add(attachment);

            var subject = @"CISELECT – Password Changed";

            //if (HttpContext.Current.Session["Layout"].ToString() != "admin")
            if (HttpContext.Current.Request.Url.AbsoluteUri.ToLower().ToString().IndexOf("/admin") != -1)           
            {
                subject = "CISELECT – Admin Password Changed";
            }

            var body = "<p style='font-family:Verdana, Geneva, sans-serif;font-size:13px;padding-top:0;margin-top:0;line-height:19px;'><span style='font-family:arial,Helvetica,sans-serif;'>The following user  <strong>" + username + " </strong> has changed the password associated with their account on <strong>" + System.DateTime.Now.ToString("yyyy-MM-dd") + "</strong> .</span></p>";
            body += "<p style='font-family:Verdana, Geneva, sans-serif;font-size:13px;padding-top:0;margin-top:0;line-height:19px;'> <span style='font-family:arial,Helvetica,sans-serif;'> If you didn&rsquo;t initiate this password change please contact <strong>" + fromEmail+ "</strong> .</span></p>";
            body += "<p style='font-family:Verdana, Geneva, sans-serif;font-size:13px;padding-top:0;margin-top:0;line-height:19px;'><span style='font-family:arial,Helvetica,sans-serif;'><em>This e-mail was sent on by the CISELECT application.  </em></span></p>";
            body += "<p style='font-family:Verdana, Geneva, sans-serif;font-size:13px;padding-top:0;margin-top:0;line-height:19px;'><img src=\"http://tierracreative.clientpreview.agency/Images/cis-logo.png\" width=\"240\" height=\"60\"></p>";

            emailmodel.Subject = subject;
            emailmodel.Body = body;
            emailmodel.IsBodyHtml = true;

            var success = emailhelper.SendEmail(emailmodel);

            return true;
        }

        public bool SendSupportEmail(
         string fromEmail,
         string toEmail,
         string fullame,
         string username,
         string message)
        {
            SendGridHelper emailhelper = new SendGridHelper();
            SendGridModel emailmodel = new SendGridModel();

            var From = new Recipient
            {
                Email = fromEmail,
                Name = fromEmail
            };
            emailmodel.From = From;

            var To = new Recipient
            {
                Email = toEmail,
                Name = toEmail
            };
            emailmodel.To.Add(To);

            /*Uncomment if need to attach single or multiple file*/
            //Attachment attachment = new Attachment(@"C:\2BInteractive\Docs\FTPAcct.txt");
            //emailmodel.Attachment.Add(attachment);

            var subject = @"CISELECT – Support Request";

            var body = "<p style='font-family:Verdana, Geneva, sans-serif;font-size:13px;padding-top:0;margin-top:0;line-height:19px;'><span style='font-family:arial,Helvetica,sans-serif;'>Support request was submitted on <strong>" + System.DateTime.Now.ToString("yyyy-MM-dd") + "</strong> .</span></p>";
            body += "<p style='font-family:Verdana, Geneva, sans-serif;font-size:13px;padding-top:0;margin-top:0;line-height:19px;'>  <span style='font-family:arial,Helvetica,sans-serif;'>User:</span></p>";
            body += string.Format("<span style='font-family:arial,Helvetica,sans-serif;'><strong>{0}</strong> (<strong>{1}</strong>)</span>", fullame, username);
            body += "<p style='font-family:Verdana, Geneva, sans-serif;font-size:13px;padding-top:0;margin-top:0;line-height:19px;'>  <span style='font-family:arial,Helvetica,sans-serif;'><strong>" + toEmail + "</strong></span></p>";
            body += "<p style='font-family:Verdana, Geneva, sans-serif;font-size:13px;padding-top:0;margin-top:0;line-height:19px;'>  <span style='font-family:arial,Helvetica,sans-serif;'>Support Message:</span></p>";
            body += "<p style='font-family:Verdana, Geneva, sans-serif;font-size:13px;padding-top:0;margin-top:0;line-height:19px;'>  <span style='font-family:arial,Helvetica,sans-serif;'> <strong>" + message + "</strong> </span></p>";
            DateTime dateTime = DateTime.UtcNow.Date;
            body += "<p style='font-family:Verdana, Geneva, sans-serif;font-size:13px;padding-top:0;margin-top:0;line-height:19px;'><span style='font-family:arial,Helvetica,sans-serif;'>< em >This e-mail was sent on by the CISELECT application.  </em></span></p>";
            body += "<p style='font-family:Verdana, Geneva, sans-serif;font-size:13px;padding-top:0;margin-top:0;line-height:19px;'><img src=\"http://tierracreative.clientpreview.agency/Images/cis-logo.png\" width=\"240\" height=\"60\"></p>";

            emailmodel.Subject = subject;
            emailmodel.Body = body;
            emailmodel.IsBodyHtml = true;

            var success = emailhelper.SendEmail(emailmodel);

            return true;
        }

        public bool SendSupportUserEmail(
        string fromEmail,
        string toEmail,
        string fullame,
        string username,
        string message)
        {
            SendGridHelper emailhelper = new SendGridHelper();
            SendGridModel emailmodel = new SendGridModel();

            var From = new Recipient
            {
                Email = fromEmail,
                Name = fromEmail
            };
            emailmodel.From = From;

            var To = new Recipient
            {
                Email = toEmail,
                Name = toEmail
            };
            emailmodel.To.Add(To);

            /*Uncomment if need to attach single or multiple file*/
            //Attachment attachment = new Attachment(@"C:\2BInteractive\Docs\FTPAcct.txt");
            //emailmodel.Attachment.Add(attachment);

            var subject = @"CISELECT – Support Request";

            var body = "<p style='font-family:Verdana, Geneva, sans-serif;font-size:13px;padding-top:0;margin-top:0;line-height:19px;'><span style='font-family:arial,Helvetica,sans-serif;'>Support request was submitted on <strong>" + System.DateTime.Now.ToString("yyyy-MM-dd") + "</strong>.</span></p>";
            body += "<p style='font-family:Verdana, Geneva, sans-serif;font-size:13px;padding-top:0;margin-top:0;line-height:19px;'><span style='font-family:arial,Helvetica,sans-serif;'>  User:</span></p>";
            body += string.Format("<span style='font-family:arial,Helvetica,sans-serif;'><strong>{0}</strong> (<strong>{1}</strong>)</span>", fullame, username);
            body += "<p style='font-family:Verdana, Geneva, sans-serif;font-size:13px;padding-top:0;margin-top:0;line-height:19px;'><span style='font-family:arial,Helvetica,sans-serif;'>  <strong>" + toEmail + "</strong></span></p>";
            body += "<p style='font-family:Verdana, Geneva, sans-serif;font-size:13px;padding-top:0;margin-top:0;line-height:19px;'> <span style='font-family:arial,Helvetica,sans-serif;'> Support Message:</span></p>";
            body += "<p style='font-family:Verdana, Geneva, sans-serif;font-size:13px;padding-top:0;margin-top:0;line-height:19px;'> <span style='font-family:arial,Helvetica,sans-serif;'>  <strong>" + message + "</strong></span> </p>";
            body += "<p style='font-family:Verdana, Geneva, sans-serif;font-size:13px;padding-top:0;margin-top:0;line-height:19px;'><span style='font-family:arial,Helvetica,sans-serif;'><em>This e-mail was sent on by the CISELECT application.  </em></span></p>";
            body += "<p style='font-family:Verdana, Geneva, sans-serif;font-size:13px;padding-top:0;margin-top:0;line-height:19px;'><img src=\"http://tierracreative.clientpreview.agency/Images/cis-logo.png\" width=\"240\" height=\"60\"></p>";

            emailmodel.Subject = subject;
            emailmodel.Body = body;
            emailmodel.IsBodyHtml = true;

            var success = emailhelper.SendEmail(emailmodel);

            return true;
        }
    }
}