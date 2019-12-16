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
            ViewBag.IsView = null;
            ViewBag.ErrorMessage = null;

            List<ReviewModel> reviews = new List<ReviewModel>();

            var dRps = _context.DRPs.Include(a => a.User).Include(a => a.ReviewedUser).Where(x=>x.DeletedById == null).ToList();
            var aILs = _context.AILs.Include(a => a.User).Include(a => a.ReviewedUser).Where(x => x.DeletedById == null).ToList();
            var sPs = _context.SupplementaryDividends.Include(a => a.User).Include(a => a.ReviewedUser).Where(x => x.DeletedById == null).ToList();

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

        public ActionResult Approve(int? id)
        {
            ViewBag.IsView = null;
            ViewBag.ErrorMessage = null;

            var source = Request.QueryString["source"];

            var review = new ReviewModel();

            review = GetApproveData(review, source, int.Parse(id.ToString()));

            return View(review);
        }

        [HttpPost]
        public ActionResult Approve(FormCollection form) {
            ViewBag.IsView = null;
            ViewBag.ErrorMessage = null;

            var id = int.Parse(form["Id"]);
            var source = form["Source"];

            if (Session["UserId"].ToString() != id.ToString())
            {
                switch (source)
                {
                    case "DRP":
                        var drp = _context.DRPs.Include(a => a.User).Include(a => a.ReviewedUser)
                                               .SingleOrDefault(x => x.DRPId == id);

                        drp.ReviewedById = int.Parse(Session["UserId"].ToString());
                        drp.ReviewedDate = System.DateTime.Now;

                        _context.Entry(drp).State = EntityState.Modified;
                        _context.SaveChanges();

                        break;
                    case "AIL":
                        var ail = _context.AILs.Include(a => a.User).Include(a => a.ReviewedUser)
                                               .SingleOrDefault(x => x.AILId == id);

                        ail.ReviewedById = int.Parse(Session["UserId"].ToString());
                        ail.ReviewedDate = System.DateTime.Now;

                        _context.Entry(ail).State = EntityState.Modified;
                        _context.SaveChanges();

                        break;
                    case "Supplementary Dividend":
                        var sP = _context.SupplementaryDividends.Include(a => a.User).Include(a => a.ReviewedUser)
                                                                .SingleOrDefault(x => x.SDId == id);

                        sP.ReviewedById = int.Parse(Session["UserId"].ToString());
                        sP.ReviewedDate = System.DateTime.Now;

                        _context.Entry(sP).State = EntityState.Modified;
                        _context.SaveChanges();

                        break;
                }

                ViewBag.IsView = "Approve";
            }
            else
            {
                var review = new ReviewModel();

                review = GetApproveData(review, source, int.Parse(id.ToString()));

                ViewBag.ErrorMessage = "You cannot approve your own entry!";

                return View(review);
            }

            return View();

            //return Redirect("/ra/review");
        }

        public ActionResult Delete(int? id)
        {
            ViewBag.IsView = null;
            ViewBag.ErrorMessage = null;

            var source = Request.QueryString["source"];

            switch (source)
            {
                case "DRP":
                    var drp = _context.DRPs.Include(a => a.User).Include(a => a.ReviewedUser)
                                           .SingleOrDefault(x => x.DRPId == id);

                    drp.DeletedById = int.Parse(Session["UserId"].ToString());
                    drp.DeletedDate = System.DateTime.Now;

                    _context.Entry(drp).State = EntityState.Modified;
                    _context.SaveChanges();

                    break;
                case "AIL":
                    var ail = _context.AILs.Include(a => a.User).Include(a => a.ReviewedUser)
                                           .SingleOrDefault(x => x.AILId == id);

                    ail.DeletedById = int.Parse(Session["UserId"].ToString());
                    ail.DeletedDate = System.DateTime.Now;

                    _context.Entry(ail).State = EntityState.Modified;
                    _context.SaveChanges();

                    break;
                case "Supplementary Dividend":
                    var sP = _context.SupplementaryDividends.Include(a => a.User).Include(a => a.ReviewedUser)
                                                            .SingleOrDefault(x => x.SDId == id);

                    sP.DeletedById = int.Parse(Session["UserId"].ToString());
                    sP.DeletedDate = System.DateTime.Now;

                    _context.Entry(sP).State = EntityState.Modified;
                    _context.SaveChanges();

                    break;
            }

            ViewBag.IsView = "Delete";

            return View("Approve");

            //return Redirect("/ra/review");
        }
        
        #region -- private methods --

        private ReviewModel GetApproveData(ReviewModel review, string source, int id) {
            switch (source)
            {
                case "DRP":
                    var drp = _context.DRPs.Include(a => a.User).Include(a => a.ReviewedUser)
                                           .SingleOrDefault(x => x.DRPId == id);
                    review = new ReviewModel
                    {
                        Id = drp.DRPId,
                        Source = "DRP",
                        From = drp.CSN,
                        To = "",
                        ISIN = drp.ISIN,
                        Amount = Convert.ToDouble(drp.DRPAmount),
                        SubmittedBy = drp.User.UserName,
                        SubmittedDate = drp.CreatedDate,
                        ApprovedBy = (drp.ReviewedById != null ? drp.ReviewedUser.UserName : "Pending")
                    };

                    break;
                case "AIL":
                    var ail = _context.AILs.Include(a => a.User).Include(a => a.ReviewedUser)
                                           .SingleOrDefault(x => x.AILId == id);
                    review = new ReviewModel
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
                    };

                    break;
                case "Supplementary Dividend":
                    var sP = _context.SupplementaryDividends.Include(a => a.User).Include(a => a.ReviewedUser)
                                                            .SingleOrDefault(x => x.SDId == id);
                    review = new ReviewModel
                    {
                        Id = sP.SDId,
                        Source = "Supplementary Dividend",
                        From = sP.FromCSN,
                        To = sP.ToCSN,
                        ISIN = sP.ISIN,
                        Amount = Convert.ToDouble(sP.TransferAmount),
                        SubmittedBy = sP.User.UserName,
                        SubmittedDate = sP.CreatedDate,
                        ApprovedBy = (sP.ReviewedById != null ? sP.ReviewedUser.UserName : "Pending")
                    };

                    break;
            }

            return review;
        }
        #endregion

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
