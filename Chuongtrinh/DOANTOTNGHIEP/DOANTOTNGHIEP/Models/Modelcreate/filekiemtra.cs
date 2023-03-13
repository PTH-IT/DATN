using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DOANTOTNGHIEP.Models;

namespace DOANTOTNGHIEP.Modelcreate
{
    public class filekiemtra   
    {
        
        public virtual TaiKhoan user1 { get; set; }
        public virtual TaiKhoan user2 { get; set; }
        public Kiemtradaovan kiemtra { get; set; }
    }
}