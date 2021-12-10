package main

import (
	"fmt"
	"io"
	"net/http"
	"os"
	"time"

	"github.com/FelixAnna/web-service-dlw/common/middleware"
	memoService "github.com/FelixAnna/web-service-dlw/memo-api/memo"
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

	router.Run(":8282")
}

func defineRoutes(router *gin.Engine) {
	router.GET("/status/running", func(c *gin.Context) {
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
