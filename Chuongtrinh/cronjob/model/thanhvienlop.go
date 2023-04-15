package model

type Thanhvienlop struct {
	ID          int64     `gorm:"primary_key;column:ID"`
	Mathanhvien string    `gorm:"primary_key;column:Mathanhvien"`
	NgayThamGia string    `gorm:"column:NgayThamGia"`
	ChucVu      string    `gorm:"column:ChucVu"`
	LopHoc      *Lophoc   `gorm:"ForeignKey:ID;references:ID"`
	TaiKhoan    *Taikhoan `gorm:"ForeignKey:TenDangNhap;references:Mathanhvien"`
}
