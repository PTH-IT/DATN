using DOANTOTNGHIEP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DOANTOTNGHIEP.Controllers
{
    public class ScoreController : Controller
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
                return (true, "Index", "Admin", user , thanhvienlop);
            }
            else if (thanhvienlop == null)
            {
                return (true, "Index", "TrangChu", user, thanhvienlop);
            }
            return (false, "", "", user , thanhvienlop);
        }



        // GET: Score
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

            string nguoitao = user.TenDangNhap;
            var diem = DOANTOTNGHIEP.Models.GetData.Getdiem.danhsachdiem(id, nguoitao);
            ViewBag.excel = DOANTOTNGHIEP.Models.exportfile.exportfile.exceldsdiem(diem, id);
            ViewBag.pdf = DOANTOTNGHIEP.Models.exportfile.exportfile.pdfdsdiem(diem, id);

            return View(diem);

        }
        public ActionResult Infordiem(string tendangnhap, string malop)
        {
            ViewBag.malop = malop;
            DB db = new DB();
            ViewBag.lophoc = db.LopHocs.SingleOrDefault(x => x.ID.ToString().Equals(malop));
            var checkcookie = checkCookie(malop);   
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            ViewBag.user = user;
            if (malop == null || tendangnhap == null) return RedirectToAction("Index", "TrangChu");

           var baitap = db.BaiTaps.Where(x => x.MaLop.ToString().Equals(malop)).ToList();
            var Listbaitaptuluan = new List<DOANTOTNGHIEP.Models.Modelcreate.BaiTap>();
            var Listbaitaptracnghiem = new List<DOANTOTNGHIEP.Models.Modelcreate.BaiTap>();
            var taikhoan = db.TaiKhoans.SingleOrDefault(x => x.TenDangNhap.Equals(tendangnhap));
            foreach (var i in baitap)
            {
                
                if (i.LoaiBaiTap.Equals("TracNghiem"))
                {
                    var baitaptracnghiem = new DOANTOTNGHIEP.Models.Modelcreate.BaiTap();
                    baitaptracnghiem.TaiKhoan = taikhoan;
                    baitaptracnghiem.BaiTaps = i;
                    var diemtn = db.BaiTapTNs.SingleOrDefault(x => x.MaBaiTap.ToString().Equals(i.ID.ToString()) && x.NguoiNop.Equals(tendangnhap));
                    if (diemtn != null)
                    {
                        baitaptracnghiem.NgayNop = diemtn.NgayNop;
                        baitaptracnghiem.Diem = diemtn.Diem;
                        
                    }
                    Listbaitaptracnghiem.Add(baitaptracnghiem);
                }
                else
                {
                    var baitaptuluan = new DOANTOTNGHIEP.Models.Modelcreate.BaiTap();
                    baitaptuluan.TaiKhoan = taikhoan;
                    baitaptuluan.BaiTaps = i;
                    var diemtn = db.BaiTapTLs.SingleOrDefault(x => x.MaBaiTap.ToString().Equals(i.ID.ToString()) && x.NguoiNop.Equals(tendangnhap));
                    if (diemtn != null)
                    {
                        baitaptuluan.NgayNop = diemtn.NgayNop;
                        baitaptuluan.Diem = diemtn.Diem;

                    }
                    Listbaitaptuluan.Add(baitaptuluan);
                }
            }

            ViewData["baitracnghiem"] = Listbaitaptracnghiem;
            ViewData["baituluan"] = Listbaitaptuluan;
            return View();
        }


    }
}