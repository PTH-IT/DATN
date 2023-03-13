using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOANTOTNGHIEP.Models.GetData
{
    public class GetAccount
    {

         
        public static TaiKhoan get(string s)
        {
            DB db = new DB();
            return db.TaiKhoans.SingleOrDefault(x => x.TenDangNhap.Equals(s));

        }
        

    }
}