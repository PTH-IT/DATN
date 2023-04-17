package repository

import "cronjob-DATN/model"

type DocumentRepository interface {
	GetDocumentcholophoc(malop int64) []*model.Document
}
