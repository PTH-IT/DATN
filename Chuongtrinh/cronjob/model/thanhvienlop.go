package model

type Thanhvienlop struct {
	MaLop       int64  `gorm:"column:MaLop"`
	Mathanhvien string `gorm:"column:Mathanhvien"`
	NgayThamGia string `gorm:"column:NgayThamGia"`
	ChucVu      string `gorm:"column:ChucVu"`
	LopHoc      Lophoc
	TaiKhoan    Taikhoan
}
