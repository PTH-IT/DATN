package usecase

import (
	"cronjob-DATN/model"
	"cronjob-DATN/repository"
	"cronjob-DATN/utils"
	"fmt"
	"net/http"
	"strconv"

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

	// i.Gomcumdulieu()
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
	mabaitap := context.QueryParam("mabaitap")
	malop := context.QueryParam("malop")
	number, error := strconv.Atoi(mabaitap)
	if error != nil {
		return context.String(http.StatusBadRequest, "mabaitap is not a number")
	}
	numbermalop, error := strconv.Atoi(malop)
	if error != nil {
		return context.String(http.StatusBadRequest, "malop is not a number")
	}
	i.Kiemtradaovanbaitap(int64(number), int64(numbermalop))

	return context.String(http.StatusOK, "")
}
func (i *Interactor) LopHoc(context echo.Context) error {
	mabaitap := context.QueryParam("mabaitap")
	malop := context.QueryParam("malop")
	number, error := strconv.Atoi(mabaitap)
	if error != nil {
		return context.String(http.StatusBadRequest, "mabaitap is not a number")
	}
	numbermalop, error := strconv.Atoi(malop)
	if error != nil {
		return context.String(http.StatusBadRequest, "malop is not a number")
	}
	i.Kiemtradaovanlophoc(int64(number), int64(numbermalop))

	return context.String(http.StatusOK, "")
}
func (i *Interactor) All(context echo.Context) error {
	mabaitap := context.QueryParam("mabaitap")
	number, error := strconv.Atoi(mabaitap)
	if error != nil {
		return context.String(http.StatusBadRequest, "mabaitap is not a number")
	}
	i.KiemtradaovanALL(int64(number))

	return context.String(http.StatusOK, "")
}
