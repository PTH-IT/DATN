using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOANTOTNGHIEP.Models.database
{
    public class taikhoan
    {
        public static TaiKhoan AddAccount(string ho ,string ten ,string email ,string tendangnhap ,string pass ,string chucvu)
        {
            DB db = new DB();
            TaiKhoan tk = new TaiKhoan();
            tk.Ho = ho;
            tk.Ten = ten;
            tk.Email = email;
            tk.TenDangNhap = tendangnhap;
            tk.MatKhau = Models.crypt.Encrypt.encryptuser(pass);
            tk.ChucVu = chucvu;
            tk.HinhAnh = "/Content/image/imageaccount/d.jpg";
            db.TaiKhoans.Add(tk);
            db.SaveChanges();
            return tk;  
        }
    }
}