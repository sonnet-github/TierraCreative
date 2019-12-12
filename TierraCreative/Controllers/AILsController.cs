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
    public class AILsController : BaseController
    {
        public ActionResult Add()
        {
            ViewBag.FromCSN = new SelectList(_context.CSNLookUps.Where(x => x.CSNId == 1 || x.CSNId == 2), "CSNName", "CSNName", (Session["FromCSN"] != null ? Session["FromCSN"] : "")).OrderByDescending(x=>x.Text);
            ViewBag.ToCSN = new SelectList(_context.CSNLookUps.Where(x => x.CSNId == 1 || x.CSNId == 2), "CSNName", "CSNName", (Session["ToCSN"] != null ? Session["ToCSN"] : "")).OrderBy(x => x.Text);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AIL aIL)
        {
            Session["FromCSN"] = aIL.FromCSN;
            Session["ToCSN"] = aIL.ToCSN;
            Session["ToCSN"] = aIL.ToCSN;
            Session["ISIN"] = aIL.ISIN;
            Session["TransferAmount"] = aIL.TransferAmount;

            return Redirect("details");
        }

        public ActionResult Details()
        {
            if (Session["FromCSN"] == null)
                return Redirect("../forms");

            ViewBag.IsView = "AILReview";

            return View();
        }

        [HttpPost]
        public ActionResult Details(AIL aIL)
        {
            if (Session["FromCSN"] == null)
                return Redirect("../forms");

            aIL.UserId = int.Parse(Session["UserId"].ToString());
            aIL.FromCSN = Session["FromCSN"].ToString();
            aIL.ToCSN = Session["ToCSN"].ToString();
            aIL.ISIN = Session["ISIN"].ToString();
            aIL.TransferAmount = double.Parse(Session["TransferAmount"].ToString());

            aIL.CreatedById = int.Parse(Session["UserId"].ToString());
            aIL.CreatedDate = System.DateTime.Now;

            //save to db
            _context.AILs.Add(aIL);
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
