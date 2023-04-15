using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOANTOTNGHIEP.Models.GetData
{
    public class GetClass
    {
       
       
        public static List<LopHoc> GetLopHoc(string s)
        {
            DB db = new DB();
            List<LopHoc> lophoc = new List<LopHoc>();

            var lop = db.ThanhVienLops.Where(x => x.Mathanhvien.Equals(s)).OrderByDescending(x=>x.NgayThamGia).ToList();
            var lp = db.LopHocs.Select(x =>x).OrderByDescending(x=>x.NgayTao).ToList();
           
              foreach(var j in lop)
                {
                foreach (var i in lp)
                {
                    if (i.ID.Equals(j.ID))
                    {
                        lophoc.Add(i);

                    }

                }

            }
            return lophoc;

        }
            public static string getnameclass(string s)
        {
            
            DB db = new DB();
            var a = db.LopHocs.SingleOrDefault(x => x.ID.ToString().Equals(s)).TenLop;
            if (a == null)
            {
                return "";
            }
            else
            {
                return a;
            }
            

        }
        public static List<BaiTap> getbaitapdanop(string s, string user)
        {
            DB db = new DB();
            List<BaiTap> baitap = new List<BaiTap>();
            var bt = db.BaiTaps.Where(x => x.MaLop.ToString().Equals(s)).OrderByDescending(x=>x.ThoiGianKetThuc).ToList();
            foreach (var i in bt)
            {
                if (i.LoaiBaiTap.Equals("TuLuan"))
                {
                    var y = i.BaiTapTLs.SingleOrDefault(x => x.MaBaiTap.Equals(i.ID) && x.NguoiNop.Equals(user));

                    if (y != null)
                    {

                        if (y.NgayNop != null)
                        {
                            baitap.Add(i);
                        }

                    }
                    

                    
                }
                else if (i.LoaiBaiTap.Equals("TracNghiem"))
                {
                    var y = i.BaiTapTNs.SingleOrDefault(x => x.MaBaiTap.Equals(i.ID) && x.NguoiNop.Equals(user));
                    if (y != null)
                    {
                        
                        

                            if (y.NgayNop != null)
                            {
                                baitap.Add(i);
                            }


                        

                        

                    }

                }
               
            }
            return baitap;

        }


        public static List<BaiTap> getbaitapnopmuon(string s, string user)
        {
            DB db = new DB();
            List<BaiTap> baitap = new List<BaiTap>();
            var bt = db.BaiTaps.Where(x => x.MaLop.ToString().Equals(s)).OrderByDescending(x => x.ThoiGianKetThuc).ToList();
            foreach (var i in bt)
            {
                if (i.LoaiBaiTap.Equals("TuLuan"))
                {
                    var y = i.BaiTapTLs.SingleOrDefault(x => x.MaBaiTap.Equals(i.ID) && x.NguoiNop.Equals(user));


                    
                        if (y.NgayNop != null)
                        {
                            if (y.NgayNop > i.ThoiGianKetThuc)
                            {
                                baitap.Add(i);
                            }

                        }
                        

                    


                }
                else if (i.LoaiBaiTap.Equals("TracNghiem"))
                {
                    var y = i.BaiTapTNs.SingleOrDefault(x => x.MaBaiTap.Equals(i.ID) && x.NguoiNop.Equals(user));
                    if (y != null)
                    {

                        if (y.NgayNop != null)
                        {
                            if (y.NgayNop > i.ThoiGianKetThuc)
                            {
                                baitap.Add(i);
                            }

                        }



                    }

                }

            }
            return baitap;

        }

        public static List<BaiTap> getbaitapchuanop(string s ,string user)
        {
            DB db = new DB();
            List<BaiTap> baitap = new List<BaiTap>();
            var bt = db.BaiTaps.Where(x => x.MaLop.ToString().Equals(s)).OrderByDescending(x => x.ThoiGianKetThuc).ToList();
            foreach (var i in bt)
            {
                if (i.LoaiBaiTap.Equals("TuLuan"))
                {
                    var y = i.BaiTapTLs.SingleOrDefault(x => x.MaBaiTap.Equals(i.ID) && x.NguoiNop.Equals(user));

                    if (y != null)
                    {
                        if (y.NgayNop == null)
                        {
                            baitap.Add(i);
                        }

                    }
                    





                }
                else if (i.LoaiBaiTap.Equals("TracNghiem"))
                {
                    var y = i.BaiTapTNs.SingleOrDefault(x => x.MaBaiTap.Equals(i.ID) && x.NguoiNop.Equals(user));

                    if (y != null)
                    {


                        if (y.NgayNop == null)
                        {
                            baitap.Add(i);
                        }

                    }




                }

            }
            return baitap;

        }





    }
}