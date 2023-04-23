namespace DOANTOTNGHIEP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MessGroup")]
    public partial class MessGroup
    {
        public long ID { get; set; }

        [StringLength(20)]
        public string NguoiGui { get; set; }

        [StringLength(20)]
        public string NguoiNhan { get; set; }

        public long? MaGroup { get; set; }

        public string TinNhan { get; set; }

        public DateTime? thoigiangui { get; set; }

        public virtual GroupChat GroupChat { get; set; }

        public virtual TaiKhoan TaiKhoan { get; set; }

        public virtual TaiKhoan TaiKhoan1 { get; set; }
    }
}
