namespace DOANTOTNGHIEP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BaiTapTL")]
    public partial class BaiTapTL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BaiTapTL()
        {
            TTBaiTapTLs = new HashSet<TTBaiTapTL>();
        }

        [Key]
        public long MaBaiNop { get; set; }

        public long MaBaiTap { get; set; }

        public bool? Trangthai { get; set; }

        public DateTime? NgayNop { get; set; }

        [StringLength(20)]
        public string NguoiNop { get; set; }

        public int? Diem { get; set; }

        public virtual BaiTap BaiTap { get; set; }

        public virtual TaiKhoan TaiKhoan { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TTBaiTapTL> TTBaiTapTLs { get; set; }
    }
}
