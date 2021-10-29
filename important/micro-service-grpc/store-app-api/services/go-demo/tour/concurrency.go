package main

import (
	"fmt"
	"sync"
	"time"
)

func main() {
	//goroutinesExample()

	//channelsExample()

	fetchResultCache = &fetchResult{status: map[string]bool{}}
	Crawl("https://golang.org/", 4, fetcher)
	//selectExample()
}

func goroutinesExample() {
	a := goroutinesExampleFunc()

	//goroutine is a lightweight thread managed by go runtime
	go a("hello")
	sum, to := a("hi")
	fmt.Printf("%v, %v\n", sum, to)
}

func goroutinesExampleFunc() func(s string) (string, int) {

	summary := ""
	count := 0

	return func(s string) (string, int) {
		for i := 0; i < 50; i++ {
			//this is not thread safe, sync can make it sychronized
			summary += s + ","
			time.Sleep(10 * time.Millisecond)
			count++
		}

		return summary, count
	}
}

func channelsExample() {
	//Channel: you can send data to channel, and receive data from channel,
	//send and receive blocks until other side is ready
	c := make(chan int)
	a := [10]int{0, 1, 2, 3, 4, 5, 6, 7, 8, 9}

	// use goroutine to run them on different thread
	go channelsExampleFunc(a[:5], c)
	go channelsExampleFunc(a[5:], c)

	//x can be the first one or second one, no sequence gurrentee
	x, y := <-c, <-c

	fmt.Println(x, y, x+y)

	//buffered channel
	cb := make(chan int, 2)
	cb <- 3
	cb <- 5
	//cb <- 6 //buffer is full
	go channelsExampleBufferFunc(a[:1], cb)
	go channelsExampleBufferFunc(a[1:5], cb)
	value, ok := <-cb
	fmt.Println(value, ok)

	//channel support range in for loop, continue until channel closed
	cf := make(chan int, 50)
	fibonacci(cap(cf), cf)
	for v := range cf {
		fmt.Println(v)
	}
}

func channelsExampleFunc(sl []int, c chan int) {
	sum := 0
	for _, v := range sl {
		sum += (2 ^ v)
		fmt.Println(v, 2^v)
	}

	c <- sum
}

func channelsExampleBufferFunc(sl []int, c chan int) {
	for _, v := range sl {
		c <- v * v
	}

	//close(c) //can close on sender side
}

func fibonacci(n int, c chan int) {
	x, y := 0, 1
	for i := 0; i < n; i++ {
		c <- x //like Enumerator
		x, y = y, x+y
	}

	close(c)
}

func selectExample() {
	sa := make([]int, 10)
	sb := make([]int, 10)

	c := make(chan int)
	q := make(chan int)
	go func() {
		//var i int
		len := cap(sa)
		for i := 0; i < len; i++ {
			c <- i
			sa[i] = i
			if i == 0 {
				sb[i] = sa[i]
			} else {
				sb[i] = sa[i] + sb[i-1]
			}
		}

		q <- 1
	}()
	for {
		//select blocks until one of it case can run.
		//if have default,  when no case, it runs into default
		select {
		case <-c:
			fmt.Println("<-c")
		case <-q:
			return
		default:
			for i, v := range sb {
				fmt.Println(i, v)
			}
			time.Sleep(time.Millisecond * 1000)
		}
	}

}

/*  Compare binary tree

package main

import (
	"golang.org/x/tour/tree"
	"fmt"
)

func WalkInternal(t *tree.Tree, ch chan int) {
	if t== nil {
		return
	}

	WalkInternal(t.Left, ch)

	ch <- t.Value
	//fmt.Println(t.Value)

	WalkInternal(t.Right, ch)
}

// Walk walks the tree t sending all values
// from the tree to the channel ch.
func Walk(t *tree.Tree, ch chan int) {
	WalkInternal(t,ch)
	close(ch)
}

// Same determines whether the trees
// t1 and t2 contain the same values.
func Same(t1, t2 *tree.Tree) bool {
	tc1 := make(chan int, 10)
	tc2 := make(chan int, 10)

	go Walk(t1, tc1)
	go Walk(t2, tc2)

	for{
		select{
			case te1, ok1 := <-tc1:
				te2, ok2 := <-tc2
				if ok1 == ok2 {
					//all done - chan closed
					if ok1 == false {
						fmt.Println("same")
						return true
					}

					//value not same, no need to sort as we read from a sorted tree
					if te1 !=te2 {
						fmt.Println("not same")
						return false
					}
					//fmt.Println("same:", te1, te2)
					continue
				}else{
					fmt.Println("not same")  //not same length
					return false
				}

		}
	}

	//return same
}

func main() {
	Same(tree.New(1), tree.New(1))
	Same(tree.New(1), tree.New(2))
	Same(tree.New(2), tree.New(2))
}
*/

type Fetcher interface {
	// Fetch returns the body of URL and
	// a slice of URLs found on that page.
	Fetch(url string) (body string, urls []string, err error)
}

// Crawl uses fetcher to recursively crawl
// pages starting with url, to a maximum of depth.
func Crawl(url string, depth int, fetcher Fetcher) {
	// TODO: Fetch URLs in parallel.
	// TODO: Don't fetch the same URL twice.
	// This implementation doesn't do either:
	if depth <= 0 {
		return
	}

	fetchResultCache.mu.Lock()

	if _, ok := fetchResultCache.status[url]; ok {
		defer fetchResultCache.mu.Unlock()
		return
	}

	body, urls, err := fetcher.Fetch(url)
	if err != nil {
		fetchResultCache.status[url] = false
		fmt.Println(err)
		fetchResultCache.mu.Unlock()
		return
	}
	fetchResultCache.status[url] = true
	fmt.Printf("found: %s %q\n", url, body)
	fetchResultCache.mu.Unlock()

	for _, u := range urls {
		Crawl(u, depth-1, fetcher)
	}

	return
}

var fetchResultCache *fetchResult

// fakeFetcher is Fetcher that returns canned results.
type fetchResult struct {
	mu     sync.Mutex
	status map[string]bool
}

type fakeFetcher map[string]*fakeResult

type fakeResult struct {
	body string
	urls []string
}

func (f fakeFetcher) Fetch(url string) (string, []string, error) {
	if res, ok := f[url]; ok {
		return res.body, res.urls, nil
	}
	return "", nil, fmt.Errorf("not found: %s", url)
}

// fetcher is a populated fakeFetcher.
var fetcher = fakeFetcher{
	"https://golang.org/": &fakeResult{
		"The Go Programming Language",
		[]string{
			"https://golang.org/pkg/",
			"https://golang.org/cmd/",
		},
	},
	"https://golang.org/pkg/": &fakeResult{
		"Packages",
		[]string{
			"https://golang.org/",
			"https://golang.org/cmd/",
			"https://golang.org/pkg/fmt/",
			"https://golang.org/pkg/os/",
		},
	},
	"https://golang.org/pkg/fmt/": &fakeResult{
		"Package fmt",
		[]string{
			"https://golang.org/",
			"https://golang.org/pkg/",
		},
	},
	"https://golang.org/pkg/os/": &fakeResult{
		"Package os",
		[]string{
			"https://golang.org/",
			"https://golang.org/pkg/",
		},
	},
}
