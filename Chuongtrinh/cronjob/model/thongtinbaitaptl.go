package model

type ThongTinBaiTapTL struct {
	Ma           int64    `gorm:"primary_key;column:Ma"`
	MaBaiNop     int64    `gorm:"column:MaBaiNop"`
	NguoiNop     string   `gorm:"column:NguoiNop"`
	IDLibrary    int64    `gorm:"column:IDLibrary"`
	Isplagiarism bool     `gorm:"column:Isplagiarism"`
	Library      Library  `gorm:"ForeignKey:IDLibrary;AssociationForeignKey:IDLibrary"`
	BaiTapTuLuan BaitapTL `gorm:"ForeignKey:MaBaiNop;AssociationForeignKey:MaBaiNop"`
	TaiKhoan     Taikhoan `gorm:"ForeignKey:NguoiNop;AssociationForeignKey:NguoiNop"`
}
