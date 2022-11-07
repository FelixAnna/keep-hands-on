package price

import "time"

type Price struct {
	ProductId int
	Price     float32
	DateTime  time.Time
}
