package usecase

import (
	"cronjob-DATN/model"
	"encoding/json"
	"fmt"
	"strconv"
	"strings"

	"github.com/muesli/clusters"
	"github.com/muesli/kmeans"
)

func processText(text string) []string {
	words := strings.Fields(text)
	return words
}

func (i *Interactor) kmeansForModel(file []*model.Library, k int) map[int][]model.Library {

	// var features [][]float64
	var d clusters.Observations
	vectordata := make(map[string]clusters.Observations, len(file))
	for i, text := range file {
		words := processText(text.Noidung)
		for _, word := range words {

			value := 0
			for _, y := range []byte(strings.ToLower(word)) {
				value += int(y)
			}
			x := float64(len(word))
			y := float64(value) / float64(len(word))
			d = append(d, clusters.Coordinates{
				x,
				y,
			})
			vectordata[strconv.Itoa(i)] = append(vectordata[strconv.Itoa(i)], clusters.Coordinates{
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
		return nil
	}
	fmt.Println(clusters)

	if len(clusters) != k {
		fmt.Printf("Expected %d clusters, got: %d\n", k, len(clusters))
	}
	kmeansResult := map[string][]int{}
	for i, cluster := range clusters {
		for _, index := range cluster.Observations {
			for z, data := range vectordata {
				for _, d := range data {
					if index.Distance(d.Coordinates()) == 0 {
						kmeansResult[z] = append(kmeansResult[z], i+1)
					}
				}

			}
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
		return nil
	}
	fmt.Println(string(jsonData))
	return result

}
func (i *Interactor) kmeansForArrayData(k int) {
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
		"Etiam semper nisi nec justo tempus, ut pharetra nunc mattis.",
	}

	// var features [][]float64
	var d clusters.Observations
	vectordata := make(map[string]clusters.Observations, len(datas))
	for i, text := range datas {
		words := processText(text)
		for _, word := range words {

			value := 0
			for _, y := range []byte(strings.ToLower(word)) {
				value += int(y)
			}
			x := float64(len(word))
			y := float64(value) / float64(len(word))
			d = append(d, clusters.Coordinates{
				x,
				y,
			})
			vectordata[strconv.Itoa(i)] = append(vectordata[strconv.Itoa(i)], clusters.Coordinates{
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
	for i, cluster := range clusters {
		for _, index := range cluster.Observations {
			for z, data := range vectordata {
				for _, d := range data {
					if index.Distance(d.Coordinates()) == 0 {
						kmeansResult[z] = append(kmeansResult[z], i+1)
					}
				}

			}
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
	jsonData, err := json.Marshal(result)
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
		frequency := make(map[int]int)

		for _, num := range nums {
			frequency[num]++
		}

		maxCount := 0
		mostFrequentNumber := 0

		for num, count := range frequency {
			if count > maxCount {
				maxCount = count
				mostFrequentNumber = num
			}
		}

		keyNumer, _ := strconv.Atoi(key)
		mostFrequent[keyNumer] = mostFrequentNumber
	}

	return mostFrequent
}