package model

type BaitapTL struct {
	MaBaiNop  int64    `gorm:"primary_key;column:MaBaiNop"`
	MaBaiTap  int64    `gorm:"column:MaBaiTap"`
	Trangthai bool     `gorm:"column:Trangthai"`
	NgayNop   string   `gorm:"column:NgayNop"`
	NguoiNop  string   `gorm:"column:NguoiNop"`
	Diem      int64    `gorm:"column:Diem"`
	BaiTap    Baitap   `gorm:"ForeignKey:MaBaiTap;AssociationForeignKey:MaBaiTap"`
	TaiKhoan  Taikhoan `gorm:"ForeignKey:NguoiNop;AssociationForeignKey:NguoiNop"`
}
