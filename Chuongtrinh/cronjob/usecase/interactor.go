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

	k := 2
	chudetailieu := i.libraryRepository.GetChuDe()
	if chudetailieu != nil {
		k = len(chudetailieu)
	}
	if k > 1 && len(library) > 0 {
		cumdulieu, center := i.KmeansForModel(library, k)
		if cumdulieu == nil {
			return
		}
		numbercluter := 0
		for index, v := range cumdulieu {
			numbercluter++
			if len(v) == 0 {
				continue
			}
			s := model.Chudetailieu{
				Chude: fmt.Sprintf("cụm %d", numbercluter),
				X:     center[index][0],
				Y:     center[index][1],
			}
			id := -1

			if len(chudetailieu) > 0 {
				s.ID = chudetailieu[numbercluter-1].ID
				id = 1
			}

			chude := i.libraryRepository.SaveCluster(s, int64(id))

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

	i.Gomcumdulieu()
}
func (i *Interactor) CronJobDaoVan() {
	baitap := i.thongtinbaitaptuluanRepository.GetthongtinbaitapForCronjob()
	for _, j := range baitap {
		i.Kiemtradaovanbaitap(j.BaitapTL.MaBaiTap, j.BaitapTL.BaiTap.MaLop)
		i.Kiemtradaovanlophoc(j.BaitapTL.MaBaiTap, j.BaitapTL.BaiTap.MaLop)
		i.KiemtradaovanALL(int64(j.BaitapTL.MaBaiTap))
	}
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
