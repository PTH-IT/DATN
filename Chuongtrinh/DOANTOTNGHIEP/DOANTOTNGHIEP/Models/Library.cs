namespace DOANTOTNGHIEP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Library")]
    public partial class Library
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Library()
        {
            documents = new HashSet<document>();
            FileBTTLs = new HashSet<FileBTTL>();
            FileTBs = new HashSet<FileTB>();
            TTBaiTapTLs = new HashSet<TTBaiTapTL>();
        }

        public long ID { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public DateTime? NgayThem { get; set; }

        public DateTime? NgayUpdate { get; set; }

        public long? MaNhom { get; set; }

        public string Noidung { get; set; }

        [StringLength(20)]
        public string NguoiAdd { get; set; }

        public bool? Isplagiarism { get; set; }

        public virtual Chudetailieu Chudetailieu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<document> documents { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FileBTTL> FileBTTLs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FileTB> FileTBs { get; set; }

        public virtual TaiKhoan TaiKhoan { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TTBaiTapTL> TTBaiTapTLs { get; set; }
    }
}
