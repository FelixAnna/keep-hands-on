package memo

import (
	_ "github.com/FelixAnna/web-service-dlw/memo-api/memo/entity"
	"github.com/gin-gonic/gin"
)

func GetMemosByUserId(c *gin.Context) {
	userId := getUserIdFromContext(c)

	_ = userId

	return
}

func GetRecentMemos(c *gin.Context) {
	return
}

func AddMemo(c *gin.Context) {
	return
}

func UpdateMemoById(c *gin.Context) {
	return
}

func RemoveMemo(c *gin.Context) {
	return
}

func getUserIdFromContext(c *gin.Context) (userId string) {
	val, _ := c.Get("userId")
	userId = val.(string)
	return
}
