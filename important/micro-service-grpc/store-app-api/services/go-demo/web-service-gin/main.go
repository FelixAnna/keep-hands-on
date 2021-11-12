package main

import (
	"fmt"
	"net/http"

	"github.com/gin-gonic/gin"
)

type album struct {
	ID     string  `json:"id"`
	Title  string  `json:"title"`
	Artist string  `json:"artist"`
	Price  float64 `json:"price"`
}

var albums = []album{
	{ID: "1", Title: "Blue Train", Artist: "John Coltrane", Price: 56.99},
	{ID: "2", Title: "Jeru", Artist: "Gerry Mulligan", Price: 17.99},
	{ID: "3", Title: "Sarah Vaughan and Clifford Brown", Artist: "Sarah Vaughan", Price: 39.99},
}

func main() {
	// Creates a gin router with default middleware:
	// logger and recovery (crash-free) middleware
	router := gin.Default()

	router = gin.New()         //no middleware
	router.Use(gin.Logger())   //write logs to gin.DefaultWriter, By default gin.DefaultWriter = os.Stdout
	router.Use(gin.Recovery()) //recover from any panics and writes a 500 if there was one
	router.GET("/albums", getAlbums)
	router.POST("/albums", postAlbums)
	router.GET("/albums/:id", getAlbumsById)

	v1 := router.Group("/v1")
	{
		v1.GET("/users/:userId/info", func(c *gin.Context) {
			version := c.Query("version")
			userId := c.Param("userId")

			c.String(http.StatusOK, fmt.Sprintf("%v %v  of v1", userId, version))
		})

		//nested group
		v11 := v1.Group("v1.1")
		v11.GET("/hello", func(c *gin.Context) {
			c.String(http.StatusOK, "hello")
		})
	}

	v2 := router.Group("/v2")
	{
		v2.GET("/users/:userId/info", func(c *gin.Context) {
			version := c.Query("version")
			userId := c.Param("userId")

			c.String(http.StatusOK, fmt.Sprintf("%v %v of v2", userId, version))
		})
	}

	router.Run(":8080")
}
func getAlbums(c *gin.Context) {
	c.IndentedJSON(http.StatusOK, albums)
}

func postAlbums(c *gin.Context) {
	var newAlbum album

	// Call BindJSON to bind the received JSON to
	// newAlbum.
	if err := c.BindJSON(&newAlbum); err != nil {
		return
	}

	/* //ShoudBindxxx: it is our resposibility to handle error
	if err := c.ShouldBindJSON(&newAlbum); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": err.Error()})
		return
	} */

	albums = append(albums, newAlbum)

	c.IndentedJSON(http.StatusCreated, newAlbum)
}

func getAlbumsById(c *gin.Context) {
	id := c.Param("id")
	name := c.Query("name")

	fmt.Println(name)

	for _, v := range albums {
		if v.ID == id {
			c.IndentedJSON(http.StatusOK, v)
			return
		}
	}
	// gin.H is a shortcut for map[string]interface{}
	c.IndentedJSON(http.StatusNotFound, gin.H{"message": "album not found"})
}
