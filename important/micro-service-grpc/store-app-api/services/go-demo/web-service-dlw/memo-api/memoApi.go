package main

import (
	"fmt"
	"io"
	"log"
	"net/http"
	"os"
	"time"

	"github.com/FelixAnna/web-service-dlw/common/middleware"
	memoService "github.com/FelixAnna/web-service-dlw/memo-api/memo"
	"github.com/asim/go-micro/plugins/registry/consul/v4"
	httpServer "github.com/asim/go-micro/plugins/server/http/v4"
	"go-micro.dev/v4"

	"github.com/gin-gonic/gin"
	"go-micro.dev/v4/registry"
	"go-micro.dev/v4/server"
)

const SERVER_NAME = "memo-api"

func main() {
	consulReg := consul.NewRegistry(registry.Addrs("localhost:8500"))

	srv := httpServer.NewServer(
		server.Name(SERVER_NAME),
		server.Address(":8282"),
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

	//router.Run(":8282")
}

func defineRoutes(router *gin.Engine) {
	router.GET("/status", func(c *gin.Context) {
		c.String(http.StatusOK, "running")
	})

	userGroupRouter := router.Group("/memos", middleware.AuthorizationHandler())
	{
		userGroupRouter.PUT("/", memoService.AddMemo)

		userGroupRouter.GET("/:id", memoService.GetMemoById)
		userGroupRouter.GET("/", memoService.GetMemosByUserId)
		userGroupRouter.GET("/recent", memoService.GetRecentMemos)

		userGroupRouter.POST("/:id", memoService.UpdateMemoById)
		userGroupRouter.DELETE("/:id", memoService.RemoveMemo)
	}
}

func initialLogger() {
	year, month, day := time.Now().UTC().Date()
	date := fmt.Sprintf("%v%v%v", year, int(month), day)
	f, _ := os.Create("../logs/" + date + ".log")
	gin.DefaultWriter = io.MultiWriter(f, os.Stdout)
}
