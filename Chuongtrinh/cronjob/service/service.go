package service

import (
	// "context"

	"fmt"

	Database "cronjob-DATN/model/database"
	"cronjob-DATN/usecase"

	"gorm.io/driver/sqlserver"
	"gorm.io/gorm"
)

func Run() {

	// ctx := context.Background()
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
	interactor := usecase.NewInteractor(db, taikhoanRepository)
	interactor.Gomcumdulieu()

}
