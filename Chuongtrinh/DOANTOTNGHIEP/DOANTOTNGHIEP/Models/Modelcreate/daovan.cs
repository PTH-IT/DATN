using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOANTOTNGHIEP.Models.Modelcreate
{
    public class daovan
    {
        public TTBaiTapTL file1 { get; set; }
        public List<DOANTOTNGHIEP.Models.Modelcreate.detaildaovan> sentence { get; set; }
        public float per { get; set; }

    }
    public class detaildaovan
    {

        public string Keywor1 { get; set; }
        public string Keywor2 { get; set; }
        public string locationfilecompare { get; set; }
        public List<string> sentenceKeyword { get; set; }
        public float per { get; set; }

    }
}