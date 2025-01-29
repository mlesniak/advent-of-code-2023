package main

import (
	"bufio"
	"container/heap"
	"io"
	"math"
	"os"
)
import "fmt"

type Step int

const (
	Left Step = iota
	Right
	Forward
)

func (s Step) String() string {
	switch s {
	case Left:
		return "L"
	case Right:
		return "R"
	case Forward:
		return "F"
	}

	panic("Unkown value")
}

type Orientation int

const (
	North Orientation = iota
	South
	West
	East
)

func (o Orientation) left() Orientation {
	switch o {
	case North:
		return West
	case West:
		return South
	case South:
		return East
	case East:
		return North
	}

	panic("Unknown value")
}

func (o Orientation) right() Orientation {
	switch o {
	case North:
		return East
	case East:
		return South
	case South:
		return West
	case West:
		return North
	}

	panic("Unknown value")
}

func (o Orientation) String() string {
	switch o {
	case North:
		return "N"
	case East:
		return "E"
	case South:
		return "S"
	case West:
		return "W"
	}

	panic("Unknown value")
}

func (o Orientation) step(x, y int) (int, int) {
	switch o {
	case North:
		return x, y - 1
	case East:
		return x + 1, y
	case South:
		return x, y + 1
	case West:
		return x - 1, y
	}

	panic("Unknown value")
}

type Path struct {
	x, y   int
	cost   int
	path   []Step
	facing Orientation

	// For pq maintenance.
	index int
}

func (p Path) String() string {
	return fmt.Sprintf("%d/%d $%d [%v] %v", p.x, p.y, p.cost, p.path, p.facing)
}

func (p *Path) clone() Path {
	var np = make([]Step, len(p.path))
	copy(np, p.path)

	return Path{
		x:      p.x,
		y:      p.y,
		cost:   p.cost,
		index:  p.index,
		facing: p.facing,
		path:   np,
	}
}

func main() {
	costs, grid := readGrid()
	visited := make(map[[2]int]struct{})
	visited[[2]int{0, 0}] = struct{}{}

	// pq contains the next node to evaluate based on their accumulated cost.
	var pq PriorityQueue
	for i := range grid {
		for j := range grid[i] {
			pq = append(pq, &Path{
				x:     j,
				y:     i,
				cost:  grid[i][j],
				index: -1,
			})
		}
	}
	heap.Init(&pq)

	next := heap.Pop(&pq).(*Path)
	for {
		//fmt.Println("\nLooking at", next)
		//for k, _ := range visited {
		//	fmt.Println("  Visited:", k)
		//}

		if next.x == len(grid[0])-1 && next.y == len(grid)-1 {
			break
		}

		// Compute available paths based on current position, orientation and past path.
		if len(next.path) == 0 || last(next.path) == Forward {
			// Add rotations
			var left = next.clone()
			left.path = append(left.path, Left)
			left.facing = left.facing.left()
			heap.Push(&pq, &left)
			//fmt.Println("Adding", left)

			var right = next.clone()
			right.path = append(right.path, Right)
			right.facing = right.facing.right()
			heap.Push(&pq, &right)
			//fmt.Println("Adding", right)

			// Add step if fewer than three steps
			if !line(next.path) {
				var forward = next.clone()
				nx, ny := forward.facing.step(forward.x, forward.y)
				if _, ok := visited[[2]int{nx, ny}]; !ok {
					if !(nx < 0 || nx >= len(grid[0]) || ny < 0 || ny >= len(grid)) {
						visited[[2]int{nx, ny}] = struct{}{}
						forward.path = append(forward.path, Forward)
						forward.cost += costs[ny][nx]
						forward.x = nx
						forward.y = ny
						heap.Push(&pq, &forward)
						//fmt.Println("Adding", forward)
					}
				}
			}
		} else {
			// After a rotation, we only allow a step. Not sure, if this limits the paths
			// we want to reach, since we can't go backwards without rotating twice in a row.
			if !line(next.path) {
				var forward = next.clone()
				nx, ny := forward.facing.step(forward.x, forward.y)
				if _, ok := visited[[2]int{nx, ny}]; !ok {
					if !(nx < 0 || nx >= len(grid[0]) || ny < 0 || ny >= len(grid)) {
						visited[[2]int{nx, ny}] = struct{}{}
						forward.path = append(forward.path, Forward)
						forward.cost += costs[ny][nx]
						forward.x = nx
						forward.y = ny
						heap.Push(&pq, &forward)
						//fmt.Println("Adding", forward)
					}
				}
			}
		}

		if pq.Len() == 0 {
			//panic("No valid path found")
			break
		}
		next = heap.Pop(&pq).(*Path)
	}

	fmt.Println(next)
	visualize(costs, next.path)
}

func visualize(cost [][]int, path []Step) {
	face := North
	x, y := 0, 0

	coords := make(map[[2]int]struct{})
	for _, p := range path {
		switch p {
		case Left:
			face = face.left()
		case Right:
			face = face.right()
		case Forward:
			x, y = face.step(x, y)
			coords[[2]int{x, y}] = struct{}{}
		}
	}

	for y = 0; y < len(cost); y++ {
		for x = 0; x < len(cost[y]); x++ {
			if _, ok := coords[[2]int{x, y}]; ok {
				print(".")
			} else {
				print(cost[y][x])
			}
		}
		println()
	}
}

func line(path []Step) bool {
	s := len(path)
	if s < 3 {
		return false
	}

	return path[s-1] == Forward && path[s-2] == Forward && path[s-3] == Forward
}

func last(path []Step) Step {
	return path[len(path)-1]
}

func readGrid() ([][]int, [][]int) {
	cost := read2D("17")
	grid := make([][]int, len(cost))
	for i := range grid {
		grid[i] = make([]int, len(cost[0]))
		for j := range grid[i] {
			grid[i][j] = math.MaxInt
		}
	}
	grid[0][0] = 0
	return cost, grid
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
