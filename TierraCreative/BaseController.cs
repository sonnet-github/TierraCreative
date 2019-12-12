using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TierraCreative.Model;

namespace TierraCreative
{
    public class BaseController : Controller
    {
        protected TierraCreativeContext _context;

        public BaseController()
        {
            _context = new TierraCreativeContext();
            _context.Configuration.LazyLoadingEnabled = true;
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();

            base.Dispose(disposing);
        }

        //public ActionResult AdminNoAccessPage(string page)
        //{
        //    if (page!="report")
        //        return RedirectToAction("noaccess");

        //    return RedirectToAction("../noaccess");
        //}
    }
}