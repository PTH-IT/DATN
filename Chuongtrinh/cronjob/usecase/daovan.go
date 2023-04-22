package usecase

import (
	"cronjob-DATN/model"
	"cronjob-DATN/utils"
	"strings"
	"unicode/utf8"
)

func GetCau(s string) []string {
	var list []string
	a := ""
	for i, r := range strings.ToLower(s) {

		a += string(r)
		if i+1 < utf8.RuneCountInString(s) && i-1 > 0 {
			if s[i] == '.' && s[i+1] != '.' && s[i-1] != '.' {
				if len(Clearstring(a)) > 0 {
					list = append(list, a)
					a = ""
				}
			} else if i == len(s)-1 {
				if len(Clearstring(a)) > 0 {
					list = append(list, a)
					a = ""
				}
			}
		} else {
			if i == len(s)-1 {
				if len(Clearstring(a)) > 0 {
					list = append(list, a)
					a = ""
				}
			}
		}
	}

	return list
}
func Clearstring(s string) string {
	s = strings.ReplaceAll(strings.ReplaceAll(s, "\r", " "), "\n", " ")
	for {
		i := strings.Split(s, "  ")
		if len(i) > 1 {
			s = strings.ReplaceAll(s, "  ", " ")
		} else {
			break
		}
	}

	return s
}
func ClearSpaceEndStart(s string) string {
	return strings.TrimRight(strings.TrimLeft(s, " "), " ")
}
func (i *Interactor) Comparetwofilepdf(file1 model.ThongTinBaiTapTL, data2 string, link2 string) model.Daovan {

	var listdv model.Daovan
	datafile1 := GetCau(file1.Library.Noidung)
	datafile2 := GetCau(data2)
	per := []float64{}
	for _, compare1 := range datafile1 {
		var dv model.Detaildaovan
		per1 := []float64{}
		checklist := map[int64][]string{}
		for j, compare2 := range datafile2 {
			a, b := Comparetsentence(ClearSpaceEndStart(compare1), ClearSpaceEndStart(compare2))
			checklist[int64(j)] = a
			per1 = append(per1, b)
			if b == 100 {
				break
			}

		}
		max, indexmax := utils.FindMax(per1)
		listsentence := checklist[indexmax]
		dv.Percent = max
		dv.Locationfilecompare = link2
		dv.Keywor1 = compare1
		dv.Keywor2 = datafile2[indexmax]
		dv.SentenceKeyword = listsentence
		listdv.Sentence = append(listdv.Sentence, dv)
		per = append(per, max)
	}
	listdv.File1 = file1
	listdv.Percent = utils.SumArray(per) / float64(len(datafile1))
	return listdv
}
func Comparetsentence(s1 string, s2 string) ([]string, float64) {

	var percents float64
	var sentence []string

	if strings.Contains(s2, s1) {
		percents = 100.0
		sentence = append(sentence, s1)
	} else {

		a, b := comparetsentence1(s1, s2)
		sentence = a
		percents = b

	}

	return sentence, percents

}
func comparetsentence1(keyword1 string, keyword2 string) ([]string, float64) {
	keyalike := []string{}
	alike := []string{}
	indexalike := []int{}
	percent := float64(0)
	if strings.Index(Clearstring(keyword1), " ") > 1 {
		indexs1 := 0
		for _, s1 := range strings.Split(Clearstring(keyword1), " ") {
			for _, s2 := range strings.Split(Clearstring(keyword2), " ") {
				if s1 == s2 {
					alike = append(alike, s1)
					indexalike = append(indexalike, indexs1)
					break
				}
			}
			indexs1 += len(s1) + 1
		}

		s := ""
		for i, _ := range alike {
			if i > 0 {
				if indexalike[i-1]+len(alike[i-1])+1 == indexalike[i] {
					if i == len(alike)-1 && len(strings.Split(s+" "+alike[i], " ")) >= 2 && strings.Contains(keyword2, s+" "+alike[i]) {
						s = s + " " + alike[i]
						keyalike = append(keyalike, s)
						s = ""
					} else if len(strings.Split(s, " ")) >= 2 && strings.Contains(keyword2, s) && !strings.Contains(keyword2, s+" "+alike[i]) {
						keyalike = append(keyalike, s)
						s = alike[i]
					} else if strings.Contains(keyword2, s+" "+alike[i]) {
						if len(s) == 0 {
							s = alike[i]
						} else {
							s = s + " " + alike[i]
						}
					} else {
						s = alike[i]
					}
				} else {
					if len(strings.Split(s, " ")) >= 2 && strings.Contains(keyword2, s) {
						keyalike = append(keyalike, s)
					}
					s = alike[i]
				}
			} else {
				s = alike[i]
			}
		}

		numberalike := 0
		for _, i := range keyalike {
			numberalike += len(strings.Split(i, " "))
		}
		percent = float64(numberalike*100) / float64(len(strings.Split(keyword1, " ")))
	}
	return keyalike, percent
}
