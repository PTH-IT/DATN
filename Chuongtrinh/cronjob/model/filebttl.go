package model

type FileBTTL struct {
	ID        int64 `gorm:"column:ID"`
	MaBT      int64 `gorm:"column:MaBT"`
	IDLibrary int64 `gorm:"column:IDLibrary"`
	Baitap    Baitap
	Library   Library
}
