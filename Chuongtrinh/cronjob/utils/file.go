package utils

import (
	"io"
	"log"
	"mime/multipart"
	"os"
	"path/filepath"

	"github.com/google/uuid"
	Docx "github.com/unidoc/unioffice/document"
	"github.com/unidoc/unipdf/v3/extractor"
	"github.com/unidoc/unipdf/v3/model"
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

	f, err := os.Open(filePath)
	if err != nil {
		panic(err)
	}
	defer f.Close()

	reader, err := model.NewPdfReaderLazy(f)
	if err != nil {
		panic(err)
	}
	data := ""
	numberpage, _ := reader.GetNumPages()
	for i := 0; i < numberpage; i++ {
		p, err := reader.GetPage(i + 1)
		if err != nil {
			panic(err)
		}
		ex, err := extractor.New(p)
		if err != nil {
			panic(err)
		}

		text, err := ex.ExtractText()
		if err != nil {
			panic(err)
		}
		data = data + text
	}

	return data, err
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
