namespace DOANTOTNGHIEP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("document")]
    public partial class document
    {
        [Key]
        public long Ma { get; set; }

        public string Ten { get; set; }

        public string Vitriluu { get; set; }

        public string Noidung { get; set; }

        [StringLength(20)]
        public string Nguoisohuu { get; set; }

        public string Image { get; set; }

        public long? MaLop { get; set; }

        public int? LuotTaiXuong { get; set; }

        public DateTime? Ngaydang { get; set; }

        public int? Luotxem { get; set; }

        public long? Machude { get; set; }

        public virtual Chudetailieu Chudetailieu { get; set; }

        public virtual LopHoc LopHoc { get; set; }

        public virtual TaiKhoan TaiKhoan { get; set; }
    }
}
