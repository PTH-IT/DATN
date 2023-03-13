using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOANTOTNGHIEP.Models;
using DOANTOTNGHIEP.Models.GetData;


namespace DOANTOTNGHIEP.Controllers
{
    public class TrangChuController : Controller
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
        // GET: TrangChu
        DB db = new DB();
        public ActionResult Index()
        {

            var checkcookie = checkCookie();
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            ViewBag.user = user;

            var lophoc = GetClass.GetLopHoc(user.TenDangNhap);
            ViewData["loimoithamgia"] = db.Loimois.Where(x => x.TenDangNhap.Equals(user.TenDangNhap)).ToList();
            return View(lophoc);

        }
        public ActionResult tuchoi(string id)
        {

            var checkcookie = checkCookie();
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            ViewBag.user = user;
            var loimoi = db.Loimois.SingleOrDefault(x => x.MaLop.ToString().Equals(id) && x.TenDangNhap.Equals(user.TenDangNhap));
            if (loimoi != null)
            {
                db.Loimois.Remove(loimoi);
                db.SaveChanges();

            }
            return RedirectToAction("Index", "TrangChu");
        }

        public ActionResult thamgia(string id)
        {
            var checkcookie = checkCookie();
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            ViewBag.user = user;
            var loimoi = db.Loimois.SingleOrDefault(x => x.MaLop.ToString().Equals(id) && x.TenDangNhap.Equals(user.TenDangNhap));
            if (loimoi != null)
            {


                ThanhVienLop tvl = new ThanhVienLop();
                tvl.MaLop = Convert.ToInt32(id);
                tvl.Mathanhvien = user.TenDangNhap;
                tvl.NgayThamGia = DateTime.Now;
                tvl.ChucVu = "SinhVien";
                db.ThanhVienLops.Add(tvl);
                db.SaveChanges();
               
                db.Loimois.Remove(loimoi);
                db.SaveChanges();
                return RedirectToAction("Index", "TrangChu");


            }
            return RedirectToAction("Index", "TrangChu");

        }


        public ActionResult menu()
        {

            var checkcookie = checkCookie();
            var user = checkcookie.Item4;
            var lophoc = GetClass.GetLopHoc(user.TenDangNhap);

            return PartialView("menu", lophoc);
        }




    }

}