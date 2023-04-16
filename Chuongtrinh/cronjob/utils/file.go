package utils

import (
	"io"
	"mime/multipart"
	"os"

	"github.com/google/uuid"
)

func SaveFile(file *multipart.FileHeader) (error, string) {
	typefile := make(map[string]string)
	typefile["application/msword"] = ".doc"
	typefile["application/pdf"] = ".pdf"
	src, err := file.Open()
	if err != nil {
		return err, ""
	}
	defer src.Close()

	// Destination
	filelocation := "store/"
	fileName := uuid.New().String() + typefile[file.Header.Get("Content-Type")]
	dst, err := os.Create(filelocation + fileName)
	if err != nil {
		return err, ""
	}
	defer dst.Close()

	// Copy
	if _, err = io.Copy(dst, src); err != nil {
		return err, ""
	}
	return nil, filelocation + fileName
}
func GetFile(path string) {

}
