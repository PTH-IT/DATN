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
        public long ID { get; set; }

        public long? Mathongbao { get; set; }

        public long? IDLibrary { get; set; }

        public virtual ThongBao ThongBao { get; set; }

        public virtual Library Library { get; set; }
    }
}
