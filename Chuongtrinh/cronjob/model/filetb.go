package model

type FileTB struct {
	ID         *int64   `gorm:"primary_key;column:ID"`
	Mathongbao int64    `gorm:"column:Mathongbao"`
	IDLibrary  int64    `gorm:"column:IDLibrary"`
	Library    *Library `gorm:"ForeignKey:ID;references:IDLibrary"`
}
