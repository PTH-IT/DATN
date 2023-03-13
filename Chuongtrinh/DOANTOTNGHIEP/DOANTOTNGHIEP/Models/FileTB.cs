namespace DOANTOTNGHIEP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FileTB")]
    public partial class FileTB
    {
        [Key]
        public long Mafile { get; set; }

        [StringLength(200)]
        public string NoiLuu { get; set; }

        public long? maTB { get; set; }

        public string TenFile { get; set; }

        public virtual ThongBao ThongBao { get; set; }
    }
}
