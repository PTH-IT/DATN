package gormdb

import (
	"cronjob-DATN/model"
	"cronjob-DATN/repository"
	"fmt"

	"gorm.io/gorm"
)

func NewThongTinBaiTapTLRepository() repository.ThongTinBaiTapTLRepository {
	return thongtinbaitaptlRepository{}
}

type thongtinbaitaptlRepository struct {
}

func PreloadTTBTTL(db *gorm.DB) *gorm.DB {
	return db.Preload("Library", func(db1 *gorm.DB) *gorm.DB {
		return db1.Table("Library")
	}).Preload("BaitapTL", func(db1 *gorm.DB) *gorm.DB {
		return db1.Table("BaiTapTL")
	}).Preload("Taikhoan", func(db1 *gorm.DB) *gorm.DB {
		return db1.Table("TaiKhoan")
	}).Preload("Plagiarism", func(db1 *gorm.DB) *gorm.DB {
		return db1.Table("Plagiarism")
	})
}
func (r thongtinbaitaptlRepository) Getthongtinbaitaptuluan(maBaiTap int64) []*model.ThongTinBaiTapTL {
	var thongtinbaitap []*model.ThongTinBaiTapTL
	db := PreloadTTBTTL(DB)

	db.Select("TTBaiTapTL.*").Table("TTBaiTapTL").Joins("JOIN BaiTapTL  ON  BaiTapTL.MaBaiTap = ? ", maBaiTap).Joins("JOIN Library  ON TTBaiTapTL.IDLibrary = Library.ID").Where("TTBaiTapTL.MaBaiNop = BaiTapTL.ID  ").Order("Library.NgayUpdate").Find(&thongtinbaitap)

	if len(thongtinbaitap) == 0 {
		fmt.Println("thongtinbaitap null")
		return nil
	}
	return thongtinbaitap

}

func (r thongtinbaitaptlRepository) Getthongtinbaitaptuluancholophoc(malop int64) []*model.ThongTinBaiTapTL {
	var thongtinbaitap []*model.ThongTinBaiTapTL
	db := PreloadTTBTTL(DB)

	db.Select("TTBaiTapTL.*").Table("TTBaiTapTL").Joins("JOIN LopHoc  on LopHoc.ID = ? ", malop).Joins("JOIN BaiTap  on BaiTap.MaLop = LopHoc.ID").Joins("JOIN BaiTapTL ON  BaiTapTL.MaBaiTap = BaiTap.ID ").Joins("JOIN Library  ON TTBaiTapTL.IDLibrary = Library.ID").Where("TTBaiTapTL.MaBaiNop = BaiTapTL.ID").Order("Library.NgayUpdate").Find(&thongtinbaitap)

	if len(thongtinbaitap) == 0 {
		fmt.Println("thongtinbaitap null")
		return nil
	}
	return thongtinbaitap

}
