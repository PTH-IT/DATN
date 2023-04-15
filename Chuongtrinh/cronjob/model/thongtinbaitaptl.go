package model

type ThongTinBaiTapTL struct {
	ID           int64         `gorm:"primary_key;column:ID"`
	MaBaiNop     int64         `gorm:"column:MaBaiNop"`
	NguoiNop     string        `gorm:"column:NguoiNop"`
	IDLibrary    int64         `gorm:"column:IDLibrary"`
	Isplagiarism bool          `gorm:"column:Isplagiarism"`
	Library      *Library      `gorm:"ForeignKey:ID;references:IDLibrary"`
	BaitapTL     *BaitapTL     `gorm:"ForeignKey:ID;references:MaBaiNop"`
	Taikhoan     *Taikhoan     `gorm:"ForeignKey:TenDangNhap;references:NguoiNop"`
	Plagiarism   []*Plagiarism `gorm:"ForeignKey:Mafile;references:ID"`
}
