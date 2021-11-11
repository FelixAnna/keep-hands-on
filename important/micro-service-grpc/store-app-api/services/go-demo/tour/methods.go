package main

import (
	"fmt"
	"image"
	"image/color"
	"io"
	"os"
	"strings"
)

func main() {
	rect := Shape{3.1, 4.2}
	fmt.Println(rect.Area(), rect.Weight(0.9), Area(rect))

	//all point indirection only happens with methods, not for pure function, funcs need the exactly type of the argument
	//pointer indirection: go interprets the statement as (&rect).Scale(2.0) automatically
	rect.Scale(2.0)
	fmt.Println(rect.Area(), rect.Weight(0.9), Area(rect))

	tra := &Shape{3.1, 4.2}
	tra.Scale(3.0)
	//pointer indirection: go interprets the statement as (*tra).Area() automatically
	fmt.Println((*tra).Area())

	f := MyFloat(1.5)
	f.methodsExample()
	fmt.Println(f)

	var geo Geometry
	geo = &Triangle{3.0, 4.0, 5.0} //a *T implement Geometry need start with &, otherwise just use value type
	geo2 := MyFloat2(0)

	fmt.Println(geo.Area(), geo.Circumference())
	fmt.Println(geo2.Area() /*, geo2.Circumference()*/)

	//empty interface can hold values of any type
	var i, j, k, l interface{}
	i = 42
	j = "string"
	k = MyFloat2(7.8)
	l = &Triangle{4, 5, 6}
	j = 3.0

	fmt.Println(i, j, k, l)

	//I.(T) print interface value's underlying type
	s, ok := k.(MyFloat2)
	fmt.Println(s, ok)

	//type switch - compare underlying type of the interface
	switch v := l.(type) {
	case string:
		fmt.Println("String")
	case Triangle:
		fmt.Println("Triangle")
	case *Triangle:
		fmt.Println("*Triangle")
	default:
		fmt.Println("Unknown", v)
	}

	//test error type
	if ret, err := Sqrt(19); err != nil {
		fmt.Println(err)
	} else {
		fmt.Println(ret)
	}

	str := strings.NewReader("Lbh penpxrq gur pbqr!")
	r := rot13Reader{str}
	io.Copy(os.Stdout, &r)
	m := image.NewRGBA(image.Rect(0, 0, 100, 100))
	fmt.Println(m.Bounds())
	fmt.Println(m.At(10, 10).RGBA())
}

type Shape struct {
	X, Y float32
}

//Methods is a function with a special receiver agument: (s Shape) here
//still recommand to use pointer receiver here: (s *Shape) for not need to copy value in memory
func (s Shape) Area() float32 {
	return s.X * s.Y
}

//Pointer receiver can modify the original value, while Value receiver modify the copy - so normally we use pointer receiver
//Pointer receiver can modify the original value, as well as no need to copy the value when passing as args
func (s *Shape) Scale(f float32) {
	s.X *= f
	s.Y *= f
}

//Normal function
func Area(s Shape) float32 {
	return s.X * s.Y
}

func (s Shape) Weight(p float32) float32 {
	return s.X * s.Y * p
}

type MyFloat float32

//method can also define on non-struct type
//the type func used shoud be in the same package, not in another package like int/float32...
func (f MyFloat) methodsExample() float32 {
	return float32(f) * 2
}

type Geometry interface {
	Area() float32
	Circumference() float32
}

type Triangle struct {
	a, b, c float32
}

//interface are been implemented implicitly: can been in different package
func (t *Triangle) Area() float32 {
	sum := float32(0)
	sum = t.a * t.b / 2
	return sum
}

func (t *Triangle) Circumference() float32 {
	return t.a + t.b + t.c
}

type MyFloat2 float32

func (f MyFloat2) Area() float32 {
	return float32(f)
}

///built-in fmt.Stringer interface have something like toString() in other language
func (t *Triangle) String() string {
	return fmt.Sprintf("Triangle: a=%v, b=%v, c=%v;", t.a, t.b, t.c)
}

///built-in fmt.error interface have Error() string
type ErrNegSqrt float64

func (er ErrNegSqrt) Error() string {
	//should use float64(er) instead, otherwise will call er.Error() when is a error type, call xx.String() for non-Error type
	return fmt.Sprintf("cannot sqrt negative: %v", float64(er))
}

func Sqrt(x float64) (float64, error) {
	if x < 0 {
		return 0, ErrNegSqrt(x)
	}

	r := float64(1.0)
	z := float64(x / 2.0)
	fmt.Println(x)
	for i := 0; i < 10; i++ {
		z -= (z*z - x) / (2 * z)
		//fmt.Println(i, z, r)

		r = z
	}
	const aa, bb, cc = iota, iota, iota //0,0,0
	fmt.Println(aa, bb, cc)

	const (
		a = 1 << iota // a == 1  (iota == 0)
		b = 1 << iota // b == 2  (iota == 1)
		c = 3         // c == 3  (iota == 2, unused)
		d = 1 << iota // d == 8  (iota == 3)
	)
	/*
		αβ :=1
		const ab =1
		fmt.Println(αβ)*/
	return r, nil
}

/// rot13 example - io.Reader implementation
type rot13Reader struct {
	r io.Reader
}

func (rot13 rot13Reader) Read(b []byte) (n int, err error) {
	le, err := rot13.r.Read(b)

	fmt.Println(le)
	for i := 0; i < le; i++ {
		b[i] = Rot13(b[i])
	}

	if err == io.EOF {
		return 0, io.EOF
	}

	return le, nil
}

func Rot13(v byte) byte {
	j := v + 13
	switch {
	case v <= 90 && v >= 65:
		if j > 90 {
			v = j - 90 + 64
		} else {
			v = j
		}

	case v <= 122 && v >= 97:
		if j > 122 {
			v = j - 122 + 96
		} else {
			v = j
		}
	default:
		//do nothing
	}

	return v
}

///Image example
type Image struct{ w, h int }

func (m Image) Bounds() image.Rectangle {

	return image.Rect(0, 0, m.w, m.h)
}

func (m Image) ColorModel() color.Model {

	return color.RGBAModel
}

func (m Image) At(x, y int) color.Color {
	return color.RGBA{uint8(x ^ y/255), uint8(y ^ x/255), 255, 255}
}
