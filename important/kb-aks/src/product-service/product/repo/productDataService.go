package repo

import "example.com/demo/product-service/product"

type ProductDataService interface {
	GetAll() []product.Product
	GetById(id int) *product.Product
}
