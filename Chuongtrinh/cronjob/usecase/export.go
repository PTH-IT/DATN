package usecase

import (
	"cronjob-DATN/model"
	"cronjob-DATN/utils"
)

func Updatepdfdaovan(linkfile string, noiluu string, resultcaudaovan []model.Detaildaovan, option string) string {
	utils.HighlightWords(linkfile, noiluu, resultcaudaovan)
	return noiluu
}
