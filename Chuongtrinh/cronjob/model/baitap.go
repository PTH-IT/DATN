package model

type Baitap struct {
	ID              int64       `gorm:"primary_key;column:ID"`
	ChuDe           string      `gorm:"column:ChuDe"`
	LoaiBaiTap      string      `gorm:"column:LoaiBaiTap"`
	ThoiGianDang    string      `gorm:"column:ThoiGianDang"`
	ThoiGianKetThuc string      `gorm:"column:ThoiGianKetThuc"`
	MaLop           int64       `gorm:"column:MaLop"`
	NguoiTao        string      `gorm:"column:NguoiTao"`
	Thongtin        string      `gorm:"column:Thongtin"`
	Lophoc          *Lophoc     `gorm:"ForeignKey:ID;references:MaLop"`
	BaitapTL        []*BaitapTL `gorm:"ForeignKey:MaBaiTap;references:ID"`
	FileBTTLs       []*FileBTTL `gorm:"ForeignKey:MaBT;references:ID"`
}
