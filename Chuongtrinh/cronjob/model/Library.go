package model

type Library struct {
	ID           int64  `gorm:"column:ID"`
	Name         string `gorm:"column:Name"`
	Location     string `gorm:"column:Location"`
	NgayThem     string `gorm:"column:NgayThem"`
	NgayUpdate   string `gorm:"column:NgayUpdate"`
	MaNhom       int64  `gorm:"column:MaNhom"`
	Noidung      string `gorm:"column:Noidung"`
	NguoiAdd     string `gorm:"column:NguoiAdd"`
	Chudetailieu ChudeTaiLieu
	TaiKhoan     Taikhoan
}
