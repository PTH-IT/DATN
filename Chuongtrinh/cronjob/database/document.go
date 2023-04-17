package gormdb

import (
	"cronjob-DATN/model"
	"cronjob-DATN/repository"
	"fmt"

	"gorm.io/gorm"
)

func NewDocumentRepository() repository.DocumentRepository {
	return documentRepository{}
}

type documentRepository struct {
}

func PreloadDocument(db *gorm.DB) *gorm.DB {
	return db.Preload("LopHoc", func(db1 *gorm.DB) *gorm.DB {
		return db1.Table("Lophoc")
	}).Preload("TaiKhoan", func(db1 *gorm.DB) *gorm.DB {
		return db1.Table("Taikhoan")
	}).Preload("Library", func(db1 *gorm.DB) *gorm.DB {
		return db1.Table("Library")
	})
}
func (r documentRepository) GetDocumentcholophoc(malop int64) []*model.Document {
	var document []*model.Document
	db := PreloadDocument(DB)

	db.Select("Document.*").Table("Document").Joins("JOIN Library  ON Document.IDLibrary = Library.ID").Where("Document.MaLop = ?", malop).Order("Library.NgayUpdate").Find(&document)

	if len(document) == 0 {
		fmt.Println("thongtinbaitap null")
		return nil
	}
	return document

}
