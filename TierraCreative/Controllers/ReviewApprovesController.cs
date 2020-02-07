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

            Utility.Utilities utilities = new Utility.Utilities();

            //email variable
            var TransactionID = string.Empty;
            var FromCSNValue = string.Empty;
            var ToCSNValue = string.Empty;
            var ISINValue = string.Empty;
            var AmountValue = string.Empty;
            var Timestamp = System.DateTime.Now.ToShortDateString();
            var FormName = string.Empty;
            var UserName = Session["UserName"].ToString();
            var FullName = Session["UserFullName"].ToString();
            var ApprovalEmail = Session["UserEmail"].ToString();
            var fromEmail = System.Configuration.ConfigurationManager.AppSettings["supportemail"];

            var id = int.Parse(form["Id"]);
            var source = form["Source"];
            var sourceval = "";
            var destinationval = "";
            var destination = form["To"];

            var submitUserEmail = string.Empty;
            var computershareEmail = string.Empty;

            if (UserName != form["SubmittedBy"].ToString())
            {
                switch (source)
                {
                    case "DRP":
                        var drp = _context.DRPs.Include(a => a.User).Include(a => a.ReviewedUser).Include(a => a.CreatedByUser)
                                               .SingleOrDefault(x => x.DRPId == id);

                        var drp_val = _context.CSNLookUps.SingleOrDefault(a => a.CSNName == drp.CSN);
                        sourceval = drp_val.CSNAccount;


                        drp.ReviewedById = int.Parse(Session["UserId"].ToString());
                        drp.ReviewedDate = System.DateTime.Now;

                        _context.Entry(drp).State = EntityState.Modified;
                        _context.SaveChanges();                       

                        //email variables
                        FormName = source;
                        TransactionID = FormName + "-" + drp.DRPId.ToString();
                        FromCSNValue = drp.CSN;
                        ISINValue = drp.ISIN;
                        AmountValue = drp.DRPAmount.ToString();                        
                        computershareEmail = "1drp@computershare.co.nz";
                        submitUserEmail = drp.CreatedByUser.Email;

                        break;
                    case "AIL":
                        var ail = _context.AILs.Include(a => a.User).Include(a => a.ReviewedUser).Include(a => a.CreatedByUser)
                                               .SingleOrDefault(x => x.AILId == id);

                        var ail_val = _context.CSNLookUps.SingleOrDefault(a => a.CSNName == ail.FromCSN);
                        sourceval = ail_val.CSNAccount;

                        ail_val = _context.CSNLookUps.SingleOrDefault(a => a.CSNName == ail.ToCSN);
                        destinationval = ail_val.CSNAccount;

                        ail.ReviewedById = int.Parse(Session["UserId"].ToString());
                        ail.ReviewedDate = System.DateTime.Now;

                        _context.Entry(ail).State = EntityState.Modified;
                        _context.SaveChanges();

                        //email variables
                        FormName = source;
                        TransactionID = FormName + "-" + ail.AILId.ToString();
                        FromCSNValue = ail.FromCSN;
                        ToCSNValue = ail.ToCSN;
                        ISINValue = ail.ISIN;
                        AmountValue = ail.TransferAmount.ToString();                        
                        fromEmail = System.Configuration.ConfigurationManager.AppSettings["supportemail"];
                        computershareEmail = "1payments@computershare.co.nz";
                        submitUserEmail = ail.CreatedByUser.Email;

                        break;
                    case "Supplementary Dividend":
                        var sP = _context.SupplementaryDividends.Include(a => a.User).Include(a => a.ReviewedUser).Include(a => a.CreatedByUser)
                                                                .SingleOrDefault(x => x.SDId == id);

                        var sp_val = _context.CSNLookUps.SingleOrDefault(a => a.CSNName == sP.FromCSN);
                        sourceval = sp_val.CSNAccount;

                        sp_val = _context.CSNLookUps.SingleOrDefault(a => a.CSNName == sP.ToCSN);
                        destinationval = sp_val.CSNAccount;
                        sP.ReviewedById = int.Parse(Session["UserId"].ToString());
                        sP.ReviewedDate = System.DateTime.Now;

                        _context.Entry(sP).State = EntityState.Modified;
                        _context.SaveChanges();

                        //email variables
                        FormName = source;
                        TransactionID = FormName + "-" + sP.CreatedById.ToString();
                        FromCSNValue = sP.FromCSN;
                        ToCSNValue = sP.ToCSN;
                        ISINValue = sP.ISIN;
                        AmountValue = sP.TransferAmount.ToString();
                        computershareEmail = "1payments@computershare.co.nz";
                        submitUserEmail = sP.CreatedByUser.Email;

                        break;
                }

                ViewBag.IsView = "Approve";

                //Email                
                var success = utilities.SendFormEmails(TransactionID,
                            FromCSNValue, ToCSNValue,
                            ISINValue,
                            AmountValue,
                            Timestamp,
                            FullName, UserName,
                            FormName,
                            fromEmail,
                            ApprovalEmail,
                            computershareEmail,
                            submitUserEmail,
                            source,
                            destination,
                            sourceval,
                            destinationval);
            }
            else
            {
                var review = new ReviewModel();
                ViewBag.NotAllowed = true;
                review = GetApproveData(review, source, int.Parse(id.ToString()));

                ViewBag.ErrorMessage = @"You are not able to review your own submission. Please request another user to approve this request.";

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
