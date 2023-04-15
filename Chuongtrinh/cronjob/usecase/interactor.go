package usecase

import (
	"cronjob-DATN/repository"

	"gorm.io/gorm"
)

func NewInteractor(
	gormDb *gorm.DB,
	taikhoanRepository repository.TaikhoanRepository,
	thongtinbaitaptuluanRepository repository.ThongTinBaiTapTL,
	plagiarismRepository repository.PlagiarismRepository,
) Interactor {

	return Interactor{
		gormDb,
		taikhoanRepository,
		thongtinbaitaptuluanRepository,
		plagiarismRepository,
	}
}

type Interactor struct {
	gormDb                         *gorm.DB
	taikhoanRepository             repository.TaikhoanRepository
	thongtinbaitaptuluanRepository repository.ThongTinBaiTapTL
	plagiarismRepository           repository.PlagiarismRepository
}

func (i *Interactor) Gomcumdulieu() {
	// i.taikhoanRepository.GetTaikhoan()
	x := i.thongtinbaitaptuluanRepository.Getthongtinbaitaptuluan(3)
	if x == nil {

	}
}
