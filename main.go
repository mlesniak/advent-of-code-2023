package main

import (
	"bufio"
	"container/heap"
	"io"
	"math"
	"os"
)
import "fmt"

type Direction int

const (
	Left Direction = iota
	Right
	Straight
)

type Path struct {
	x, y int
	cost int
	path []Direction

	// For pq maintenance.
	index int
}

func main() {
	arr := read2D("17")
	//print2D(arr)

	dist := make([][]int, len(arr))
	for i := range dist {
		dist[i] = make([]int, len(arr[0]))
		for j := range dist[i] {
			dist[i][j] = math.MaxInt
		}
	}
	dist[0][0] = 0
	//print2D(dist)

	var pq PriorityQueue
	for i := range dist {
		for j := range dist[i] {
			pq = append(pq, &Path{
				x:     j,
				y:     i,
				cost:  dist[i][j],
				index: -1,
			})
		}
	}
	heap.Init(&pq)

	next := heap.Pop(&pq).(*Path)
	for {
		fmt.Println("Looking at %s", next)
		if next.x == len(arr[0])-1 && next.y == len(arr)-1 {
			break
		}

		// compute available paths based on current position, orientation and past path.

		if pq.Len() == 0 {
			panic("No valid path found")
		}
		next = heap.Pop(&pq).(*Path)
	}

	println("Solution")
	fmt.Println("%s", next)
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
