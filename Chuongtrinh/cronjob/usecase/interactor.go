package usecase

import (
	"cronjob-DATN/repository"
	"net/http"

	"github.com/labstack/echo/v4"
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
func (i *Interactor) CronJob() {

}
func (i *Interactor) BaiTap(context echo.Context) error {
	return context.String(http.StatusOK, "")
}
func (i *Interactor) LopHoc(context echo.Context) error {
	return context.String(http.StatusOK, "")
}
func (i *Interactor) All(context echo.Context) error {
	return context.String(http.StatusOK, "")
}
