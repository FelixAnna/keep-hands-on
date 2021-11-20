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

	router.GET("/status/running", func(c *gin.Context) {
		c.String(http.StatusOK, "running")
	})

	userGroupRouter := router.Group("/users")
	{
		userGroupRouter.GET("/", userService.GetAllUsers)
		userGroupRouter.GET("/:userId", userService.GetUserById)

		userGroupRouter.POST("/:userId", userService.UpdateUserBirthdayById)
		userGroupRouter.POST("/:userId/address", userService.UpdateUserAddressById)

		userGroupRouter.PUT("/", userService.AddUser)

		userGroupRouter.DELETE("/:userId", userService.RemoveUser)
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

update name:
curl -H "Content-Type:application/json" -X POST  http://localhost:8181/users/1637418650081?birthday=1989-07-12

curl -H "Content-Type:application/json" -X POST -d '[{"country":"China","state":"Guangdong","city":"Shenzhen","details":"futian2"}]' http://localhost:8181/users/1637418650081/address

curl -H "Content-Type:application/json" -X PUT -d '{"name":"felix","email":"felix@example.com","phone":"+8612345678901","birthday": "1989-07-11","address":[{"country":"China","state":"Guangdong","city":"Shenzhen","details":"futian"}]}' http://localhost:8181/users/

curl -H "Content-Type:application/json" -X DELETE  http://localhost:8181/users/1637418650081

*/
