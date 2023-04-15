package gormdb

import (
	"log"
	"os"
	"time"

	"gorm.io/gorm"
	"gorm.io/gorm/logger"
)

var DB *gorm.DB

func Start(gormdb *gorm.DB) {
	// gormdb.Preload("BaiTap", func(db *gorm.DB) *gorm.DB {
	// 	return db.Table("BaiTap")
	// }).Preload("BaiTapTL", func(db *gorm.DB) *gorm.DB {
	// 	return db.Table("BaiTapTL")
	// }).Preload("Chudetailieu", func(db *gorm.DB) *gorm.DB {
	// 	return db.Table("Chudetailieu")
	// }).Preload("document", func(db *gorm.DB) *gorm.DB {
	// 	return db.Table("document")
	// }).Preload("FileBTTL", func(db *gorm.DB) *gorm.DB {
	// 	return db.Table("FileBTTL")
	// }).Preload("FileTB", func(db *gorm.DB) *gorm.DB {
	// 	return db.Table("FileTB")
	// }).Preload("KeywordTailieu", func(db *gorm.DB) *gorm.DB {
	// 	return db.Table("KeywordTailieu")
	// }).Preload("Library", func(db *gorm.DB) *gorm.DB {
	// 	return db.Table("Library")
	// }).Preload("LopHoc", func(db *gorm.DB) *gorm.DB {
	// 	return db.Table("LopHoc")
	// }).Preload("Plagiarism", func(db *gorm.DB) *gorm.DB {
	// 	return db.Table("Plagiarism")
	// }).Preload("TaiKhoan", func(db *gorm.DB) *gorm.DB {
	// 	return db.Table("TaiKhoan")
	// }).Preload("ThanhVienLop", func(db *gorm.DB) *gorm.DB {
	// 	return db.Table("ThanhVienLop")
	// }).Preload("TTBaiTapTL", func(db *gorm.DB) *gorm.DB {
	// 	return db.Table("TTBaiTapTL")
	// })

	logger := logger.New(
		log.New(os.Stdout, "\r\n", log.LstdFlags), // io writer
		logger.Config{
			SlowThreshold:             time.Second, // Thời gian tối đa để được xem là truy vấn chậm
			LogLevel:                  logger.Info, // Log level
			IgnoreRecordNotFoundError: true,        // Bỏ qua lỗi khi không tìm thấy bản ghi
			Colorful:                  true,        // Sử dụng màu sắc
		},
	)
	gormdb.Logger = logger
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
