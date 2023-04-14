package gormdb

import (
	"cronjob-DATN/model"
	"cronjob-DATN/repository"
	"fmt"
)

func NewThongTinBaiTapTL() repository.ThongTinBaiTapTL {
	return thongtinbaitaptlRepository{}
}

type thongtinbaitaptlRepository struct {
}

func (r thongtinbaitaptlRepository) Getthongtinbaitaptuluan(mabaitap int64) []*model.ThongTinBaiTapTL {
	var thongtinbaitap []*model.ThongTinBaiTapTL
	DB.Table("TTBaiTapTL tt").Joins("JOIN BaiTapTL bt ON  bt.MaBaiTap = ? ", mabaitap).Joins("JOIN Library l ON tt.IDLibrary = l.ID").Where("bt.MaBaiTap = ?  ", mabaitap).Scan(&thongtinbaitap)
	if len(thongtinbaitap) == 0 {
		fmt.Println("thongtinbaitap null")
		return nil
	}
	return thongtinbaitap

}
