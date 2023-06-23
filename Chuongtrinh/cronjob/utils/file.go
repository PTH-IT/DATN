package utils

import (
	object "cronjob-DATN/model"
	"fmt"
	"io"
	"log"
	"mime/multipart"
	"os"
	"path/filepath"
	"strings"

	"github.com/google/uuid"
	"github.com/unidoc/unioffice/document"
	"github.com/unidoc/unipdf/v3/creator"
	"github.com/unidoc/unipdf/v3/extractor"
	"github.com/unidoc/unipdf/v3/model"
)

func ReadFileDoc(filePath string) (string, error) {
	doc, err := document.Open(filePath)
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

// getBoundingBoxes returns the bounding boxes of all instances of search`term` in the Pdf file.
func getBoundingBoxes(page *model.PdfPage, term string) ([]*model.PdfRectangle, error) {
	boundingBoxes := []*model.PdfRectangle{}
	ex, _ := extractor.New(page)
	pageText, _, _, err := ex.ExtractPageText()
	if err != nil {
		return nil, err
	}
	textMarks := pageText.Marks()
	text := pageText.Text()
	indexes := indexAllSubstrings(text, term)
	if len(indexes) == 0 {
		return nil, nil
	}
	for _, start := range indexes {
		end := start + len(term)
		spanMarks, err := textMarks.RangeOffset(start, end)
		if err != nil {
			return nil, err
		}
		bbox, ok := spanMarks.BBox()
		if !ok {
			return nil, fmt.Errorf("spanMarks.BBox has no bounding box. spanMarks=%s", spanMarks)
		}
		boundingBoxes = append(boundingBoxes, &bbox)
	}
	return boundingBoxes, nil

}

// indexAllSubstrings gets the begning indexes where `term` occures inside `text`.
func indexAllSubstrings(text, term string) []int {
	if len(term) == 0 {
		return nil
	}
	var indexes []int
	for start := 0; start < len(text); {
		i := strings.Index(text[start:], term)
		if i < 0 {
			return indexes
		}
		indexes = append(indexes, start+i)
		start += i + len(term)
	}
	return indexes
}
func Clearstring(s string) string {
	s = strings.ReplaceAll(strings.ReplaceAll(s, "\r", ""), "\n", "")

	return s
}
func HighlightWords(inputPath, outputPath string, resultcaudaovan []object.Detaildaovan) error {
	reader, f, err := model.NewPdfReaderFromFile(inputPath, nil)
	if err != nil {
		panic(err)
	}
	defer f.Close()

	numPages, err := reader.GetNumPages()
	if err != nil {
		return err
	}
	cr := creator.New()
	for n := 1; n <= numPages; n++ {
		page, err := reader.GetPage(n)
		if err != nil {
			return err
		}
		if err := cr.AddPage(page); err != nil {
			return err
		}
		for i, value := range resultcaudaovan {
			if value.Percent > 0 {
				term := Clearstring(value.Keywor2)
				bBoxes, err := getBoundingBoxes(page, term)
				if err != nil {
					return err
				}

				mediaBox, err := page.GetMediaBox()
				if err != nil {
					return err
				}
				if page.MediaBox == nil {
					page.MediaBox = mediaBox
				}

				h := mediaBox.Ury
				for _, bbox := range bBoxes {
					rect := cr.NewRectangle(bbox.Llx, h-bbox.Lly, bbox.Urx-bbox.Llx, -(bbox.Ury - bbox.Lly))
					rect.SetFillColor(creator.ColorRGBFromHex("#FFFF00"))
					rect.SetBorderWidth(0)
					rect.SetFillOpacity(0.5)
					if err := cr.Draw(rect); err != nil {
						return nil
					}

				}
				if len(bBoxes) > 0 {
					if i < len(resultcaudaovan)-1 {
						resultcaudaovan = RemoveValueForObject(resultcaudaovan, i)

					}
				}
			}
		}
	}

	cr.SetOutlineTree(reader.GetOutlineTree())

	if err := cr.WriteToFile(outputPath); err != nil {
		return fmt.Errorf("failed to write the output file %s", outputPath)
	}
	return nil
}

func RemoveValueForObject(list []object.Detaildaovan, i int) []object.Detaildaovan {

	return append(list[:i], list[i+1:]...)

}
