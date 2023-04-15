using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOANTOTNGHIEP.Models;
using System.Data;

namespace DOANTOTNGHIEP.Controllers
{
    public class ClassController : Controller
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
        public (bool, string , string ,DOANTOTNGHIEP.Models.TaiKhoan) checkCookie()
        {
            
            var user = checkcookieuser();
            if (user == null)
            { 
                return (true,"Login", "Login", user);
            }
            else if ( user.ChucVu.ToLower().Contains("Admin".ToLower()))
            {
                return (true, "Index", "Admin", user);
            }
            return (false, "", "", user);
        }

        // GET: Class
        //hien thong báo 
        [HttpPost]
        //tham gia lop hoc
        public ActionResult checkclass()
        {
            var checkcookie = checkCookie();
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            ViewBag.user = user;
            string nguoitao = user.TenDangNhap;
            string id = Request.Form["maclassid"];
            string ma = Models.crypt.Encrypt.Decryptclass(id).ToString();


            if (ModelState.IsValid)
            {
                DB db = new DB();
                var TK = db.LopHocs.SingleOrDefault(x => x.ID.ToString().Equals(ma));
                if (TK != null)
                {
                    if (db.ThanhVienLops.SingleOrDefault(x => x.ID.ToString().Equals(ma) && x.Mathanhvien.Equals(nguoitao)) != null)
                    {
                        ModelState.AddModelError("ErrorJoinClass", "Lop đã tham gia ");
                        return View("JoinClass");
                    }
                    var chucvu = "SinhVien";
                    var tvl =  DOANTOTNGHIEP.Models.database.lophoc.AddMember(ma, nguoitao , chucvu);

                    
                    return RedirectToAction("Index", "TrangChu");
                }
                else
                {
                    ModelState.AddModelError("ErrorJoinClass", "Lop khong ton  tai ");
                }


            }
            return View("JoinClass");
        }
        [HttpPost]
        //tao lop hoc
        public ActionResult createclass()
        {
            var checkcookie = checkCookie();
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            ViewBag.user = user;
            string nguoitao = user.TenDangNhap;
            string name = Request.Form["classname"];
            string infor = Request.Form["inforclass"];


            DB db = new DB();

            if (name != null && infor != null)
            {
                string chucvu = "GiaoVien";
                var lp = DOANTOTNGHIEP.Models.database.lophoc.AddClass(name, infor,  nguoitao);
                var tvl = DOANTOTNGHIEP.Models.database.lophoc.AddMember( Convert.ToString(lp.ID), nguoitao, chucvu);
                return RedirectToAction("Index", "TrangChu");
            }

            return RedirectToAction("Index", "TrangChu");
        }
        //chinh sua thoong tin lop hoc
        [HttpGet]
        public ActionResult Editclass(string malop)
        {
            ViewBag.malop = malop;
            DB db = new DB();
            var checkcookie = checkCookie();
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            ViewBag.user = user;
            string nguoitao = user.TenDangNhap;
            if (malop == null) return RedirectToAction("Index", "TrangChu");
            if (db.ThanhVienLops.SingleOrDefault(x => x.ChucVu.ToLower().Equals("GiaoVien".ToLower()) && x.Mathanhvien.Equals(nguoitao)) == null)
            {
                return RedirectToAction("Index", "TrangChu");
            }
            var lophoc = db.LopHocs.SingleOrDefault(x => x.ID.ToString().Equals(malop));
            return View(lophoc);
        }
        // kiem tra chinh sua thong tin lop hoc
        [HttpPost]
        public ActionResult Checkeditclass(LopHoc s, string malop, HttpPostedFileBase file)
        {
            ViewBag.malop = malop;
            DB db = new DB();
            var checkcookie = checkCookie();
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            ViewBag.user = user;
            string nguoitao = user.TenDangNhap;
            if (malop == null)
            {
                return RedirectToAction("Index", "TrangChu");

            }
            var thanhvien = db.ThanhVienLops.SingleOrDefault(x => x.ChucVu.ToLower().Equals("GiaoVien".ToLower()) && x.Mathanhvien.Equals(nguoitao));
           
                var lop = db.LopHocs.SingleOrDefault(x => x.ID.ToString().Equals(malop) );
            if (lop == null && thanhvien == null) return RedirectToAction("Index", "Notification", new { id = malop });
            lop.TenLop = s.TenLop;
            lop.ThongTinLopHoc = s.ThongTinLopHoc;
            if (file != null)
            {

                string path = Server.MapPath("~/Content/image/imageclass/" + lop.ID + file.FileName);

                lop.Hinhanh = "/Content/image/imageclass/" + lop.ID + file.FileName;

                file.SaveAs(path);
            }
            db.SaveChanges();

            return RedirectToAction("Index", "Notification", new { id = malop });

        }
        //bagn thong tin tao lop
        public ActionResult AddClass()
        {

            var checkcookie = checkCookie();
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            ViewBag.user = user;
            if (user.ChucVu.ToLower().Contains("SinhVien".ToLower()))
            {
                return RedirectToAction("Index", "TrangChu");
            }
            return View();
        }
        //bang thong tin tham gia lop
        public ActionResult JoinClass()
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
       

    }

}