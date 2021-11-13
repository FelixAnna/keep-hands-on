package main

import (
	"fmt"
	"io"
	"net/http"
	"os"
	"strconv"
	"time"

	"github.com/gin-gonic/gin"
	"github.com/web-service-dlw/user-api/lib"
	UserModel "github.com/web-service-dlw/user-model"
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
		userGroupRouter.GET("/", getAllUsers)
		userGroupRouter.GET("/:userId", getUserById)
		userGroupRouter.POST("/:userId", updateUserById)
		userGroupRouter.PUT("/", addUser)
	}

	router.Run(":8181")
}

func getAllUsers(c *gin.Context) {
	users := getUsers()
	//fmt.Println(inmemoryUsers, inmemoryUsers[0])
	c.JSON(http.StatusOK, users)
}

func getUserById(c *gin.Context) {
	strId := c.Param("userId")
	id, _ := strconv.Atoi(strId)
	fmt.Println(strId)
	for _, val := range getUsers() {
		if val.Id == id {
			user := val
			c.JSON(http.StatusOK, user)
			return
		}
	}

	c.JSON(http.StatusNotFound, "User not found!")
}

func updateUserById(c *gin.Context) {
	strId := c.Param("userId")
	id, _ := strconv.Atoi(strId)
	name := c.Query("name")
	for _, val := range getUsers() {
		if val.Id == id {
			val.Name = name
			c.JSON(http.StatusOK, val)
			return
		}
	}
	c.JSON(http.StatusNotFound, "User not found")
}

func addUser(c *gin.Context) {
	var new_user UserModel.User
	if err := c.BindJSON(&new_user); err != nil {
		fmt.Println(err)
		return
	}

	UserModel.InmemoryUsers = append(UserModel.InmemoryUsers, new_user)
	c.JSON(http.StatusOK, new_user)
}

func getUsers() []UserModel.User {
	return UserModel.InmemoryUsers
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
