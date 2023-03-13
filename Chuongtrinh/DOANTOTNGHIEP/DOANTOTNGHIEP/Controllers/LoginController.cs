using BundleTransformer.Core.Constants;
using CaptchaMvc.HtmlHelpers;
using DOANTOTNGHIEP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DOANTOTNGHIEP.Controllers
{
    public class LoginController : Controller
    {
        public DOANTOTNGHIEP.Models.TaiKhoan checkcookieuser()
        {
            DB db = new DB();
            var user = Request.Cookies["user"];
            if (user != null && user["TenDangNhap"].ToString().Length > 0 && user["Matkhau"].ToString().Length > 0)
            {
                var tendangnhap = Models.crypt.Encrypt.Decryptuser(user["TenDangNhap"].ToString());
                var matkhau = Models.crypt.Encrypt.Decryptuser(user["Matkhau"].ToString());
                var TK = db.TaiKhoans.SingleOrDefault(x => x.TenDangNhap.Equals(tendangnhap) && x.MatKhau.Equals(matkhau));
                return TK;

            }
            return null;
        }
        public (bool, string, string , DOANTOTNGHIEP.Models.TaiKhoan) checkCookie()
        {
           
            var user = checkcookieuser();
            if (user != null &&  user.ChucVu.ToLower().Contains("Admin".ToLower()))
            {
                return (true, "Index", "Admin", user);
            } else  if (user != null)
            {
                return (true, "Index", "TrangChu" , user);

            }
            return (false, "", "", user);
        }
        // GET: Login
        public ActionResult Login()
        {
            var checkcookie = checkCookie();
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            TaiKhoan taiKhoan = new TaiKhoan();

            return View(taiKhoan);
        }
        [HttpPost]

        public ActionResult checkaccount(TaiKhoan taiKhoan)
        {
            var checkcookie = checkCookie();
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            ViewBag.user = user;

            /*if (!this.IsCaptchaValid(""))
            {
                ModelState.AddModelError("", "Invalid Captcha");
            }

            else if (ModelState.IsValid)
            {*/
            DB db = new DB();
            string mk = Models.crypt.Encrypt.encryptuser(taiKhoan.MatKhau);
            var TK = db.TaiKhoans.SingleOrDefault(x => x.TenDangNhap.Equals(taiKhoan.TenDangNhap) && x.MatKhau.Equals(mk));
          
            if (TK != null)
            {
                HttpCookie userCookie = new HttpCookie("user");
                userCookie["TenDangNhap"] = Models.crypt.Encrypt.encryptuser(TK.TenDangNhap);
                userCookie["Matkhau"] = Models.crypt.Encrypt.encryptuser(TK.MatKhau);
                userCookie.Expires = DateTime.Now.AddDays(365 * 10);
                Response.Cookies.Add(userCookie);
                return RedirectToAction("Index", "TrangChu");
            }
            else if (db.TaiKhoans.SingleOrDefault(x => x.TenDangNhap.Equals(taiKhoan.TenDangNhap)) != null)
            {
                ModelState.AddModelError("", "Mật khẩu không đúng ");
            }
            else if (db.TaiKhoans.SingleOrDefault(x => x.TenDangNhap.Equals(taiKhoan.TenDangNhap)) == null)
            {
                ModelState.AddModelError("", "tên đăng nhập không đúng ");
            }

            /*}*/
            return View("Login");
        }

        public ActionResult logout()
        {
            
            HttpCookie user = new HttpCookie("user");
            user.Expires = DateTime.Now.AddDays(-1d);
            Response.Cookies.Add(user);
            return RedirectToAction("Login", "Login");
        }
    }

}