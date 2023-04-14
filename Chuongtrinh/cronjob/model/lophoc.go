package model

type Lophoc struct {
	MaLop          int64    `gorm:"primary_key;column:MaLop"`
	TenLop         string   `gorm:"column:TenLop"`
	NgayTao        string   `gorm:"column:NgayTao"`
	NguoiTao       string   `gorm:"column:NguoiTao"`
	ThongTinLopHoc string   `gorm:"column:ThongTinLopHoc"`
	Hinhanh        string   `gorm:"column:Hinhanh"`
	Taikhoan       Taikhoan `gorm:"ForeignKey:NguoiTao;AssociationForeignKey:NguoiTao"`
}
