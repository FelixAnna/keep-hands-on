package service

import (
	"net/http"
	"testing"

	"example.com/demo/product-service/product"
	"example.com/demo/product-service/product/mocks"
	commonmock "github.com/FelixAnna/web-service-dlw/common/mocks"
	"github.com/stretchr/testify/assert"
	mockit "github.com/stretchr/testify/mock"
)

func setupService() (*mocks.ProductDataService, *ProductService) {
	mockService := &mocks.ProductDataService{}
	service := NewProductService(mockService)

	return mockService, service
}

func TestProvideDateApi(t *testing.T) {
	_, service := setupService()

	assert.NotNil(t, service)
	assert.NotNil(t, service.dataService)
}

func TestGetAll(t *testing.T) {
	mockService, service := setupService()

	ctx, writer := commonmock.GetGinContext(&commonmock.Parameter{})
	mockService.On("GetAll").Return([]product.Product{})

	//need mock gin.Context.Writer
	service.GetAll(ctx)

	assert.NotNil(t, ctx)
	assert.NotNil(t, writer)
	assert.Equal(t, writer.Code, http.StatusOK)
	mockService.AssertExpectations(t)
}

func TestGetById(t *testing.T) {
	mockService, service := setupService()

	ctx, writer := commonmock.GetGinContext(&commonmock.Parameter{Params: map[string]string{"id": "1"}})
	mockService.On("GetById", mockit.Anything).Return(&product.Product{})

	//need mock gin.Context.Writer
	service.GetById(ctx)

	assert.NotNil(t, ctx)
	assert.NotNil(t, writer)
	assert.Equal(t, writer.Code, http.StatusOK)
	mockService.AssertExpectations(t)
}
