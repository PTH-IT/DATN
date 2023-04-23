namespace DOANTOTNGHIEP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MemberGroup")]
    public partial class MemberGroup
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long MaGroup { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string IDMember { get; set; }

        public DateTime? ThoiGianThamGia { get; set; }

        public virtual GroupChat GroupChat { get; set; }

        public virtual TaiKhoan TaiKhoan { get; set; }
    }
}
