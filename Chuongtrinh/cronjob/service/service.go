package service

import (
	// "context"

	"fmt"

	Database "cronjob-DATN/database"
	"cronjob-DATN/usecase"

	"gorm.io/driver/sqlserver"
	"gorm.io/gorm"
)

func Run() {

	// a, b := Comparetsentence("Sinh viên ngành Công học hệ chính quy nhưng chưa hết thời gian đào tạo tối đa.", "Sinh viên ngành Công nghệ thông tin các lớp đã kết thúc khóa học hệ ")
	// // ctx := context.Background()
	// fmt.Println(a)
	// fmt.Println(b)
	username := "admin"
	password := "admin"
	server := "localhost"
	host := "1433"
	database := "DOANTOTNGHIEP"
	dsn := fmt.Sprintf("sqlserver://%s:%s@%s:%s?database=%s", username, password, server, host, database)
	db, err := gorm.Open(sqlserver.Open(dsn), &gorm.Config{})
	if err != nil {
		fmt.Println("Failed to connect to database:", err)
		return
	}
	Database.Start(db)
	taikhoanRepository := Database.NewTaikhoan()
	thongtinbaitaptuluanRepository := Database.NewThongTinBaiTapTL()
	interactor := usecase.NewInteractor(db, taikhoanRepository, thongtinbaitaptuluanRepository)
	interactor.Gomcumdulieu()

}
