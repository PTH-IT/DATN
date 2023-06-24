package main

import (
	Service "cronjob-DATN/service"
	"fmt"
	"os"

	licenseoffice "github.com/unidoc/unioffice/common/license"
	licensepdf "github.com/unidoc/unipdf/v3/common/license"
)

var licenseKey = `026f1a04b7ab0bdf29ca672f74fe0fd3f64e57acdf0c5f0fa48748d91812a9ec`

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
