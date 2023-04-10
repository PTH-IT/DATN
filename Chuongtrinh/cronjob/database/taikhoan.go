package gormdb

import (
	"cronjob-DATN/repository"
	"fmt"
)

func NewTaikhoan() repository.TaikhoanRepository {
	return taikhoanRepository{}
}

type taikhoanRepository struct {
}

func (r taikhoanRepository) GetTaikhoan() {
	var user []map[string]interface{}
	DB.Table("TaiKhoan").Find(&user)
	if len(user) == 0 {
		fmt.Println("user null")
		return
	}

}
