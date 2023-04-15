package config

import (
	"log"
	"os"

	"github.com/joho/godotenv"
)

type AppConfig struct {
	Env       string          `json:"env"`
	Port      string          `json:"port"`
	SqlServer SqlServerConfig `json:"mysql"`
}
type SqlServerConfig struct {
	Host string `json:"host"`
	Port string `json:"port"`
	User string `json:"user"`
	Pass string `json:"password"`
	Db   string `json:"db"`
}

func Getconfig() AppConfig {
	var appConfig AppConfig
	if err := godotenv.Load(); err != nil {
		log.Println("No .env file found")
	}
	if appConfig.Port == "" {
		appConfig.Port = os.Getenv("PORT")
	}
	if appConfig.SqlServer.User == "" {
		appConfig.SqlServer.User = os.Getenv("DB_USER")

	}
	if appConfig.SqlServer.Pass == "" {
		appConfig.SqlServer.Pass = os.Getenv("DB_PASSWORD")

	}
	if appConfig.SqlServer.Host == "" {
		appConfig.SqlServer.Host = os.Getenv("DB_HOST")

	}
	if appConfig.SqlServer.Port == "" {
		appConfig.SqlServer.Port = os.Getenv("DB_PORT")

	}
	if appConfig.SqlServer.Db == "" {
		appConfig.SqlServer.Db = os.Getenv("DB_NAME")

	}
	return appConfig
}
