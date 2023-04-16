package usecase

import (
	"cronjob-DATN/repository"
	"cronjob-DATN/utils"
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
func (i *Interactor) Upload(context echo.Context) error {
	file, err := context.FormFile("file")
	if err != nil {
		return context.JSON(http.StatusBadRequest, err)
	}
	err, path := utils.SaveFile(file)
	if err != nil {
		return context.JSON(http.StatusBadRequest, err)
	}
	return context.String(http.StatusOK, path)
}
func (i *Interactor) Download(context echo.Context) error {

	location := context.QueryParam("location")
	return context.File(location)
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
