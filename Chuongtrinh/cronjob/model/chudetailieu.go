package model

type ChudeTaiLieu struct {
	Machude int64  `gorm:"column:Machude"`
	Chude   string `gorm:"column:Chude"`
}
