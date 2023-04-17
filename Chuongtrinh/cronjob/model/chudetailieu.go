package model

type Chudetailieu struct {
	ID             *int64            `gorm:"primary_key;column:ID"`
	Chude          string            `gorm:"column:Chude"`
	KeywordTailieu []*KeywordTailieu `gorm:"ForeignKey:Machude;references:ID"`
	Library        []*Library        `gorm:"ForeignKey:MaNhom;references:ID"`
}
