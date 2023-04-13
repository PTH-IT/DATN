package model

type ThongTinBaiTapTL struct {
	Ma           int64  `gorm:"column:Ma"`
	MaBaiNop     int64  `gorm:"column:MaBaiNop"`
	NguoiNop     string `gorm:"column:NguoiNop"`
	IDLibrary    int64  `gorm:"column:IDLibrary"`
	Isplagiarism bool   `gorm:"column:Isplagiarism"`
	Library      Library
	BaiTapTuLuan BaitapTL
	TaiKhoan     Taikhoan
}
