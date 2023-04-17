package model

type Lophoc struct {
	ID             *int64          `gorm:"primary_key;column:ID"`
	TenLop         string          `gorm:"column:TenLop"`
	NgayTao        string          `gorm:"column:NgayTao"`
	NguoiTao       string          `gorm:"column:NguoiTao"`
	ThongTinLopHoc string          `gorm:"column:ThongTinLopHoc"`
	Hinhanh        string          `gorm:"column:Hinhanh"`
	Taikhoan       *Taikhoan       `gorm:"ForeignKey:TenDangNhap;references:NguoiTao"`
	Baitap         []*Baitap       `gorm:"ForeignKey:MaLop;references:ID"`
	Document       []*Document     `gorm:"ForeignKey:MaLop;references:ID"`
	Thanhvienlop   []*Thanhvienlop `gorm:"ForeignKey:ID;references:ID"`
}
