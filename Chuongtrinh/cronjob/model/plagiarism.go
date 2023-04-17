package model

type Plagiarism struct {
	ID               *int64            `gorm:"primary_key;autoIncrementIncrement;column:ID"`
	Percents         float64           `gorm:"column:Percents"`
	Mafile           int64             `gorm:"column:Mafile"`
	Loaikiemtra      string            `gorm:"column:Loaikiemtra"`
	Location         string            `gorm:"column:Location"`
	ThongTinBaiTapTL *ThongTinBaiTapTL `gorm:"ForeignKey:ID;references:Mafile"`
}
