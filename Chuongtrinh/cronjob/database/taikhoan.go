package gormdb

import (
	"cronjob-DATN/model"
	"cronjob-DATN/repository"
	"fmt"
)

func NewTaikhoan() repository.TaikhoanRepository {
	return taikhoanRepository{}
}

type taikhoanRepository struct {
}

func (r taikhoanRepository) GetTaikhoan() []*model.Taikhoan {
	var user []*model.Taikhoan
	DB.Table("TaiKhoan").Find(&user)
	if len(user) == 0 {
		fmt.Println("user null")
		return nil
	}
	return user

}
