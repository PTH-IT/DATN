using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace DOANTOTNGHIEP.Models.database
{
    public class lophoc
    {
        public static LopHoc AddClass(string name, string infor, string tendangnhap)
        {
            DB db = new DB();
            LopHoc lp = new LopHoc();
            lp.TenLop = name;
            lp.ThongTinLopHoc = infor;
            lp.NguoiTao = tendangnhap;
            lp.NgayTao = DateTime.Now;
            lp.Hinhanh = "/Content/image/imageclass/img_backtoschool.jpg";
            db.LopHocs.Add(lp);
            db.SaveChanges();
            DOANTOTNGHIEP.Models.foder.foder.CreateFolder(HostingEnvironment.MapPath("~/Content/document/" + Models.crypt.Encrypt.encryptfoder(lp.MaLop.ToString()).Replace("+", "").Replace("=", "").Replace("-", "").Replace("_", "")));

            return lp;
        }

            public static ThanhVienLop AddMember(string malop , string tendangnhap , string chucvu)
        {
            DB db = new DB();
            ThanhVienLop tvl = new ThanhVienLop();
            tvl.MaLop = Convert.ToInt32(malop);
            tvl.Mathanhvien = tendangnhap;
            tvl.NgayThamGia = DateTime.Now;
            tvl.ChucVu = chucvu;
            db.ThanhVienLops.Add(tvl);
            db.SaveChanges();
            DOANTOTNGHIEP.Models.foder.foder.CreateFolder(HostingEnvironment.MapPath("~/Content/document/" + Models.crypt.Encrypt.encryptfoder(malop).Replace("+", "").Replace("=", "").Replace("-", "").Replace("_", "") + "/" + Models.crypt.Encrypt.encryptfoder(tvl.Mathanhvien).Replace("+", "").Replace("=", "").Replace("-", "").Replace("_", "")));

            return tvl;

        }
       
       
    }
}