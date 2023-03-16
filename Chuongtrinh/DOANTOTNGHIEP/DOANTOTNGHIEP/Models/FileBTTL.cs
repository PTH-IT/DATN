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
        public long ID { get; set; }

        public long? MaBT { get; set; }

        public long? IDLibrary { get; set; }

        public virtual BaiTap BaiTap { get; set; }

        public virtual Library Library { get; set; }
    }
}
