using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOANTOTNGHIEP.Modelcreate
{
    public class Diem
    {
        public string taikhoan { get; set; }
        public string Hoten { get; set; }
        public string email { get; set; }
        public string anh { get; set; }
        public long? diem { get; set; }
        public int soluongbaicodiem { get; set; }
        public int soluongbaitap { get; set; }
        public int soluongtre { get; set; }
        public int soluongchuanop { get; set; }
        public DateTime  ngaythamgia { get; set; }

    }
 
}