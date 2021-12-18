package main

import (
	"fmt"
	"io"
	"log"
	"net/http"
	"os"
	"time"

	"github.com/FelixAnna/web-service-dlw/common/middleware"
	dateService "github.com/FelixAnna/web-service-dlw/date-api/date"

	httpServer "github.com/asim/go-micro/plugins/server/http/v4"
	"go-micro.dev/v4"

	"github.com/asim/go-micro/plugins/registry/consul/v4"
	"github.com/gin-gonic/gin"
	"go-micro.dev/v4/registry"
	"go-micro.dev/v4/server"
)

const SERVER_NAME = "date-api"

func main() {
	consulReg := consul.NewRegistry(registry.Addrs("localhost:8500"))

	srv := httpServer.NewServer(
		server.Name(SERVER_NAME),
		server.Address(":8383"),
	)

	router := gin.New()

	//define middleware before apis
	initialLogger()
	router.Use(gin.Logger())
	router.Use(middleware.ErrorHandler())
	router.Use(gin.Recovery())

	defineRoutes(router)

	hd := srv.NewHandler(router)
	if err := srv.Handle(hd); err != nil {
		log.Fatalln(err)
	}

	service := micro.NewService(
		micro.Server(srv),
		micro.Registry(consulReg),
	)
	service.Init()
	service.Run()

	//router.Run(":8383")
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
