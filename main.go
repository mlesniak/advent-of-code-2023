package main

import (
	"bufio"
	"fmt"
	"io"
	"math"
	"os"
)

func main() {
	arr := read2D("17")
	print2D(arr)

	dist := make([][]int, len(arr))
	for i := range dist {
		dist[i] = make([]int, len(arr[0]))
		for j := range dist[i] {
			dist[i][j] = math.MaxInt
		}
	}
	print2D(dist)
}

func print2D(arr [][]int) {
	for _, a := range arr {
		for i, v := range a {
			fmt.Printf("%d", v)
			if i != len(a)-1 {
				fmt.Printf(",")
			}
		}
		fmt.Println()
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
