package repo

import "example.com/demo/price-service/price"

type PriceDataService interface {
	GetAllById(productId int) []price.Price
	GetLatestById(productId int) *price.Price
}
