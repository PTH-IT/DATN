package gormdb

import (
	"cronjob-DATN/model"
	"cronjob-DATN/repository"
	"fmt"

	"gorm.io/gorm"
)

func NewLibraryRepository() repository.LibraryRepository {
	return libraryRepository{}
}

type libraryRepository struct {
}

func PreloadLibrary(db *gorm.DB) *gorm.DB {
	return db.Preload("Chudetailieu", func(db1 *gorm.DB) *gorm.DB {
		return db1.Table("Chudetailieu")
	}).Preload("Document", func(db1 *gorm.DB) *gorm.DB {
		return db1.Table("Document")
	}).Preload("FileBTTL", func(db1 *gorm.DB) *gorm.DB {
		return db1.Table("FileBTTL")
	}).Preload("FileTB", func(db1 *gorm.DB) *gorm.DB {
		return db1.Table("FileTB")
	}).Preload("TaiKhoan", func(db1 *gorm.DB) *gorm.DB {
		return db1.Table("TaiKhoan")
	}).Preload("ThongTinBaiTapTL", func(db1 *gorm.DB) *gorm.DB {
		return db1.Table("TTBaiTapTL")
	})
}

func (r libraryRepository) GetLibrary(idNhom int64) []*model.Library {
	var library []*model.Library
	db := PreloadLibrary(DB)

	db.Select("Library.*").Table("Library")
	if idNhom != 0 {
		db.Where("Library.MaNhom = ?", idNhom)
	}

	db.Order("Library.NgayUpdate").Find(&library)

	if len(library) == 0 {
		fmt.Println("thongtinbaitap null")
		return nil
	}
	return library
}
func (r libraryRepository) GetforLopHoc(maLop int64) []*model.Library {
	var library []*model.Library
	db := PreloadLibrary(DB)

	db.Select("Library.*").Table("Library").Joins("JOIN Document on Document.MaLop = ? ", maLop)
	db.Joins("JOIN ThongBao on ThongBao.MaLop = ? ", maLop).Joins("JOIN FileTB on FileTB.Mathongbao = ThongBao.ID ")
	db.Joins("JOIN LopHoc  on LopHoc.ID = ? ", maLop).Joins("JOIN BaiTap  on BaiTap.MaLop = LopHoc.ID")
	db.Joins("JOIN BaiTapTL ON  BaiTapTL.MaBaiTap = BaiTap.ID ").Joins("JOIN TTBaiTapTL on TTBaiTapTL.MaBaiNop = BaiTapTL.ID ")
	db.Where("Library.ID = FileTB.IDLibrary or Library.ID = Document.IDLibrary or TTBaiTapTL.IDLibrary = Library.ID  ")
	db.Distinct("Library.*").Order("Library.NgayUpdate").Find(&library)

	if len(library) == 0 {
		fmt.Println("thongtinbaitap null")
		return nil
	}
	return library
}

func (r libraryRepository) GetforAll() []*model.Library {
	var library []*model.Library
	db := PreloadLibrary(DB)

	db.Select("Library.*").Table("Library")

	db.Order("Library.NgayUpdate").Find(&library)

	if len(library) == 0 {
		fmt.Println("thongtinbaitap null")
		return nil
	}
	return library
}

func (r libraryRepository) SaveCluster(chude model.Chudetailieu) *model.Chudetailieu {
	// DB.Raw("INSERT INTO Chudetailieu (Chude) VALUES ( ? )", chude.Chude).ScanRows(&Chudetailieu)
	DB.Table("Chudetailieu").Create(&chude)
	return &chude
}
func (r libraryRepository) UpdateLibrary(tailieu model.Library) {
	DB.Table("Library").Where("ID = ? ", tailieu.ID).UpdateColumn("MaNhom", tailieu.MaNhom)
}
