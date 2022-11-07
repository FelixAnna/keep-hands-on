package service

import (
	"net/http"
	"strconv"

	"example.com/demo/product-service/product/repo"
	"github.com/gin-gonic/gin"
)

type ProductService struct {
	dataService repo.ProductDataService
}

func NewProductService(dataService repo.ProductDataService) *ProductService {
	return &ProductService{
		dataService: dataService,
	}
}

func (service *ProductService) GetAll(c *gin.Context) {
	data := service.dataService.GetAll()

	c.JSON(http.StatusOK, data)
}

func (service *ProductService) GetById(c *gin.Context) {
	sid := c.Param("id")
	id, err := strconv.ParseInt(sid, 10, 32)
	if err != nil {
		c.JSON(http.StatusBadRequest, err)
		return
	}

	data := service.dataService.GetById(int(id))
	c.JSON(http.StatusOK, data)
}

func (api *ProductService) MemoryCosty(c *gin.Context) {
	//get result from somewhere
	times := c.DefaultQuery("times", "1000")
	itimes, err := strconv.ParseInt(times, 10, 32)
	if err != nil {
		itimes = 1000
	}

	results := make(map[int]int, itimes)

	for i := 1; i <= int(itimes); i++ {
		if i <= 2 {
			results[i] = i
			continue
		}

		results[i] = results[i-1] + results[i-2]
	}

	c.JSON(http.StatusOK, results)
}
