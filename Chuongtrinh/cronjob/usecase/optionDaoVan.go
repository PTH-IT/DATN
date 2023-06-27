package usecase

import (
	"cronjob-DATN/model"
	"fmt"
	"strings"
)

func MaxResult(arr []model.Detaildaovan) (model.Detaildaovan, int64) {
	max := arr[0]
	index := 0
	for i, value := range arr {
		if value.Percent > max.Percent {
			max = value
			index = i
		}
	}

	return max, int64(index)
}
func SumDetaildaovan(arr []model.Detaildaovan) float64 {
	sum := float64(0)
	max := []float64{}
	for _, value := range arr {
		sum += value.Percent
		max = append(max, value.Percent)
	}
	fmt.Println((max))
	return sum
}
func (i *Interactor) Kiemtradaovanbaitap(mabaitap int64, malop int64) bool {
	thongtinbaitap := i.thongtinbaitaptuluanRepository.Getthongtinbaitaptuluan(mabaitap)

	if len(thongtinbaitap) > 0 {
		for _, thongtincankiemtra := range thongtinbaitap {
			check := true
			for _, vthongtincankiemtra := range thongtincankiemtra.Plagiarism {
				if vthongtincankiemtra.Loaikiemtra == "Lophoc" {
					check = false
					break
				}
			}
			if check {
				var checkdaovan []model.Daovan
				for _, bailam := range thongtinbaitap {
					if thongtincankiemtra.Library.NgayUpdate > bailam.Library.NgayUpdate && thongtincankiemtra.NguoiNop != bailam.NguoiNop {

						kiemtradaovan := i.Comparetwofilepdf(*thongtincankiemtra, bailam.Library.Noidung, bailam.Library.Location)
						checkdaovan = append(checkdaovan, kiemtradaovan)

					}

				}
				caudaovan := map[int64][]model.Detaildaovan{}
				for i, k := range checkdaovan {
					caudaovan[int64(i)] = k.Sentence
				}
				resultcaudaovan := []model.Detaildaovan{}
				if len(caudaovan) > 0 {
					for j := 0; j < len(caudaovan[0]); j++ {
						resultcaulist := []model.Detaildaovan{}
						for i := 0; i < len(caudaovan); i++ {
							resultcaulist = append(resultcaulist, caudaovan[int64(i)][j])

						}
						result, _ := MaxResult(resultcaulist)
						resultcaudaovan = append(resultcaudaovan, result)

					}
				}
				if len(resultcaudaovan) > 0 {
					plagiarism1 := model.Plagiarism{}
					plagiarism1.Mafile = thongtincankiemtra.ID
					Percents := SumDetaildaovan(resultcaudaovan) / float64(len(resultcaudaovan))
					if Percents > 100 {
						Percents = 100
					}
					plagiarism1.Percents = Percents
					plagiarism1.Location = Updatepdfdaovan(thongtincankiemtra.Library.Location, strings.ReplaceAll(thongtincankiemtra.Library.Location, ".pdf", "-baitap.pdf"), resultcaudaovan, "bài Tập")
					plagiarism1.Loaikiemtra = "Baitap"
					i.plagiarismRepository.Save(plagiarism1)

				}

				if len(checkdaovan) == 0 {

					plagiarism1 := model.Plagiarism{}
					plagiarism1.Mafile = thongtincankiemtra.ID
					plagiarism1.Percents = 0
					plagiarism1.Location = Updatepdfdaovan(thongtincankiemtra.Library.Location, strings.ReplaceAll(thongtincankiemtra.Library.Location, ".pdf", "-baitap.pdf"), resultcaudaovan, "bài Tập")
					plagiarism1.Loaikiemtra = "Baitap"
					i.plagiarismRepository.Save(plagiarism1)
				}
			}

		}
		return true
	}
	return false
}

func (i *Interactor) Kiemtradaovanlophoc(mabaitap int64, malop int64) bool {
	thongtinbaitap := i.thongtinbaitaptuluanRepository.Getthongtinbaitaptuluan(mabaitap)
	tailieucheck := i.libraryRepository.GetforLopHoc(malop)
	if len(thongtinbaitap) > 0 {

		for _, thongtincankiemtra := range thongtinbaitap {
			check := true
			for _, vthongtincankiemtra := range thongtincankiemtra.Plagiarism {
				if vthongtincankiemtra.Loaikiemtra == "Lophoc" {
					check = false
					break
				}
			}
			if check {
				var checkdaovan []model.Daovan
				for _, bailam := range tailieucheck {
					if thongtincankiemtra.Library.NgayUpdate > bailam.NgayUpdate && thongtincankiemtra.NguoiNop != bailam.NguoiAdd {

						kiemtradaovan := i.Comparetwofilepdf(*thongtincankiemtra, bailam.Noidung, bailam.Location)
						checkdaovan = append(checkdaovan, kiemtradaovan)

					}

				}
				caudaovan := map[int64][]model.Detaildaovan{}
				for i, k := range checkdaovan {
					caudaovan[int64(i)] = k.Sentence
				}
				resultcaudaovan := []model.Detaildaovan{}
				if len(caudaovan) > 0 {
					for j := 0; j < len(caudaovan[0]); j++ {
						resultcaulist := []model.Detaildaovan{}
						for i := 0; i < len(caudaovan); i++ {
							resultcaulist = append(resultcaulist, caudaovan[int64(i)][j])

						}
						result, _ := MaxResult(resultcaulist)
						resultcaudaovan = append(resultcaudaovan, result)

					}
				}
				if len(resultcaudaovan) > 0 {
					plagiarism1 := model.Plagiarism{}
					plagiarism1.Mafile = thongtincankiemtra.ID
					Percents := SumDetaildaovan(resultcaudaovan) / float64(len(resultcaudaovan))
					if Percents > 100 {
						Percents = 100
					}
					plagiarism1.Percents = Percents
					plagiarism1.Location = Updatepdfdaovan(thongtincankiemtra.Library.Location, strings.ReplaceAll(thongtincankiemtra.Library.Location, ".pdf", "-lophoc.pdf"), resultcaudaovan, "lớp học")
					plagiarism1.Loaikiemtra = "Lophoc"
					i.plagiarismRepository.Save(plagiarism1)

				}

				if len(checkdaovan) == 0 {

					plagiarism1 := model.Plagiarism{}
					plagiarism1.Mafile = thongtincankiemtra.ID
					plagiarism1.Percents = 0
					plagiarism1.Location = Updatepdfdaovan(thongtincankiemtra.Library.Location, strings.ReplaceAll(thongtincankiemtra.Library.Location, ".pdf", "-lophoc.pdf"), resultcaudaovan, "lớp học")
					plagiarism1.Loaikiemtra = "Lophoc"
					i.plagiarismRepository.Save(plagiarism1)
				}
			}

		}
		return true
	}
	return false
}

func (i *Interactor) KiemtradaovanALL(mabaitap int64) bool {
	thongtinbaitap := i.thongtinbaitaptuluanRepository.Getthongtinbaitaptuluan(mabaitap)
	library := i.libraryRepository.GetLibrary(0)
	if len(thongtinbaitap) > 0 {

		for _, thongtincankiemtra := range thongtinbaitap {
			check := true
			for _, vthongtincankiemtra := range thongtincankiemtra.Plagiarism {
				if vthongtincankiemtra.Loaikiemtra == "ALL" {
					check = false
					break
				}
			}
			if check {
				var checkdaovan []model.Daovan
				for _, bailam := range library {
					if thongtincankiemtra.Library.MaNhom != 0 && thongtincankiemtra.Library.MaNhom != bailam.MaNhom {
						continue
					}
					if thongtincankiemtra.Library.NgayUpdate > bailam.NgayUpdate && thongtincankiemtra.NguoiNop != bailam.NguoiAdd {

						kiemtradaovan := i.Comparetwofilepdf(*thongtincankiemtra, bailam.Noidung, bailam.Location)
						checkdaovan = append(checkdaovan, kiemtradaovan)

					}

				}
				caudaovan := map[int64][]model.Detaildaovan{}
				for i, k := range checkdaovan {
					caudaovan[int64(i)] = k.Sentence
				}
				resultcaudaovan := []model.Detaildaovan{}
				if len(caudaovan) > 0 {
					for j := 0; j < len(caudaovan[0]); j++ {
						resultcaulist := []model.Detaildaovan{}
						for i := 0; i < len(caudaovan); i++ {
							resultcaulist = append(resultcaulist, caudaovan[int64(i)][j])

						}
						result, _ := MaxResult(resultcaulist)
						resultcaudaovan = append(resultcaudaovan, result)

					}
				}
				if len(resultcaudaovan) > 0 {
					plagiarism1 := model.Plagiarism{}
					plagiarism1.Mafile = thongtincankiemtra.ID
					Percents := SumDetaildaovan(resultcaudaovan) / float64(len(resultcaudaovan))
					if Percents > 100 {
						Percents = 100
					}
					plagiarism1.Percents = Percents
					plagiarism1.Location = Updatepdfdaovan(thongtincankiemtra.Library.Location, strings.ReplaceAll(thongtincankiemtra.Library.Location, ".pdf", "-all.pdf"), resultcaudaovan, "all")
					plagiarism1.Loaikiemtra = "ALL"
					i.plagiarismRepository.Save(plagiarism1)

				}

				if len(checkdaovan) == 0 {

					plagiarism1 := model.Plagiarism{}
					plagiarism1.Mafile = thongtincankiemtra.ID
					plagiarism1.Percents = 0
					plagiarism1.Location = Updatepdfdaovan(thongtincankiemtra.Library.Location, strings.ReplaceAll(thongtincankiemtra.Library.Location, ".pdf", "-all.pdf"), resultcaudaovan, "all")
					plagiarism1.Loaikiemtra = "ALL"
					i.plagiarismRepository.Save(plagiarism1)
				}
			}

		}
		return true
	}
	return false
}
