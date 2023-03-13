namespace DOANTOTNGHIEP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Loimoi")]
    public partial class Loimoi
    {
        [Key]
        public long ma { get; set; }

        public long MaLop { get; set; }

        [Required]
        [StringLength(20)]
        public string TenDangNhap { get; set; }

        public virtual LopHoc LopHoc { get; set; }

        public virtual TaiKhoan TaiKhoan { get; set; }
    }
}
