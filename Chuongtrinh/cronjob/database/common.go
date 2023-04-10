package gormdb

import "gorm.io/gorm"

var DB *gorm.DB

func Start(gormdb *gorm.DB) {
	DB = gormdb
}
func Begin() *gorm.DB {
	DB = DB.Begin()
	return DB
}
func Commit() *gorm.DB {
	DB = DB.Commit()
	return DB
}
