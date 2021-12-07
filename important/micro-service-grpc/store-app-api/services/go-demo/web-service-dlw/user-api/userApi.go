package main

import (
	"fmt"
	"io"
	"net/http"
	"os"
	"time"

	"github.com/gin-gonic/gin"
	"github.com/web-service-dlw/user-api/auth"
	"github.com/web-service-dlw/user-api/middleware"
	userService "github.com/web-service-dlw/user-api/users"
)

func main() {
	router := gin.New()

	//define middleware before apis
	initialLogger()
	router.Use(gin.Logger())
	router.Use(middleware.ErrorHandler())
	router.Use(gin.Recovery())

	defineRoutes(router)

	router.Run(":8181")
}

func defineRoutes(router *gin.Engine) {
	router.GET("/status/running", func(c *gin.Context) {
		c.String(http.StatusOK, "running")
	})

	authNativeRouter := router.Group("/oauth2")
	{
		//authNativeRouter.GET("/authorize", auth.FireNativeAuthorize)
		//authNativeRouter.GET("/token", auth.GetNativeToken)

		authNativeRouter.GET("/redirect", auth.GetRedirectUrl)
		authNativeRouter.GET("/token", auth.GetToken)
		authNativeRouter.GET("/refresh", auth.RefreshToken)
		authNativeRouter.GET("/test", auth.TestAccess)
		authNativeRouter.GET("/pwd", auth.PassordLogin)
		authNativeRouter.GET("/client", auth.ClientSecretLogin)
	}

	authGitHubRouter := router.Group("/oauth2/github")
	{
		authGitHubRouter.GET("/authorize", auth.AuthorizeGithub)
		authGitHubRouter.GET("/authorize/url", auth.AuthorizeGithubUrl)
		authGitHubRouter.GET("/redirect", auth.GetTokenGithub)
		authGitHubRouter.GET("/user", auth.GetUserGitHub)
		authGitHubRouter.GET("/checktoken", auth.TempCheckToken)
	}

	userGroupRouter := router.Group("/users", middleware.AuthorizationHandler())
	{
		userGroupRouter.GET("/", userService.GetAllUsers)
		userGroupRouter.GET("/:userId", userService.GetUserById)
		userGroupRouter.GET("/email/:email", userService.GetUserByEmail)

		userGroupRouter.POST("/:userId", userService.UpdateUserBirthdayById)
		userGroupRouter.POST("/:userId/address", userService.UpdateUserAddressById)

		userGroupRouter.PUT("/", userService.AddUser)

		userGroupRouter.DELETE("/:userId", userService.RemoveUser)
	}
}

func initialLogger() {
	year, month, day := time.Now().UTC().Date()
	date := fmt.Sprintf("%v%v%v", year, int(month), day)
	f, _ := os.Create("../logs/" + date + ".log")
	gin.DefaultWriter = io.MultiWriter(f, os.Stdout)
}

/*

update name:
curl -H "Content-Type:application/json" -X PUT -d '{"name":"felix","email":"felix@example.com","phone":"+8612345678901","birthday": "1989-07-11","address":[{"country":"China","state":"Guangdong","city":"Shenzhen","details":"futian"}]}' http://localhost:8181/users/

curl -H "Content-Type:application/json" -X POST  http://localhost:8181/users/1637418999081?birthday=1989-07-12

curl -H "Content-Type:application/json" -X POST -d '[{"country":"China","state":"Guangdong","city":"Shenzhen","details":"futian2"}]' http://localhost:8181/users/1637418999081/address


curl -H "Content-Type:application/json" -X DELETE  http://localhost:8181/users/1637418999081

http://localhost:8181/users/email/felix@example.com
*/
