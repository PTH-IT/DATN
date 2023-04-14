package repository

import "cronjob-DATN/model"

type ThongTinBaiTapTL interface {
	Getthongtinbaitaptuluan(mabaitap int64) []*model.ThongTinBaiTapTL
}
