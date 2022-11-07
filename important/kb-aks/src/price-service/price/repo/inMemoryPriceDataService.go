package repo

import (
	"time"

	"example.com/demo/price-service/price"
	linq "github.com/ahmetb/go-linq/v3"
)

var inMemoryPrices []price.Price

func init() {
	inMemoryPrices = []price.Price{
		{
			ProductId: 1,
			Price:     5.9,
			DateTime:  time.Date(2022, 01, 01, 00, 00, 00, 0, time.UTC),
		},
		{
			ProductId: 2,
			Price:     6.9,
			DateTime:  time.Date(2022, 01, 01, 00, 00, 00, 0, time.UTC),
		},
		{
			ProductId: 3,
			Price:     7.9,
			DateTime:  time.Date(2022, 01, 01, 00, 00, 00, 0, time.UTC),
		},
		{
			ProductId: 4,
			Price:     8.9,
			DateTime:  time.Date(2022, 01, 01, 00, 00, 00, 0, time.UTC),
		},
		{
			ProductId: 5,
			Price:     9.9,
			DateTime:  time.Date(2022, 01, 01, 00, 00, 00, 0, time.UTC),
		},
		{
			ProductId: 6,
			Price:     9.98,
			DateTime:  time.Date(2022, 01, 01, 00, 00, 00, 0, time.UTC),
		},

		{
			ProductId: 1,
			Price:     5.5,
			DateTime:  time.Date(2022, 01, 03, 00, 00, 00, 0, time.UTC),
		},

		{
			ProductId: 2,
			Price:     6.8,
			DateTime:  time.Date(2022, 05, 03, 00, 00, 00, 0, time.UTC),
		},
	}
}

type InMemoryPriceDataService struct {
	prices []price.Price
}

func NewInMemoryPriceDataService() PriceDataService {
	return &InMemoryPriceDataService{
		prices: inMemoryPrices,
	}
}

func (service *InMemoryPriceDataService) GetAllById(productId int) []price.Price {
	var query linq.Query = linq.From(service.prices)
	query = query.Where(func(i interface{}) bool {
		currentId := i.(price.Price).ProductId
		return currentId == productId
	})

	var results []price.Price
	query.ToSlice(&results)
	if len(results) > 0 {
		return results
	}

	return nil
}

func (service *InMemoryPriceDataService) GetLatestById(productId int) *price.Price {
	query := linq.From(service.prices).Where(func(i interface{}) bool {
		currentId := i.(price.Price).ProductId
		return currentId == productId
	}).OrderByDescending(func(i interface{}) interface{} {
		return i.(price.Price).DateTime.UTC().Unix()
	})

	var results []price.Price
	query.ToSlice(&results)
	if len(results) > 0 {
		return &results[0]
	}

	return nil
}
