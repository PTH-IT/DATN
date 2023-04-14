package model

type Baitap struct {
	MaBaiTap        int64  `gorm:"primary_key;column:MaBaiTap"`
	ChuDe           string `gorm:"column:ChuDe"`
	LoaiBaiTap      string `gorm:"column:LoaiBaiTap"`
	ThoiGianDang    string `gorm:"column:ThoiGianDang"`
	ThoiGianKetThuc string `gorm:"column:ThoiGianKetThuc"`
	MaLop           int64  `gorm:"column:MaLop"`
	NguoiTao        string `gorm:"column:NguoiTao"`
	Thongtin        string `gorm:"column:Thongtin"`
	Lophoc          Lophoc `gorm:"ForeignKey:MaLop;AssociationForeignKey:MaLop"`
}
