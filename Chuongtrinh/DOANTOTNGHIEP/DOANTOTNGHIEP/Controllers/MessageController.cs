using DOANTOTNGHIEP.Models;
using Spire.Pdf.Exporting.XPS.Schema.Mc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DOANTOTNGHIEP.Controllers
{
    public class MessageController : Controller
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


        // GET: Message
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
            if (id == null) return RedirectToAction("Index", "TrangChu");
            var lop = db.ThanhVienLops.SingleOrDefault(x => x.ID.ToString().Equals(id) && x.Mathanhvien.Equals(user.TenDangNhap));
            if (lop == null)
            {
                return RedirectToAction("Index", "TrangChu");
            }
            var thanhvienlop = db.ThanhVienLops.Where(x => x.ID.ToString().Equals(id)).OrderBy(x => x.TaiKhoan.Ten).ToList();

            var thanhvien1 = db.ThanhVienLops.Where(x => x.ID.ToString().Equals(id)).OrderBy(x => x.TaiKhoan.Ten).ToList();

            var thanhvien2 = db.ThanhVienLops.Where(x => x.ID.ToString().Equals(id)).OrderBy(x => x.TaiKhoan.Ten).ToList();

            List<Mess> tin11 = new List<Mess>();
            foreach (var i in thanhvienlop)
            {
                var mess1 = db.Messes.Where(x => ((x.NguoiNhan.Equals(user.TenDangNhap) && x.NguoiGui.Equals(i.Mathanhvien))) && x.malop.ToString().Equals(id)).OrderByDescending(x => x.thoigiangui.Value).ToList();
                if (mess1.Count > 0)
                {
                    tin11.Add(mess1[0]);
                }

            }
            foreach (var i in thanhvienlop)
            {
                var mess1 = db.Messes.Where(x => ((x.NguoiNhan.Equals(i.Mathanhvien) && x.NguoiGui.Equals(user.TenDangNhap))) && x.malop.ToString().Equals(id)).OrderByDescending(x => x.thoigiangui.Value).ToList();
                if (mess1.Count > 0)
                {
                    tin11.Add(mess1[0]);
                }

            }
            for (int i = 0; i < tin11.Count; i++)
                for (int j = 0; j < tin11.Count - 1; j++)
                    if (tin11[j].thoigiangui.Value > tin11[j + 1].thoigiangui.Value)
                    {
                        var a = tin11[j];
                        tin11[j] = tin11[j + 1];
                        tin11[j + 1] = a;

                    }

            List<ThanhVienLop> tv11 = new List<ThanhVienLop>();
            foreach (var i in tin11)
            {
                var s = i.NguoiGui;
                var t = i.thoigiangui;
                var tv1 = db.ThanhVienLops.SingleOrDefault(x => x.Mathanhvien.Equals(i.NguoiGui) && x.ID.ToString().Equals(id));
                tv11.Add(tv1);
                var tv2 = db.ThanhVienLops.SingleOrDefault(x => x.Mathanhvien.Equals(i.NguoiNhan) && x.ID.ToString().Equals(id));
                tv11.Add(tv2);
            }
            foreach (var j in tv11)
            {
                foreach (var i in thanhvien2)
                {

                    if (i.Mathanhvien.Equals(j.Mathanhvien))
                    {
                        thanhvienlop.Remove(j);
                        thanhvienlop.Insert(0, j);

                        break;
                    }
                }

            }


            var groupchat = db.GroupChats.Where(x => x.Malop.ToString().Equals(id) && x.MemberGroups.Where(y => y.MaGroup.ToString().Equals(x.ID.ToString()) && y.IDMember.Equals(user.TenDangNhap)).ToList().Count > 0).ToList();

            ViewBag.ListGroup = groupchat;



            return View(thanhvienlop);
        }

        [HttpPost]
        public ActionResult AddGroup(HttpPostedFileBase fileimageupload, string id)
        {

            var result = new DOANTOTNGHIEP.Modelcreate.JsonResult();
            DB db = new DB();
            var checkcookie = checkCookie(id);
            if (checkcookie.Item1)
            {
                RedirectToAction(checkcookie.Item2, checkcookie.Item3);
                result.result = false;
                result.value = "tai khoan khong ton tai";
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            var user = checkcookie.Item4;
           var  imagegroupLocation = "/Content/image/imageaccount/d.jpg";
            if (fileimageupload != null)
            {
                var path = "/Content/image/imageaccount/" + fileimageupload.FileName.Replace(" ","");
                imagegroupLocation = path;
                fileimageupload.SaveAs(Server.MapPath("~"+path));
                    
            }
            string name = Request.Form["namegroup"];
            GroupChat newgroup = new GroupChat();
            newgroup.Malop = Convert.ToInt64(id);
            newgroup.admin = user.TenDangNhap;
            newgroup.Name = name;
            newgroup.image = imagegroupLocation;
            db.GroupChats.Add(newgroup);
            db.SaveChanges();

            MemberGroup member =new MemberGroup();
            member.IDMember = user.TenDangNhap;
            member.MaGroup = newgroup.ID;
            db.MemberGroups.Add(member);
            db.SaveChanges();
            result.result = true;
            result.value = "Tao group thanh cong";
            return RedirectToAction("Index", "Message", new { id = id });

        }
        [HttpPost]
        public JsonResult AddMemberGroup(string idgroup ,string email)
        {
            
            var result = new DOANTOTNGHIEP.Modelcreate.JsonResult();
            DB db = new DB();
            var username = "";
            var tk =db.TaiKhoans.SingleOrDefault(x=>x.Email.ToLower().Equals(email.ToLower()));
            if (tk == null)
            {
                result.result = false;
                result.value = " User không ton tai trong lớp học";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            username = tk.TenDangNhap;
            var checkuser = db.MemberGroups.Where(x => x.IDMember.ToString().Equals(username) && x.MaGroup.ToString().Equals(idgroup)).ToList();
            if (checkuser.Count > 0)
            {
                result.result = false;
                result.value = " User da ton tai trong group";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            MemberGroup member = new MemberGroup();
            member.IDMember = username;
            member.MaGroup = Convert.ToInt64(idgroup);
            db.MemberGroups.Add(member);
            db.SaveChanges();
            result.result = true;
            result.value = "Them user thanh cong";
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        //hien tin nhan trong csdl
        [HttpPost]
        public ActionResult InforMess(string id, string malop)
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
            string nguoitao = user.TenDangNhap;

            var taikhoan = db.TaiKhoans.SingleOrDefault(x => x.TenDangNhap.Equals(id));
            ViewBag.nguoinhan = taikhoan;
            ViewData["tinnhan"] = db.Messes.Where(x => x.malop.ToString().Equals(malop) && ((x.NguoiGui.Equals(nguoitao) && x.NguoiNhan.Equals(id)) || (x.NguoiGui.Equals(id) && x.NguoiNhan.Equals(nguoitao)))).OrderByDescending(y => y.thoigiangui).Take(50).OrderBy(y => y.thoigiangui).ToList();

            return PartialView(taikhoan);
        }
        [HttpPost]
        public ActionResult InforMessGroupChat(string id, string malop)
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
            string nguoitao = user.TenDangNhap;

            var groupchat = db.GroupChats.SingleOrDefault(x => x.ID.ToString().Equals(id));
            ViewData["tinnhan"] = db.MessGroups.Where(x => x.MaGroup.ToString().Equals(id)).OrderByDescending(y => y.thoigiangui).Take(50).OrderBy(y => y.thoigiangui).ToList();

            return PartialView("inforgroupchat",groupchat);
        }
        [HttpPost]
        [ValidateInput(false)]
        public void SaveInforMess(string nguoinhan, string tinnhan, string malop)
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
            string nguoitao = user.TenDangNhap;

            if (!tinnhan.Equals(""))
            {
                DOANTOTNGHIEP.Models.Mess mess = new Mess();
                mess.NguoiGui = nguoitao;
                mess.NguoiNhan = nguoinhan;
                mess.malop = long.Parse(malop);
                mess.TinNhan = tinnhan;
                mess.thoigiangui = DateTime.Now;
                db.Messes.Add(mess);
                db.SaveChanges();
            }


        }
        [HttpPost]
        [ValidateInput(false)]
        public void SaveInforMessGroupChat(string nguoinhan, string tinnhan, string malop)
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
            string nguoitao = user.TenDangNhap;

            if (!tinnhan.Equals(""))
            {
                DOANTOTNGHIEP.Models.MessGroup mess = new MessGroup();
                mess.NguoiGui = nguoitao;
                mess.MaGroup = Convert.ToInt64( nguoinhan);
                mess.MaLop = long.Parse(malop);
                mess.TinNhan = tinnhan;
                mess.thoigiangui = DateTime.Now;
                db.MessGroups.Add(mess);
                db.SaveChanges();
            }


        }
        [HttpPost]
        public System.Web.Mvc.JsonResult UploadFile(HttpPostedFileBase uploadedFiles)
        {

            string returnImagePath = string.Empty;
            string fileName;
            string Extension;
            string imageName;
            string imageSavePath;

            if (uploadedFiles.ContentLength > 0)
            {
                fileName = Path.GetFileNameWithoutExtension(uploadedFiles.FileName);
                Extension = Path.GetExtension(uploadedFiles.FileName);
                imageName = fileName + DateTime.Now.ToString("yyyyMMddHHmmss");
                imageSavePath = Server.MapPath("~/Content/image/imagetinnhan/") + imageName +
Extension;

                uploadedFiles.SaveAs(imageSavePath);
                returnImagePath = "/Content/image/imagetinnhan/" + imageName +
Extension;
            }

            return Json(Convert.ToString(returnImagePath), JsonRequestBehavior.AllowGet);
        }
        // luu tin nhan

    }


}