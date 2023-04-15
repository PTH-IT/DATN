namespace DOANTOTNGHIEP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("replycommentBT")]
    public partial class replycommentBT
    {
        public long ID { get; set; }

        public long? MaComment { get; set; }

        [StringLength(20)]
        public string Nguoidang { get; set; }

        public string Noidung { get; set; }

        public DateTime? Thoigiandang { get; set; }

        public virtual commentbaitap commentbaitap { get; set; }

        public virtual TaiKhoan TaiKhoan { get; set; }
    }
}
