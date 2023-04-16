package utils

import (
	"bytes"
	"io"
	"log"
	"mime/multipart"
	"os"
	"path/filepath"

	"github.com/google/uuid"
	"github.com/ledongthuc/pdf"
	Docx "github.com/unidoc/unioffice/document"
)

func ReadFileDoc(filePath string) (string, error) {
	doc, err := Docx.Open(filePath)
	if err != nil {
		log.Fatalf("Failed to open example.docx: %v\n", err)
		return "", err

	}

	// Get the text from the Word file
	text := doc.ExtractText().Text()

	return text, nil
}

func ReadFilePdf(filePath string) (string, error) {

	f, r, err := pdf.Open(filePath)
	// remember close file
	defer f.Close()
	if err != nil {
		return "", err
	}
	var buf bytes.Buffer
	b, err := r.GetPlainText()
	if err != nil {
		return "", err
	}
	buf.ReadFrom(b)
	return buf.String(), nil

}
func SaveFile(file *multipart.FileHeader) (error, string, string) {
	src, err := file.Open()
	if err != nil {
		return err, "", ""
	}
	defer src.Close()

	// Destination
	filelocation := "store/"
	fileName := uuid.New().String() + filepath.Ext(file.Filename)
	dst, err := os.Create(filelocation + fileName)
	if err != nil {
		return err, "", ""
	}
	defer dst.Close()

	// Copy
	if _, err = io.Copy(dst, src); err != nil {
		return err, "", ""
	}
	text := ""
	if filepath.Ext(file.Filename) == ".pdf" {
		text, err = ReadFilePdf(filelocation + fileName)
		if err != nil {
			return err, "", ""
		}
	} else {
		text, err = ReadFileDoc(filelocation + fileName)
		if err != nil {
			return err, "", ""
		}
	}
	return nil, filelocation + fileName, text
}
func GetFile(path string) {

}
