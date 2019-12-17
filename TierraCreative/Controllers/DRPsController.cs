using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TierraCreative;
using TierraCreative.Model;
using TierraCreative.ViewModels;

namespace TierraCreative.Controllers
{
    [SessionExpire]
    public class DRPsController : BaseController
    {
        public ActionResult Add()
        {
            ViewBag.CSN = new SelectList(_context.CSNLookUps.Where(x => x.CSNId == 1 || x.CSNId == 3), "CSNName", "CSNName", (Session["CSN"] != null ? Session["CSN"] : ""));

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(DRP dRP)
        {
            Session["CSN"] = dRP.CSN;
            Session["ISIN"] = dRP.ISIN;
            Session["DRPAmount"] = dRP.DRPAmount;

            return Redirect("details");
        }

        public ActionResult Details()
        {
            if (Session["CSN"]==null)
                return Redirect("../forms");

            ViewBag.IsView = "DRPReview";

            return View();
        }

        [HttpPost]
        public ActionResult Details(DRP dRP)
        {
            if (Session["CSN"] == null)
                return Redirect("../forms");

            dRP.UserId = int.Parse(Session["UserId"].ToString());
            dRP.CSN = Session["CSN"].ToString();
            dRP.ISIN = Session["ISIN"].ToString();
            dRP.DRPAmount = double.Parse(Session["DRPAmount"].ToString());

            dRP.CreatedById = int.Parse(Session["UserId"].ToString());
            dRP.CreatedDate = System.DateTime.Now;

            //save to db
            _context.DRPs.Add(dRP);
            _context.SaveChanges();

            //initialize sessions
            Session["CSN"] = null;
            Session["ISIN"] = null;
            Session["DRPAmount"] = null;

            #region -- Email --
            SendGridHelper emailhelper = new SendGridHelper();
            SendGridModel emailmodel = new SendGridModel();

            var From = new Recipient
            {
                Email = "raymondm@sonnet.digital",
                Name = "Raymond Milca"
            };
            emailmodel.From = From;

            var To = new Recipient
            {
                Email = "raymondm@sonnet.digital",
                Name = "Raymond Milca"
            };
            emailmodel.To.Add(To);

            /*Uncomment if need to attach single or multiple file*/
            //Attachment attachment = new Attachment(@"C:\2BInteractive\Docs\FTPAcct.txt");
            //emailmodel.Attachment.Add(attachment);

            emailmodel.Subject = "Email Subject";
            emailmodel.Body = "<strong>Email Body</strong>";
            emailmodel.IsBodyHtml = true;

            var success = emailhelper.SendEmail(emailmodel);
            #endregion

            ViewBag.IsView = "Acknowledgement";

            return View();           
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
