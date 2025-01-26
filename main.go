package main

import "container/heap"
import "math"
import "fmt"
import "os"
import "bufio"
import "io"

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
	pq = append(pq, &Path{
		x:     0,
		y:     0,
		cost:  0,
		index: 0,
	})
	pq = append(pq, &Path{
		x:     1,
		y:     0,
		cost:  -3,
		index: 0,
	})
	heap.Init(&pq)

	var next *Path = heap.Pop(&pq).(*Path)
	fmt.Println(*next)
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
