package main

import (
	"bufio"
	"container/heap"
	"io"
	"os"
	"strings"
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
	// @mlesniak do we really need it when we have the global cost matrix?
	visited map[[2]int]struct{}

	// For pq maintenance.
	index int
}

func (p Path) String() string {
	return fmt.Sprintf("%d/%d $%d [%v] %v", p.x, p.y, p.cost, p.path, p.facing)
}

func (p *Path) clone() Path {
	var np = make([]Step, len(p.path))
	copy(np, p.path)
	var vs = make(map[[2]int]struct{})
	for k, _ := range p.visited {
		vs[k] = struct{}{}
	}

	return Path{
		x:       p.x,
		y:       p.y,
		cost:    p.cost,
		index:   p.index,
		facing:  p.facing,
		path:    np,
		visited: vs,
	}
}

func main() {
	cost := read2D("17")
	visited := make(map[[2]int]struct{})
	visited[[2]int{0, 0}] = struct{}{}

	var pq PriorityQueue
	pq = append(pq, &Path{
		x:     0,
		y:     0,
		cost:  0,
		index: -1,
	})
	heap.Init(&pq)

	pathCost := make(map[string]int)

	minCost := -1
	for {
		if len(pq) == 0 {
			break
		}
		cur := heap.Pop(&pq).(*Path)
		if minCost != cur.cost {
			println(minCost)
			minCost = cur.cost
		}
		//fmt.Println(cur)

		// Did we reach the goal?
		if cur.x == len(cost[0])-1 && cur.y == len(cost)-1 && LastValues(cur.path, 4, Forward) {
			fmt.Println("\n\nSolution:", cur)
			visualize(cost, cur.path)
			break
			//continue
		}

		// Add rotations.
		if len(cur.path) == 0 || lastSteps(cur.path, 4, Forward) {
			var left = cur.clone()
			left.path = append(left.path, Left)
			left.facing = left.facing.left()
			heap.Push(&pq, &left)

			var right = cur.clone()
			right.path = append(right.path, Right)
			right.facing = right.facing.right()
			heap.Push(&pq, &right)
		}

		// After a rotation, we only allow steps. Not sure, if this limits the paths
		// we want to reach, since we can't go backwards without rotating twice in a row.
		//if strings.Contains(cur.String(), "[[R F F F F F F F F F F") {
		//	println("breakpoint")
		//}
		// a minimum of four blocks in that direction before it can turn (or even before it can stop at the end).
		if forwards(cur.path) < 10 {
			//if !line(cur.path, 3) {
			var forward = cur.clone()
			nx, ny := forward.facing.step(forward.x, forward.y)
			if _, ok := cur.visited[[2]int{nx, ny}]; !ok {
				if !(nx < 0 || nx >= len(cost[0]) || ny < 0 || ny >= len(cost)) {
					// check that entering a field is worth it by checking if we are cheaper than a visited path
					forward.visited[[2]int{nx, ny}] = struct{}{}
					forward.path = append(forward.path, Forward)
					forward.cost += cost[ny][nx]
					forward.x = nx
					forward.y = ny
					if nx == len(cost[0])-1 && ny == len(cost)-1 {
						// At the end? Do not add to visit list.
					} else {
						cur.visited[[2]int{nx, ny}] = struct{}{}
					}
					v := visitedKey(Visited{nx, ny, forward.facing, LastN(forward.path, 10)})
					curCost, found := pathCost[v]
					// We can't simply compare cost, since we have additional restrictions.
					if forward.cost < curCost || !found {
						heap.Push(&pq, &forward)
						pathCost[v] = forward.cost
					}
				}
			}
		}
	}
}

// 881 too high

func forwards(steps []Step) int {
	count := 0
	for k := len(steps) - 1; k >= 0; k-- {
		if steps[k] == Forward {
			count++
		} else {
			break
		}
	}
	return count
}

func LastThree[T any](s []T) []T {
	if len(s) < 3 {
		return s
	}

	return s[len(s)-3:]
}

func LastValues(path []Step, num int, expected Step) bool {
	if len(path) < num {
		return false
	}

	for k := len(path) - 1; k >= len(path)-num; k-- {
		if path[k] != expected {
			return false
		}
	}

	return true
}

func LastN[T any](s []T, num int) []T {
	if len(s) < num {
		return s
	}

	return s[len(s)-num:]
}

type Visited struct {
	x, y   int
	facing Orientation
	steps  []Step
}

func visitedKey(v Visited) string {
	var sb strings.Builder
	sb.WriteString(fmt.Sprintf("%d,%d,%s", v.x, v.y, v.facing.String()))
	for _, step := range v.steps {
		sb.WriteString("," + step.String())
	}
	return sb.String()
}

func visualize(cost [][]int, path []Step) {
	face := North
	x, y := 0, 0

	coords := make(map[[2]int]Orientation)
	for _, p := range path {
		switch p {
		case Left:
			face = face.left()
		case Right:
			face = face.right()
		case Forward:
			x, y = face.step(x, y)
			coords[[2]int{x, y}] = face
		}
	}

	for y = 0; y < len(cost); y++ {
		for x = 0; x < len(cost[y]); x++ {
			if d, ok := coords[[2]int{x, y}]; ok {
				fmt.Printf("%c", d.char())
			} else {
				fmt.Printf("%d", cost[y][x])
			}
		}
		fmt.Println()
	}
}

func (o Orientation) char() int8 {
	switch o {
	case East:
		return '>'
	case West:
		return '<'
	case North:
		return '^'
	case South:
		return 'v'
	}

	panic("")
}

func line(path []Step, max int) bool {
	s := len(path)
	if s < max {
		return false
	}

	for k := s - 1; k >= s-max; k-- {
		if path[k] != Forward {
			return false
		}
	}

	return true
}

func lastSteps(path []Step, num int, step Step) bool {
	if len(path) < num {
		return false
	}

	for k := len(path) - 1; k >= len(path)-num; k-- {
		if path[k] != step {
			return false
		}
	}

	return true
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
