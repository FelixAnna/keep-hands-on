package main

import (
	"bytes"
	"encoding/json"
	"io"
	"log"
	"net/http"
	"net/http/httptest"
	"testing"

	"example.com/demo/product-service/di"
	"example.com/demo/product-service/product"
	"github.com/gin-gonic/gin"
	"github.com/stretchr/testify/assert"
)

var router *gin.Engine

func init() {
	gin.SetMode(gin.TestMode)
	initialMockDependency()
	router = GetGinRouter()
}

func TestRunning(t *testing.T) {

	w := performRequest(router, "GET", "/status", nil)

	assert.Equal(t, http.StatusOK, w.Code)
	assert.Equal(t, "running", w.Body.String())
}

func TestGetAllProduct(t *testing.T) {
	//Act
	w := performRequest(router, "GET", "/products/", nil)

	var response []product.Product
	err := json.Unmarshal(w.Body.Bytes(), &response)

	//Assert
	assert.Equal(t, http.StatusOK, w.Code)
	assert.Nil(t, err)
	assert.NotNil(t, response)
}

func TestGetProductById(t *testing.T) {
	//Act
	w := performRequest(router, "GET", "/products/1", nil)

	var response product.Product
	err := json.Unmarshal(w.Body.Bytes(), &response)

	//Assert
	assert.Equal(t, http.StatusOK, w.Code)
	assert.Nil(t, err)
	assert.NotNil(t, response)
}

func TestSlowAuthorized(t *testing.T) {
	//Act
	w := performRequest(router, "GET", "/products/slow", nil)

	var response map[int]int
	err := json.Unmarshal(w.Body.Bytes(), &response)

	//Assert
	assert.Equal(t, http.StatusOK, w.Code)
	assert.Nil(t, err)
	assert.NotNil(t, response)
}

func performRequest(r http.Handler, method, path string, body interface{}) *httptest.ResponseRecorder {
	var readerOfBody io.Reader = nil
	if body != nil {
		data, err := json.Marshal(body)
		if err != nil {
			log.Fatal(err)
		}

		readerOfBody = bytes.NewReader(data)
	}

	req, _ := http.NewRequest(method, path, readerOfBody)
	w := httptest.NewRecorder()
	r.ServeHTTP(w, req)
	return w
}

func initialMockDependency() {
	apiBoot = &ApiBoot{}
	productApiService := di.InitialProductService()

	apiBoot = &ApiBoot{
		ProductService: productApiService,
		ErrorHandler:   di.InitialErrorMiddleware(),
		Registry:       di.InitialMockRegistry(),
	}
}
