using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOANTOTNGHIEP.Models;
using DOANTOTNGHIEP.Modelcreate;

using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using Spire.Doc.Fields.OMath;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using Spire.Pdf.Tables;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Data.Entity.Migrations;

namespace DOANTOTNGHIEP.Controllers
{
    public class ExerciseController : Controller
    {
        public (DOANTOTNGHIEP.Models.TaiKhoan, DOANTOTNGHIEP.Models.ThanhVienLop) checkcookieuser(string malop)
        {
            DB db = new DB();
            var user = Request.Cookies["user"];
            if (user != null && user["TenDangNhap"].ToString().Length > 0 && user["Matkhau"].ToString().Length > 0)
            {
                var tendangnhap = Models.crypt.Encrypt.Decryptuser(user["TenDangNhap"].ToString());
                var matkhau = Models.crypt.Encrypt.Decryptuser(user["Matkhau"].ToString());
                var thanhvienlop = db.ThanhVienLops.SingleOrDefault(x => x.MaLop.ToString().Equals(malop) && x.Mathanhvien.Equals(tendangnhap));
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

        // GET: Exercise
        public ActionResult Index(string id)
        {

            DB db = new DB();
            ViewBag.lophoc = db.LopHocs.SingleOrDefault(x => x.MaLop.ToString().Equals(id));

            var checkcookie = checkCookie(id);
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            var thanhvien = checkcookie.Item5;
            var baitap = db.BaiTaps.Where(x => x.MaLop.ToString().Equals(id)).OrderByDescending(x => x.ThoiGianDang).ToList();
            ViewBag.malop = id;
            ViewBag.user = user;
            ViewBag.chucvu = thanhvien.ChucVu;
            return View(baitap);
        }
        // xoa bai tap 
        public ActionResult deletebaitap(string mabaitap, string malop)
        {

            DB db = new DB();
            var checkcookie = checkCookie(malop);
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            var thanhvien = checkcookie.Item5;
            if (!thanhvien.ChucVu.ToLower().Equals("giaovien".ToLower()))
            {
                return RedirectToAction("Index", "Exercise", new { id = malop });
            }
            var bt = db.BaiTaps.SingleOrDefault(c => c.MaBaiTap.ToString().Equals(mabaitap));
            if (bt != null)
            {
                if (bt.LoaiBaiTap.Equals("TracNghiem"))
                {


                    var tn = db.BaiTapTNs.Where(c => c.MaBaiTap.ToString().Equals(mabaitap)).ToList();
                    foreach (var bai in tn)
                    {
                        string mabai = bai.MaBaiNop.ToString();
                        var ttbai = db.TTBaiTapTNs.Where(x => x.MaBaiNop.ToString().Equals(mabai)).ToList();
                        db.TTBaiTapTNs.RemoveRange(ttbai);
                        db.SaveChanges();
                        db.BaiTapTNs.Remove(bai);
                        db.SaveChanges();
                    }
                    var thongbaobaitap = db.ThongBaos.SingleOrDefault(x => x.MaBaiTap.ToString().Equals(mabaitap));
                    db.ThongBaos.Remove(thongbaobaitap);
                    var cauhoi = db.CauHois.Where(x => x.MaBaiTap.ToString().Equals(mabaitap)).ToList();
                    foreach (var i in cauhoi)
                    {
                        db.DapAns.RemoveRange(i.DapAns.ToList());
                        db.SaveChanges();
                        db.CauHois.Remove(i);
                        db.SaveChanges();
                    }
                    db.BaiTaps.Remove(bt);
                    db.SaveChanges();

                }
                else if (bt.LoaiBaiTap.Equals("TuLuan"))
                {

                    var tn = db.BaiTapTLs.Where(c => c.MaBaiTap.ToString().Equals(mabaitap)).ToList();
                    var filebt = db.FileBTTLs.Where(x => x.MaBT.ToString().Equals(mabaitap)).ToList();
                    foreach(var item in filebt)
                    {
                        db.Libraries.Remove(item.Library);
                        db.SaveChanges();
                        db.FileBTTLs.Remove(item);
                        db.SaveChanges();
                    }

                    foreach (var bai in tn)
                    {
                        string mabai = bai.MaBaiNop.ToString();
                        var ttbai = db.TTBaiTapTLs.Where(x => x.MaBaiNop.ToString().Equals(mabai)).ToList();
                        db.TTBaiTapTLs.RemoveRange(ttbai);
                        db.SaveChanges();
                        db.BaiTapTLs.Remove(bai);
                        db.SaveChanges();
                    }
                    var thongbaobaitap = db.ThongBaos.SingleOrDefault(x => x.MaBaiTap.ToString().Equals(mabaitap));
                    db.ThongBaos.Remove(thongbaobaitap);
                    db.BaiTaps.Remove(bt);
                    db.SaveChanges();
                }
            }
            ViewBag.user = user;
            ViewBag.malop = malop;
            return Index(malop);
        }
        // chinh sua thoong tin bai tap
        public ActionResult Editbaitap(string mabaitap, string malop)
        {

            DB db = new DB();
            var checkcookie = checkCookie(malop);
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            var thanhvien = checkcookie.Item5;
            if (!thanhvien.ChucVu.ToLower().Equals("giaovien".ToLower()))
            {
                return RedirectToAction("Index", "Exercise", new { id = malop }); ;
            }
            string chude = Request.Form["Chude"];
            string noidung = Request.Unvalidated.Form["noidungbt"];
            string thoihan = Request.Form["thoigiankethuc"];

            var baitap = db.BaiTaps.SingleOrDefault(x => x.MaBaiTap.ToString().Equals(mabaitap));
            if (!baitap.ChuDe.Equals(chude) || !baitap.Thongtin.Equals(noidung) || !baitap.ThoiGianKetThuc.Equals(Convert.ToDateTime(thoihan)))
            {
                baitap.ChuDe = chude;
                baitap.Thongtin = noidung;
                baitap.ThoiGianKetThuc = Convert.ToDateTime(thoihan);
                baitap.ThoiGianDang = DateTime.Now;
                db.SaveChanges();
                var tb = db.ThongBaos.SingleOrDefault(x => x.MaBaiTap.ToString().Equals(mabaitap));
                tb.Thongtin = "Bài tập mới:" + chude;
                tb.NgayDang = DateTime.Now;
                db.SaveChanges();

            }
            return RedirectToAction("Index", "Exercise", new { id = malop });
        }

        //xong
        // thong tin bai tu luan
        public ActionResult ShowInforTL(string ten, string mabaitap, string malop)
        {

            DB db = new DB();
            var checkcookie = checkCookie(malop);
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            if (ten == null || mabaitap == null)
            {
                return Index(malop);
            }
            var thanhvien = checkcookie.Item5;
            var bt = db.BaiTapTLs.SingleOrDefault(x => x.NguoiNop.Equals(ten) && x.MaBaiTap.ToString().Equals(mabaitap));
            var taikhoan = db.TaiKhoans.SingleOrDefault(x => x.TenDangNhap.Equals(ten));
            var baitap = db.BaiTaps.SingleOrDefault(x => x.MaBaiTap.ToString().Equals(mabaitap));
            var baitaptuluan = new DOANTOTNGHIEP.Models.Modelcreate.BaiTap();
            baitaptuluan.TaiKhoan = taikhoan;
            baitaptuluan.BaiTaps = baitap;
            if (bt != null)
            {
                baitaptuluan.TTBaiTapTls = db.TTBaiTapTLs.Where(x => x.MaBaiNop.ToString().Equals(bt.MaBaiNop.ToString())).ToList();
                baitaptuluan.NgayNop = bt.NgayNop;
                baitaptuluan.Diem = bt.Diem;
                ViewBag.nguoinop = bt.NguoiNop;

            }
            ViewBag.chucvu = thanhvien.ChucVu;
            ViewBag.user = user;
            ViewBag.malop = malop;
            return View("ShowInforBaiTapTL", baitaptuluan);
        }
        //xong
        // thong tin bai trac nghiem
        public ActionResult ShowInforTN(string ten, string mabaitap, string malop)
        {

            DB db = new DB();
            var checkcookie = checkCookie(malop);
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            if (malop == null) return RedirectToAction("Index", "TrangChu");

            if (ten == null || mabaitap == null)
            {
                return RedirectToAction("Index", "Exercise", new { id = malop });
            }
            var bt = db.BaiTapTNs.SingleOrDefault(x => x.NguoiNop.Equals(ten) && x.MaBaiTap.ToString().Equals(mabaitap));
            var taikhoan = db.TaiKhoans.SingleOrDefault(x => x.TenDangNhap.Equals(ten));
            var baitap = db.BaiTaps.SingleOrDefault(x => x.MaBaiTap.ToString().Equals(mabaitap));
            var baitaptracnghiem = new DOANTOTNGHIEP.Models.Modelcreate.BaiTap();
            baitaptracnghiem.TaiKhoan = taikhoan;
            baitaptracnghiem.BaiTaps = baitap;
            if (bt != null)
            {
                baitaptracnghiem.TTBaiTapTns = db.TTBaiTapTNs.Where(x => x.MaBaiNop.ToString().Equals(bt.MaBaiNop.ToString())).ToList();
                baitaptracnghiem.NgayNop = bt.NgayNop;
                baitaptracnghiem.Diem = bt.Diem;
                ViewBag.mabainop = bt.MaBaiNop;
                ViewBag.nguoinop = bt.NguoiNop;
            }

            ViewBag.user = user;
            ViewBag.malop = malop;
            return View("ShowInforBaiTapTn", baitaptracnghiem);
        }
        //xong 
        public ActionResult chambaitl(BaiTapTL baitap, string malop, string mabaitap, string nguoinop)
        {

            DB db = new DB();
            var checkcookie = checkCookie(malop);
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            if (malop == null) return RedirectToAction("Index", "TrangChu");
            string ma = mabaitap;
            string ten = nguoinop;
            var bt = db.BaiTapTLs.SingleOrDefault(x => x.MaBaiTap.ToString().Equals(ma) && x.NguoiNop.Equals(nguoinop));
            bt.Diem = baitap.Diem;
            db.SaveChanges();
            return RedirectToAction("ShowInforTL", new { ten = ten, mabaitap = bt.MaBaiTap, malop = malop });
        }
        static DOANTOTNGHIEP.Models.BaiTap ADDdetracnghiem = new BaiTap();
        static List<DOANTOTNGHIEP.Models.CauHoi> ADDcauhoitracnghiem = new List<CauHoi>();
        // dang bai tap trac nghiem
        [HttpPost]
        public ActionResult DangBaiTapTN(string malop, HttpPostedFileBase file)
        {

            DB db = new DB();
            var checkcookie = checkCookie(malop);
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            var thanhvien = checkcookie.Item5;
            if (!thanhvien.ChucVu.ToLower().Equals("giaovien".ToLower()))
            {
                return RedirectToAction("Index", "Exercise", new { id = malop });
            }
            string nguoitao = user.TenDangNhap;
            string noidung = Request.Unvalidated.Form["noidungbt"];
            string chude = Request.Form["Chude"];
            string thoigianketthuc = Request.Form["thoigiankethuc"].ToString();
            DateTime dt = Convert.ToDateTime(thoigianketthuc);
            BaiTap bt = new BaiTap();
            bt.Thongtin = noidung;
            bt.ThoiGianDang = DateTime.Now;
            bt.ThoiGianKetThuc = dt;
            bt.NguoiTao = nguoitao;
            bt.LoaiBaiTap = "TracNghiem";
            bt.ChuDe = chude;
            bt.MaLop = long.Parse(malop);
            object path = Server.MapPath("~/Content/FileBTTN/" + file.FileName);
            if (System.IO.File.Exists(path.ToString()))
            {
                System.IO.File.Delete(path.ToString());
            }

            file.SaveAs(path.ToString());

            if (docfiletracnghiem(path.ToString(), malop).Count == 0)
            {
                if (System.IO.File.Exists(path.ToString()))
                {
                    System.IO.File.Delete(path.ToString());
                }
                return Index(malop);
            }
            var cauhoi = docfiletracnghiem(path.ToString(), malop);
            ADDdetracnghiem = bt;
            ADDcauhoitracnghiem = cauhoi;
            return RedirectToAction("SaveCauhoitracnghiem", "Exercise", new { malop = malop });
        }
        public ActionResult SaveCauhoitracnghiem(string malop)
        {
            DB db = new DB();
            var checkcookie = checkCookie(malop);
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            var thanhvien = checkcookie.Item5;
            if (!thanhvien.ChucVu.ToLower().Equals("giaovien".ToLower()))
            {
                return RedirectToAction("Index", "Exercise", new { id = malop });
            }
            var baitap = ADDdetracnghiem;
            baitap.LopHoc = db.LopHocs.SingleOrDefault(x => x.MaLop.ToString().Equals(malop));
            ViewBag.user = user;
            ViewBag.malop = malop;
            ViewBag.baitap = baitap;
            return View( ADDcauhoitracnghiem);
        } 
            public List<CauHoi> docfiletracnghiem(string file  , string malop)
        {
            var checkcookie = checkCookie(malop);
            if (checkcookie.Item1)
            {
                RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            List<CauHoi> cauhoi = new List<CauHoi>();


            Document document = new Document(file);

            int sas = 1;
            Section section = document.Sections[0];
            if (section.Tables.Count > 0)
            {
                Table table = section.Tables[0] as Table;
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    CauHoi cau = new CauHoi();
                    for (int j = 0; j < table.Rows[i].Cells.Count; j++)
                    {

                        foreach (Paragraph paragraph in table.Rows[i].Cells[j].Paragraphs)
                        {
                            string noidung = "";
                            DapAn da = new DapAn();
                            //Get Each Document Object of Paragraph Items

                            foreach (DocumentObject docObject in paragraph.ChildObjects)
                            {

                                //If Type of Document Object is Picture, Extract.  
                                if (docObject.DocumentObjectType == DocumentObjectType.Picture)
                                {
                                    String anh = null;
                                    string s = user.TenDangNhap + "-" + sas + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
                                    DocPicture pic = docObject as DocPicture;
                                    String imgName = Server.MapPath("~/Content/image/ImageCauhoiTracnghiem/" + s);
                                    anh = "/Content/image/ImageCauhoiTracnghiem/" + s;
                                    //Save Image  
                                    pic.Image.Save(imgName, System.Drawing.Imaging.ImageFormat.Png);
                                    noidung = noidung + "   <img src='" + anh + "'>  ";
                                    sas++;
                                }
                                else if (docObject.DocumentObjectType == DocumentObjectType.TextRange)
                                {

                                    TextRange nd = docObject as TextRange;

                                    noidung = noidung + nd.Text;




                                }
                                else if (docObject.DocumentObjectType == DocumentObjectType.OfficeMath)
                                {

                                    noidung = noidung + (docObject as OfficeMath).ToMathMLCode().Replace("mml:", "");

                                }




                            }
                            if (j == 0)
                            {
                                cau.NoiDung = noidung;

                            }
                            else if (j != 0)
                            {
                                if (!noidung.ToString().Equals(""))
                                {
                                    da.NoiDung = noidung.ToString();


                                    if (noidung.Substring(0, noidung.ToString().IndexOf("*") + 1).Replace(" ", "").Equals("*"))
                                    {
                                        da.NoiDung = noidung.ToString().Substring(noidung.ToString().IndexOf("*") + 1, noidung.ToString().Length - noidung.ToString().IndexOf("*") - 1);
                                        da.LoaiDapAn = true;
                                    }
                                    else
                                    {

                                        da.LoaiDapAn = false;
                                    }


                                    cau.DapAns.Add(da);
                                }

                            }

                        }

                    }
                    cauhoi.Add(cau);

                }
            }

            return cauhoi;
        }
        //xong 
        // luu cau hoi trac nghiem
        public ActionResult luucauhoitracnghiem(string malop)
        {
            DOANTOTNGHIEP.Models.BaiTap detracnghiem = ADDdetracnghiem;
            List<DOANTOTNGHIEP.Models.CauHoi> cauhoitracnghiem = ADDcauhoitracnghiem;
            if (cauhoitracnghiem.Count == 0) return RedirectToAction("Index", "TrangChu");
           
            DB db = new DB();
            var checkcookie = checkCookie(malop);
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
           
            var bt = detracnghiem;
            bt.LopHoc = null;
            db.BaiTaps.Add(bt);
            db.SaveChanges();
            ThongBao tb = new ThongBao();
            tb.NguoiDang = user.TenDangNhap;
            tb.NgayDang = DateTime.Now;
            tb.MaBaiTap = bt.MaBaiTap;
            tb.MaLop = long.Parse(malop);
            tb.Thongtin = "Bài tập mới:" + bt.ChuDe;
            db.ThongBaos.Add(tb);
            db.SaveChanges();

            var cauhoi = cauhoitracnghiem;
            foreach (var i in cauhoi)
            {
                CauHoi ch = new CauHoi();
                ch.MaBaiTap = bt.MaBaiTap;

                ch.NgayThem = DateTime.Now;
                ch.NoiDung = i.NoiDung;
                db.CauHois.Add(ch);
                db.SaveChanges();
                foreach (var j in i.DapAns.ToList())
                {
                    DapAn dapAn = new DapAn();
                    dapAn.MaCauHoi = ch.MaCauHoi;
                    dapAn.NoiDung = j.NoiDung;

                    dapAn.LoaiDapAn = j.LoaiDapAn;
                    db.DapAns.Add(dapAn);
                    db.SaveChanges();

                }
            }

            var lophoc = db.LopHocs.SingleOrDefault(x => x.MaLop.ToString().Equals(malop));
            var thanhvien = db.ThanhVienLops.Where(x => x.MaLop.ToString().Equals(malop)).ToList();

            foreach (var tv in thanhvien)
            {
                
  
                    DOANTOTNGHIEP.Models.mail.mail.SendEmail(tv.TaiKhoan.Email, "Bài tâptrong Lớp :" + lophoc.TenLop + " của giáo viên " + lophoc.TaiKhoan.Ho + " " + lophoc.TaiKhoan.Ten, "\n.Chủ đề :" + bt.ChuDe + "\n. Nội dung :" + bt.Thongtin);
                

            }
            return RedirectToAction("Index", "Exercise", new { id = malop });
        }
        // hien thi cau hoi tu luan
        public ActionResult showcauhoituluan(string id, string malop)
        {
            
            DB db = new DB();
            var checkcookie = checkCookie(malop);
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            var thanhvien = checkcookie.Item5;
            if (!thanhvien.ChucVu.ToLower().Equals("giaovien".ToLower()))
            {
                return RedirectToAction("Index", "Exercise", new { id = malop });
            }
            var bt = db.BaiTaps.SingleOrDefault(x => x.MaBaiTap.ToString().Equals(id) );
            ViewBag.mabaitaptl = bt.MaBaiTap;
            ViewBag.user = user;
            ViewBag.malop = malop;
            return View(bt);
        }

        //xong
        // chinh sua bai tap nop
        [HttpPost]
        public ActionResult editnopbaitap(string malop, string mabaitaptl, HttpPostedFileBase[] file)
        {
            
            DB db = new DB();
            var checkcookie = checkCookie(malop);
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            var bai = db.TTBaiTapTLs.SingleOrDefault(x => x.MaBaiNop.ToString().Equals(mabaitaptl));
          
                if (bai.Isplagiarism != false)
                {
                    return RedirectToAction("Index", "Exercise", new { id = malop });
                }
           
            foreach (var fil in file)
            {
                if (fil == null)
                {
                    break;
                }
                string fileName;
                string Extension;
                string imageName;
                fileName = Path.GetFileNameWithoutExtension(fil.FileName);
                Extension = Path.GetExtension(fil.FileName);
                imageName = fileName + DateTime.Now.ToString("yyyyMMddHHmmss");
                string path = Server.MapPath("~/Content/BTTL/" + Models.crypt.Encrypt.encryptfoder(malop).Replace("+", "").Replace("=", "").Replace("-", "").Replace("_", "") + "/" + Models.crypt.Encrypt.encryptfoder(user.TenDangNhap).Replace("+", "").Replace("=", "").Replace("-", "").Replace("_", "") + "/" + imageName + Extension);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                fil.SaveAs(path);
                var library = bai.Library;
                library.Name = fil.FileName;
                library.Location = "/Content/BTTL/" + Models.crypt.Encrypt.encryptfoder(malop).Replace("+", "").Replace("=", "").Replace("-", "").Replace("_", "") + "/" + Models.crypt.Encrypt.encryptfoder(user.TenDangNhap).Replace("+", "").Replace("=", "").Replace("-", "").Replace("_", "") + "/" + imageName + Extension;
                library.NgayUpdate = DateTime.Now;
                library.NguoiAdd = user.TenDangNhap;
                library.Noidung = DOANTOTNGHIEP.Models.exportfile.exportfile.getdatapdf(library.Location);
                db.Libraries.AddOrUpdate(library);
                db.SaveChanges();
               
               


            }


            return RedirectToAction("Index", "Exercise", new { id = malop });
        }
       
        //xong
        // nop bai tu luan
        [HttpPost]
        public ActionResult nopbaitapTL(string malop, string mabaitaptl, HttpPostedFileBase[] file)
        {
           
            DB db = new DB();
            var checkcookie = checkCookie(malop);
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            var baitaptuluan = DOANTOTNGHIEP.Models.database.baitap.AddBaitapTL(long.Parse( mabaitaptl), user.TenDangNhap);

            foreach (var fil in file)
            {
                if (fil == null)
                {
                    break;
                }
                string fileName;
                string Extension;
                string imageName;
                fileName = Path.GetFileNameWithoutExtension(fil.FileName);
                Extension = Path.GetExtension(fil.FileName);
                imageName = fileName + DateTime.Now.ToString("yyyyMMddHHmmss");
                string path = Server.MapPath("~/Content/BTTL/" + Models.crypt.Encrypt.encryptfoder(malop).Replace("+", "").Replace("=", "").Replace("-", "").Replace("_", "") + "/" + Models.crypt.Encrypt.encryptfoder(user.TenDangNhap).Replace("+", "").Replace("=", "").Replace("-", "").Replace("_", "") + "/" + imageName + Extension);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                fil.SaveAs(path);
                Library library = new Library();
                library.Name = fil.FileName;
                library.Location = "/Content/BTTL/" + Models.crypt.Encrypt.encryptfoder(malop).Replace("+", "").Replace("=", "").Replace("-", "").Replace("_", "") + "/" + Models.crypt.Encrypt.encryptfoder(user.TenDangNhap).Replace("+", "").Replace("=", "").Replace("-", "").Replace("_", "") + "/" + imageName + Extension;
                library.NgayThem = DateTime.Now;
                library.NguoiAdd = user.TenDangNhap;
                library.NgayUpdate = library.NgayThem;
                library.Noidung = DOANTOTNGHIEP.Models.exportfile.exportfile.getdatapdf(library.Location);
                db.Libraries.Add(library);
                db.SaveChanges();

                TTBaiTapTL bttl = new TTBaiTapTL();
                bttl.MaBaiNop =baitaptuluan.MaBaiNop;
                bttl.NguoiNop = user.TenDangNhap;
                bttl.IDLibrary = library.ID;
                db.TTBaiTapTLs.Add(bttl);
                var bt = db.BaiTapTLs.SingleOrDefault(x => x.MaBaiNop.ToString().Equals(baitaptuluan.MaBaiNop.ToString()));
                bt.NgayNop = DateTime.Now;
                bt.Trangthai = true;
                db.SaveChanges();
            }

            return RedirectToAction("Index", "Exercise", new { id = malop });
        }
        //xong
        // bai bai tap trac nghiem
        public ActionResult LamBaiTN(string id, string malop)
        {
            
            DB db = new DB();
            var checkcookie = checkCookie(malop);
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            var cauhoi = db.CauHois.Where(x => x.MaBaiTap.ToString().Equals(id)).ToList();
            ViewBag.user = user;
            ViewBag.malop = malop;
            ViewBag.mabai = id;
            return View(cauhoi);
        }
        //xong
        // luu cau hhoi khi lam
        public void Luucauhoikhilam(string macauhoi, string madapan, string malop, string mabai)
        {
            
            DB db = new DB();
            var checkcookie = checkCookie(malop);
            if (checkcookie.Item1)
            {
                RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            var bai = db.BaiTapTNs.SingleOrDefault(x => x.MaBaiTap.ToString().Equals(mabai) && x.NguoiNop.Equals(user.TenDangNhap));
            if (bai == null)
            {
                bai = DOANTOTNGHIEP.Models.database.baitap.AddBaitapTN(Convert.ToInt64( mabai), user.TenDangNhap);
            }
            var s = db.TTBaiTapTNs.SingleOrDefault(x => x.MaBaiNop.ToString().Equals(bai.MaBaiNop.ToString()) && x.NguoiNop.Equals(user.TenDangNhap) && x.MaCauHoi.ToString().Equals(macauhoi));
            if (s == null)
            {
                TTBaiTapTN tn = new TTBaiTapTN();
                tn.MaBaiNop = bai.MaBaiNop;
                tn.MaCauHoi = long.Parse(macauhoi);
                tn.MaDapAnluaChon = long.Parse(madapan);
                tn.NguoiNop = user.TenDangNhap;
                db.TTBaiTapTNs.Add(tn);
                db.SaveChanges();
            }
            else
            {
                s.MaCauHoi = long.Parse(macauhoi);
                s.MaDapAnluaChon = long.Parse(madapan);
                db.SaveChanges();

            }





        }
        //xong
        //luu bai thi sau khi lam
        public ActionResult LuuBaiTN(string malop, string mabai)
        {
           
            DB db = new DB();
            var checkcookie = checkCookie(malop);
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            var bai = db.BaiTapTNs.SingleOrDefault(x => x.MaBaiTap.ToString().Equals(mabai) && x.NguoiNop.Equals(user.TenDangNhap));
            var s = db.TTBaiTapTNs.Where(x => x.MaBaiNop.ToString().Equals(bai.MaBaiNop.ToString()) && x.NguoiNop.Equals(user.TenDangNhap)).ToList();
            var cauhoi = db.CauHois.Where(x => x.MaBaiTap.ToString().Equals(mabai)).ToList();
            int slcaudung = 0;
            foreach (var i in s)
            {
                if (i.DapAn.LoaiDapAn.Value)
                {
                    slcaudung++;
                }
            }
            bai.Diem = ((100 / cauhoi.Count) * slcaudung);
            if (slcaudung == cauhoi.Count)
            {
                bai.Diem = 100;
            }
            bai.Trangthai = true;
            bai.NgayNop = DateTime.Now;
            db.SaveChanges();
            ViewBag.user = user;
            ViewBag.malop = malop;
            return ShowInforBaiTap(mabai, malop);
        }
        // bai kiem tra trac nghiem
        public ActionResult ShowBaiThiTN(string mabaitap, string malop ,string tendangnhap )
        {
            DB db = new DB();
            var checkcookie = checkCookie(malop);
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            var bai = db.BaiTapTNs.SingleOrDefault(x => x.MaBaiTap.ToString().Equals(mabaitap) && (x.NguoiNop.Equals(tendangnhap) ));
            if (bai == null)
            {
                return RedirectToAction("Index", "Exercise", new { id = malop });
            }
            var cauhoi = db.CauHois.Where(x => x.MaBaiTap.ToString().Equals(bai.MaBaiTap.ToString())).ToList();
            ViewBag.user = user;
            ViewBag.malop = malop;
            ViewData["ttbaitt"] = db.TTBaiTapTNs.Where(x => x.MaBaiNop.ToString().Equals(bai.MaBaiNop.ToString())).ToList();
            return View(cauhoi);
        }
        // hien thi cau hoi trac nghiem
        public ActionResult ShowcauhoiTracnghiem(string id, string malop)
        {
            
            DB db = new DB();
            var checkcookie = checkCookie(malop);
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            var thanhvien = checkcookie.Item5;
            if (!thanhvien.ChucVu.ToLower().Equals("giaovien".ToLower()))
            {
                return RedirectToAction("Index", "Exercise", new { id = malop });
               
            }
            var cauhoi = db.CauHois.Where(x => x.MaBaiTap.ToString().Equals(id)).ToList();
           ViewBag.mabaitaptn = id;
            ViewBag.user = user;
            ViewBag.malop = malop;
            return View(cauhoi);
        }
        // thong tin bai tap da nop
        public ActionResult Bainop(string malop)
        {
           
            var checkcookie = checkCookie(malop);
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            var bt = DOANTOTNGHIEP.Models.GetData.GetClass.getbaitapdanop(malop, user.TenDangNhap);
            ViewBag.malop = malop;
            ViewBag.user = user;
            return View("ShowBaiTap", bt);
        }
        //bai tap chua nop
        public ActionResult Baichuanop(string malop)
        {
            
            var checkcookie = checkCookie(malop);
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            var bt = DOANTOTNGHIEP.Models.GetData.GetClass.getbaitapchuanop(malop, user.TenDangNhap);
            ViewBag.user = user;
            ViewBag.malop = malop;
            return View("ShowBaiTap", bt);
        }
        // bai tap nop muon

        public ActionResult Bainopmuon(string malop)
        {
            
            var checkcookie = checkCookie(malop);
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
           
            var bt = DOANTOTNGHIEP.Models.GetData.GetClass.getbaitapnopmuon(malop, user.TenDangNhap);
            ViewBag.user = user;
            ViewBag.malop = malop;

            return View("ShowBaiTap", bt);
        }

        // chinh sua bai tap tu luan
        public ActionResult Editbaitaptl(string id, string malop, HttpPostedFileBase[] file)
        {
            
            DB db = new DB();
            var checkcookie = checkCookie(malop);
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            var thanhvien = checkcookie.Item5;
            
            
            if (!thanhvien.ChucVu.ToLower().Equals("giaovien".ToLower() ))
            {
                return RedirectToAction("Index", "Exercise", new { id = malop });
            }
            string chude = Request.Form["Chude"];
            string noidung = Request.Unvalidated.Form["noidungbt"];
            string thoihan = Request.Form["thoigiankethuc"];
            var baitap = db.BaiTaps.SingleOrDefault(x => x.MaBaiTap.ToString().Equals(id));
            if (!baitap.ChuDe.Equals(chude) || !baitap.Thongtin.Equals(noidung) || !baitap.ThoiGianKetThuc.Equals(Convert.ToDateTime(thoihan)))
            {
                baitap.ChuDe = chude;
                baitap.Thongtin = noidung;
                baitap.ThoiGianKetThuc = Convert.ToDateTime(thoihan);
                baitap.ThoiGianDang = DateTime.Now;
                db.SaveChanges();
                var tb = db.ThongBaos.SingleOrDefault(x => x.MaBaiTap.ToString().Equals(id));
                tb.Thongtin = "Bài tập mới:" + chude;
                tb.NgayDang = DateTime.Now;
                db.SaveChanges();

            }
            var filebt = baitap.FileBTTLs.Where(x => x.MaBT.ToString().Equals(id)).ToList();
            foreach( var item in filebt)
            {
                db.Libraries.Remove(item.Library);
            }
            db.FileBTTLs.RemoveRange(filebt);
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

                    Library library =new Library();

                    library.Name = fil.FileName;
                    library.Location = "/Content/document/" + Models.crypt.Encrypt.encryptfoder(malop).Replace("+", "").Replace("=", "").Replace("-", "").Replace("_", "") + "/" + Models.crypt.Encrypt.encryptfoder(user.TenDangNhap).Replace("+", "").Replace("=", "").Replace("-", "").Replace("_", "") + "/" + imageName + Extension;
                    library.NgayThem = DateTime.Now;
                    library.NguoiAdd = user.TenDangNhap;
                    library.NgayUpdate = library.NgayThem;
                    library.Noidung = DOANTOTNGHIEP.Models.exportfile.exportfile.getdatapdf(library.Location);
                    db.Libraries.Add(library);
                    FileBTTL fileBTTL = new FileBTTL();
                    fileBTTL.IDLibrary = library.ID;
                    fileBTTL.MaBT = baitap.MaBaiTap;
                    db.FileBTTLs.Add(fileBTTL);
                db.SaveChanges();

            }

            return RedirectToAction("showcauhoituluan", "Exercise", new { id = id, malop = malop });

        }

        // chinh sua bai tap trac nghiem
        public ActionResult Editbaitaptn(string id, string malop, HttpPostedFileBase[] file)
        {
           
            DB db = new DB();
            var checkcookie = checkCookie(malop);
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            
            var thanhvien = checkcookie.Item5;

            if (!thanhvien.ChucVu.ToLower().Equals("giaovien".ToLower()))
            {
                return RedirectToAction("Index", "Exercise", new { id = malop });

            }
            string chude = Request.Form["Chude"];
            string noidung = Request.Unvalidated.Form["noidungbt"];
            string thoihan = Request.Form["thoigiankethuc"];
            var baitap = db.BaiTaps.SingleOrDefault(x => x.MaBaiTap.ToString().Equals(id));
            if (!baitap.ChuDe.Equals(chude) || !baitap.Thongtin.Equals(noidung) || !baitap.ThoiGianKetThuc.Equals(Convert.ToDateTime(thoihan)))
            {
                baitap.ChuDe = chude;
                baitap.Thongtin = noidung;
                baitap.ThoiGianKetThuc = Convert.ToDateTime(thoihan);
                baitap.ThoiGianDang = DateTime.Now;
                db.SaveChanges();
                var tb = db.ThongBaos.SingleOrDefault(x => x.MaBaiTap.ToString().Equals(id));
                tb.Thongtin = "Bài tập mới:" + chude;
                tb.NgayDang = DateTime.Now;
                db.SaveChanges();

            }

           /* var cauhoi = db.CauHois.Where(x => x.MaBaiTap.ToString().Equals(id)).ToList();*/
            return RedirectToAction("ShowcauhoiTracnghiem", new { id = id, malop = malop });
        }

        // khong can malop 
        public ActionResult ShowBaiTap(BaiTap baitap , string malop)
        {

            var checkcookie = checkCookie(malop);
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            ViewBag.user = user;
            return View(baitap);
        }
        //chi tiec bai tap 
        // xong 
        public ActionResult ShowInforBaiTap(string mabaitap, string malop)
        {
           
            DB db = new DB();
            var checkcookie = checkCookie(malop);
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            if (mabaitap == null)
            {
                return RedirectToAction("Index", "TrangChu");

            }
            var thanhvien = checkcookie.Item5;
            var bt = db.BaiTaps.SingleOrDefault(c => c.MaBaiTap.ToString().Equals(mabaitap));
            if (bt != null)
            {
                if (thanhvien.ChucVu.ToLower().Equals("giaovien".ToLower()))
                {
                    /* ViewBag.mabaitap = mabaitap;*/

                    return RedirectToAction("ShowInforBaiTapForTeacher", new { malop = malop, mabaitap = mabaitap });
                }
                else 
                {
                    if (bt.LoaiBaiTap.Equals("TracNghiem"))
                    {

                        var bttn = db.BaiTapTNs.SingleOrDefault(x => x.NguoiNop.Equals(user.TenDangNhap) && x.MaBaiTap.ToString().Equals(mabaitap));
                        var taikhoan = db.TaiKhoans.SingleOrDefault(x => x.TenDangNhap.Equals(user.TenDangNhap));
                        var baitap = db.BaiTaps.SingleOrDefault(x => x.MaBaiTap.ToString().Equals(mabaitap));
                        var baitaptracnghiem = new DOANTOTNGHIEP.Models.Modelcreate.BaiTap();
                        baitaptracnghiem.TaiKhoan = taikhoan;
                        baitaptracnghiem.BaiTaps = baitap;
                        if (bttn != null)
                        {
                            baitaptracnghiem.TTBaiTapTns = db.TTBaiTapTNs.Where(x => x.MaBaiNop.ToString().Equals(bttn.MaBaiNop.ToString())).ToList();
                            baitaptracnghiem.NgayNop = bttn.NgayNop;
                            baitaptracnghiem.Diem = bttn.Diem;
                            ViewBag.mabainop = bttn.MaBaiNop;
                            ViewBag.nguoinop = bttn.NguoiNop;
                        }
                        ViewBag.user = user;
                        ViewBag.malop = malop;
                        return View("ShowInforBaiTapTn", baitaptracnghiem);
                    }
                    else if (bt.LoaiBaiTap.Equals("TuLuan"))
                    {

                        var bttl = db.BaiTapTLs.SingleOrDefault(x => x.NguoiNop.Equals(user.TenDangNhap) && x.MaBaiTap.ToString().Equals(mabaitap));
                        var taikhoan = db.TaiKhoans.SingleOrDefault(x => x.TenDangNhap.Equals(user.TenDangNhap));
                        var baitap = db.BaiTaps.SingleOrDefault(x => x.MaBaiTap.ToString().Equals(mabaitap));
                        var baitaptuluan = new DOANTOTNGHIEP.Models.Modelcreate.BaiTap();
                        baitaptuluan.TaiKhoan = taikhoan;
                        baitaptuluan.BaiTaps = baitap;
                        if (bttl != null)
                        {
                            baitaptuluan.TTBaiTapTls = db.TTBaiTapTLs.Where(x => x.MaBaiNop.ToString().Equals(bttl.MaBaiNop.ToString())).ToList();
                            baitaptuluan.NgayNop = bttl.NgayNop;
                            baitaptuluan.Diem = bttl.Diem;
                            ViewBag.nguoinop = bttl.NguoiNop;

                        }
                        ViewBag.user = user;
                        ViewBag.malop = malop;
                        return View("ShowInforBaiTapTL", baitaptuluan);
                    }
                }
            }
            return RedirectToAction("Index", new { id = malop });

        }

        /* Thong tin bai tap cho giao vien*/
        //xong
        public ActionResult ShowInforBaiTapForTeacher(string malop, string mabaitap)
        {
            
            DB db = new DB();
            var checkcookie = checkCookie(malop);
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
           
            var Listbaitaptuluan = new List<DOANTOTNGHIEP.Models.Modelcreate.BaiTap>();
            var Listbaitaptracnghiem = new List<DOANTOTNGHIEP.Models.Modelcreate.BaiTap>();
            var baitap = db.BaiTaps.SingleOrDefault(c => c.MaBaiTap.ToString().Equals(mabaitap));
            var thanhvienlop = db.ThanhVienLops.Where(x => x.MaLop.ToString().Equals(malop) && x.ChucVu.ToLower().Equals("sinhvien".ToLower())).OrderBy(x => x.TaiKhoan.Ten).ToList();
            foreach(var i in thanhvienlop)
            {
                if (baitap.LoaiBaiTap.Equals("TracNghiem")){
                    var baitaptracnghiem = new DOANTOTNGHIEP.Models.Modelcreate.BaiTap();
                    baitaptracnghiem.TaiKhoan = i.TaiKhoan;
                    baitaptracnghiem.BaiTaps = baitap;
                    var diemtn = db.BaiTapTNs.SingleOrDefault(x => x.MaBaiTap.ToString().Equals(baitap.MaBaiTap.ToString()) && x.NguoiNop.Equals(i.Mathanhvien));
                    if (diemtn != null)
                    {
                        baitaptracnghiem.NgayNop = diemtn.NgayNop;
                        baitaptracnghiem.Diem = diemtn.Diem;
                        var bttn = db.TTBaiTapTNs.Where(x => x.MaBaiNop.ToString().Equals(diemtn.MaBaiNop.ToString())).ToList();
                        baitaptracnghiem.TTBaiTapTns = bttn;
                    }
                    Listbaitaptracnghiem.Add(baitaptracnghiem);
                }else
                {
                    var baitaptuluan = new DOANTOTNGHIEP.Models.Modelcreate.BaiTap();
                    baitaptuluan.TaiKhoan = i.TaiKhoan;
                    baitaptuluan.BaiTaps = baitap;
                    var diemtn = db.BaiTapTLs.SingleOrDefault(x => x.MaBaiTap.ToString().Equals(baitap.MaBaiTap.ToString()) && x.NguoiNop.Equals(i.Mathanhvien));
                    if (diemtn != null)
                    {
                        baitaptuluan.NgayNop = diemtn.NgayNop;
                        baitaptuluan.Diem = diemtn.Diem;
                        baitaptuluan.TTBaiTapTls = db.TTBaiTapTLs.Where(x => x.MaBaiNop.ToString().Equals(diemtn.MaBaiNop.ToString())).ToList();

                    }
                    Listbaitaptuluan.Add(baitaptuluan);

                }

            }
            
            if(Listbaitaptracnghiem.Count > 0)
            {
                ViewBag.excel = DOANTOTNGHIEP.Models.exportfile.exportfile.exceldsdiembaitap(Listbaitaptracnghiem);
                ViewBag.pdf = DOANTOTNGHIEP.Models.exportfile.exportfile.pdfdsdiembaitap(Listbaitaptracnghiem, malop);

            }else
            {
                ViewBag.excel = DOANTOTNGHIEP.Models.exportfile.exportfile.exceldsdiembaitap(Listbaitaptuluan);
                ViewBag.pdf = DOANTOTNGHIEP.Models.exportfile.exportfile.pdfdsdiembaitap(Listbaitaptuluan, malop);
            }
            ViewBag.user = user;
            ViewBag.malop = malop;
            ViewData["baituluan"] = Listbaitaptuluan;
            ViewData["baitracnghiem"] = Listbaitaptracnghiem;
            return View("ShowInforBaiTapForTeacher");
            
          
        }
        /*dang bai tap tu luan*/
        [HttpPost]
        public ActionResult DangBaiTapTL(string malop, HttpPostedFileBase[] file)
        {
            
            DB db = new DB();
            var checkcookie = checkCookie(malop);
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
           
            var thanhvien = checkcookie.Item5;

            if (!thanhvien.ChucVu.ToLower().Equals("giaovien".ToLower()))
            {
                return RedirectToAction("Index", "Exercise", new { id = malop });
            }
            string nguoitao = user.TenDangNhap;
            string noidung = Request.Unvalidated.Form["noidungbt"];
            string chude = Request.Form["Chude"];
            string thoigianketthuc = Request.Form["thoigiankethuc"].ToString();
            DateTime dt = Convert.ToDateTime(thoigianketthuc);
            BaiTap bt = new BaiTap();
            bt.Thongtin = noidung;
            bt.ThoiGianDang = DateTime.Now;
            bt.ThoiGianKetThuc = dt;
            bt.NguoiTao = nguoitao;
            bt.LoaiBaiTap = "TuLuan";
            bt.ChuDe = chude;
            bt.MaLop = long.Parse(malop);
            db.BaiTaps.Add(bt);
            db.SaveChanges();
            ThongBao tb = new ThongBao();
            tb.NguoiDang = nguoitao;
            tb.NgayDang = DateTime.Now;
            tb.MaBaiTap = bt.MaBaiTap;
            tb.MaLop = long.Parse(malop);
            tb.Thongtin = "Bài tập mới:" + chude;
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
                FileBTTL fileBTTL = new FileBTTL();
                fileBTTL.IDLibrary = library.ID;
                fileBTTL.MaBT = bt.MaBaiTap;
                db.FileBTTLs.Add(fileBTTL);
                db.SaveChanges();


            }


         

           

            var lophoc = db.LopHocs.SingleOrDefault(x => x.MaLop.ToString().Equals(malop));
            var thanhvienlop = db.ThanhVienLops.Where(x => x.MaLop.ToString().Equals(malop)).ToList();
            foreach (var tv in thanhvienlop)
            {
               

                    DOANTOTNGHIEP.Models.mail.mail.SendEmail(tv.TaiKhoan.Email, "Bài tâp mới trong Lớp :" + lophoc.TenLop + " của giáo viên " + lophoc.TaiKhoan.Ho + " " + lophoc.TaiKhoan.Ten, "\n.Chủ đề :" + bt.ChuDe + "\n. Nội dung :" + bt.Thongtin);
               
            }

            return RedirectToAction("Index", "Exercise", new { id = malop });
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult editcauhoitracnghiem(string macauhoi, string malop)
        {
            DB db = new DB();
            var checkcookie = checkCookie(malop);
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            var cauhoi = Request.Unvalidated.Form["cauhoi-" + macauhoi].ToString();
            var dbcauhoi = db.CauHois.SingleOrDefault(x => x.MaCauHoi.ToString().Equals(macauhoi));
            dbcauhoi.NoiDung = cauhoi;
            var dapan1 = Request.Unvalidated.Form["dapan-" + macauhoi].ToString();
            foreach (var dapan in dbcauhoi.DapAns.ToList())
            {
                if (dapan.MaDapAn.ToString().Equals(dapan1))
                {
                    dapan.LoaiDapAn = true;
                }
                else dapan.LoaiDapAn = false;

                var da = Request.Unvalidated.Form["dapan-" + dapan.MaDapAn].ToString();
                dapan.NoiDung = da;
                db.SaveChanges();
            }
            db.SaveChanges();

            return RedirectToAction("ShowcauhoiTracnghiem", "Exercise", new { id = dbcauhoi.BaiTap.MaBaiTap.ToString(), malop = malop });
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult addcauhoitracnghiem(string id, string malop)
        {
            DB db = new DB();
            var checkcookie = checkCookie(malop);
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
           
            var noidungcauhoi = Request.Unvalidated.Form["addcauhoi"].ToString().Replace("<p>", "").Replace("</p>", "");
            CauHoi cauhoi = new CauHoi();
            cauhoi.NoiDung = noidungcauhoi;
            cauhoi.NgayThem = DateTime.Now;
            cauhoi.MaBaiTap = long.Parse(id);
            db.CauHois.Add(cauhoi);
            db.SaveChanges();
            var dapan1 = Request.Unvalidated.Form["da"].ToString();
            string[] slda = { "A", "B", "C", "D" };
            for (int i = 0; i < 4; i++)
            {
                DapAn dapan = new DapAn();
                var da = Request.Unvalidated.Form["" + slda[i]].ToString().Replace("<p>", "").Replace("</p>", "");
                dapan.MaCauHoi = cauhoi.MaCauHoi;
                dapan.NoiDung = da;
                if (dapan1.Equals(slda[i]))
                {
                    dapan.LoaiDapAn = true;
                }
                else dapan.LoaiDapAn = false;
                db.DapAns.Add(dapan);
                db.SaveChanges();
            }



            return RedirectToAction("ShowcauhoiTracnghiem", new { id = id, malop = malop });
        }
        [HttpPost]
        public void deletecauhoitracnghiem(string macauhoi , string malop)
        {
            DB db = new DB();
            var checkcookie = checkCookie(malop);
            if (checkcookie.Item1)
            {
                RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            var cauhoi = db.CauHois.SingleOrDefault(x => x.MaCauHoi.ToString().Equals(macauhoi));
            var bainop = db.BaiTapTNs.Where(x => x.MaBaiTap.ToString().Equals(cauhoi.MaBaiTap.ToString())).ToList();
            foreach (var i in bainop)
            {
                var bailam = db.TTBaiTapTNs.Where(x => x.MaBaiNop.ToString().Equals(i.MaBaiNop.ToString())).ToList();
                db.TTBaiTapTNs.RemoveRange(bailam);
                db.SaveChanges();
                i.NgayNop = null;
                i.Diem = null;
                i.Trangthai = null;
                db.SaveChanges();
            }
            var dapan = db.DapAns.Where(x => x.MaCauHoi.ToString().Equals(macauhoi)).ToList();
            db.DapAns.RemoveRange(dapan);
            db.CauHois.Remove(cauhoi);
            db.SaveChanges();

        }
        [HttpPost]
        public System.Web.Mvc.JsonResult checkdaovan(string mabaitap, string malop ,string option)
        {
           
            
            var checkcookie = checkCookie(malop);
            if (checkcookie.Item1)
            {
                RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var check = false;
            if (option.Equals("1")){

                check = DOANTOTNGHIEP.Models.database.daovan.kiemtradaovanbaitap(mabaitap, malop);

            }else if (option.Equals("2")){

                    check = DOANTOTNGHIEP.Models.database.daovan.kiemtradaovanlophoc(mabaitap, malop);
                
            }
            else if (option.Equals("3"))
            {
                check = DOANTOTNGHIEP.Models.database.daovan.kiemtradaovanall(mabaitap);

            }

            if (check ){
                return Json("true");
            }
            return Json("false");

        }




        public ActionResult kiemtradaovan(string ma, string malop)
        {
            DB db = new DB();
            var checkcookie = checkCookie(malop);
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            var dbfile = db.TTBaiTapTLs.Where(x => x.MaBaiNop.ToString().Equals(ma)).ToList();
            List<Kiemtradaovan> kiemtra = new List<Kiemtradaovan>();
            foreach (var ifile in dbfile)
            {
                var dv = ifile.Plagiarism.Where(x => x.Mafile.Equals(ifile.Ma)).ToList();

                Kiemtradaovan kt = new Kiemtradaovan();
                kt.Tailieu = ifile;
                kt.daovan = dv;
                kiemtra.Add(kt);
            }
            ViewBag.user = user;
            return View(kiemtra);
        }
        /*[HttpPost]*/
        public ActionResult Commentbaitap(string malop, string mabaitap )
        {
            DB db = new DB();
            var checkcookie = checkCookie(malop);
            var user = checkcookie.Item4;
            ViewBag.user = user;
            ViewBag.malop = malop;
            ViewBag.mabaitap = mabaitap;
            List<DOANTOTNGHIEP.Models.commentbaitap> test = db.commentbaitaps.Where(x => x.MaBaiTap.ToString().Equals(mabaitap)).ToList();
            return PartialView(test);
        }
        public ActionResult ReplycommenBaitap(string malop, string macomment)
        {
            DB db = new DB();
            var checkcookie = checkCookie(malop);
            var user = checkcookie.Item4;
            ViewBag.user = user;
            ViewBag.malop = malop;
            ViewBag.mabinhluan = macomment;
            List<DOANTOTNGHIEP.Models.replycommentBT> test = db.replycommentBTs.Where(x => x.MaComment.ToString().Equals(macomment)).ToList();


            return PartialView(test);
        }
        public string savebinhluanbaitap(string mabaitap, string mabinhluan, string noidung ,string  malop)
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
                commentbaitap comment = new commentbaitap();
                comment.MaBaiTap = Convert.ToInt64(Convert.ToDouble(mabaitap));
                comment.Thoigiandang = DateTime.Now;
                comment.Noidung = noidung;
                comment.Nguoidang = user.TenDangNhap;
                db.commentbaitaps.Add(comment);
                db.SaveChanges();
                return comment.Ma.ToString();
            }
            else
            {
                replycommentBT comment = new replycommentBT();
                comment.MaComment = Convert.ToInt64(Convert.ToDouble(mabinhluan));
                comment.Thoigiandang = DateTime.Now;
                comment.Noidung = noidung;
                comment.Nguoidang = user.TenDangNhap;
                db.replycommentBTs.Add(comment);
                db.SaveChanges();
                return "";
            }

        }


    }
}