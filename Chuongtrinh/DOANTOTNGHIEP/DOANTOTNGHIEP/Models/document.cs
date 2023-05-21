namespace DOANTOTNGHIEP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("document")]
    public partial class document
    {
        public long ID { get; set; }

        public string Ten { get; set; }

        [StringLength(20)]
        public string Nguoisohuu { get; set; }


        public long? MaLop { get; set; }

        public int? LuotTaiXuong { get; set; }

        public int? Luotxem { get; set; }

        public long? IDLibrary { get; set; }

        public virtual LopHoc LopHoc { get; set; }

        public virtual TaiKhoan TaiKhoan { get; set; }

        public virtual Library Library { get; set; }
    }
}
