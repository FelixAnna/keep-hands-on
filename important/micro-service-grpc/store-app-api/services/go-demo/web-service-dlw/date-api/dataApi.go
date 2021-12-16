package main

import (
	"fmt"
	"io"
	"net/http"
	"os"
	"time"

	"github.com/FelixAnna/web-service-dlw/common/middleware"
	dateService "github.com/FelixAnna/web-service-dlw/date-api/date"
	"github.com/gin-gonic/gin"
)

func main() {
	router := gin.New()

	//define middleware before apis
	initialLogger()
	router.Use(gin.Logger())
	router.Use(middleware.ErrorHandler())
	router.Use(gin.Recovery())

	defineRoutes(router)

	router.Run(":8383")
}

func defineRoutes(router *gin.Engine) {
	router.GET("/status", func(c *gin.Context) {
		c.String(http.StatusOK, "running")
	})

	userGroupRouter := router.Group("/date")
	{
		userGroupRouter.GET("/current/month", dateService.GetMonthDate)
		userGroupRouter.GET("/distance", dateService.GetDateDistance)
		userGroupRouter.GET("/distance/lunar", dateService.GetLunarDateDistance)
	}
}

func initialLogger() {
	year, month, day := time.Now().UTC().Date()
	date := fmt.Sprintf("%v%v%v", year, int(month), day)
	f, _ := os.Create("../logs/" + date + ".log")
	gin.DefaultWriter = io.MultiWriter(f, os.Stdout)
}
