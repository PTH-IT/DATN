package repository

import "cronjob-DATN/model"

type ThongTinBaiTapTLRepository interface {
	Getthongtinbaitaptuluan(mabaitap int64) []*model.ThongTinBaiTapTL
	Getthongtinbaitaptuluancholophoc(malop int64) []*model.ThongTinBaiTapTL
	GetthongtinbaitaptuluanchoAll() []*model.ThongTinBaiTapTL
	GetthongtinbaitapForCronjob() []*model.ThongTinBaiTapTL
}
