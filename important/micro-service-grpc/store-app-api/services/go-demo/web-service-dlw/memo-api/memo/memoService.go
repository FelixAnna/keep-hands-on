package memo

import (
	"fmt"
	"log"
	"net/http"

	"github.com/FelixAnna/web-service-dlw/memo-api/memo/entity"
	"github.com/FelixAnna/web-service-dlw/memo-api/memo/repository"
	"github.com/gin-gonic/gin"
)

var repo repository.MemoRepo

func init() {
	repo = &repository.MemoRepoDynamoDB{}
}

func AddMemo(c *gin.Context) {
	userId := getUserIdFromContext(c)
	var request entity.MemoRequest
	if err := c.BindJSON(&request); err != nil {
		log.Println(err)
		c.JSON(http.StatusBadRequest, err.Error())
		return
	}

	var new_memo entity.Memo = entity.Memo{
		Subject:     request.Subject,
		Description: request.Description,
		UserId:      userId,
		MemoDate:    request.MemoDate,
		Lunar:       request.Lunar,
	}

	id, err := repo.Add(&new_memo)
	if err != nil {
		c.JSON(http.StatusInternalServerError, err.Error())
		return
	}

	c.JSON(http.StatusOK, fmt.Sprintf("Memo %v created!", *id))
}

func GetMemoById(c *gin.Context) {
	id := c.Param("id")
	memo, err := repo.GetById(id)
	if err != nil {
		c.String(http.StatusNotFound, err.Error())
		return
	}

	c.JSON(http.StatusOK, memo)
}

func GetMemosByUserId(c *gin.Context) {
	userId := getUserIdFromContext(c)
	memos, err := repo.GetByUserId(userId)
	if err != nil {
		c.JSON(http.StatusInternalServerError, err)
		return
	}

	c.JSON(http.StatusOK, memos)
}

func GetRecentMemos(c *gin.Context) {
	userId := getUserIdFromContext(c)
	start := c.Query("start")
	end := c.Query("end")
	//TODO - get by month+ day range
	//TODO - calculate distance
	memos, err := repo.GetByDateRange(start, end, userId)
	if err != nil {
		c.JSON(http.StatusInternalServerError, err)
		return
	}

	c.JSON(http.StatusOK, memos)
}

func UpdateMemoById(c *gin.Context) {
	id := c.Param("id")
	userId := getUserIdFromContext(c)
	var request entity.MemoRequest
	if err := c.BindJSON(&request); err != nil {
		log.Println(err)
		c.JSON(http.StatusBadRequest, err.Error())
		return
	}

	var new_memo entity.Memo = entity.Memo{
		Id:          id,
		Subject:     request.Subject,
		Description: request.Description,
		UserId:      userId, //keep current userId for compare before update db
		MemoDate:    request.MemoDate,
		Lunar:       request.Lunar,
	}

	err := repo.Update(new_memo)
	if err != nil {
		c.JSON(http.StatusInternalServerError, err.Error())
		return
	}

	c.JSON(http.StatusOK, fmt.Sprintf("Memo %v updated!", id))
}

func RemoveMemo(c *gin.Context) {
	id := c.Param("id")
	err := repo.Delete(id)
	if err != nil {
		c.JSON(http.StatusInternalServerError, err.Error())
	} else {
		c.JSON(http.StatusOK, fmt.Sprintf("Memo %v deleted!", id))
	}
}

func getUserIdFromContext(c *gin.Context) (userId string) {
	val, _ := c.Get("userId")
	userId = val.(string)
	return
}
