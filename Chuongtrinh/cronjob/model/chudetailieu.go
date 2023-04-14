package model

type Chudetailieu struct {
	Machude int64  `gorm:"primary_key;column:Machude"`
	Chude   string `gorm:"column:Chude"`
}
