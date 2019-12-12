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

namespace TierraCreative.Controllers
{
    [SessionExpire]
    public class SupplementaryDividendsController : BaseController
    {
        public ActionResult Add()
        {
            ViewBag.FromCSN = new SelectList(_context.CSNLookUps.Where(x => x.CSNId == 1 || x.CSNId == 3), "CSNName", "CSNName", (Session["FromCSN"] != null ? Session["FromCSN"] : "")).OrderByDescending(x=>x.Text);
            ViewBag.ToCSN = new SelectList(_context.CSNLookUps.Where(x => x.CSNId == 1 || x.CSNId == 3), "CSNName", "CSNName", (Session["ToCSN"] != null ? Session["ToCSN"] : "")).OrderBy(x => x.Text);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(SupplementaryDividend sD)
        {
            Session["FromCSN"] = sD.FromCSN;
            Session["ToCSN"] = sD.ToCSN;
            Session["ToCSN"] = sD.ToCSN;
            Session["ISIN"] = sD.ISIN;
            Session["TransferAmount"] = sD.TransferAmount;

            return Redirect("details");
        }

        public ActionResult Details()
        {
            if (Session["FromCSN"] == null)
                return Redirect("../forms");

            ViewBag.IsView = "SDReview";

            return View();
        }

        [HttpPost]
        public ActionResult Details(SupplementaryDividend sD)
        {
            if (Session["FromCSN"] == null)
                return Redirect("../forms");

            sD.UserId = int.Parse(Session["UserId"].ToString());
            sD.FromCSN = Session["FromCSN"].ToString();
            sD.ToCSN = Session["ToCSN"].ToString();
            sD.ISIN = Session["ISIN"].ToString();
            sD.TransferAmount = double.Parse(Session["TransferAmount"].ToString());

            sD.CreatedById = int.Parse(Session["UserId"].ToString());
            sD.CreatedDate = System.DateTime.Now;

            //save to db
            _context.SupplementaryDividends.Add(sD);
            _context.SaveChanges();

            //initialize sessions
            Session["FromCSN"] = null;
            Session["ToCSN"] = null;
            Session["ISIN"] = null;
            Session["TransferAmount"] = null;

            //email client


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
