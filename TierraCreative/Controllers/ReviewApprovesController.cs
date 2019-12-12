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
    public class ReviewApprovesController : BaseController
    {
        public ActionResult Review()
        {
            List<ReviewModel> reviews = new List<ReviewModel>();

            var dRps = _context.DRPs.Include(a => a.User).Include(a => a.ReviewedUser).ToList();
            var aILs = _context.AILs.Include(a => a.User).Include(a => a.ReviewedUser).ToList();
            var sPs = _context.SupplementaryDividends.Include(a => a.User).Include(a => a.ReviewedUser).ToList();

            foreach (var drp in dRps) {
                reviews.Add(new ReviewModel
                {
                    Id = drp.DRPId,
                    Source = "DRP",
                    From = drp.CSN,
                    To = "",
                    ISIN = drp.ISIN,
                    Amount = Convert.ToDouble(drp.DRPAmount),
                    SubmittedBy = drp.User.UserName,
                    SubmittedDate = drp.CreatedDate,
                    ApprovedBy = (drp.ReviewedById!=null ? drp.ReviewedUser.UserName : "Pending")
                });
            }

            foreach (var ail in aILs)
            {
                reviews.Add(new ReviewModel
                {
                    Id = ail.AILId,
                    Source = "AIL",
                    From = ail.FromCSN,
                    To = ail.ToCSN,
                    ISIN = ail.ISIN,
                    Amount = Convert.ToDouble(ail.TransferAmount),
                    SubmittedBy = ail.User.UserName,
                    SubmittedDate = ail.CreatedDate,
                    ApprovedBy = (ail.ReviewedById != null ? ail.ReviewedUser.UserName : "Pending")
                });
            }

            foreach (var sP in sPs)
            {
                reviews.Add(new ReviewModel
                {
                    Id= sP.SDId,
                    Source = "Supplementary Dividend",
                    From = sP.FromCSN,
                    To = sP.ToCSN,
                    ISIN = sP.ISIN,
                    Amount = Convert.ToDouble(sP.TransferAmount),
                    SubmittedBy = sP.User.UserName,
                    SubmittedDate = sP.CreatedDate,
                    ApprovedBy = (sP.ReviewedById != null ? sP.ReviewedUser.UserName : "Pending")
                });
            }

            return View(reviews.OrderByDescending(x=>x.SubmittedDate).ToList());
        }

        public ActionResult Approve(int? id) {

            var source = Request.QueryString["source"];


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
