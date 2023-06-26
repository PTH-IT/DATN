package main

import (
	Service "cronjob-DATN/service"
	"fmt"
	"os"

	licenseoffice "github.com/unidoc/unioffice/common/license"
	licensepdf "github.com/unidoc/unipdf/v3/common/license"
)

var licenseKey = `f6bfc7bf11efad3fa6ad8cb06f2eaa22c9eaf0c7b0fb46c2ba759d31fbb2d3e1`

func main() {

	err := licenseoffice.SetMeteredKey(licenseKey)
	if err != nil {
		fmt.Printf("Error loading license: %v\n", err)
		os.Exit(1)
	}
	err = licensepdf.SetMeteredKey(licenseKey)

	if err != nil {
		fmt.Printf("Error loading license: %v\n", err)
		os.Exit(1)
	}
	fmt.Println("License loaded successfully")
	Service.Run()

}
