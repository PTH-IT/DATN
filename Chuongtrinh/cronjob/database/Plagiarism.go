package gormdb

import (
	"cronjob-DATN/model"
	"cronjob-DATN/repository"
)

func NewPlagiarism() repository.PlagiarismRepository {
	return plagiarismRepository{}
}

type plagiarismRepository struct {
}

func (r plagiarismRepository) Save(plagiarism model.Plagiarism) {

	var library []*model.Plagiarism
	DB.Select("*").Table("Plagiarism").Where("Mafile = ? and Loaikiemtra = ?", plagiarism.Mafile, plagiarism.Loaikiemtra).Find(&library)
	if len(library) == 0 {
		err := DB.Table("Plagiarism").Create(&plagiarism)
		if err != nil {
		}
	} else {
		err := DB.Table("Plagiarism").Where("Mafile = ? and Loaikiemtra = ?", plagiarism.Mafile, plagiarism.Loaikiemtra).UpdateColumns(&plagiarism)
		if err != nil {
		}
	}

}
