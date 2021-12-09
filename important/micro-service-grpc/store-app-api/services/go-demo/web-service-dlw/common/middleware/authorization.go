package middleware

import (
	"log"
	"net/http"

	"github.com/FelixAnna/web-service-dlw/common/jwt"

	"github.com/gin-gonic/gin"
)

func AuthorizationHandler() gin.HandlerFunc {
	return func(c *gin.Context) {
		// Set example variable
		token := jwt.GetToken(c)

		log.Println(token)

		claims, err := jwt.ParseToken(token)
		if err != nil {
			log.Fatal(err.Error())
			c.String(http.StatusForbidden, err.Error())
			return
		}

		// before request
		log.Printf("User with email %v, Id %v send this request", claims.Email, claims.UserId)
		c.Next()
	}
}
