using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DOANTOTNGHIEP.Modelcreate;


namespace DOANTOTNGHIEP.Models.GetData
{
    public class Getdiem
    {
        public static List<Diem> danhsachdiem(string malop, string user )
        {
            DB db = new DB();
            List<Diem> diem = new List<Diem>();
            var d = db.BaiTaps.Where(x => x.MaLop.ToString().Equals(malop)).ToList();
            List<ThanhVienLop> thanhvienlop = new List<ThanhVienLop>();

            var checkthanhVien = db.ThanhVienLops.SingleOrDefault(x => x.MaLop.ToString().Equals(malop) && x.ChucVu.ToLower().Equals("sinhvien".ToLower()) && x.Mathanhvien.Equals(user));



            if (checkthanhVien != null)
            {
                thanhvienlop.Add(checkthanhVien);
            }
            else
            {
                thanhvienlop = db.ThanhVienLops.Where(x => x.MaLop.ToString().Equals(malop) && x.ChucVu.ToLower().Equals("sinhvien".ToLower())).OrderBy(x => x.TaiKhoan.Ten).ToList();
            }
           

            foreach (var j in thanhvienlop)
            {
                int sl = 0;
                int slchuanop = 0;
                int slnopmuon = 0;
                int slcodiem = 0;
                Diem diemtv = new Diem();
                long? dim = 0;

                foreach (var i in d)
                {
                    if (i.LoaiBaiTap.Equals("TracNghiem"))
                    {
                        var diemtn = db.BaiTapTNs.SingleOrDefault(x => x.MaBaiTap.ToString().Equals(i.MaBaiTap.ToString()) && x.NguoiNop.Equals(j.Mathanhvien));
                      
                        if (diemtn != null && diemtn.NgayNop != null)
                        {
                            if (diemtn.Diem != null)
                            {
                                dim = dim + diemtn.Diem;
                                slcodiem++;
                            }
                            sl++;
                        }
                        if (diemtn == null)
                        {
                            slchuanop++;
                        }
                        if (diemtn != null && diemtn.NgayNop > i.ThoiGianKetThuc)
                        {
                            slnopmuon++;

                        }


                    }
                    else
                    {

                        var diemtn = db.BaiTapTLs.SingleOrDefault(x => x.MaBaiTap.ToString().Equals(i.MaBaiTap.ToString()) && x.NguoiNop.Equals(j.Mathanhvien));
                            if (diemtn != null && diemtn.NgayNop != null)
                        {
                            if (diemtn.Diem != null)
                            {
                                dim = dim + diemtn.Diem;
                                slcodiem++;
                            }

                            sl++;
                        }
                        if (diemtn == null)
                        {
                            slchuanop++;
                        }
                        if (diemtn != null && diemtn.NgayNop > i.ThoiGianKetThuc)
                        {
                            slnopmuon++;

                        }

                    }


                }
                diemtv.taikhoan = j.Mathanhvien;
                diemtv.soluongchuanop = slchuanop;
                diemtv.soluongtre = slnopmuon;
                if (slcodiem > 0)
                {
                    diemtv.diem = dim / long.Parse(slcodiem.ToString());

                }
                diemtv.ngaythamgia = j.NgayThamGia.Value;
                diemtv.email = j.TaiKhoan.Email;
                diemtv.Hoten = j.TaiKhoan.Ho + " " + j.TaiKhoan.Ten;
                diemtv.soluongbaitap = sl;
                diemtv.anh = j.TaiKhoan.HinhAnh;
                diemtv.soluongbaicodiem = slcodiem;
                diem.Add(diemtv);
            }

            return diem;

        }

    }
}