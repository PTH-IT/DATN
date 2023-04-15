namespace DOANTOTNGHIEP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Plagiarism
    {
        public long ID { get; set; }

        public double? Percents { get; set; }

        public long? Mafile { get; set; }

        public string Loaikiemtra { get; set; }

        public string Location { get; set; }

        public virtual TTBaiTapTL TTBaiTapTL { get; set; }
    }
}
