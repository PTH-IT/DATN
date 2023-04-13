package model

type KeywordTailieu struct {
	MaKeyword    int64  `gorm:"column:MaKeyword"`
	Keyword      string `gorm:"column:Keyword"`
	Machude      int64  `gorm:"column:Machude"`
	Chudetailieu ChudeTaiLieu
}
