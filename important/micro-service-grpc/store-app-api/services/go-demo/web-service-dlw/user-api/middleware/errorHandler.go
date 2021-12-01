package middleware

import (
	"log"

	"github.com/gin-gonic/gin"
)

func ErrorHandler() gin.HandlerFunc {
	return func(c *gin.Context) {
		log.Println("error handler start")
		c.Next()
		log.Println("error handler end")
		err := c.Errors.Last()
		if err == nil {
			return
		}

		log.Printf("errorhandler: error while handling your request: %v", err)

		// Use reflect.TypeOf(err.Err) to known the type of your error
		/*if error, ok := errors.Cause(err.Err).(*myspace.KindOfClientError); ok {
			c.JSON(400, gin.H{
				"error": "Blah blahhh"
			})
			return
		}*/
	}
}
