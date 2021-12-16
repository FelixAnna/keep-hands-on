package entity

import (
	"encoding/json"
	"fmt"
	"io/ioutil"
	"log"
	"net/http"
	"time"
)

type Memo struct {
	Id               string `json:"Id" binding:""`
	Subject          string `json:"Subject" binding:"required"`
	Description      string `json:"Description" binding:""`
	UserId           string `json:"UserId" binding:"required"`
	MonthDay         int    `json:"MonthDay" binding:"required"`
	StartYear        int    `json:"StartYear" binding:""`
	Lunar            bool   `json:"Lunar" binding:""` //user care about chinese Lunar only if checked
	CreateTime       string `json:"CreateTime,omitempty"`
	LastModifiedTime string `json:"LastModifiedTime,omitempty"`
}

type MemoRequest struct {
	Subject     string `json:"Subject" binding:"required"`
	Description string `json:"Description" binding:""`
	MonthDay    int    `json:"MonthDay" binding:"required"`
	StartYear   int    `json:"StartYear" binding:""`
	Lunar       bool   `json:"Lunar" binding:""`
}

type MemoResponse struct {
	Id               string `json:"Id" binding:""`
	UserId           string `json:"UserId" binding:"required"`
	Subject          string `json:"Subject" binding:"required"`
	Description      string `json:"Description" binding:""`
	MonthDay         int    `json:"MonthDay" binding:"required"`
	StartYear        int    `json:"StartYear" binding:""`
	Lunar            bool   `json:"Lunar" binding:""`
	Distance         []int  `json:"Distance" binding:"required"`
	CreateTime       string `json:"CreateTime,omitempty"`       //  - TODO convert to formated datetime
	LastModifiedTime string `json:"LastModifiedTime,omitempty"` //  - TODO convert to formated datetime
}

type Distance struct {
	StartYMD  int
	TargetYMD int
	Lunar     bool
	Before    int64
	After     int64
}

func (memo *Memo) ToResponse(now *time.Time) *MemoResponse {
	resp := &MemoResponse{
		Id:               memo.Id,
		UserId:           memo.UserId,
		Subject:          memo.Subject,
		Description:      memo.Description,
		MonthDay:         memo.MonthDay,
		StartYear:        memo.StartYear,
		Lunar:            memo.Lunar,
		CreateTime:       memo.CreateTime,
		LastModifiedTime: memo.LastModifiedTime,
		Distance:         memo.getDistance(now),
	}

	return resp
}

func (memo *Memo) getDistance(target *time.Time) []int {
	year := memo.StartYear
	if year <= 1900 {
		year = time.Now().Year()
	}

	startDate := year*10000 + memo.MonthDay
	targetDate := target.Year()*10000 + int(target.Month())*100 + target.Day()

	if memo.Lunar {
		url := fmt.Sprintf("http://localhost:8383/date/distance/lunar?start=%v&end=%v", startDate, targetDate)
		result, _ := getDistance(url)
		return []int{int(result.Before), int(result.After)}
	} else {
		url := fmt.Sprintf("http://localhost:8383/date/distance/?start=%v&end=%v", startDate, targetDate)
		result, _ := getDistance(url)
		return []int{int(result.Before), int(result.After)}
	}
}

func getDistance(url string) (*Distance, error) {
	response, err := http.Get(url)
	if err != nil {
		log.Fatal(err.Error())
		return nil, err
	}

	responseData, err := ioutil.ReadAll(response.Body)
	if err != nil {
		log.Fatal(err)
		return nil, err
	}

	var distance *Distance = &Distance{}
	json.Unmarshal(responseData, distance)

	return distance, nil
}
