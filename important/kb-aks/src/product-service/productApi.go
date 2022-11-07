package main

import (
	"net/http"

	"example.com/demo/product-service/di"
	"example.com/demo/product-service/product/service"
	"github.com/FelixAnna/web-service-dlw/common/mesh"
	"github.com/FelixAnna/web-service-dlw/common/micro"
	"github.com/FelixAnna/web-service-dlw/common/middleware"
	"github.com/gin-gonic/gin"
)

const SERVER_NAME = "product-api"

func main() {
	initialDependency()
	router := GetGinRouter()

	//router.Run(":8181")
	micro.StartApp(SERVER_NAME, ":8181", router, apiBoot.Registry.GetRegistry())
}

type ApiBoot struct {
	ProductService *service.ProductService
	ErrorHandler   *middleware.ErrorHandlingMiddleware
	Registry       *mesh.Registry
}

var apiBoot *ApiBoot

func initialDependency() {
	apiBoot = &ApiBoot{
		ProductService: di.InitialProductService(),
		ErrorHandler:   di.InitialErrorMiddleware(),
		Registry:       di.InitialRegistry(),
	}
}

func GetGinRouter() *gin.Engine {
	router := gin.New()

	//define middleware before apis
	micro.RegisterMiddlewares(router, apiBoot.ErrorHandler.ErrorHandler())
	defineRoutes(router)

	return router
}

func defineRoutes(router *gin.Engine) {
	router.GET("/status", func(c *gin.Context) {
		c.String(http.StatusOK, "running")
	})

	userGroupRouter := router.Group("/products")
	{
		userGroupRouter.GET("/", apiBoot.ProductService.GetAll)
		userGroupRouter.GET("/:id", apiBoot.ProductService.GetById)
		userGroupRouter.GET("/slow", apiBoot.ProductService.MemoryCosty)
	}
}
