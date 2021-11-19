package users

import (
	"fmt"
	"net/http"

	"github.com/gin-gonic/gin"
	"github.com/web-service-dlw/user-api/users/entity"
	"github.com/web-service-dlw/user-api/users/repository"
)

var repo *repository.UserRepo = &repository.UserRepo{
	TableName: "Users",
	DynamoDB:  repository.GetClient(),
}

func GetAllUsers(c *gin.Context) {
	users := getUsers()
	//fmt.Println(inmemoryUsers, inmemoryUsers[0])
	c.JSON(http.StatusOK, users)
}

func GetUserById(c *gin.Context) {
	strId := c.Param("userId")
	user, err := repo.GetUserById(strId)
	if err != nil {
		c.JSON(http.StatusNotFound, err.Error())
	} else {
		c.JSON(http.StatusOK, user)
		return
	}

	/*for _, val := range getUsers() {
		if val.Id == strId {
			user := val
			c.JSON(http.StatusOK, user)
			return
		}
	}

	c.JSON(http.StatusNotFound, "User not found!")*/
}

func UpdateUserById(c *gin.Context) {
	strId := c.Param("userId")
	//id, _ := strconv.Atoi(strId)
	name := c.Query("name")
	for _, val := range getUsers() {
		if val.Id == strId {
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

	id, err := repo.CreateUser(&new_user)
	if err != nil {
		c.JSON(http.StatusInternalServerError, err.Error())
	}

	//entity.InmemoryUsers = append(entity.InmemoryUsers, new_user)
	c.JSON(http.StatusOK, fmt.Sprintf("User %v created!", id))
}

func getUsers() []entity.User {
	return entity.InmemoryUsers
}
