namespace DOANTOTNGHIEP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Chudetailieu")]
    public partial class Chudetailieu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Chudetailieu()
        {
            Libraries = new HashSet<Library>();
        }

        public long ID { get; set; }

        public string Chude { get; set; }

        public double? x { get; set; }

        public double? y { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Library> Libraries { get; set; }
    }
}
