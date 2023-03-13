namespace DOANTOTNGHIEP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ThongBao")]
    public partial class ThongBao
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ThongBao()
        {
            commentnotifications = new HashSet<commentnotification>();
            FileTBs = new HashSet<FileTB>();
        }

        [Key]
        public long MaBaiDang { get; set; }

        [StringLength(20)]
        public string NguoiDang { get; set; }

        public long? MaLop { get; set; }

        public DateTime? NgayDang { get; set; }

        public string Thongtin { get; set; }

        [StringLength(20)]
        public string LoaiThongBao { get; set; }

        public long? MaBaiTap { get; set; }

        public virtual BaiTap BaiTap { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<commentnotification> commentnotifications { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FileTB> FileTBs { get; set; }

        public virtual LopHoc LopHoc { get; set; }

        public virtual TaiKhoan TaiKhoan { get; set; }
    }
}
