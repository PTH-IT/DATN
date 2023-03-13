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
        [Key]
        public long Ma { get; set; }

        public string Keyword { get; set; }
    }
}
