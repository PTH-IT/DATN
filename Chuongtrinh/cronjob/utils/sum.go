package utils

func SumArray(arr []float64) float64 {
	sum := float64(0)
	for _, value := range arr {
		sum += value
	}

	return sum
}
func FindMax(arr []float64) (float64, int64) {
	max := arr[0]
	index := 0
	for i, value := range arr {
		if value > max {
			max = value
			index = i
		}
	}

	return max, int64(index)
}
