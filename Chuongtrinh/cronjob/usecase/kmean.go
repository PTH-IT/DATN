package usecase

import (
	"cronjob-DATN/model"
	"encoding/json"
	"fmt"
	"strconv"
	"strings"
	"unicode"

	cls "github.com/muesli/clusters"
	"github.com/muesli/kmeans"
	"golang.org/x/text/runes"
	"golang.org/x/text/transform"
	"golang.org/x/text/unicode/norm"
)

func processText(text string) []string {
	words := strings.Fields(text)
	return words
}
func removeVietnameseDiacritics(input string) string {
	// Chuẩn hóa chuỗi Unicode NFC
	t := transform.Chain(norm.NFD, runes.Remove(runes.In(unicode.Mn)), norm.NFC)
	result, _, _ := transform.String(t, input)
	return result
}

func (i *Interactor) KmeansForModel(file []*model.Library, k int) (map[int][]model.Library, map[int]cls.Coordinates) {

	// var features [][]float64
	var d cls.Observations
	vectordata := make(map[string]cls.Observations, len(file))
	for i, text := range file {
		sentence := strings.Split(Clearstring(text.Noidung), ". ")
		var words []string
		for _, vsentence := range sentence {
			if len(vsentence) == 0 {
				continue
			}
			words = append(words, processText(vsentence)...)
			if vsentence[len(vsentence)-1] != '.' {
				words = append(words, ".")
			}
		}

		for _, word := range words {

			value := 0
			word := removeVietnameseDiacritics(word)
			for _, y := range []byte(strings.ToLower(word)) {
				value += int(y)
			}
			var x float64
			var y float64
			if word == "." {
				x = 0.0
				y = 0.0
				vectordata[strconv.Itoa(i)] = append(vectordata[strconv.Itoa(i)], cls.Coordinates{
					x,
					y,
				})
				continue
			} else {
				x = float64(len(word))
				y = float64(value) / (float64(len(word)) * 100)
			}

			d = append(d, cls.Coordinates{
				x,
				y,
			})
			vectordata[strconv.Itoa(i)] = append(vectordata[strconv.Itoa(i)], cls.Coordinates{
				x,
				y,
			})
		}
	}
	fmt.Println(vectordata)

	fmt.Println(d)
	km := kmeans.New()
	group, err := km.Partition(d, k)
	if err != nil {
		fmt.Printf("Unexpected error partitioning: %d\n", err)
		return nil, nil
	}
	fmt.Println(group)

	if len(group) != k {
		fmt.Printf("Expected %d clusters, got: %d\n", k, len(group))
	}
	kmeansResult := map[string][]int{}
	center := make(map[int]cls.Coordinates)
	for z, data := range vectordata {
		for _, d := range data {
			if d.Distance(cls.Coordinates{
				0.0,
				0.0,
			}) == 0 {
				kmeansResult[z] = append(kmeansResult[z], 0)
				continue
			}
			var cls []float64
			for i, cluster := range group {
				cls = append(cls, cluster.Center.Distance(d.Coordinates()))
				center[i+1] = cluster.Center
			}
			kmeansResult[z] = append(kmeansResult[z], findMaxIndex(cls)+1)

		}

	}
	mostFrequent := findMostFrequentNumber(kmeansResult)
	result := map[int][]model.Library{}

	fmt.Println(kmeansResult)
	for key, value := range mostFrequent {
		fmt.Printf("Key: %d, Số xuất hiện nhiều nhất: %d\n", key, value)
		result[value] = append(result[value], *file[key])
	}
	fmt.Println(kmeansResult)
	fmt.Println(result)
	jsonData, err := json.Marshal(result)
	if err != nil {
		fmt.Println("Error:", err)
		return nil, nil
	}
	fmt.Println(string(jsonData))
	return result, center

}
func findMaxIndex(slice []float64) int {
	if len(slice) == 0 {
		return -1 // Trường hợp slice rỗng
	}

	maxIndex := 0
	maxValue := slice[0]

	for i, value := range slice {
		if value > maxValue {
			maxIndex = i
			maxValue = value
		}
	}

	return maxIndex
}
func (i *Interactor) KmeansForArrayData(k int) {
	datas := []string{
		"C (ngôn ngữ lập trình) – Wikipedia tiếng Việt.",
		"C cơ bản: Giới thiệu ngôn ngữ C - DevIOT.",
		"Ngôn ngữ lập trình C là gì? Tìm hiểu về ngôn ngữ lập trình C.",
		"Khi nào nên dùng Golang? Nó dùng tốt trong trường hợp nào?.",
		"Golang là gì? Backend Developer có nên học Golang?.",
		"Golang là gì? Vì sao nên sử dụng ngôn ngữ Golang.",
		"Python (ngôn ngữ lập trình) – Wikipedia tiếng Việt.",
		"Python Là Gì? Các Bước Tự Học Lập Trình Python - TopDev.",
		"Python là gì? - Giải thích về ngôn ngữ Python - Amazon AWS.",
	}

	// var features [][]float64
	var d cls.Observations
	vectordata := make(map[string]cls.Observations, len(datas))
	for i, text := range datas {
		words := processText(text)
		for _, word := range words {

			value := 0
			for _, y := range []byte(strings.ToLower(word)) {
				value += int(y)
			}
			x := float64(len(word))
			y := float64(value) / (float64(len(word)) * 100)
			d = append(d, cls.Coordinates{
				x,
				y,
			})
			vectordata[strconv.Itoa(i)] = append(vectordata[strconv.Itoa(i)], cls.Coordinates{
				x,
				y,
			})
		}
	}
	fmt.Println(vectordata)

	fmt.Println(d)

	km := kmeans.New()
	clusters, err := km.Partition(d, k)
	if err != nil {
		fmt.Printf("Unexpected error partitioning: %d\n", err)
		return
	}
	fmt.Println(clusters)

	if len(clusters) != k {
		fmt.Printf("Expected %d clusters, got: %d\n", k, len(clusters))
	}
	kmeansResult := map[string][]int{}
	center := make(map[int]interface{})

	for z, data := range vectordata {
		for _, d := range data {
			if d.Distance(cls.Coordinates{
				0.0,
				0.0,
			}) == 0 {
				kmeansResult[z] = append(kmeansResult[z], 0)
				continue
			}
			var cls []float64
			for i, cluster := range clusters {
				center[i] = map[string]float64{"x": cluster.Center[0], "y": cluster.Center[1]}
				cls = append(cls, cluster.Center.Distance(d.Coordinates()))
			}
			kmeansResult[z] = append(kmeansResult[z], findMaxIndex(cls)+1)

		}

	}
	mostFrequent := findMostFrequentNumber(kmeansResult)
	result := map[int][]string{}

	fmt.Println(kmeansResult)
	for key, value := range mostFrequent {
		fmt.Printf("Key: %d, Số xuất hiện nhiều nhất: %d\n", key, value)
		result[value] = append(result[value], datas[key])
	}
	fmt.Println(kmeansResult)
	fmt.Println(result)
	var jsonvalue = make(map[string]interface{})
	jsonvalue["list"] = result
	jsonvalue["point"] = center
	jsonData, err := json.Marshal(jsonvalue)
	if err != nil {
		fmt.Println("Error:", err)
		return
	}
	fmt.Println(string(jsonData))
}
func findMostFrequentNumber(numbers map[string][]int) map[int]int {
	mostFrequent := make(map[int]int)

	// Tìm số xuất hiện nhiều nhất cho từng key
	for key, nums := range numbers {
		frequencys := make(map[int]int)
		frequency := make(map[int]int)
		for _, num := range nums {
			frequency[num]++
			if num <= 0 {
				maxCount := 0
				mostFrequentNumber := 0

				for num, count := range frequency {
					if count > maxCount {
						maxCount = count
						mostFrequentNumber = num
					}
				}
				frequencys[mostFrequentNumber]++
				frequency = make(map[int]int)
			}

		}

		maxCount := 0
		mostFrequentsNumber := 0

		for num, count := range frequencys {
			if count > maxCount {
				maxCount = count
				mostFrequentsNumber = num
			}
		}

		keyNumer, _ := strconv.Atoi(key)
		mostFrequent[keyNumer] = mostFrequentsNumber
	}

	return mostFrequent
}
