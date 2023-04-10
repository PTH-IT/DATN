package model

type Document struct {
	Ma           int64  `gorm:"column:Ma"`
	Ten          string `gorm:"column:Ten"`
	Nguoisohuu   string `gorm:"column:Nguoisohuu"`
	Image        string `gorm:"column:Image"`
	MaLop        int64  `gorm:"column:MaLop"`
	LuotTaiXuong int64  `gorm:"column:LuotTaiXuong"`
	Luotxem      int64  `gorm:"column:Luotxem"`
	IDLibrary    int64  `gorm:"column:IDLibrary"`
}
