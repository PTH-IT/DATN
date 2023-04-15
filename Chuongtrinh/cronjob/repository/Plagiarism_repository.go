package repository

import "cronjob-DATN/model"

type PlagiarismRepository interface {
	Save(plagiarism model.Plagiarism)
}
