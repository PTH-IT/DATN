package usecase

import (
	"cronjob-DATN/model"
	"cronjob-DATN/repository"
	"cronjob-DATN/utils"
	"fmt"
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
	// i.KiemtradaovanALL(3)
	// utils.Highline("store/5acb582d-6bc9-4fa5-ad4e-a1e4cd64fb89.pdf")
	library := i.libraryRepository.GetforAll()
	k := 3
	if k > 1 && len(library) > 0 {
		cumdulieu := i.kmeansForModel(library, k)
		if cumdulieu == nil {
			return
		}
		for index, v := range cumdulieu {
			s := model.Chudetailieu{
				Chude: fmt.Sprintf("cluster %d", index),
			}
			chude := i.libraryRepository.SaveCluster(s)

			if chude == nil {
				return
			}
			for _, tailieu := range v {

				tailieu.MaNhom = *chude.ID
				i.libraryRepository.UpdateLibrary(tailieu)
			}

		}
	}

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
