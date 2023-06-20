package model

type Chudetailieu struct {
	ID      *int64     `gorm:"primary_key;column:ID"`
	Chude   string     `gorm:"column:Chude"`
	X       float64    `gorm:"column:x"`
	Y       float64    `gorm:"column:y"`
	Library []*Library `gorm:"ForeignKey:MaNhom;references:ID"`
}
