package repository

import "cronjob-DATN/model"

type LibraryRepository interface {
	GetLibrary(idNhom int64) []*model.Library
	GetforLopHoc(maLop int64) []*model.Library
	GetforAll() []*model.Library
	SaveCluster(chude model.Chudetailieu, id int64) *model.Chudetailieu
	UpdateLibrary(tailieu model.Library)
	GetChuDe() []*model.Chudetailieu
}
