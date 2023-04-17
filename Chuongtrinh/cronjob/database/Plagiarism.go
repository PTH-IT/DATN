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

	err := DB.Table("Plagiarism").Create(&plagiarism)
	if err != nil {
	}
}
