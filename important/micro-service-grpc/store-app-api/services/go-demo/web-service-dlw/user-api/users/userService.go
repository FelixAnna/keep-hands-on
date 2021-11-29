package users

import (
	"fmt"
	"net/http"

	"github.com/gin-gonic/gin"
	"github.com/web-service-dlw/user-api/aws"
	"github.com/web-service-dlw/user-api/users/entity"
	"github.com/web-service-dlw/user-api/users/repository"
)

var repo *repository.UserRepo

func init() {
	repo = &repository.UserRepo{
		TableName: "dlf.Users",
		DynamoDB:  aws.GetDynamoDBClient(),
	}
}

func GetAllUsers(c *gin.Context) {
	users, err := repo.GetAllUsers()
	if err != nil {
		c.JSON(http.StatusInternalServerError, err)
	}
	c.JSON(http.StatusOK, users)
}

func GetUserByEmail(c *gin.Context) {
	email := c.Param("email")
	user, err := repo.GetUserByEmail(email)
	if err != nil {
		c.JSON(http.StatusNotFound, err.Error())
	} else {
		c.JSON(http.StatusOK, user)
		return
	}
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
}

func UpdateUserBirthdayById(c *gin.Context) {
	userId := c.Param("userId")
	birthday := c.Query("birthday")
	err := repo.UpdateUserBirthday(userId, birthday)
	if err != nil {
		c.JSON(http.StatusInternalServerError, err.Error())
	} else {
		c.JSON(http.StatusOK, fmt.Sprintf("User birthday updated, userId: %v.", userId))
		return
	}
}

func UpdateUserAddressById(c *gin.Context) {
	userId := c.Param("userId")
	var addresses []entity.Address
	if err := c.BindJSON(&addresses); err != nil {
		fmt.Println(err)
		return
	}

	err := repo.UpdateUserAddress(userId, addresses)
	if err != nil {
		c.JSON(http.StatusInternalServerError, err.Error())
	} else {
		c.JSON(http.StatusOK, fmt.Sprintf("User address updated, userId: %v.", userId))
		return
	}
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

	c.JSON(http.StatusOK, fmt.Sprintf("User %v created!", *id))
}

func RemoveUser(c *gin.Context) {
	userId := c.Param("userId")
	err := repo.DeleteUser(userId)
	if err != nil {
		c.JSON(http.StatusInternalServerError, err.Error())
	} else {
		c.JSON(http.StatusOK, fmt.Sprintf("User %v deleted!", userId))
		return
	}
}

func getUsers() []entity.User {
	return entity.InmemoryUsers
}
