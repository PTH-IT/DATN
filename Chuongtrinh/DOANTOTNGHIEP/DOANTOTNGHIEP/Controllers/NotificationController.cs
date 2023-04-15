using DOANTOTNGHIEP.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DOANTOTNGHIEP.Controllers
{
    public class NotificationController : Controller
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
                return (TK , thanhvienlop);

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
                return (true, "Login", "Login", user , thanhvienlop);
            }
            else if (user.ChucVu.ToLower().Contains("Admin".ToLower()))
            {
                return (true, "Index", "Admin", user , thanhvienlop);
            }else if(thanhvienlop == null)
            {
                return (true, "Index", "TrangChu", user, thanhvienlop);
            }
            return (false, "", "", user , thanhvienlop);
        }
        // GET: Notification
        public ActionResult Index(string id)
        {
            ViewBag.malop = id;
            DB db = new DB();
            ViewBag.lophoc = db.LopHocs.SingleOrDefault(x => x.ID.ToString().Equals(id));
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
           
            var thongbao = db.ThongBaos.Where(x => x.MaLop.ToString().Equals(id)).OrderByDescending(y => y.NgayDang).ToList();


            return View(thongbao);
        }

        //dang thong bao
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult DangTB(string malop, HttpPostedFileBase[] file)
        {
            ViewBag.malop = malop;
            DB db = new DB();
            var checkcookie = checkCookie(malop);
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            ViewBag.user = user;
           
            string nguoitao = user.TenDangNhap;
            string noidung = Request.Unvalidated.Form["noidung"].ToString();
            ThongBao tb = new ThongBao();
            tb.Thongtin = noidung;
            tb.NgayDang = DateTime.Now;
            tb.NguoiDang = nguoitao;
            tb.MaLop = long.Parse(malop);
            db.ThongBaos.Add(tb);
            db.SaveChanges();

            foreach (var fil in file)
            {
                if (fil == null)
                {
                    break;
                }
                var fileName = Path.GetFileNameWithoutExtension(fil.FileName);
                var Extension = Path.GetExtension(fil.FileName);
                var imageName = fileName + DateTime.Now.ToString("yyyyMMddHHmmss");
                var imageSavePath = Server.MapPath("~/Content/document/" + Models.crypt.Encrypt.encryptfoder(malop).Replace("+", "").Replace("=", "").Replace("-", "").Replace("_", "") + "/" + Models.crypt.Encrypt.encryptfoder(user.TenDangNhap).Replace("+", "").Replace("=", "").Replace("-", "").Replace("_", "") + "/") + imageName + Extension;
                fil.SaveAs(imageSavePath);
                Library library = new Library();

                library.Name = fil.FileName;
                library.Location = "/Content/document/" + Models.crypt.Encrypt.encryptfoder(malop).Replace("+", "").Replace("=", "").Replace("-", "").Replace("_", "") + "/" + Models.crypt.Encrypt.encryptfoder(user.TenDangNhap).Replace("+", "").Replace("=", "").Replace("-", "").Replace("_", "") + "/" + imageName + Extension;
                library.NgayThem = DateTime.Now;
                library.NguoiAdd = user.TenDangNhap;
                library.NgayUpdate = library.NgayThem;
                library.Noidung = DOANTOTNGHIEP.Models.exportfile.exportfile.getdatapdf(library.Location);
                db.Libraries.Add(library);

                FileTB ftb = new FileTB();
                ftb.Mathongbao = tb.ID;
                ftb.IDLibrary = library.ID;
                db.FileTBs.Add(ftb);
                db.SaveChanges();
               
                   

            }
            var lophoc = db.LopHocs.SingleOrDefault(x => x.ID.ToString().Equals(malop));
            var thanhvien = db.ThanhVienLops.Where(x => x.ID.ToString().Equals(tb.MaLop.ToString()) && !x.Mathanhvien.Equals(nguoitao)).ToList();
            foreach (var tv in thanhvien)
            {
                DOANTOTNGHIEP.Models.mail.mail.SendEmail(tv.TaiKhoan.Email, "Thông báo mới trong Lớp :" + lophoc.TenLop + "  của giáo viên " + lophoc.TaiKhoan.Ho + " " + lophoc.TaiKhoan.Ten, tb.Thongtin);
            }

            var thongbao = db.ThongBaos.Where(x => x.MaLop.ToString().Equals(malop)).OrderByDescending(y => y.NgayDang).ToList();

            return RedirectToAction("Index", "Notification", new { id = malop });
        }

        //xong

        //chinh sua thong bao
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditThongBao(string id, string malop, HttpPostedFileBase[] file)
        {
            ViewBag.malop = malop;
            DB db = new DB();
            var checkcookie = checkCookie(malop);
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            ViewBag.user = user;
            if (malop == null) return RedirectToAction("Index", "TrangChu");

            string noidung = Request.Unvalidated.Form["suanoidungthongbao"];

            ThongBao tb = db.ThongBaos.SingleOrDefault(x => x.ID.ToString().Equals(id) && x.NguoiDang.Equals(user.TenDangNhap));
            if (tb == null)
            {
                return RedirectToAction("Index", "Notification", new { id = malop });
            }
            tb.Thongtin = noidung;
            tb.NgayDang = DateTime.Now;

            db.SaveChanges();
            foreach (var fil in file)
            {
                if (fil != null)
                {
                    var xoafiletb = db.FileTBs.Where(x => x.Mathongbao.ToString().Equals(id)).ToList();
                    if (xoafiletb != null)
                    {
                        db.FileTBs.RemoveRange(xoafiletb);
                        db.SaveChanges();
                        break;
                    }
                    break;


                }

            }
            foreach (var fil in file)
            {
                if (fil == null)
                {
                    break;
                }

                var fileName = Path.GetFileNameWithoutExtension(fil.FileName);
                var Extension = Path.GetExtension(fil.FileName);
                var imageName = fileName + DateTime.Now.ToString("yyyyMMddHHmmss");
                var imageSavePath = Server.MapPath("~/Content/document/" + Models.crypt.Encrypt.encryptfoder(malop).Replace("+", "").Replace("=", "").Replace("-", "").Replace("_", "") + "/" + Models.crypt.Encrypt.encryptfoder(user.TenDangNhap).Replace("+", "").Replace("=", "").Replace("-", "").Replace("_", "") + "/") + imageName + Extension;
                fil.SaveAs(imageSavePath);
                Library library = new Library();

                library.Name = fil.FileName;
                library.Location = "/Content/document/" + Models.crypt.Encrypt.encryptfoder(malop).Replace("+", "").Replace("=", "").Replace("-", "").Replace("_", "") + "/" + Models.crypt.Encrypt.encryptfoder(user.TenDangNhap).Replace("+", "").Replace("=", "").Replace("-", "").Replace("_", "") + "/" + imageName + Extension;
                library.NgayThem = DateTime.Now;
                library.NguoiAdd = user.TenDangNhap;
                library.NgayUpdate = library.NgayThem;
                library.Noidung = DOANTOTNGHIEP.Models.exportfile.exportfile.getdatapdf(library.Location);
                db.Libraries.Add(library);

                FileTB ftb = new FileTB();
                ftb.Mathongbao = tb.ID;
                ftb.IDLibrary = library.ID;
                db.FileTBs.Add(ftb);
                db.SaveChanges();
                

            }
            var thongbao = db.ThongBaos.Where(x => x.MaLop.ToString().Equals(malop)).OrderByDescending(y => y.NgayDang).ToList();
            ViewData["lophoc"] = db.LopHocs.SingleOrDefault(x => x.ID.ToString().Equals(malop));
            return RedirectToAction("Index", "Notification", new { id = malop });
        }
        // xoa thong bao 
        public ActionResult DeleteThongBao(string id, string malop)
        {
            ViewBag.malop = malop;
            DB db = new DB();
            var checkcookie = checkCookie(malop);
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            ViewBag.user = user;
            string noidung = Request.Form["suanoidungthongbao"];
            ThongBao tb = db.ThongBaos.SingleOrDefault(x => x.ID.ToString().Equals(id) && x.NguoiDang.Equals(user.TenDangNhap));
            if (tb == null)
            {
                return RedirectToAction("Index", "Notification", new { id = malop });
            }
            var ftb = db.FileTBs.Where(x => x.Mathongbao.ToString().Equals(id)).ToList();
            foreach (var f in ftb)
            {
                db.FileTBs.Remove(f);
                db.SaveChanges();
            }

            db.ThongBaos.Remove(tb);
            db.SaveChanges();

            var thongbao = db.ThongBaos.Where(x => x.MaLop.ToString().Equals(malop)).OrderByDescending(y => y.NgayDang).ToList();
            ViewData["lophoc"] = db.LopHocs.SingleOrDefault(x => x.ID.ToString().Equals(malop));
            return RedirectToAction("Index", "Notification", new { id = malop });
        }
        [HttpPost]
        public ActionResult Comment(string malop, string mabaitongbao)
        {
            DB db = new DB();
            var checkcookie = checkCookie(malop);
            var user = checkcookie.Item4;
            ViewBag.user = user;
            ViewBag.malop = malop;
            ViewBag.mabaithongbao = mabaitongbao;
            List<DOANTOTNGHIEP.Models.commentnotification> test = db.commentnotifications.Where(x => x.MaThongbao.ToString().Equals(mabaitongbao)).ToList();


            return PartialView(test);
        }
        [HttpPost]
        public string savebinhluan(string mabaitongbao, string mabinhluan, string noidung ,string  malop)
        {

            DB db = new DB();
            var checkcookie = checkCookie(malop);
            if (checkcookie.Item1)
            {
                RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            if (mabinhluan == null)
            {
                commentnotification comment = new commentnotification();
                comment.MaThongbao = Convert.ToInt64(Convert.ToDouble(mabaitongbao));
                comment.Thoigiandang = DateTime.Now;
                comment.Noidung = noidung;
                comment.Nguoidang = user.TenDangNhap;
               db.commentnotifications.Add(comment);
                db.SaveChanges();
                return comment.ID.ToString();
}
            else
            {
                replycomment comment = new replycomment();
                comment.Macomment = Convert.ToInt64(Convert.ToDouble(mabinhluan));
                comment.Thoigiandang = DateTime.Now;
                comment.Noidung = noidung;
                comment.Nguoidang = user.TenDangNhap;
                db.replycomments.Add(comment);
                db.SaveChanges();
                return "";
            }

        }


        public ActionResult Replycomment(string malop, string macomment)
        {
            DB db = new DB();
            var checkcookie = checkCookie(malop);
            var user = checkcookie.Item4;
            ViewBag.user = user;
            ViewBag.malop = malop;
            ViewBag.mabinhluan = macomment;
            List<DOANTOTNGHIEP.Models.replycomment> test = db.replycomments.Where(x => x.Macomment.ToString().Equals(macomment)).ToList();


            return PartialView(test);
        }
    }
}