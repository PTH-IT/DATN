package model

type KeywordTailieu struct {
	ID           *int64        `gorm:"primary_key;column:ID"`
	Keyword      string        `gorm:"column:Keyword"`
	Machude      int64         `gorm:"column:Machude"`
	Chudetailieu *Chudetailieu `gorm:"ForeignKey:ID;references:Machude"`
}
