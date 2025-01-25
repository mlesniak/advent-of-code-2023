package main

import (
	"bufio"
	"io"
	"os"
)

func main() {
	arr := read2D("17")

	for _, a := range arr {
		for _, v := range a {
			print(v)
		}
		println()
	}
}

func read2D(filename string) [][]int {
	file, _ := os.Open(filename)
	defer file.Close()
	r := bufio.NewReader(file)

	var arr [][]int
	for {
		s, err := r.ReadString('\n')
		if s[len(s)-1] == '\n' {
			s = s[:len(s)-1]
		}
		l := make([]int, len(s))
		for i, v := range s {
			l[i] = int(v) - '0'
		}
		arr = append(arr, l)
		if err == io.EOF {
			break
		}
	}
	return arr
}
