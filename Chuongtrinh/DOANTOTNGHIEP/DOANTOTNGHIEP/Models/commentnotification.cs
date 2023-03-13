namespace DOANTOTNGHIEP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("commentnotification")]
    public partial class commentnotification
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public commentnotification()
        {
            replycomments = new HashSet<replycomment>();
        }

        [Key]
        public long Ma { get; set; }

        public long? MaThongbao { get; set; }

        [StringLength(20)]
        public string Nguoidang { get; set; }

        public string Noidung { get; set; }

        public DateTime? Thoigiandang { get; set; }

        public virtual TaiKhoan TaiKhoan { get; set; }

        public virtual ThongBao ThongBao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<replycomment> replycomments { get; set; }
    }
}
