package gormdb

import (
	"cronjob-DATN/model"
	"cronjob-DATN/repository"
	"fmt"

	"gorm.io/gorm"
)

func NewThongTinBaiTapTL() repository.ThongTinBaiTapTL {
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

	db.Select("tt.*").Table("TTBaiTapTL tt").Joins("JOIN BaiTapTL bt ON  bt.MaBaiTap = ? ", maBaiTap).Joins("JOIN Library l ON tt.IDLibrary = l.ID").Where("bt.MaBaiTap = ?  ", maBaiTap).Find(&thongtinbaitap)

	if len(thongtinbaitap) == 0 {
		fmt.Println("thongtinbaitap null")
		return nil
	}
	return thongtinbaitap

}
