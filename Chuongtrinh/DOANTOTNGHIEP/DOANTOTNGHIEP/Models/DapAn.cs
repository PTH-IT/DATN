namespace DOANTOTNGHIEP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DapAn")]
    public partial class DapAn
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DapAn()
        {
            TTBaiTapTNs = new HashSet<TTBaiTapTN>();
        }

        [Key]
        public long MaDapAn { get; set; }

        public string NoiDung { get; set; }

        public bool? LoaiDapAn { get; set; }

        public long? MaCauHoi { get; set; }

        public virtual CauHoi CauHoi { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TTBaiTapTN> TTBaiTapTNs { get; set; }
    }
}
