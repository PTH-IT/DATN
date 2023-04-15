namespace DOANTOTNGHIEP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BaiTap")]
    public partial class BaiTap
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BaiTap()
        {
            BaiTapTLs = new HashSet<BaiTapTL>();
            BaiTapTNs = new HashSet<BaiTapTN>();
            CauHois = new HashSet<CauHoi>();
            commentbaitaps = new HashSet<commentbaitap>();
            ThongBaos = new HashSet<ThongBao>();
            FileBTTLs = new HashSet<FileBTTL>();
        }

        public long ID { get; set; }

        [StringLength(2000)]
        public string ChuDe { get; set; }

        [StringLength(20)]
        public string LoaiBaiTap { get; set; }

        public DateTime? ThoiGianDang { get; set; }

        public DateTime? ThoiGianKetThuc { get; set; }

        public long? MaLop { get; set; }

        [StringLength(20)]
        public string NguoiTao { get; set; }

        public string Thongtin { get; set; }

        public virtual LopHoc LopHoc { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BaiTapTL> BaiTapTLs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BaiTapTN> BaiTapTNs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CauHoi> CauHois { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<commentbaitap> commentbaitaps { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ThongBao> ThongBaos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FileBTTL> FileBTTLs { get; set; }
    }
}
