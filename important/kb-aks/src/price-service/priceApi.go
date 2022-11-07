package main

import (
	"net/http"

	"example.com/demo/price-service/di"
	"example.com/demo/price-service/price/service"
	"github.com/FelixAnna/web-service-dlw/common/mesh"
	"github.com/FelixAnna/web-service-dlw/common/micro"
	"github.com/FelixAnna/web-service-dlw/common/middleware"
	"github.com/gin-gonic/gin"
)

const SERVER_NAME = "price-api"

func main() {
	initialDependency()
	router := GetGinRouter()

	//router.Run(":8282")
	micro.StartApp(SERVER_NAME, ":8282", router, apiBoot.Registry.GetRegistry())
}

type ApiBoot struct {
	PriceService *service.PriceService
	ErrorHandler *middleware.ErrorHandlingMiddleware
	Registry     *mesh.Registry
}

var apiBoot *ApiBoot

func initialDependency() {
	apiBoot = &ApiBoot{
		PriceService: di.InitialPriceService(),
		ErrorHandler: di.InitialErrorMiddleware(),
		Registry:     di.InitialRegistry(),
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

	userGroupRouter := router.Group("/price")
	{
		userGroupRouter.GET("/:id", apiBoot.PriceService.GetAllById)
		userGroupRouter.GET("/:id/latest", apiBoot.PriceService.GetLatestById)
		userGroupRouter.GET("/slow", apiBoot.PriceService.MemoryCosty)
	}
}
