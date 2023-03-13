using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DOANTOTNGHIEP.Models;


namespace DOANTOTNGHIEP.Modelcreate
{
    public class sortmesstime: IComparer<Mess>
    {
        public int Compare( Mess x,  Mess y)
        {
            return x.thoigiangui.Value.CompareTo(y.thoigiangui.Value);
            //return y.Name.CompareTo(x.Name); ////Thử đổi phần return như này xem kết quả thay đổi ra sao nhé
        }
    }
}