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
	thongtinbaitaptuluanRepository repository.ThongTinBaiTapTLRepository,
	plagiarismRepository repository.PlagiarismRepository,
	documentRepository repository.DocumentRepository,
	libraryRepository repository.LibraryRepository,
) Interactor {

	return Interactor{
		gormDb,
		taikhoanRepository,
		thongtinbaitaptuluanRepository,
		plagiarismRepository,
		documentRepository,
		libraryRepository,
	}
}

type Interactor struct {
	gormDb                         *gorm.DB
	taikhoanRepository             repository.TaikhoanRepository
	thongtinbaitaptuluanRepository repository.ThongTinBaiTapTLRepository
	plagiarismRepository           repository.PlagiarismRepository
	documentRepository             repository.DocumentRepository
	libraryRepository              repository.LibraryRepository
}

func (i *Interactor) Gomcumdulieu() {
	// i.taikhoanRepository.GetTaikhoan()
	i.KiemtradaovanALL(3)
}
func (i *Interactor) CronJob() {

}

type jsonUploadFile struct {
	Location string
	Data     string
}

func (i *Interactor) Upload(context echo.Context) error {
	file, err := context.FormFile("file")
	if err != nil {
		return context.JSON(http.StatusBadRequest, err)
	}
	err, path, data := utils.SaveFile(file)
	if err != nil {
		return context.JSON(http.StatusBadRequest, err)
	}

	return context.JSON(http.StatusOK, jsonUploadFile{Data: data, Location: path})
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
