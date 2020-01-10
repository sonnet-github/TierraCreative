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
                            Session["UserFullName"] = user.FullName;
                            Session["UserEmail"] = user.Email;
                            Session["Layout"] = "admin";
                            Session["IsFirstLog"] = "false";
                            if (user.IsFirstLog == false)
                            {
                                return Redirect("/admin/main");
                            }
                            else
                            {
                                Session["IsFirstLog"] = "true";
                                return Redirect("ChangePassword");
                            }

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

            if (Session["Deleted"] != null)
            {
                ViewBag.IsView = "Deleted";
                ViewBag.IsAllowed = Session["allowed"];
                Session["Deleted"] = null;
            }

            var users = _context.Users.Include(u => u.Role).Where(x => x.DeletedById == null);

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
            User user = _context.Users.Include(u => u.Role).SingleOrDefault(x => x.UserId == id);
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
                ViewBag.RoleId = new SelectList(_context.Roles.Where(x => x.RoleId == 2 || x.RoleId == 3), "RoleId", "RoleName");
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
            var error_ = false;
            
            if (check_users.Count != 0)
            {
                ViewBag.ErrorMessage = "UserName/Email already exists!<br>";
                error_ = true;
                
            }
            var newpassword = user.Password;
            if (newpassword != Request["ConfirmPassword"].ToString())
            {
                ViewBag.ErrorMessage += "Password and Confirm Password did not match!<br/>"; error_ = true;
            }
            if (newpassword.Length < 8)
            {
                ViewBag.ErrorMessage += "Minimum of 8 characters is required!<br/>"; error_ = true;
            }
            if (!newpassword.Any(char.IsUpper))
            {
                ViewBag.ErrorMessage += "Password must have at least 1 uppercase!<br/>"; error_ = true;
            }
            if (!newpassword.Any(char.IsLower))
            {
                ViewBag.ErrorMessage += "Password must have at least 1 lowercase!<br/>"; error_ = true;
            }
            if (!newpassword.Any(ch => !Char.IsLetterOrDigit(ch)))
            {
                ViewBag.ErrorMessage += "Password must have at least 1 special character!<br/>"; error_ = true;
            }
            if (!newpassword.Any(char.IsNumber))
            {
                ViewBag.ErrorMessage += "Password must have at least 1 numeric character!<br/>"; error_ = true;
            }
            if (!error_ )
            {
                user.IsEnabled = true;
                user.CreatedById = int.Parse(Session["UserId"].ToString());
                user.CreatedDate = System.DateTime.Now;
                user.IsFirstLog = true;

                _context.Users.Add(user);
                _context.SaveChanges();

                ViewBag.IsView = "Created";
            }
            //return RedirectToAction("../admin/main");


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
                ViewBag.RoleId = new SelectList(_context.Roles.Where(x => x.RoleId == 2 || x.RoleId == 3), "RoleId", "RoleName", user.RoleId);
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
                var check_users = _context.Users.Where(x => (x.UserName == user.UserName || x.Email == user.Email) && x.UserId != user.UserId).ToList();
                if (check_users.Count != 0)
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

            User user = _context.Users.Include(x => x.Role).SingleOrDefault(x => x.UserId == id);

            user.IsEnabled = false;
            user.DeletedById = int.Parse(Session["UserId"].ToString());
            user.DeletedDate = System.DateTime.Now;

            var userid = int.Parse(Session["UserId"].ToString());
            var cur_user = _context.Users.SingleOrDefault(x => x.UserId == userid);
            Session["allowed"] = false;
            Session["with_transaction"] = false;
            var with_transaction = true;

            var dRps = _context.DRPs.Include(a => a.User).Include(a => a.ReviewedUser).Where(x => x.CreatedById == id).Where(x => x.DeletedById == null).ToList();
            var aILs = _context.AILs.Include(a => a.User).Include(a => a.ReviewedUser).Where(x => x.CreatedById == id).Where(x => x.DeletedById == null).ToList();
            var sPs = _context.SupplementaryDividends.Include(a => a.User).Include(a => a.ReviewedUser).Where(x => x.CreatedById == id).Where(x => x.DeletedById == null).ToList();

            if (dRps.Count == 0 || dRps.Count == 0 || dRps.Count == 0) {
                with_transaction = false;
            }
            ViewBag.with_transaction = with_transaction;
            if (Session["UserRole"].ToString() != "Super User" && with_transaction==false)
            {
                if (user != null) {
                    if (user.Role.RoleName != "Super User")
                    {
                        Session["allowed"] = true;
                        _context.Entry(user).State = EntityState.Modified;
                        _context.SaveChanges();
                    }
                }
            }
            else
            {
                if (with_transaction) {
                    Session["allowed"] = true;
                    _context.Entry(user).State = EntityState.Modified;
                    _context.SaveChanges();
                }
            }



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
            User user = _context.Users.Find(Session["UserId"]);
            if (Session["UserId"] == null)
                return Redirect("/admin");

            ViewBag.IsSuccess = null;

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Support(User user, FormCollection form)
        {
            if (Session["UserId"] == null)
                return Redirect("/admin");

            Utility.Utilities utilities = new Utility.Utilities();

            //string UserId = Session["UserId"].ToString();

            string msg = form["txtmessage"];

            //send email link
            var success = utilities.SendSupportEmail(System.Configuration.ConfigurationManager.AppSettings["supportemail"], System.Configuration.ConfigurationManager.AppSettings["supportemail"], user.FullName, user.UserName, msg);
            success = utilities.SendSupportUserEmail(System.Configuration.ConfigurationManager.AppSettings["supportemail"], user.Email, user.FullName, user.UserName, msg);

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
            ViewBag.ErrorMessage = null;

            var username = form["txtusername"].ToString();
            var password = form["txtpassword"].ToString();

            var user = _context.Users.Include(x => x.Role).SingleOrDefault(x => x.UserName == username || x.Email == username);
            if (user != null)
            {
                if (password == user.Password)
                    if (user.IsEnabled == true)
                    {
                        Session["UserId"] = user.UserId;
                        Session["UserName"] = user.UserName;
                        Session["UserRole"] = user.Role.RoleName;
                        Session["UserFullName"] = user.FullName;
                        Session["UserEmail"] = user.Email;
                        Session["Layout"] = "user";
                        Session["IsFirstLog"] = "false";
                        if (user.IsFirstLog == false)
                        {
                            return Redirect("Forms");
                        }
                        else
                        {
                            Session["IsFirstLog"] = "true";
                            return Redirect("ChangePassword");
                        }
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
            Session["IsFirstLog"] = "false";
            if (Session["UserEmail"] == null)
            {
                return Redirect("Login");
            }
            else
            {
                var email = Session["UserEmail"].ToString();
                var userid = int.Parse(Session["UserId"].ToString());
                var user = _context.Users.SingleOrDefault(x => x.UserId == userid);
                if (user != null)
                {
                    if (user.IsFirstLog == false)
                    {
                        return View("../Forms");
                    }
                    else
                    {
                        Session["IsFirstLog"] = "true";
                        return Redirect("ChangePassword");
                    }
                }
                else
                {
                    return View("../Forms");
                }
            }

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
            Utility.Utilities utilities = new Utility.Utilities();

            ViewBag.IsSuccess = null;

            var newpassword = form["New password"].ToString();

            var userid = int.Parse(Session["UserId"].ToString());
            var user = _context.Users.SingleOrDefault(x => x.UserId == userid);
            var error_ = false;
            if (newpassword != Request["ConfirmPassword"].ToString())
            {
                ViewBag.ErrorMessage += "Password and Confirm Password did not match!<br/>"; error_ = true;
            }
            if (newpassword.Length < 8)
            {
                ViewBag.ErrorMessage += "Minimum of 8 characters is required!<br/>"; error_ = true;
            }
            if (!newpassword.Any(char.IsUpper))
            {
                ViewBag.ErrorMessage += "Password must have at least 1 uppercase!<br/>"; error_ = true;
            }
            if (!newpassword.Any(char.IsLower))
            {
                ViewBag.ErrorMessage += "Password must have at least 1 lowercase!<br/>"; error_ = true;
            }
            if (!newpassword.Any(ch => !Char.IsLetterOrDigit(ch)))
            {
                ViewBag.ErrorMessage += "Password must have at least 1 special character!<br/>"; error_ = true;
            }
            if (!newpassword.Any(char.IsNumber))
            {
                ViewBag.ErrorMessage += "Password must have at least 1 numeric character!<br/>"; error_ = true;
            }
            if (!error_)
            {
                user.Password = newpassword;
                user.IsFirstLog = false;
                _context.Entry(user).State = EntityState.Modified;
                _context.SaveChanges();

                ViewBag.IsSuccess = "Success";

                //send email link
                var success = utilities.SendChangePasswordEmail(System.Configuration.ConfigurationManager.AppSettings["supportemail"], user.Email, user.UserName);

            }

            return View("../ChangePassword");
        }

        public ActionResult ForgotPassword()
        {
            ViewBag.IsSuccess = null;
            ViewBag.ErrorMessage = null;

            return View("../ForgotPassword");
        }

        [HttpPost]
        public ActionResult ForgotPassword(FormCollection form)
        {
            Utility.Utilities utilities = new Utility.Utilities();

            ViewBag.IsSuccess = null;
            ViewBag.ErrorMessage = null;

            //get new guid
            Guid guid = Guid.NewGuid();

            var username_parameter = form["txtusername"].ToString();

            var user = _context.Users.Include(x => x.Role).SingleOrDefault(x => x.UserName == username_parameter || x.Email == username_parameter);
            //var error_ = false;

            if (user != null)
            {
                //if (user.Password != Request["ConfirmPassword"])
                //{
                //    ViewBag.ErrorMessage += "Password and Confirm Password did not match!"; error_ = true;
                //}
                //if (!error_)
                //{
                    //add new forgotpassword token
                    var forgotpassword = new ForgotPasswordToken
                    {
                        Unique_Guid = guid.ToString(),
                        Email = user.Email,
                        CreatedDate = System.DateTime.Now
                    };
                    _context.ForgotPasswordTokens.Add(forgotpassword);
                    _context.SaveChanges();

                    //send email link
                    var success = utilities.SendForgotPasswordEmail(guid.ToString(), System.Configuration.ConfigurationManager.AppSettings["supportemail"], user.Email, user.UserName);

                    ViewBag.IsSuccess = "Success";
                //}
            }
            else
                ViewBag.ErrorMessage = "User not found.";

            return View("../ForgotPassword");
        }

        public ActionResult ForgotPasswordChange()
        {
            ViewBag.IsSuccess = null;
            ViewBag.ErrorMessage = null;

            var guid = Request.QueryString["guid"];
            var email = Request.QueryString["email"];

            var forgotpasswordtoken = _context.ForgotPasswordTokens.SingleOrDefault(x => x.Unique_Guid == guid);

            if (forgotpasswordtoken != null)
            {
                ViewBag.Email = email;
                return View("../ForgotPasswordChange");
            }

            return Redirect("/");
        }

        [HttpPost]
        public ActionResult ForgotPasswordChange(FormCollection form)
        {
            Utility.Utilities utilities = new Utility.Utilities();

            ViewBag.IsSuccess = null;
            var email = form["email"];
            ViewBag.Email = email;
            var newpassword = form["New Password"];

            var user = _context.Users.SingleOrDefault(x => x.Email == email);
            var error_ = false;
            if (user != null)
            {
                if (newpassword != Request["ConfirmPassword"].ToString())
                {
                    ViewBag.ErrorMessage += "Password and Confirm Password did not match.<br/>"; error_ = true;
                }
                if (newpassword.Length < 8)
                {
                    ViewBag.ErrorMessage += "Minimum of 8 characters is required.<br/>"; error_ = true;
                }
                if (!newpassword.Any(char.IsUpper))
                {
                    ViewBag.ErrorMessage += "Password must have at least 1 uppercase.<br/>"; error_ = true;
                }
                if (!newpassword.Any(char.IsLower))
                {
                    ViewBag.ErrorMessage += "Password must have at least 1 lowercase.<br/>"; error_ = true;
                }
                if (!newpassword.Any(ch => !Char.IsLetterOrDigit(ch)))
                {
                    ViewBag.ErrorMessage += "Password must have at least 1 special character.<br/>"; error_ = true;
                }
                if (!newpassword.Any(char.IsNumber))
                {
                    ViewBag.ErrorMessage += "Password must have at least 1 numeric character!<br/>"; error_ = true;
                }
                if (!error_)
                {
                    //update password
                    user.Password = newpassword;
                    _context.Entry(user).State = EntityState.Modified;
                    _context.SaveChanges();

                    //delete all token of user forgot password
                    var forgotpasswordtoken = _context.ForgotPasswordTokens.Where(x => x.Email == email).ToList();
                    if (forgotpasswordtoken.Any())
                    {
                        _context.ForgotPasswordTokens.RemoveRange(forgotpasswordtoken);
                        _context.SaveChanges();
                    }

                    ViewBag.IsSuccess = "Success";
                }
            }

            return View("../ForgotPasswordChange");
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
