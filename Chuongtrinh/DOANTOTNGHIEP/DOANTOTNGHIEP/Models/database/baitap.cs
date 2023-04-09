using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOANTOTNGHIEP.Models.database
{
    public class baitap
    {
        public static BaiTapTN AddBaitapTN(long MaBaiTap , string TenDangNhap)
        {
            DB db = new DB();
            BaiTapTN btn = new BaiTapTN();
            btn.MaBaiTap = MaBaiTap;
            btn.NguoiNop = TenDangNhap;
            btn.NgayNop = DateTime.Now;
            db.BaiTapTNs.Add(btn);
            db.SaveChanges();
            return btn;
        }
        public static BaiTapTL AddBaitapTL(long MaBaiTap, string TenDangNhap)
        {

            DB db = new DB();
            BaiTapTL btl = new BaiTapTL();
            btl.MaBaiTap = MaBaiTap;
            btl.NguoiNop = TenDangNhap;
            btl.NgayNop = DateTime.Now;
            db.BaiTapTLs.Add(btl);
            db.SaveChanges();
            return btl;
        }
    }
}