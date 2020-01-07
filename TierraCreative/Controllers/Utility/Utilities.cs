using System;
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
            string toEmail)
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

            var subject = "[TransactionID] - [FormName] Transaction Approved - [Timestamp]";
            subject = subject
                      .Replace("[TransactionID]", TransactionID)
                      .Replace("[FormName]", FormName)
                      .Replace("[Timestamp]", Timestamp);                     

            string body = @"The following [FormName] transaction was approved by [FullName] ([Username]): <br />
                                <table border='1'>
                                    <tr>
                                        <td>ID</td>
                                        <td>[TransactionID]</td>
                                    </tr>
                                    <tr>
                                        <td>CSN</td>
                                        <td>[FromCSNValue]</td>
                                    </tr>
                                    <tr>
                                        <td>ISIN</td>
                                        <td>[ISINValue]</td>
                                    </tr>
                                    <tr>
                                        <td>DRP Amount</td>
                                        <td>[AmountValue]</td>
                                    </tr>
                                    <tr>
                                        <td>Date</td>
                                        <td>[Timestamp]</td>
                                    </tr>
                                </table>";

            if (FormName == "AIL" || FormName == "Supplementary Dividend") { 
            body = @"The following [FormName] transaction was approved by [FullName] ([Username]): <br />
                                <table border='1'>
                                    <tr>
                                        <td>ID</td>
                                        <td>[TransactionID]</td>
                                    </tr>
                                    <tr>
                                        <td>From CSN</td>
                                        <td>[FromCSNValue]</td>
                                    </tr>
                                    <tr>
                                        <td>To CSN</td>
                                        <td>[ToCSNValue]</td>
                                    </tr>
                                    <tr>
                                        <td>ISIN</td>
                                        <td>[ISINValue]</td>
                                    </tr>
                                    <tr>
                                        <td>DRP Amount</td>
                                        <td>[AmountValue]</td>
                                    </tr>
                                    <tr>
                                        <td>Date</td>
                                        <td>[Timestamp]</td>
                                    </tr>
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
                    .Replace("[Timestamp]", Timestamp);

            emailmodel.Subject = subject;
            emailmodel.Body = body;
            emailmodel.IsBodyHtml = true;

            var success = emailhelper.SendEmail(emailmodel);
           
            return true;
        }
    }
}