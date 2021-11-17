package main

import (
	"fmt"
	"io"
	"net/http"
	"os"
	"time"

	"github.com/gin-gonic/gin"
	"github.com/web-service-dlw/user-api/lib"
	userService "github.com/web-service-dlw/user-api/users"
)

func main() {
	router := gin.New()

	lib.Add(1, 2)
	initialLogger()
	router.Use(gin.Logger())
	router.Use(gin.Recovery())

	router.Use()

	router.GET("/status/running", func(c *gin.Context) {
		c.String(http.StatusOK, "running")
	})

	userGroupRouter := router.Group("/users")
	{
		userGroupRouter.GET("/", userService.GetAllUsers)
		userGroupRouter.GET("/:userId", userService.GetUserById)
		userGroupRouter.POST("/:userId", userService.UpdateUserById)
		userGroupRouter.PUT("/", userService.AddUser)
	}

	router.Run(":8181")
}

func initialLogger() {
	year, month, day := time.Now().UTC().Date()
	date := fmt.Sprintf("%v%v%v", year, int(month), day)
	f, _ := os.Create("../logs/" + date + ".log")
	gin.DefaultWriter = io.MultiWriter(f, os.Stdout)
}

/*

curl -H "Content-Type:application/json" -X POST  http://localhost:8181/users/1?name=felix1

curl -H "Content-Type:application/json" -X PUT -d '{"id":3,"name":"felix","email":"felix@example.com","phone":"+8612345678901","birthday":{"year":1989,"month":7, "day":11},"address":[{"country":"China","state":"Guangdong","city":"Shenzhen","details":"futian"}]}' http://localhost:8181/users/

*/
