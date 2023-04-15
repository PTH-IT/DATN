package model

type BaitapTL struct {
	ID               int64               `gorm:"primary_key;column:ID"`
	MaBaiTap         int64               `gorm:"column:MaBaiTap"`
	Trangthai        bool                `gorm:"column:Trangthai"`
	NgayNop          string              `gorm:"column:NgayNop"`
	NguoiNop         string              `gorm:"column:NguoiNop"`
	Diem             int64               `gorm:"column:Diem"`
	BaiTap           *Baitap             `gorm:"ForeignKey:ID;references:MaBaiTap"`
	TaiKhoan         *Taikhoan           `gorm:"ForeignKey:TenDangNhap;references:NguoiNop"`
	ThongTinBaiTapTL []*ThongTinBaiTapTL `gorm:"ForeignKey:MaBaiNop;references:ID"`
}
