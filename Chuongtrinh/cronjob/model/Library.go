package model

type Library struct {
	ID           int64         `gorm:"primary_key;column:ID"`
	Name         string        `gorm:"column:Name"`
	Location     string        `gorm:"column:Location"`
	NgayThem     string        `gorm:"column:NgayThem"`
	NgayUpdate   string        `gorm:"column:NgayUpdate"`
	MaNhom       int64         `gorm:"column:MaNhom"`
	Noidung      string        `gorm:"column:Noidung"`
	NguoiAdd     string        `gorm:"column:NguoiAdd"`
	Chudetailieu *Chudetailieu `gorm:"ForeignKey:MaNhom;AssociationForeignKey:MaNhom"`
	TaiKhoan     *Taikhoan     `gorm:"ForeignKey:NguoiAdd;AssociationForeignKey:NguoiAdd"`
}
