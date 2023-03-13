namespace DOANTOTNGHIEP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FileBTTL")]
    public partial class FileBTTL
    {
        [Key]
        public long Mafile { get; set; }

        public string NoiLuu { get; set; }

        public long? MaBt { get; set; }

        [StringLength(200)]
        public string TenFile { get; set; }

        public virtual BaiTap BaiTap { get; set; }
    }
}
