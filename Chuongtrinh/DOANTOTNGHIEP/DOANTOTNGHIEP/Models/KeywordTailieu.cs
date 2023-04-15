namespace DOANTOTNGHIEP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KeywordTailieu")]
    public partial class KeywordTailieu
    {
        public long ID { get; set; }

        public string Keyword { get; set; }

        public long? Machude { get; set; }

        public virtual Chudetailieu Chudetailieu { get; set; }
    }
}
