package model

type Plagiarism struct {
	Ma          int64   `gorm:"column:Ma"`
	Percents    float64 `gorm:"column:Percents"`
	Mafile      int64   `gorm:"column:Mafile"`
	Loaikiemtra string  `gorm:"column:Loaikiemtra"`
	Location    string  `gorm:"column:Location"`
}
