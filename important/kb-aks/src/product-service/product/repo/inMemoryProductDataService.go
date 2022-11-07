package repo

import (
	"example.com/demo/product-service/product"
	linq "github.com/ahmetb/go-linq/v3"
)

var inMemoryProducts []product.Product

func init() {
	inMemoryProducts = []product.Product{
		{
			Id:       1,
			Picture:  "1.png",
			Name:     "Apple",
			Category: "Fruit",
		},
		{
			Id:       2,
			Picture:  "2.png",
			Name:     "Orange",
			Category: "Fruit",
		},
		{
			Id:       3,
			Picture:  "3.png",
			Name:     "Banana",
			Category: "Fruit",
		},
		{
			Id:       4,
			Picture:  "4.png",
			Name:     "potato",
			Category: "Vegetable",
		},
		{
			Id:       5,
			Picture:  "5.png",
			Name:     "Tomato",
			Category: "Vegetable",
		},
		{
			Id:       6,
			Picture:  "6.png",
			Name:     "Vegetable",
			Category: "Vegetable",
		},
	}
}

type InMemoryProductDataService struct {
	products []product.Product
}

func NewInMemoryProductDataService() ProductDataService {
	return &InMemoryProductDataService{
		products: inMemoryProducts,
	}
}

func (service *InMemoryProductDataService) GetAll() []product.Product {
	return service.products
}

func (service *InMemoryProductDataService) GetById(id int) *product.Product {
	var query linq.Query = linq.From(service.products)
	query = query.Where(func(i interface{}) bool {
		currentId := i.(product.Product).Id
		return currentId == id
	})

	var results []product.Product
	query.ToSlice(&results)
	if len(results) > 0 {
		return &results[0]
	}

	return nil
}
