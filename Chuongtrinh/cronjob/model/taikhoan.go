package model

type Taikhoan struct {
	TenDangNhap string `gorm:"column:TenDangNhap"`
	MatKhau     string `gorm:"column:MatKhau"`
	Email       string `gorm:"column:Email"`
	Ho          string `gorm:"column:Ho"`
	Ten         string `gorm:"column:Ten"`
	HinhAnh     string `gorm:"column:HinhAnh"`
	Token       string `gorm:"column:token"`
	ChucVu      string `gorm:"column:ChucVu"`
}
