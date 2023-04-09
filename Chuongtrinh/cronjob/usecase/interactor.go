package usecase

import (
	"cronjob-DATN/repository"

	"gorm.io/gorm"
)

func NewInteractor(
	gormDb *gorm.DB,
	taikhoanRepository repository.TaikhoanRepository,
) Interactor {

	return Interactor{
		gormDb,
		taikhoanRepository,
	}
}

type Interactor struct {
	gormDb             *gorm.DB
	taikhoanRepository repository.TaikhoanRepository
}

func (i *Interactor) Gomcumdulieu() {
	i.taikhoanRepository.GetTaikhoan()
}
