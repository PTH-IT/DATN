package main

import (
	Service "cronjob-DATN/service"
	"fmt"
	"os"

	licenseoffice "github.com/unidoc/unioffice/common/license"
	licensepdf "github.com/unidoc/unipdf/v3/common/license"
)

var licenseKey = `35c193a69cc5d56cd8f23895f73bfa1a6d9eb736dc3e336e4b694420d3162f65`

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
