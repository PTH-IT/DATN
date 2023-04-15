package model

type Document struct {
	ID           int64     `gorm:"primary_key;column:ID"`
	Ten          string    `gorm:"column:Ten"`
	Nguoisohuu   string    `gorm:"column:Nguoisohuu"`
	Image        string    `gorm:"column:Image"`
	MaLop        int64     `gorm:"column:MaLop"`
	LuotTaiXuong int64     `gorm:"column:LuotTaiXuong"`
	Luotxem      int64     `gorm:"column:Luotxem"`
	IDLibrary    int64     `gorm:"column:IDLibrary"`
	LopHoc       *Lophoc   `gorm:"ForeignKey:ID;references:MaLop;"`
	TaiKhoan     *Taikhoan `gorm:"ForeignKey:TenDangNhap;references:Nguoisohuu;"`
	Library      *Library  `gorm:"ForeignKey:ID;references:IDLibrary;"`
}
