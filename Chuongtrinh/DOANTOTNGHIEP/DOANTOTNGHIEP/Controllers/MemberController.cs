using DOANTOTNGHIEP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DOANTOTNGHIEP.Controllers
{
    public class MemberController : Controller
    {
        public (DOANTOTNGHIEP.Models.TaiKhoan, DOANTOTNGHIEP.Models.ThanhVienLop) checkcookieuser(string malop)
        {
            DB db = new DB();
            var user = Request.Cookies["user"];
            if (user != null && user["TenDangNhap"].ToString().Length > 0 && user["Matkhau"].ToString().Length > 0)
            {
                var tendangnhap = Models.crypt.Encrypt.Decryptuser(user["TenDangNhap"].ToString());
                var matkhau = Models.crypt.Encrypt.Decryptuser(user["Matkhau"].ToString());
                var thanhvienlop = db.ThanhVienLops.SingleOrDefault(x => x.ID.ToString().Equals(malop) && x.Mathanhvien.Equals(tendangnhap));
                var TK = db.TaiKhoans.SingleOrDefault(x => x.TenDangNhap.Equals(tendangnhap) && x.MatKhau.Equals(matkhau));
                return (TK, thanhvienlop);

            }
            return (null, null);
        }
        public (bool, string, string, DOANTOTNGHIEP.Models.TaiKhoan, DOANTOTNGHIEP.Models.ThanhVienLop) checkCookie(string malop)
        {
            if (malop == null)
            {
                return (true, "Index", "TrangChu", null, null);
            }
            var cookie = checkcookieuser(malop);
            var user = cookie.Item1;
            var thanhvienlop = cookie.Item2;
            if (user == null)
            {
                return (true, "Login", "Login", user, thanhvienlop);
            }
            else if (user.ChucVu.ToLower().Contains("Admin".ToLower()))
            {
                return (true, "Index", "Admin", user, thanhvienlop);
            }
            else if (thanhvienlop == null)
            {
                return (true, "Index", "TrangChu", user, thanhvienlop);
            }
            return (false, "", "", user, thanhvienlop);
        }


        // GET: Member

        public ActionResult Index(string id)
        {
            ViewBag.malop = id;
            DB db = new DB();
            var checkcookie = checkCookie(id);
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            ViewBag.user = user;
            if (id == null)
            {
                return RedirectToAction("Index", "TrangChu");
            }


            var lop = db.ThanhVienLops.SingleOrDefault(x => x.ID.ToString().Equals(id) && x.Mathanhvien.Equals(user.TenDangNhap));
            if (lop == null)
            {
                return RedirectToAction("Index", "TrangChu");
            }
            var thanhvien = db.ThanhVienLops.Where(x => x.ID.ToString().Equals(id)).OrderBy(x => x.TaiKhoan.Ten).ToList();

            return View(thanhvien);
        }
        [HttpPost]
        public System.Web.Mvc.JsonResult inviteclass(string email, string malop)
        {
            ViewBag.malop = malop;
            DB db = new DB();
            var checkcookie = checkCookie(malop);
            if (checkcookie.Item1)
            {
                RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            ViewBag.user = user;
            var ema = email;

            var tk = db.TaiKhoans.SingleOrDefault(x => x.Email.Equals(email));

            if (tk != null)
            {
                var thanhvienlop = db.ThanhVienLops.SingleOrDefault(x => x.Mathanhvien.Equals(tk.TenDangNhap) && x.ID.ToString().Equals(malop));
                if (thanhvienlop == null)
                {

                    var lm = db.Loimois.SingleOrDefault(x => x.MaLop.ToString().Equals(malop) && x.TenDangNhap.Equals(tk.TenDangNhap));
                    if (lm == null)
                    {
                        Loimoi loimoi = new Loimoi();
                        loimoi.MaLop = long.Parse(malop);
                        loimoi.TenDangNhap = tk.TenDangNhap;
                        db.Loimois.Add(loimoi);
                        db.SaveChanges();
                        return Json("<label class='result' >Đã  gửi lời mời .</label>");
                    }
                    else return Json("<label style='color: red' class='result' >Đã gửi trước đó.</label>");



                }
                else
                {
                    return Json("<label style='color: red' class='result' >tài khoản đã tham gia lớp học trước đó.</label>");

                }
            }
            else
            {
                return Json("<label style='color: red' class='result'>Tài khoản không tồn tại</label>");
            }


           

        }


    }
}