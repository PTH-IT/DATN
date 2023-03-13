namespace DOANTOTNGHIEP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("commentbaitap")]
    public partial class commentbaitap
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public commentbaitap()
        {
            replycommentBTs = new HashSet<replycommentBT>();
        }

        [Key]
        public long Ma { get; set; }

        public long? MaBaiTap { get; set; }

        [StringLength(20)]
        public string Nguoidang { get; set; }

        public string Noidung { get; set; }

        public DateTime? Thoigiandang { get; set; }

        public virtual BaiTap BaiTap { get; set; }

        public virtual TaiKhoan TaiKhoan { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<replycommentBT> replycommentBTs { get; set; }
    }
}
