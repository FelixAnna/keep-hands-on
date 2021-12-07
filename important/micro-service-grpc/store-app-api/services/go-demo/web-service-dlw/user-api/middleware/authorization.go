package middleware

import (
	"log"

	"github.com/web-service-dlw/user-api/auth/jwt"

	"github.com/gin-gonic/gin"
)

func AuthorizationHandler() gin.HandlerFunc {
	return func(c *gin.Context) {
		// Set example variable
		token := c.Query("access_code")
		if token == "" {
			token = c.GetHeader("Authorization")
			token = token[7:]
		}

		log.Println(token)

		claims, err := jwt.ParseToken(token)
		if err != nil {
			panic("not authorized!")
		}

		// before request

		log.Printf("User with email %v, Id %v send this request", claims.Email, claims.UserId)
		c.Next()
	}
}
