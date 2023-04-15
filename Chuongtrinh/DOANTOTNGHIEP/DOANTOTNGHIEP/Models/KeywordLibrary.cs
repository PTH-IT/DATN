namespace DOANTOTNGHIEP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KeywordLibrary")]
    public partial class KeywordLibrary
    {
        public long ID { get; set; }

        public string Keyword { get; set; }
    }
}
