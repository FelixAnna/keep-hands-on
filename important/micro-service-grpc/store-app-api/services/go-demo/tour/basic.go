package main

import (
	"fmt"
	"runtime"
	"strings"
	"time"
)

func main() {
	i, j := 42, 100
	p := &i
	*p = 21
	fmt.Println(i, &j, p, *p)

	sub(i)
	subwith(&i)

	forLoop(10, 100000000)

	var a string
	a = ifExample(10)
	fmt.Println(a)

	swExample(time.Monday)

	deferExample()

	pointExample()

	structExample()

	arrayAndSlicesExample()

	mapExample()

	funcExample()
}

func sub(v int) {
	v *= v
	fmt.Println(&v, v)
}

func subwith(p *int) {
	*p *= *p
	fmt.Println(p, *p)
}

func forLoop(round, max int) {
	sum := 0
	//the only loop in go,
	for i := 0; i < round; i++ {
		sum += i * i
	}

	//only condition is necessary, like while in other language
	for sum < max {
		sum += sum
	}

	/* loop forever example:
	for {
	}
	*/

	fmt.Println(sum)
}

func ifExample(number int) string {
	if number < 0 {
		return "neg"
	}

	//if with a short statement
	if i := number % 2; i == 0 {
		return "even"
	} else {
		return "odd"
	}
}

func swExample(day time.Weekday) {

	//summary: switch without break; switch with evaluation; swith with different expression(no condition, condition on case side)

	//switch example, dont need break (go auto add for us)
	fmt.Print("Go runs on ")
	switch os := runtime.GOOS; os {
	case "darwin":
		fmt.Println("OS X.")
	case "linux":
		fmt.Println("Linux.")
	default:
		fmt.Printf("%s.\n", os)
	}

	//switch evaluation example
	fmt.Printf("When's %s?\n", day)
	today := time.Now().Weekday()
	switch day {
	case today:
		fmt.Println("Today")
	case today + 1:
		fmt.Println("Tommorrow")
	case today + 2:
		fmt.Println("The day after tommorrow")
	default:
		fmt.Println("Too far away")
	}

	//switch without condition
	t := time.Now()
	switch {
	case t.Hour() < 12:
		fmt.Println("Good Afternoon!")
	case t.Weekday() == time.Saturday:
		fmt.Println("Happy weekend!")
	default:
		fmt.Println("Good luck!")
	}
}

func deferExample() {
	//defer function execute when the surrounding function returns, in a fist-in-last-out order,
	// the evalution of their argument are calculated immediately, then push the calls to stack, finally excute them in a reverse order
	//can use for cleanup actions
	for i := 1; i < 10; i++ {
		defer fmt.Println(i)
	}

	fmt.Println("Start count down:")
}

func pointExample() {
	var p *float64
	f64 := float64(1.34)
	p = &f64
	*p *= *p
	var q *float64
	fmt.Println(*p, q)
	q = p
	*q = 35.0
	fmt.Println(*p, *q)
}

//struct is collection of fields
type Shape struct {
	X    float32
	Y    float32
	W, Z int
}

func structExample() {
	var nouse Shape
	nouse = Shape{7.10, 9.03, 10, 1}
	nouse = Shape{} //X,Y,W,Z=0

	nouse2 := &Shape{} //*Shape

	fmt.Println(nouse2, nouse)

	shape := Shape{X: 1.5, Y: 6.07} //W,Z=0 are implicit
	shape.X = 2.5

	rectange := &shape
	//(*rectange).X can me simplified in go
	rectange.X = 2.03e5
	fmt.Println(shape, rectange)
}

func arrayAndSlicesExample() {
	var arr [10]int
	arr2 := [3]int{1, 4, 7}

	fmt.Println(arr2, arr)

	var sl []int
	sl = arr2[1:2] //from 1 to 2 index, but not include the higher end - 2, index start from 0
	fmt.Println(sl)

	//Slices are like reference to arrays: modify slice also modify the underlying array
	sl[0] = 5
	fmt.Println(sl, arr2)

	sl2 := []int{1, 3, 5, 7, 9, 10} //slice Literals
	sl3 := []struct {
		a, b int
	}{{1, 2}, {2, 4}, {3, 6}}
	sl4 := sl2[1:] //use default bound expression
	fmt.Println("sl4:", sl4)
	sl4 = sl4[:2] //decrease its length
	fmt.Println("sl4:", sl4)
	sl4 = sl4[1:] //drop the first 1 elements

	fmt.Println(sl2, sl3, sl4)

	var sl5 []int //define nil slice
	if sl5 == nil {
		fmt.Println("Found nil!", sl5, len(sl5), cap(sl5))
	}

	//define slice by make function
	sl6 := make([]int, 0, 10)
	fmt.Println(sl6)

	//Slices of slices
	sl7 := [][]float32{{1.01, 2.0}, {3.1, 4.2}}
	sl7[0][1] = 2.01

	//append slice
	sl7 = append(sl7, []float32{5.01, 6.07, 7.08})
	sl7[0] = append(sl7[0], 3.11, 3.12, 3.13)
	fmt.Println(sl7)

	//loop slice by retun index and value
	for i, v := range sl7 {
		fmt.Println(i, v)
	}

	//use _ to drop index/value
	for _, v := range sl7[0] {
		fmt.Println(v)
	}

	//only return index, can ignore value
	for i := range sl7[1] {
		fmt.Println(i)
	}
}

func Pic(dx, dy int) [][]uint8 {

	sl := make([][]uint8, dy)

	//range for slice
	for i := range sl {
		sl[i] = make([]uint8, dx)
		for j := range sl[i] {
			sl[i][j] = uint8(i ^ j)
		}
	}

	/* working:
	for i := 0; i < dy; i++ {
		sl[i] = make([]uint8, dx, dx)

		for j := 0; j < dx; j++ {
			sl[i][j] = (uint8(j + i)) / 2
		}
	}

	/* not working
	for r, v := range sl {
		v = make([]uint8, dx, dx)
		for i := 0; i < dx; i++ {
			v[i] = (uint8(r + i)) / 2
		}
	}*/

	return sl
}
func mapExample() {
	var mp map[string]int
	mp = make(map[string]int)          //make assignment
	mp = map[string]int{"iwhat": 2019} //Literal assignment
	mp["hi"] = 2016
	mp["test"] = 2020
	delete(mp, "hi") //delete map element
	element, ok := mp["test"]
	element2, ok2 := mp["test2"]

	fmt.Println(mp, element, ok, element2, ok2)
}

func WordCount(s string) map[string]int {
	wordCount := make(map[string]int)
	words := strings.Fields(s)
	for _, w := range words {
		_, ok := wordCount[w]
		if ok {
			wordCount[w] += 1
		} else {
			wordCount[w] = 1
		}
	}
	return wordCount
}

func funcExampleMu(x, y int) int {
	return x * y
}

func funcExampleDi(x, y int) int {
	return x / y
}

func funcExampleFunc() func(int, int) int {
	//Function closures: func is bound to variable
	sum := 0
	return func(x, y int) int {
		sum += x + y
		return sum
	}
}

func funcExampleCaller(fn func(int, int) int, x, y int) int {
	ret := fn(x, y)

	return ret
}

func funcExample() int {
	//func can pass/return func,
	//inbound function can accept outside variables and update inside variables - function closure
	tf := funcExampleCaller(funcExampleMu, 3, 4)

	ff := funcExampleCaller(funcExampleDi, 10, 5)

	fmt.Println(tf, ff)

	rs := funcExampleFunc()

	for i := 0; i < 10; i++ {
		fmt.Println(rs(tf+i, ff))
	}

	return funcExampleCaller(funcExampleMu, tf, ff)
}

func fibonacci() func() int {
	v := make([]int, 0, 100)
	index := 0
	return func() int {
		switch index {
		case 0:
			v = append(v, 0)
		case 1:
			v = append(v, 1)
		default:
			v = append(v, v[index-1]+v[index-2])
		}

		index += 1

		return v[index-1]
	}
}
