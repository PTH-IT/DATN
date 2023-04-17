package model

type Detaildaovan struct {
	Keywor1             string
	Keywor2             string
	Locationfilecompare string
	SentenceKeyword     []string
	Percent             float64
}

type Daovan struct {
	File1    ThongTinBaiTapTL
	Sentence []Detaildaovan
	Percent  float64
}
