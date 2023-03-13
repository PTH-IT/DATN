namespace DOANTOTNGHIEP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BaiTapTN")]
    public partial class BaiTapTN
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BaiTapTN()
        {
            TTBaiTapTNs = new HashSet<TTBaiTapTN>();
        }

        [Key]
        public long MaBaiNop { get; set; }

        public long MaBaiTap { get; set; }

        public DateTime? NgayNop { get; set; }

        public bool? Trangthai { get; set; }

        [StringLength(20)]
        public string NguoiNop { get; set; }

        public int? Diem { get; set; }

        public virtual BaiTap BaiTap { get; set; }

        public virtual TaiKhoan TaiKhoan { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TTBaiTapTN> TTBaiTapTNs { get; set; }
    }
}
