package repository

import "cronjob-DATN/model"

type TaikhoanRepository interface {
	GetTaikhoan() []*model.Taikhoan
}
