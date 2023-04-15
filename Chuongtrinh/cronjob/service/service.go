package service

import (
	// "context"

	"fmt"

	"cronjob-DATN/config"
	Database "cronjob-DATN/database"
	"cronjob-DATN/usecase"

	"github.com/labstack/echo/v4"
	"gopkg.in/robfig/cron.v2"
	"gorm.io/driver/sqlserver"
	"gorm.io/gorm"
)

func Run() {

	// a, b := Comparetsentence("Sinh viên ngành Công học hệ chính quy nhưng chưa hết thời gian đào tạo tối đa.", "Sinh viên ngành Công nghệ thông tin các lớp đã kết thúc khóa học hệ ")
	// // ctx := context.Background()
	// fmt.Println(a)
	// fmt.Println(b)
	connectString := fmt.Sprintf("sqlserver://%s:%s@%s:%s?database=%s",
		config.Getconfig().SqlServer.User,
		config.Getconfig().SqlServer.Pass,
		config.Getconfig().SqlServer.Host,
		config.Getconfig().SqlServer.Port,
		config.Getconfig().SqlServer.Db,
	)

	db, err := gorm.Open(sqlserver.Open(connectString), &gorm.Config{})
	if err != nil {
		fmt.Println("Failed to connect to database:", err)
		return
	}
	Database.Start(db)
	taikhoanRepository := Database.NewTaikhoan()
	thongtinbaitaptuluanRepository := Database.NewThongTinBaiTapTL()
	plagiarismRepository := Database.NewPlagiarism()
	interactor := usecase.NewInteractor(db, taikhoanRepository, thongtinbaitaptuluanRepository, plagiarismRepository)

	e := echo.New()
	private := e.Group("/api")
	private.POST("/baitap", interactor.BaiTap)
	private.POST("/lophoc", interactor.LopHoc)
	private.POST("/all", interactor.All)

	cronjob := cron.New()

	cronjob.AddFunc("@every 0h0m0s", interactor.CronJob)

	cronjob.Start()
	e.Logger.Fatal(e.Start(":" + config.Getconfig().Port))
}
