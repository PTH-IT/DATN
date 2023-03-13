using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOANTOTNGHIEP.Models.Modelcreate
{
    public class BaiTap
    {
        public virtual DOANTOTNGHIEP.Models.TaiKhoan TaiKhoan { get; set; }
        public DateTime? NgayNop { get; set; }
        public int? Diem { get; set; }
        public virtual DOANTOTNGHIEP.Models.BaiTap BaiTaps { get; set; }
        public virtual List<DOANTOTNGHIEP.Models.TTBaiTapTL> TTBaiTapTls { get; set; }
        public virtual List<DOANTOTNGHIEP.Models.TTBaiTapTN> TTBaiTapTns { get; set; }
    }
}