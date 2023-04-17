package model

type FileBTTL struct {
	ID        *int64   `gorm:"primary_key;column:ID"`
	MaBT      int64    `gorm:"column:MaBT"`
	IDLibrary int64    `gorm:"column:IDLibrary"`
	Baitap    *Baitap  `gorm:"ForeignKey:ID;references:MaBT"`
	Library   *Library `gorm:"ForeignKey:ID;references:IDLibrary"`
}
