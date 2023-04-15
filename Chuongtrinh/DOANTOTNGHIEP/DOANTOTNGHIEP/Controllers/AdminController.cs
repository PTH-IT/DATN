using DOANTOTNGHIEP.Models;
using DOANTOTNGHIEP.Models.GetData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DOANTOTNGHIEP.Controllers
{
    public class AdminController : Controller
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
        public (bool, string, string, DOANTOTNGHIEP.Models.TaiKhoan) checkCookie()
        {

            var user = checkcookieuser();
            if (user == null)
            {
                return (true, "Login", "Login", user);
            }
            else if (!user.ChucVu.ToLower().Contains("Admin".ToLower()))
            {
                return (true, "Index", "TrangChu", user);
            }
            return (false, "", "", user);
        }
        // GET: Admin
        public ActionResult Index()
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
        public ActionResult menu()
        {
            var checkcookie = checkCookie();
            var user = checkcookie.Item4;
            ViewBag.user = user;
            var lophoc = GetClass.GetLopHoc(user.TenDangNhap);

            return PartialView("menu", lophoc);
        }
        public ActionResult danhsachlop()
        {

            var checkcookie = checkCookie();
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            ViewBag.user = user;
            DB db = new DB();
            var lophoc = db.LopHocs.Select(x => x).OrderByDescending(x => x.NgayTao).ToList();

            return View(lophoc);
        }
        [HttpGet]
        public ActionResult danhsachtaikhoan()
        {
            var checkcookie = checkCookie();
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            ViewBag.user = user;
            DB db = new DB();
            var taikhoan = db.TaiKhoans.Where(x => !x.ChucVu.ToLower().Contains("Admin".ToLower())).ToList();
            return View(taikhoan);
        }
        public ActionResult chitietlophoc(string id)
        {
            if (id == null) return RedirectToAction("Index", "Admin");
            var checkcookie = checkCookie();
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            ViewBag.user = user;
            DB db = new DB();
            var thanhvienlop = db.LopHocs.SingleOrDefault(x => x.ID.ToString().Equals(id)).ThanhVienLops.ToList();
            ViewBag.malop = id;
            return View(thanhvienlop);
        }
        [HttpPost]
        public JsonResult editpermisionClass(string id, string malop, string chucvu)
        {
            var result = new DOANTOTNGHIEP.Modelcreate.JsonResult();
            var checkcookie = checkCookie();
            if (checkcookie.Item1)
            {
                RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            ViewBag.user = user;
            DB db = new DB();
            var thanhvienlop = db.ThanhVienLops.SingleOrDefault(x => x.Mathanhvien.Equals(id) && x.ID.ToString().Equals(malop));
            thanhvienlop.ChucVu = chucvu;
            db.SaveChanges();
            result.result = true;
            result.value = "Chinh sửa thành công";

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult AddAccountClass(string email, string malop, string chucvu)
        {
            var checkcookie = checkCookie();
            if (checkcookie.Item1)
            {
                RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            ViewBag.user = user;
            var result = new DOANTOTNGHIEP.Modelcreate.JsonResult();

            DB db = new DB();
            var taikhoan = db.TaiKhoans.SingleOrDefault(x => x.Email.Equals(email));

            if (taikhoan == null)
            {

                result.result = false;
                result.value = "Tài khoản không tồn tại ";

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else if (taikhoan.ChucVu.ToLower().Contains("Admin".ToLower()))
            {
                result.result = false;
                result.value = "Tài khoản là ADMIN nên không thể tham gia lớp học ";

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {

                var TK = db.LopHocs.SingleOrDefault(x => x.ID.ToString().Equals(malop));
                if (TK != null)
                {
                    if (db.ThanhVienLops.SingleOrDefault(x => x.ID.ToString().Equals(malop) && x.Mathanhvien.Equals(taikhoan.TenDangNhap)) != null)
                    {

                        result.result = false;
                        result.value = "Tài khoản đã tham gia lớp học";

                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    var tvl = DOANTOTNGHIEP.Models.database.lophoc.AddMember(malop, taikhoan.TenDangNhap, chucvu);
                    var baitap = db.BaiTaps.Where(x => x.MaLop.ToString().Equals(malop)).ToList();

                }

                result.result = true;
                result.value = "Thêm thành công ";

                return Json(result, JsonRequestBehavior.AllowGet);

            }

        }

        [HttpPost]
        public JsonResult editpermisionAccount(string id, string chucvu)
        {
            var result = new DOANTOTNGHIEP.Modelcreate.JsonResult();
            var checkcookie = checkCookie();
            if (checkcookie.Item1)
            {
                RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            ViewBag.user = user;
            DB db = new DB();
            var taikhoan = db.TaiKhoans.SingleOrDefault(x => x.TenDangNhap.Equals(id));
            taikhoan.ChucVu = chucvu;
            db.SaveChanges();

            result.result = true;
            result.value = "Chinh sửa thành công";

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult AddAccount(string ho, string ten, string email, string tendangnhap, string pass, string chucvu)
        {
            var checkcookie = checkCookie();
            if (checkcookie.Item1)
            {
                RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            ViewBag.user = user;
            var result = new List<DOANTOTNGHIEP.Modelcreate.JsonResult>();
            DB db = new DB();
            var taikhoan = db.TaiKhoans.SingleOrDefault(x => x.Email.ToLower().Contains(email.ToLower()) || x.TenDangNhap.Equals(tendangnhap));
            if (taikhoan == null)
            {
                var tk = DOANTOTNGHIEP.Models.database.taikhoan.AddAccount(ho, ten, email, tendangnhap, pass, chucvu);
                var result1 = new DOANTOTNGHIEP.Modelcreate.JsonResult();
                result1.result = true;
                result1.value = "Thêm tài khoản thành công";
                result.Add(result1);

            }
            else
            {
                if (taikhoan.Email.ToLower().Contains(email.ToLower()))
                {
                    var result1 = new DOANTOTNGHIEP.Modelcreate.JsonResult();
                    result1.result = false;
                    result1.value = "Email đã tồn tại";
                    result.Add(result1);
                }
                else
                {
                    var result1 = new DOANTOTNGHIEP.Modelcreate.JsonResult();
                    result1.result = true;
                    result1.value = "";
                    result.Add(result1);

                }
                if (taikhoan.TenDangNhap.Equals(tendangnhap))
                {
                    var result1 = new DOANTOTNGHIEP.Modelcreate.JsonResult();
                    result1.result = false;
                    result1.value = "Tên đăng nhập đã tồn tại";
                    result.Add(result1);
                }
                else
                {
                    var result1 = new DOANTOTNGHIEP.Modelcreate.JsonResult();
                    result1.result = true;
                    result1.value = "";
                    result.Add(result1);

                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult danhsachchudetailieu()
        {

            var checkcookie = checkCookie();
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            ViewBag.user = user;
            DB db = new DB();
            var chudetailieu = db.Chudetailieux.Select(x => x).OrderByDescending(x => x.Chude).ToList();

            return View(chudetailieu);
        }
        public ActionResult chitietchudetailieulophoc(string id)
        {
            if (id == null) return RedirectToAction("Index", "Admin");
            var checkcookie = checkCookie();
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            ViewBag.user = user;
            DB db = new DB();
            var chitietchude = db.KeywordTailieux.Where(x => x.Machude.ToString().Equals(id)).ToList();
            ViewBag.machude = id;
            return View(chitietchude);
        }
        [HttpPost]
        public JsonResult AddChudeTailieu(string tenchude)
        {
            var checkcookie = checkCookie();
            if (checkcookie.Item1)
            {
                RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            ViewBag.user = user;
            var result = new DOANTOTNGHIEP.Modelcreate.JsonResult();

            DB db = new DB();
            var tailieu = db.Chudetailieux.SingleOrDefault(x => x.Chude.Replace(" ","").ToLower().Equals(tenchude.Replace(" ", "").ToLower()));

            if (tailieu != null)
            {

                result.result = false;
                result.value = "Chủ đề đã tồn tại vui lòng thêm chủ đề khác";

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {

                Chudetailieu chude = new Chudetailieu();
                chude.Chude = tenchude;
                db.Chudetailieux.Add(chude);
                db.SaveChanges();

                result.result = true;
                result.value = "Thêm thành công ";

                return Json(result, JsonRequestBehavior.AllowGet);

            }

        }



        [HttpPost]
        public JsonResult AddKeywordTailieu(string keyword , string machude)
        {
            var checkcookie = checkCookie();
            if (checkcookie.Item1)
            {
                RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            ViewBag.user = user;
            var result = new DOANTOTNGHIEP.Modelcreate.JsonResult();

            DB db = new DB();
            var keywordTailieu = db.KeywordTailieux.SingleOrDefault(x => x.Machude.ToString().Equals(machude) && x.Keyword.Replace(" ", "").ToLower().Equals(keyword.Replace(" ", "").ToLower()));

            if (keywordTailieu != null)
            {

                result.result = false;
                result.value = "Từ khóa đã tồn tại vui lòng thêm từ khóa khác";

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {

                KeywordTailieu keywords = new KeywordTailieu();
                keywords.Machude = long.Parse(machude);
                keywords.Keyword = keyword;
                db.KeywordTailieux.Add(keywords);
                db.SaveChanges();

                result.result = true;
                result.value = "Thêm thành công ";

                return Json(result, JsonRequestBehavior.AllowGet);

            }

        }



    }


}


