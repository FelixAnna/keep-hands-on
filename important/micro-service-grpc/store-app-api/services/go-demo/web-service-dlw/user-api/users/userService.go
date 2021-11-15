package users

import (
	"fmt"
	"net/http"
	"strconv"

	"github.com/gin-gonic/gin"
	"github.com/web-service-dlw/user-api/users/entity"
)

func GetAllUsers(c *gin.Context) {
	users := getUsers()
	//fmt.Println(inmemoryUsers, inmemoryUsers[0])
	c.JSON(http.StatusOK, users)
}

func GetUserById(c *gin.Context) {
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

func UpdateUserById(c *gin.Context) {
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

func AddUser(c *gin.Context) {
	var new_user entity.User
	if err := c.BindJSON(&new_user); err != nil {
		fmt.Println(err)
		return
	}

	entity.InmemoryUsers = append(entity.InmemoryUsers, new_user)
	c.JSON(http.StatusOK, new_user)
}

func getUsers() []entity.User {
	return entity.InmemoryUsers
}
