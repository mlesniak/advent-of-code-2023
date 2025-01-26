// Source: https://pkg.go.dev/container/heap#example-package-PriorityQueue
package main

type PriorityQueue []*Path

// Less defined via sort.Interface.
func (pq PriorityQueue) Len() int {
	return len(pq)
}

// Less defined via sort.Interface.
func (pq PriorityQueue) Less(i, j int) bool {
	return pq[i].cost < pq[j].cost
}

// Swap defined via sort.Interface.
func (pq PriorityQueue) Swap(i, j int) {
	pq[i], pq[j] = pq[j], pq[i]
	pq[i].index = i
	pq[j].index = j
}

// Push defined via heap.Interface.
func (pq *PriorityQueue) Push(x any) {
	n := len(*pq)
	item := x.(*Path)
	item.index = n
	*pq = append(*pq, item)
}

// Push defined via heap.Interface.
func (pq *PriorityQueue) Pop() any {
	old := *pq
	n := len(old)
	item := old[n-1]
	old[n-1] = nil
	item.index = -1
	*pq = old[0 : n-1]
	return item
}
