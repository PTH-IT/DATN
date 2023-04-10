package model

type FileTB struct {
	ID         int64 `gorm:"column:ID"`
	Mathongbao int64 `gorm:"column:Mathongbao"`
	IDLibrary  int64 `gorm:"column:IDLibrary"`
}
