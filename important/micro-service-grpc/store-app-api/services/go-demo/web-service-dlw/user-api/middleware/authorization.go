package middleware

import (
	"log"

	"github.com/gin-gonic/gin"
)

func AuthorizationHandler() gin.HandlerFunc {
	return func(c *gin.Context) {
		// Set example variable
		access_token := c.Query("access_token")
		if access_token == "" {
			access_token = c.GetHeader("Authorization")
		}

		log.Println(access_token)
		if access_token == "" {
			panic("not authorized!")
		}

		// before request

		log.Println("auth handler start")
		c.Next()
		log.Println("auth handler end")
	}
}
