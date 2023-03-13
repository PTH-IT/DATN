namespace DOANTOTNGHIEP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TTBaiTapTN")]
    public partial class TTBaiTapTN
    {
        [Key]
        public long Ma { get; set; }

        public long? MaCauHoi { get; set; }

        public long? MaDapAnluaChon { get; set; }

        public long? MaBaiNop { get; set; }

        [StringLength(20)]
        public string NguoiNop { get; set; }

        public virtual BaiTapTN BaiTapTN { get; set; }

        public virtual CauHoi CauHoi { get; set; }

        public virtual DapAn DapAn { get; set; }

        public virtual TaiKhoan TaiKhoan { get; set; }
    }
}
