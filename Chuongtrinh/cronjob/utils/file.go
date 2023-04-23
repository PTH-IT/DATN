package utils

import (
	"io"
	"log"
	"mime/multipart"
	"os"
	"path/filepath"
	"strings"

	"github.com/google/uuid"
	"github.com/unidoc/unioffice/document"
	"github.com/unidoc/unipdf/v3/contentstream"
	"github.com/unidoc/unipdf/v3/core"
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
func Highline(path string) {

	f, err := os.Open(path)
	if err != nil {

		return
	}
	defer f.Close()

	// Create a new PDF reader.
	r, err := model.NewPdfReader(f)
	if err != nil {

		return
	}

	// Get the number of pages in the PDF file.
	numPages, err := r.GetNumPages()
	if err != nil {

		return
	}

	// Create a new PDF writer.
	// w := creator.New()
	pdfWriter := model.NewPdfWriter()
	// Loop through all pages in the PDF file.
	for i := 1; i <= numPages; i++ {
		// Load the page.
		page, err := r.GetPage(i)
		if err != nil {

			return
		}
		err = searchReplacePageText(page, "TRƯỜNG ĐẠI HỌC GIAO THÔNG VẬN TẢI ", "replaceText")
		if err != nil {

		}

		err = pdfWriter.AddPage(page)
		if err != nil {

		}
	}

	// Save the output PDF file.
	err = pdfWriter.WriteToFile("output.pdf")
	if err != nil {

		return
	}
}
func searchReplacePageText(page *model.PdfPage, searchText, replaceText string) error {
	contents, err := page.GetAllContentStreams()
	if err != nil {
		return err
	}

	ops, err := contentstream.NewContentStreamParser(contents).Parse()
	if err != nil {
		return err
	}

	// Generate text chunks.
	var currFont *model.PdfFont
	tc := textChunks{}

	textProcFunc := func(objptr *core.PdfObject) {
		strObj, ok := core.GetString(*objptr)
		if !ok {

			return
		}

		str := strObj.String()
		if currFont != nil {
			decoded, _, numMisses := currFont.CharcodeBytesToUnicode(strObj.Bytes())
			if numMisses != 0 {

			}
			str = decoded
		}

		tc.chunks = append(tc.chunks, &textChunk{
			font:   currFont,
			strObj: strObj,
			val:    str,
			idx:    len(tc.text),
		})
		tc.text += str
	}

	processor := contentstream.NewContentStreamProcessor(*ops)
	processor.AddHandler(contentstream.HandlerConditionEnumAllOperands, "",
		func(op *contentstream.ContentStreamOperation, gs contentstream.GraphicsState, resources *model.PdfPageResources) error {
			switch op.Operand {
			case `Tj`, `'`:
				if len(op.Params) != 1 {

					return nil
				}
				textProcFunc(&op.Params[0])
			case `''`:
				if len(op.Params) != 3 {

					return nil
				}
				textProcFunc(&op.Params[3])
			case `TJ`:
				if len(op.Params) != 1 {

					return nil
				}
				arr, _ := core.GetArray(op.Params[0])
				for i := range arr.Elements() {
					obj := arr.Get(i)
					textProcFunc(&obj)
					arr.Set(i, obj)
				}
			case "Tf":
				if len(op.Params) != 2 {

					return nil
				}

				fname, ok := core.GetName(op.Params[0])
				if !ok || fname == nil {

					return nil
				}

				fObj, has := resources.GetFontByName(*fname)
				if !has {

					return nil
				}

				pdfFont, err := model.NewPdfFontFromPdfObject(fObj)
				if err != nil {

					return nil
				}
				currFont = pdfFont
			}

			return nil
		})

	if err = processor.Process(page.Resources); err != nil {
		return err
	}

	tc.replace(searchText, replaceText)
	return page.SetContentStreams([]string{ops.String()}, core.NewFlateEncoder())
}

type textChunks struct {
	text   string
	chunks []*textChunk
}
type textChunk struct {
	font   *model.PdfFont
	strObj *core.PdfObjectString
	val    string
	idx    int
}

func (tc *textChunks) replace(search, replacement string) {
	text := tc.text
	chunks := tc.chunks

	// Steps:
	// 1. Search for the first index of the search term in the text.
	// 2. Use the found index to match the text chunk which contains
	//    (or partly contains) the search term.
	// 3. Replace the search term in the found text chunk. The search term
	//    will not always start at the beginning of the text chunk. Also,
	//    the search term could be split in multiple text chunks. If that's
	//    the case, replace the portion of the search term in the found
	//    chunk and continue removing characters from the following chunks
	//    until the search term has been completely erased.
	// 4. Offset the text chunks slice to the last processed text chunk from
	//    the previous step, if the text chunk was not completely erased, or
	//    to the next one otherwise. This is necessary so that the visited
	//    text chunks are skipped when searching for the next occurrence of the
	//    search term.
	// 5. Discard the part of the text up to (and including) the index found
	//    in step one.
	// 6. Move to step 1 in order to search for the search term in the remaining
	//    text.
	var chunkOffset int
	matchIdx := strings.Index(text, search)
	for currMatchIdx := matchIdx; matchIdx != -1; {
		for i, chunk := range chunks[chunkOffset:] {
			idx, lenChunk := chunk.idx, len(chunk.val)
			if currMatchIdx < idx || currMatchIdx > idx+lenChunk-1 {
				continue
			}
			chunkOffset += i + 1

			start := currMatchIdx - idx
			remaining := len(search) - (lenChunk - start)

			replaceVal := chunk.val[:start] + replacement
			if remaining < 0 {
				replaceVal += chunk.val[lenChunk+remaining:]
				chunkOffset--
			}

			chunk.val = replaceVal
			for j := chunkOffset; remaining > 0; j++ {
				c := chunks[j]
				l := len(c.val)

				if l > remaining {
					c.val = c.val[remaining:]
				} else {
					c.val = ""
					chunkOffset++
				}

				remaining -= l
			}

			break
		}

		text = text[matchIdx+1:]
		matchIdx = strings.Index(text, search)
		currMatchIdx += matchIdx + 1
	}

	tc.text = strings.Replace(tc.text, search, replacement, -1)
}
