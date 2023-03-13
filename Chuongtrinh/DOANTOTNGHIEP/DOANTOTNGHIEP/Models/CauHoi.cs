namespace DOANTOTNGHIEP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CauHoi")]
    public partial class CauHoi
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CauHoi()
        {
            DapAns = new HashSet<DapAn>();
            TTBaiTapTNs = new HashSet<TTBaiTapTN>();
        }

        [Key]
        public long MaCauHoi { get; set; }

        public string NoiDung { get; set; }

        public DateTime? NgayThem { get; set; }

        public long? MaBaiTap { get; set; }

        public virtual BaiTap BaiTap { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DapAn> DapAns { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TTBaiTapTN> TTBaiTapTNs { get; set; }
    }
}
