package main

import (
	"fmt"
	"net/http"
	"strconv"

	"github.com/gin-gonic/gin"
)

type Date struct {
	Year  int `json:"year" binding:"required"`
	Month int `json:"month" binding:"required"`
	Day   int `json:"day" binding:"required"`
}

type User struct {
	Id       int        `json:"id" binding:"required"`
	Name     string     `json:"name" binding:"required"`
	Email    string     `json:"email" binding:"required,email"`
	Phone    string     `json:"phone" binding:"-"`
	Birthday *Date      `json:"birthday" binding:"required"`
	Address  []*Address `json:"address" binding:"required,dive,required"`
}

type Address struct {
	Country string `json:"country" binding:"required"`
	State   string `json:"state" binding:"required"`
	City    string `json:"city" binding:"required"`
	Details string `json:"details" binding:"required"`
}

var addresses = []*Address{
	&Address{Country: "China", State: "Guangdong", City: "Shenzhen", Details: "futian"},
}
var date = Date{Year: 1989, Month: 4, Day: 22}
var inmemoryUsers = []*User{
	&User{Id: 1, Name: "felix", Email: "felix@example.com", Phone: "+8612345678901", Birthday: &date, Address: addresses},
	&User{Id: 2, Name: "anna", Email: "anna@example.com", Phone: "+8612345678902", Birthday: &date, Address: addresses},
}

func main() {
	router := gin.Default()

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

	c.JSON(http.StatusOK, users)
}

func getUserById(c *gin.Context) {
	id := c.Param("userId")
	i, _ := strconv.Atoi(id)
	fmt.Println(id)
	for _, val := range getUsers() {
		fmt.Println(val.Id, string(val.Id))
		if val.Id == i {
			user := val
			c.JSON(http.StatusOK, user)
			return
		}
	}

	c.JSON(http.StatusNotFound, "User not found!")
}

func updateUserById(c *gin.Context) {
	id := c.Param("userId")
	i, _ := strconv.Atoi(id)

	name := c.Query("name")

	for _, val := range getUsers() {
		fmt.Println(val.Id, string(val.Id))
		if val.Id == i {
			val.Name = name
			c.JSON(http.StatusOK, val)
			return
		}
	}
	c.JSON(http.StatusNotFound, "User not found")
}

func addUser(c *gin.Context) {
	var new_user User
	if err := c.BindJSON(&new_user); err != nil {
		fmt.Println(err)
		return
	}

	inmemoryUsers = append(inmemoryUsers, &new_user)
	c.JSON(http.StatusOK, new_user)
}

func getUsers() []*User {
	return inmemoryUsers
}

/*

curl -H "Content-Type:application/json" -X POST  http://localhost:8181/users/1?name=felix1

curl -H "Content-Type:application/json" -X PUT -d '{"id":3,"name":"felix","email":"felix@example.com","phone":"+8612345678901","birthday":{"year":1989,"month":7, "day":11},"address":[{"country":"China","state":"Guangdong","city":"Shenzhen","details":"futian"}]}' http://localhost:8181/users/

*/
