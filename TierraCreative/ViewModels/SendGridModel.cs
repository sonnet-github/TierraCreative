using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace TierraCreative.ViewModels
{
    public class SendGridModel
    {
        public SendGridModel()
        {
            To = new List<Recipient>();           
            Cc = new List<Recipient>();
            Bcc = new List<Recipient>();
            Attachment = new List<Attachment>();
        }

        public Recipient From { get; set; }

        public List<Recipient> To { get; set; }

        public List<Recipient> Cc { get; set; }

        public List<Recipient> Bcc { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public bool IsBodyHtml { get; set; }

        public List<Attachment> Attachment { get; set; }
    }

    public class Recipient
    {
        public string Name { get; set; }

        public string Email { get; set; }
    }
}