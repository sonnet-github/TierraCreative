using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TierraCreative;
using TierraCreative.Model;
using TierraCreative.ViewModels;

namespace TierraCreative.Controllers
{
    public class UsersController : BaseController
    {
        #region -- Admin --
        
        public ActionResult Admin()
        {
            return View("Admin");
        }

        [HttpPost]
        public ActionResult Admin(FormCollection form)
        {
            ViewBag.ErrorMessage = null;

            var username = form["txtusername"].ToString();
            var password = form["txtpassword"].ToString();

            var user = _context.Users.Include(x => x.Role).SingleOrDefault(x => x.UserName == username || x.Email == username);
            if (user != null)
            {
                if (password == user.Password)
                    if (user.IsEnabled == true)
                    {
                        if ((user.Role.RoleName == "Super User" || user.Role.RoleName == "Admin"))
                        {
                            Session["UserId"] = user.UserId;
                            Session["UserName"] = user.UserName;
                            Session["UserRole"] = user.Role.RoleName;
                            Session["UserFullName"] = "";
                            Session["UserEmail"] = user.Email;

                            return Redirect("/admin/main");
                        }
                        else
                            ViewBag.ErrorMessage = "You do not have access to the admin panel!";
                    }
                    else
                        ViewBag.ErrorMessage = "UserName is disabled!";
                else
                    ViewBag.ErrorMessage = "Invalid password!";
            }
            else
                ViewBag.ErrorMessage = "UserName does not exists!";

            return View("admin");
        }

        public ActionResult Main()
        {
            if (Session["UserId"] == null)
                return Redirect("/admin");

            if (Session["Deleted"] != null) { 
                ViewBag.IsView = "Deleted";
                Session["Deleted"] = null;
            }

            var users = _context.Users.Include(u => u.Role).Where(x=>x.DeletedById == null);

            return View("Main", users.ToList());
        }
               
        public ActionResult Details(int? id)
        {
            if (Session["UserId"] == null)
                return Redirect("/admin");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = _context.Users.Include(u => u.Role).SingleOrDefault(x=>x.UserId == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
              
        public ActionResult Create()
        {
            if (Session["UserId"] == null)
                return Redirect("/admin");

            ViewBag.IsView = null;
            ViewBag.ErrorMessage = null;

            if (Session["UserRole"].ToString() == "Admin")
                ViewBag.RoleId = new SelectList(_context.Roles.Where(x=>x.RoleId == 2 || x.RoleId == 3), "RoleId", "RoleName");
            else
                ViewBag.RoleId = new SelectList(_context.Roles, "RoleId", "RoleName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user) 
        {
            if (Session["UserId"] == null)
                return Redirect("/admin");

            ViewBag.IsView = null;
            ViewBag.ErrorMessage = null;

            var check_users = _context.Users.Where(x => x.UserName == user.UserName || x.Email == user.Email).ToList();
            if (check_users.Count!=0)
            {
                ViewBag.ErrorMessage = @"UserName/Email already exists!";
            }
            else
            {
                user.IsEnabled = true;
                user.CreatedById = int.Parse(Session["UserId"].ToString());
                user.CreatedDate = System.DateTime.Now;

                _context.Users.Add(user);
                _context.SaveChanges();

                ViewBag.IsView = "Created";
                //return RedirectToAction("../admin/main");
            }

            if (Session["UserRole"].ToString() == "Admin")
                ViewBag.RoleId = new SelectList(_context.Roles.Where(x => x.RoleId == 2 || x.RoleId == 3), "RoleId", "RoleName");
            else
                ViewBag.RoleId = new SelectList(_context.Roles, "RoleId", "RoleName");

            return View(user);
        }
       
        public ActionResult Edit(int? id)
        {
            if (Session["UserId"] == null)
                return Redirect("/admin");

            ViewBag.IsView = null;
            ViewBag.ErrorMessage = null;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = _context.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            if (Session["UserRole"].ToString() == "Admin")
                ViewBag.RoleId = new SelectList(_context.Roles.Where(x => x.RoleId == 2 || x.RoleId == 3), "RoleId", "RoleName",user.RoleId);
            else
                ViewBag.RoleId = new SelectList(_context.Roles, "RoleId", "RoleName", user.RoleId);

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (Session["UserId"] == null)
                return Redirect("/admin");

            ViewBag.IsView = null;
            ViewBag.ErrorMessage = null;

            if (ModelState.IsValid)
            {
                var check_users = _context.Users.Where(x => (x.UserName == user.UserName || x.Email == user.Email) && x.UserId!=user.UserId).ToList();
                if (check_users.Count!=0)
                {
                    ViewBag.ErrorMessage = @"UserName/Email already exists!";
                }
                else
                {
                    user.UpdatedById = int.Parse(Session["UserId"].ToString());
                    user.UpdatedDate = System.DateTime.Now;

                    _context.Entry(user).State = EntityState.Modified;
                    _context.SaveChanges();
                    return RedirectToAction("../admin/main");
                }
            }

            if (Session["UserRole"].ToString() == "Admin")
                ViewBag.RoleId = new SelectList(_context.Roles.Where(x => x.RoleId == 2 || x.RoleId == 3), "RoleId", "RoleName");
            else
                ViewBag.RoleId = new SelectList(_context.Roles, "RoleId", "RoleName");

            return View(user);
        }
              
        //public ActionResult Delete(int? id)
        //{
        //    //if (Session["UserId"] == null)
        //    //    return Redirect("/admin");

        //    //ViewBag.IsView = null;
        //    //ViewBag.ErrorMessage = null;

        //    //if (id == null)
        //    //{
        //    //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    //}
        //    //User user = _context.Users.Include(u => u.Role).SingleOrDefault(x => x.UserId == id);
        //    //if (user == null)
        //    //{
        //    //    return HttpNotFound();
        //    //}
        //    //return View(user);
        //}

        public ActionResult DeleteConfirm(int id)
        {
            if (Session["UserId"] == null)
                return Redirect("/admin");

            ViewBag.IsView = null;
            ViewBag.ErrorMessage = null;

            User user = _context.Users.Find(id);

            user.IsEnabled = false;
            user.DeletedById = int.Parse(Session["UserId"].ToString());
            user.DeletedDate = System.DateTime.Now;

            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();

            Session["Deleted"] = true; 

            return RedirectToAction("../admin/main");

            //return View("delete",user);
        }

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    //if (Session["UserId"] == null)
        //    //    return Redirect("/admin");

        //    //User user = _context.Users.Find(id);

        //    //user.IsEnabled = false;
        //    //user.DeletedById = int.Parse(Session["UserId"].ToString());
        //    //user.DeletedDate = System.DateTime.Now;

        //    //_context.Entry(user).State = EntityState.Modified;
        //    //_context.SaveChanges();

        //    ////_context.Users.Remove(user);

        //    return RedirectToAction("../admin/main");
        //}

        public ActionResult Support()
        {
            if (Session["UserId"] == null)
                return Redirect("/admin");

            ViewBag.IsSuccess = null;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Support(User user, FormCollection form)
        {
            if (Session["UserId"] == null)
                return Redirect("/admin");

            //string UserId = Session["UserId"].ToString();

            string msg = form["txtmessage"];

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

            if (success)
                ViewBag.IsSuccess = true;

            return View(user);
        }

        public ActionResult LogoutAdmin()
        {
            Session.Clear();
            Session.RemoveAll();

            return Redirect("/Admin");
            //return View("../Login");
        }


        #endregion

        #region -- Users -- 
        public ActionResult Login()
        {
            return View("../Login");
        }

        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            var username = form["txtusername"].ToString();
            var password = form["txtpassword"].ToString();
           
            var user = _context.Users.Include(x=>x.Role).SingleOrDefault(x => x.UserName == username || x.Email == username);
            if (user != null)
            {
                if (password == user.Password)
                    if (user.IsEnabled == true) {
                        Session["UserId"] = user.UserId;
                        Session["UserName"] = user.UserName;
                        Session["UserRole"] = user.Role.RoleName;
                        Session["UserFullName"] = "";
                        Session["UserEmail"] = user.Email;

                        return Redirect("Forms");
                    }
                    else
                        ViewBag.ErrorMessage = "UserName is disabled!";
                else
                    ViewBag.ErrorMessage = "Invalid password!";
            }
            else 
                ViewBag.ErrorMessage = "UserName does not exists!";
            
            return View("../Login");
        }

        [SessionExpire]
        public ActionResult Forms()
        {
            Session["CSN"] = null;
            Session["FromCSN"] = null;
            Session["ToCSN"] = null;
            Session["ToCSN"] = null;
            Session["ISIN"] = null;
            Session["DRPAmount"] = null;
            Session["TransferAmount"] = null;

            return View("../Forms");
        }

        [SessionExpire]
        public ActionResult ChangePassword()
        {
            ViewBag.IsSuccess = null;
            return View("../ChangePassword");
        }

        [SessionExpire]
        [HttpPost]
        public ActionResult ChangePassword(FormCollection form)
        {
            ViewBag.IsSuccess = null;

            var newpassword = form["New password"].ToString();

            var userid = int.Parse(Session["UserId"].ToString());
            var user = _context.Users.SingleOrDefault(x => x.UserId == userid);

            if (user != null)
            {
                user.Password = newpassword;
                _context.Entry(user).State = EntityState.Modified;
                _context.SaveChanges();

                ViewBag.IsSuccess = "Success";

            }

            return View("../ChangePassword");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.RemoveAll();

            return RedirectToAction("Login", "Users", null);
            //return View("../Login");
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
