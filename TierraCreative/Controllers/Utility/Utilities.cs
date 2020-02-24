using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
            string submitUserEmail,
            string Source,
            string Destination,
            string SourceValue,
            string DestinationValue)
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

            //var Bcc = new Recipient
            //{
            //    Email = "inaha@sonnet.digital",
            //    Name = "inaha@sonnet.digital"
            //};
            //emailmodel.Bcc.Add(Bcc);

            /*Uncomment if need to attach single or multiple file*/
            //Attachment attachment = new Attachment(@"C:\2BInteractive\Docs\FTPAcct.txt");
            //emailmodel.Attachment.Add(attachment);

            var subject = "[TransactionID] - [FormName] Transaction Approved - [Timestamp]";
            subject = subject
                      .Replace("[TransactionID]", TransactionID)
                      .Replace("[FormName]", FormName)
                      .Replace("[Timestamp]", System.DateTime.Now.ToString("yyyy-MM-dd"));
            //string body = "<style type=\"text/css\">body{font-family:arial,Helvetica,sans-serif;font-size:13pxpadding-top:0;margin-top:0;line-height:19px;}table {border: solid #666 1px;font-size:13px;line-height:19px;}td {border: solid #666 1px;padding:5px;}tr td:nth-child(2){font-weigth:bold;}</ style >";
            string body = @"<p><img src='http://tierracreative.clientpreview.agency/Images/cis-email-header.png' alt='Computershare RBNZ Election Portal' width='500' height='156'></p>";
            body += "<p style='font-family:arial,Helvetica,sans-serif;font-size:13pxpadding-top:0;margin-top:0;padding-bottom:0;margin-bottom:0;line-height:19px;'><span style='font-family:arial,Helvetica,sans-serif;font-size:13px'>The following <strong>[FormName]</strong> transaction was approved by  <strong>[FullName]</strong> (<strong>[Username]</strong>):</span></p><br/>";
            body += @"<table  border='1' cellspacing='0' cellpadding='0' style='border: solid #666 1px;font-size:13px;line-height:19px;'>
                                    <tr>
                                        <td width='160' valign='top' style='border: solid #666 1px;padding:5px;'><span style='font-family:arial,Helvetica,sans-serif;font-size:13px'>ID</span></td>
                                        <td width='151' valign='top' style='font-weigth:bold;border: solid #666 1px;padding:5px;font-weight:bold;'><span style='font-family:arial,Helvetica,sans-serif;font-size:13px'>[TransactionID]</span></td>
                                    </tr>
                                    <tr>
                                        <td width='160' valign='top' style='border: solid #666 1px;padding:5px;'><span style='font-family:arial,Helvetica,sans-serif;font-size:13px'>CSN</span></td>
                                        <td width='151' valign='top' style='font-weigth:bold;border: solid #666 1px;padding:5px;font-weight:bold;'><span style='font-family:arial,Helvetica,sans-serif;font-size:13px'>[Source] ([FromCSNValue])</span></td>
                                    </tr>
                                    <tr>
                                        <td width='160' valign='top' style='border: solid #666 1px;padding:5px;'><span style='font-family:arial,Helvetica,sans-serif;font-size:13px'>ISIN</span></td>
                                        <td width='151' valign='top' style='font-weigth:bold;border: solid #666 1px;padding:5px;font-weight:bold;'><span style='font-family:arial,Helvetica,sans-serif;font-size:13px'>[ISINValue]</td>
                                    </tr>
                                    <tr>
                                        <td width='160' valign='top' style='border: solid #666 1px;padding:5px;'><span style='font-family:arial,Helvetica,sans-serif;font-size:13px'>DRP Amount</span></td>
                                        <td width='151' valign='top' style='font-weigth:bold;border: solid #666 1px;padding:5px;font-weight:bold;'><span style='font-family:arial,Helvetica,sans-serif;font-size:13px'>[AmountValue]</span></td>
                                    </tr>
                                    <tr>
                                        <td width='160' valign='top' style='border: solid #666 1px;padding:5px;'><span style='font-family:arial,Helvetica,sans-serif;font-size:13px'>Date</span></td>
                                        <td width='151' valign='top' style='font-weigth:bold;border: solid #666 1px;padding:5px;font-weight:bold;'><span style='font-family:arial,Helvetica,sans-serif;font-size:13px'>[Timestamp]</span></td>
                                    </tr>
                                </table>";

            if (FormName == "AIL" || FormName == "Supplementary Dividend")
            {
                body = @"<p><img src='http://tierracreative.clientpreview.agency/Images/cis-email-header.png' alt='Computershare RBNZ Election Portal' width='500' height='156'></p>";

                body += "<p style='font-family:arial,Helvetica,sans-serif;font-size:13pxpadding-top:0;margin-top:0;line-height:19px;margin-bottom:10px;'><span style='font-family:arial,Helvetica,sans-serif;font-size:13px'> The following <strong>[FormName]</strong> transaction was approved by  <strong>[FullName]</strong> (<strong>[Username]</strong>):</span></p>";
                body += @"<table border='1' cellspacing='0' cellpadding='0'>
                                    <tbody>
                                    <tr>
                                        <td width='160' valign='top' style='border: solid #666 1px;padding:5px;' ><span style='font-family:arial,Helvetica,sans-serif;font-size:13px'>ID</span></td>
                                        <td width='151' valign='top' style='font-weigth:bold;border: solid #666 1px;padding:5px;font-weight:bold;'><span style='font-family:arial,Helvetica,sans-serif;font-size:13px'>
<strong>[TransactionID]</strong></span></td>
                                    </tr>
                                    <tr>
                                        <td width='160' valign='top' style='border: solid #666 1px;padding:5px;' ><span style='font-family:arial,Helvetica,sans-serif;font-size:13px'>
From CSN</span></td>
                                        <td width='151' valign='top' style='font-weigth:bold;border: solid #666 1px;padding:5px;font-weight:bold;'><span style='font-family:arial,Helvetica,sans-serif;font-size:13px'>
<strong>[Source]</strong> (<strong>[FromCSNValue]</strong>)</span></td>
                                    </tr>
                                    <tr>
                                        <td width='160' valign='top' style='border: solid #666 1px;padding:5px;' ><span style='font-family:arial,Helvetica,sans-serif;font-size:13px'>
To CSN</span></td>
                                        <td width='151' valign='top' style='font-weigth:bold;border: solid #666 1px;padding:5px;font-weight:bold;'><span style='font-family:arial,Helvetica,sans-serif;font-size:13px'>
<strong>[Destination]</strong> (<strong>[ToCSNValue]</strong>)</span></td>
                                    </tr>
                                    <tr>
                                        <td width='160' valign='top' style='border: solid #666 1px;padding:5px;' ><span style='font-family:arial,Helvetica,sans-serif;font-size:13px'>
ISIN</span></td>
                                        <td width='151' valign='top' style='font-weigth:bold;border: solid #666 1px;padding:5px;font-weight:bold;'><span style='font-family:arial,Helvetica,sans-serif;font-size:13px'>
<strong>[ISINValue]</strong></span></td>
                                    </tr>
                                    <tr>
                                        <td width='160' valign='top' style='border: solid #666 1px;padding:5px;' ><span style='font-family:arial,Helvetica,sans-serif;font-size:13px'>
Transfer Amount</span></td>
                                        <td width='151' valign='top' style='font-weigth:bold;border: solid #666 1px;padding:5px;font-weight:bold;'><span style='font-family:arial,Helvetica,sans-serif;font-size:13px'>
<strong>[AmountValue]</strong></span></td>
                                    </tr>
                                    <tr>
                                        <td width='160' valign='top' style='border: solid #666 1px;padding:5px;' ><span style='font-family:arial,Helvetica,sans-serif;font-size:13px'>Date</span></td>
                                        <td width='151' valign='top' style='font-weigth:bold;border: solid #666 1px;padding:5px;font-weight:bold;'><span style='font-family:arial,Helvetica,sans-serif;font-size:13px'><strong>[Timestamp]</strong></span></td>
                                    </tr>
                                  </tbody>
                                </table>";
            }

            body = body
                    .Replace("[TransactionID]", TransactionID)
                    .Replace("[FormName]", FormName)
                    .Replace("[FullName]", FullName)
                    .Replace("[Username]", UserName)
                    .Replace("[FromCSNValue]", SourceValue)
                    .Replace("[ToCSNValue]", DestinationValue)
                    .Replace("[ISINValue]", ISINValue)
                    .Replace("[AmountValue]", AmountValue)
                    .Replace("[Source]", Source)
                    .Replace("[Destination]", Destination)
                    .Replace("[Timestamp]", Convert.ToDateTime(Timestamp).ToString("yyyy-MM-dd"));

            emailmodel.Subject = subject ;
            emailmodel.Body = body + "<p style='font-family:arial,Helvetica,sans-serif;font-size:13pxline-height:19px;'>  <em>This notification was sent on  <strong style=\"color:#f30;\">" + System.DateTime.Now.ToString("yyyy-MM-dd") + "</strong> by the CISELECT application.</em></p>";
            emailmodel.Body += "<p style='font-family:arial,Helvetica,sans-serif;font-size:13pxline-height:19px;'><img src=\"http://tierracreative.clientpreview.agency/Images/cis-logo.png\" width=\"240\" height=\"60\"></p>";
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

            var Bcc = new Recipient
            {
                Email = "inaha@sonnet.digital",
                Name = "inaha@sonnet.digital"
            };
            emailmodel.Bcc.Add(Bcc);

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
            var body = "<p style='font-family:arial,Helvetica,sans-serif;font-size:13pxpadding-top:0;margin-top:0;line-height:19px;'><span style='font-family:arial,Helvetica,sans-serif;font-size:13px'>The following user <strong>" + username + "</strong> initiated a forgotten password request on <strong>" + System.DateTime.Now.ToString("yyyy-MM-dd") + "</strong>.</span></p>";
            body += " <p style='font-family:arial,Helvetica,sans-serif;font-size:13pxpadding-top:0;margin-top:0;line-height:19px;'> <span style='font-family:arial,Helvetica,sans-serif;font-size:13px'> Please use the following link to reset your password:</span></p>";
            body += " <p style='font-family:arial,Helvetica,sans-serif;font-size:13pxpadding-top:0;margin-top:0;line-height:19px;'> <span style='font-family:arial,Helvetica,sans-serif;font-size:13px'><a href='" + string.Format("{0}forgotpasswordchange?guid={1}&email={2}", HttpContext.Current.Request.Url.AbsoluteUri.Replace("forgotpassword", ""), guid.ToString(), toEmail) + "'>Click here</a></span></p> ";
            body += " <p style='font-family:arial,Helvetica,sans-serif;font-size:13pxpadding-top:0;margin-top:0;line-height:19px;'> <span style='font-family:arial,Helvetica,sans-serif;font-size:13px'> If you didn&rsquo;t initiate this request please use the link  below to invalidate it and contact <strong>" + fromEmail+ "</strong>.</span></p> ";
            body += " <p style='font-family:arial,Helvetica,sans-serif;font-size:13pxpadding-top:0;margin-top:0;line-height:19px;'> <span style='font-family:arial,Helvetica,sans-serif;font-size:13px'>  <strong>InvalidateLink</strong> </span></p>";
            body += " <p style='font-family:arial,Helvetica,sans-serif;font-size:13pxpadding-top:0;margin-top:0;line-height:19px;'><span style='font-family:arial,Helvetica,sans-serif;font-size:13px'><em>This e-mail was sent on by the CISELECT application.  </em></span></p>";
            body += "<p style='font-family:arial,Helvetica,sans-serif;font-size:13pxpadding-top:0;margin-top:0;line-height:19px;'><img src=\"http://tierracreative.clientpreview.agency/Images/cis-logo.png\" width=\"240\" height=\"60\"></p>";

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

            var Bcc = new Recipient
            {
                Email = "inaha@sonnet.digital",
                Name = "inaha@sonnet.digital"
            };
            emailmodel.Bcc.Add(Bcc);

            /*Uncomment if need to attach single or multiple file*/
            //Attachment attachment = new Attachment(@"C:\2BInteractive\Docs\FTPAcct.txt");
            //emailmodel.Attachment.Add(attachment);

            var subject = @"CISELECT – Password Changed";

            //if (HttpContext.Current.Session["Layout"].ToString() != "admin")
            if (HttpContext.Current.Request.Url.AbsoluteUri.ToLower().ToString().IndexOf("/admin") != -1)           
            {
                subject = "CISELECT – Admin Password Changed";
            }

            var body = "<p style='font-family:arial,Helvetica,sans-serif;font-size:13pxpadding-top:0;margin-top:0;line-height:19px;'><span style='font-family:arial,Helvetica,sans-serif;font-size:13px'>The following user  <strong>" + username + " </strong> has changed the password associated with their account on <strong>" + System.DateTime.Now.ToString("yyyy-MM-dd") + "</strong> .</span></p>";
            body += "<p style='font-family:arial,Helvetica,sans-serif;font-size:13pxpadding-top:0;margin-top:0;line-height:19px;'> <span style='font-family:arial,Helvetica,sans-serif;font-size:13px'> If you didn&rsquo;t initiate this password change please contact <strong>" + fromEmail+ "</strong> .</span></p>";
            body += "<p style='font-family:arial,Helvetica,sans-serif;font-size:13pxpadding-top:0;margin-top:0;line-height:19px;'><span style='font-family:arial,Helvetica,sans-serif;font-size:13px'><em>This e-mail was sent on by the CISELECT application.  </em></span></p>";
            body += "<p style='font-family:arial,Helvetica,sans-serif;font-size:13pxpadding-top:0;margin-top:0;line-height:19px;'><img src=\"http://tierracreative.clientpreview.agency/Images/cis-logo.png\" width=\"240\" height=\"60\"></p>";

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

            var Bcc = new Recipient
            {
                Email = "inaha@sonnet.digital",
                Name = "inaha@sonnet.digital"
            };
            emailmodel.Bcc.Add(Bcc);

            /*Uncomment if need to attach single or multiple file*/
            //Attachment attachment = new Attachment(@"C:\2BInteractive\Docs\FTPAcct.txt");
            //emailmodel.Attachment.Add(attachment);

            var subject = @"CISELECT – Support Request";

            var body = "<p style='font-family:arial,Helvetica,sans-serif;font-size:13pxpadding-top:0;margin-top:0;line-height:19px;'><span style='font-family:arial,Helvetica,sans-serif;font-size:13px'>Support request was submitted on <strong>" + System.DateTime.Now.ToString("yyyy-MM-dd") + "</strong>.</span></p>";
            body += "<p style='font-family:arial,Helvetica,sans-serif;font-size:13pxpadding-top:0;margin-top:0;line-height:19px;'>  <span style='font-family:arial,Helvetica,sans-serif;font-size:13px'>User:</span></p>";
            body += string.Format("<span style='font-family:arial,Helvetica,sans-serif;font-size:13px'><strong>{0}</strong> (<strong>{1}</strong>)</span>", fullame, username);
            body += "<p style='font-family:arial,Helvetica,sans-serif;font-size:13pxpadding-top:0;margin-top:0;line-height:19px;'>  <span style='font-family:arial,Helvetica,sans-serif;font-size:13px'><strong>" + toEmail + "</strong></span></p>";
            body += "<p style='font-family:arial,Helvetica,sans-serif;font-size:13pxpadding-top:0;margin-top:0;line-height:19px;'>  <span style='font-family:arial,Helvetica,sans-serif;font-size:13px'>Support Message:</span></p>";
            body += "<p style='font-family:arial,Helvetica,sans-serif;font-size:13pxpadding-top:0;margin-top:0;line-height:19px;'>  <span style='font-family:arial,Helvetica,sans-serif;font-size:13px'> <strong>" + message + "</strong> </span></p>";
            DateTime dateTime = DateTime.UtcNow.Date;
            body += "<p style='font-family:arial,Helvetica,sans-serif;font-size:13pxpadding-top:0;margin-top:0;line-height:19px;'><span style='font-family:arial,Helvetica,sans-serif;font-size:13px'><em>This e-mail was sent on by the CISELECT application.  </em></span></p>";
            body += "<p style='font-family:arial,Helvetica,sans-serif;font-size:13pxpadding-top:0;margin-top:0;line-height:19px;'><img src=\"http://tierracreative.clientpreview.agency/Images/cis-logo.png\" width=\"240\" height=\"60\"></p>";

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

            var Bcc = new Recipient
            {
                Email = "inaha@sonnet.digital",
                Name = "inaha@sonnet.digital"
            };
            emailmodel.Bcc.Add(Bcc);

            /*Uncomment if need to attach single or multiple file*/
            //Attachment attachment = new Attachment(@"C:\2BInteractive\Docs\FTPAcct.txt");
            //emailmodel.Attachment.Add(attachment);

            var subject = @"CISELECT – Support Request";

            var body = "<p style='font-family:arial,Helvetica,sans-serif;font-size:13pxpadding-top:0;margin-top:0;line-height:19px;'><span style='font-family:arial,Helvetica,sans-serif;font-size:13px'>Support request was submitted on <strong>" + System.DateTime.Now.ToString("yyyy-MM-dd") + "</strong>.</span></p>";
            body += "<p style='font-family:arial,Helvetica,sans-serif;font-size:13pxpadding-top:0;margin-top:0;line-height:19px;'><span style='font-family:arial,Helvetica,sans-serif;font-size:13px'>  User:</span></p>";
            body += string.Format("<span style='font-family:arial,Helvetica,sans-serif;font-size:13px'><strong>{0}</strong> (<strong>{1}</strong>)</span>", fullame, username);
            body += "<p style='font-family:arial,Helvetica,sans-serif;font-size:13pxpadding-top:0;margin-top:0;line-height:19px;'><span style='font-family:arial,Helvetica,sans-serif;font-size:13px'>  <strong>" + toEmail + "</strong></span></p>";
            body += "<p style='font-family:arial,Helvetica,sans-serif;font-size:13pxpadding-top:0;margin-top:0;line-height:19px;'> <span style='font-family:arial,Helvetica,sans-serif;font-size:13px'> Support Message:</span></p>";
            body += "<p style='font-family:arial,Helvetica,sans-serif;font-size:13pxpadding-top:0;margin-top:0;line-height:19px;'> <span style='font-family:arial,Helvetica,sans-serif;font-size:13px'>  <strong>" + message + "</strong></span> </p>";
            body += "<p style='font-family:arial,Helvetica,sans-serif;font-size:13pxpadding-top:0;margin-top:0;line-height:19px;'><span style='font-family:arial,Helvetica,sans-serif;font-size:13px'><em>This e-mail was sent on by the CISELECT application.  </em></span></p>";
            body += "<p style='font-family:arial,Helvetica,sans-serif;font-size:13pxpadding-top:0;margin-top:0;line-height:19px;'><img src=\"http://tierracreative.clientpreview.agency/Images/cis-logo.png\" width=\"240\" height=\"60\"></p>";

            emailmodel.Subject = subject;
            emailmodel.Body = body;
            emailmodel.IsBodyHtml = true;

            var success = emailhelper.SendEmail(emailmodel);

            return true;
        }

        #region -- Encryption --
        public string GenerateEncryptionKey()
        {
            string EncryptionKey = string.Empty;

            Random Robj = new Random();
            int Rnumber = Robj.Next();
            EncryptionKey = "TIE" + Convert.ToString(Rnumber);

            return EncryptionKey;
        }

        public string Encrypt(string clearText, string EncryptionKey)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public string Decrypt(string cipherText, string EncryptionKey)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        #endregion
    }
}