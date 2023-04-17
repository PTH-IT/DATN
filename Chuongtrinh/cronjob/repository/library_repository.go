package repository

import "cronjob-DATN/model"

type LibraryRepository interface {
	GetLibrary(idNhom int64) []*model.Library
	GetforLopHoc(maLop int64) []*model.Library
}
