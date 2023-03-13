using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOANTOTNGHIEP.Models;
using DOANTOTNGHIEP.Models.GetData;
namespace DOANTOTNGHIEP.Controllers
{
    public class AccountController : Controller
    {
        
        // GET: Account
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

            return (false, "", "" ,user);
        }
        public ActionResult Editaccount()
        {

            var checkcookie = checkCookie();
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            ViewBag.user = user;
            string nguoitao = user.TenDangNhap;
            var tk = GetAccount.get(nguoitao);


            return View(tk);
        }
        public ActionResult Register()
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
        public ActionResult ConfirmEmail()
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
        static string staticmaxacnhan = "";
        static DOANTOTNGHIEP.Models.TaiKhoan staticregisteruser = new TaiKhoan();
        static TaiKhoan staticforgotuser =new TaiKhoan();
        public ActionResult checkmaxacnhan()
        {
            var checkcookie = checkCookie();
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            ViewBag.user = user;
            var maxacnhan = Request.Form["ma"];
            string ma = staticmaxacnhan;
            if (maxacnhan.Equals(ma))
            {
                var tk = staticregisteruser;
                if (tk != null)
                {
                    
                    var taikhoan = DOANTOTNGHIEP.Models.database.taikhoan.AddAccount(tk.Ho, tk.Ten, tk.Email, tk.TenDangNhap, tk.MatKhau, tk.ChucVu);
                    Session.Remove("registeruser");
                    return RedirectToAction("Login", "Login");
                }
                else
                {
                    return View("NewPass");
                }


            }
            else ModelState.AddModelError("", "Mã xác nhận không đúng");

            return View("ConfirmEmail");
        }
        public ActionResult Forgotpass()
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
        public ActionResult checkuser()
        {
            DB db = new DB();
            var checkcookie = checkCookie();
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
           
            var user = Request.Form["username"];
            var email = Request.Form["email"];
            var TK = db.TaiKhoans.SingleOrDefault(x => x.TenDangNhap.Equals(user) && x.Email.Equals(email));
            if (TK != null)
            {
                TaiKhoan tk = new TaiKhoan();
                tk.TenDangNhap = user;
                tk.Email = email;
                staticforgotuser = tk;
                Random r = new Random();
                staticmaxacnhan = r.Next(100000, 999999).ToString();

                DOANTOTNGHIEP.Models.mail.mail.SendEmail(tk.Email, "ma xac nhan", staticmaxacnhan);
                return View("ConfirmEmail");

            }
            else ModelState.AddModelError("", "Tên đăng nhập và mật khẩu không đúng");

            return View("Forgotpass");
        }
        public ActionResult NewPass()
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

        public ActionResult checkpassxacnhan()
        {
            DB db = new DB();
            var checkcookie = checkCookie();
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            ViewBag.user = user;
            var pass = Request.Form["pass"];
            var pass1 = Request.Form["confirmpass"];
            var tk = staticforgotuser;
            if (pass1.Equals(pass) && tk != null)
            {

                var taikhoan = db.TaiKhoans.SingleOrDefault(x => x.TenDangNhap.Equals(tk.TenDangNhap) && x.Email.Equals(tk.Email));

                taikhoan.MatKhau = Models.crypt.Encrypt.encryptuser(pass);
                db.SaveChanges();
                return RedirectToAction("Login", "Login");

            }
            else ModelState.AddModelError("", "Mật khẩu xác nhận không đúng");

            return View("NewPass");
        }

        [HttpPost]
        public ActionResult checkaccountRegister(TaiKhoan taiKhoan)
        {


            var checkcookie = checkCookie();
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            ViewBag.user = user;
            var pass = Request.Form["confirmpass"];
            if (pass.Equals(taiKhoan.MatKhau))
            {
                DB db = new DB();
                var TK = db.TaiKhoans.SingleOrDefault(x => x.TenDangNhap.Equals(taiKhoan.TenDangNhap));
                var TK2 = db.TaiKhoans.Where(x => x.Email.Equals(taiKhoan.Email)).ToList();
                if (TK == null && TK2.Count == 0)
                {
                    taiKhoan.HinhAnh = "/Content/image/imageaccount/d.jpg";
                    taiKhoan.ChucVu = "SinhVien";
                    staticregisteruser = taiKhoan;
                    Random r = new Random();
                    staticmaxacnhan = r.Next(100000, 999999).ToString();
                    DOANTOTNGHIEP.Models.mail.mail.SendEmail(taiKhoan.Email, "ma xac nhan", staticmaxacnhan); 
                    return View("ConfirmEmail");

                }
                else if (TK != null)
                {
                    ModelState.AddModelError("", "Tên đăng nhập đã tồn tại ");
                }
                else if (TK2.Count != 0)
                {
                    ModelState.AddModelError("", "Email đã tồn tại");
                }

            }
            else ModelState.AddModelError("", "Mật khẩu xác nhận không đúng ");



            return View("Register");
        }

        [HttpPost]
        public ActionResult Checkaccount(TaiKhoan taikhoan, HttpPostedFileBase file)
        {
            DB db = new DB();
            var checkcookie = checkCookie();
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }


            var user = checkcookie.Item4;
            ViewBag.user = user;
            if (user == null) return RedirectToAction("Login", "Login");

            string nguoitao = user.TenDangNhap;
            if (taikhoan.Ho != null && taikhoan.Ten != null)
            {

                var tk = db.TaiKhoans.SingleOrDefault(x => x.TenDangNhap.Equals(nguoitao));

                taikhoan.TenDangNhap = tk.TenDangNhap;
                taikhoan.Email = tk.Email;
                tk.Ho = taikhoan.Ho;
                tk.Ten = taikhoan.Ten;
                if (file != null)
                {

                    string path = Server.MapPath("~/Content/image/imageaccount/" + nguoitao + file.FileName);

                    tk.HinhAnh = "/Content/image/imageaccount/" + nguoitao + file.FileName;



                    file.SaveAs(path);
                }
                taikhoan.HinhAnh = tk.HinhAnh;
                db.SaveChanges();
                ModelState.AddModelError("Erroreditaccount", "chinh sua thanh cong");
                return View("Editaccount", taikhoan);
            }
            else
            {
                ModelState.AddModelError("Erroreditaccount", "thong tin con trong vui long nhap lai");
            }
            return View("Editaccount", taikhoan);



        }
        public ActionResult EditPass()
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
        [HttpPost]
        public ActionResult CheckPass()
        {
            DB db = new DB();
            var checkcookie = checkCookie();
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            ViewBag.user = user;
            var pass1 = Request.Form["pass1"];
            var pass2 = Request.Form["pass2"];
            var pass3 = Request.Form["pass3"];
            
            string nguoitao = user.TenDangNhap;
            string pass = Models.crypt.Encrypt.Decryptuser(user.MatKhau);
            if (pass1.Equals(pass))
            {
                if (pass2 == pass3)
                {
                    var tk = db.TaiKhoans.SingleOrDefault(x => x.TenDangNhap.Equals(nguoitao));
                    tk.MatKhau = Models.crypt.Encrypt.encryptuser(pass2);
                    Session["pass"] = Models.crypt.Encrypt.encryptuser(pass2);
                    db.SaveChanges();
                    
                    return  RedirectToAction("logout", "Login");
                }
                else
                {
                    ModelState.AddModelError("Erroreditpass", "xác nhận mật khẩu không khớp");
                }
            }
            else
            {
                ModelState.AddModelError("Erroreditpass", "mat khau khong chinh xac");
            }
            return View("EditPass");



        }
    }

}