package main

import (
	Service "cronjob-DATN/service"
	"fmt"
	"os"

	"github.com/unidoc/unioffice/common/license"
)

var licenseKey = `35c193a69cc5d56cd8f23895f73bfa1a6d9eb736dc3e336e4b694420d3162f65`

func main() {

	err := license.SetMeteredKey(licenseKey)
	if err != nil {
		fmt.Printf("Error loading license: %v\n", err)
		os.Exit(1)
	}

	fmt.Println("License loaded successfully")
	Service.Run()

}
