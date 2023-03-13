using DOANTOTNGHIEP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DOANTOTNGHIEP.Controllers
{
    public class FeedbackController : Controller
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
            if (user == null)
            {
                return (true, "Login", "Login", user);
            }
            else if (user.ChucVu.ToLower().Contains("Admin".ToLower()))
            {
                return (true, "Index", "Admin", user);
            }
            return (false, "", "", user);
        }
        // GET: Feedback
        public ActionResult Sendfeedback()
        {
            var checkcookie = checkCookie();
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            ViewBag.user = user;
            return View();

        }
        public ActionResult Send()
        {
            var checkcookie = checkCookie();
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            ViewBag.user = user;
            string thongtin = Request.Form["thongtin"].ToString();
            
            if (user == null) return RedirectToAction("Login", "Login");

            DOANTOTNGHIEP.Models.mail.mail.SendEmail("haupham404@gmail.com", "Góp ý cải thiện hệ thống từ email : " + user.Email, thongtin);
            return RedirectToAction("Index", "TrangChu");

        }
    }

}