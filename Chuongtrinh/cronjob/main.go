package main

import (
	Service "cronjob-DATN/service"
	"fmt"
	"os"

	licenseoffice "github.com/unidoc/unioffice/common/license"
	licensepdf "github.com/unidoc/unipdf/v3/common/license"
)

var licenseKey = `3bc8aa0d8552b48ece6fdc75f2ba60ee4cb27750aeee19675b0f4d3b3f0adb50`

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
