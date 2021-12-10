package entity

type Memo struct {
	Id               string `json:"Id" binding:""`
	Subject          string `json:"Subject" binding:"required"`
	Description      string `json:"Description" binding:""`
	UserId           string `json:"UserId" binding:"required"`
	MemoDate         string `json:"MemoDate" binding:"required"`
	Lunar            bool   `json:"Lunar" binding:""` //user care about chinese Lunar only if checked
	CreateTime       string `json:"CreateTime,omitempty"`
	LastModifiedTime string `json:"LastModifiedTime,omitempty"`
}

type MemoRequest struct {
	Subject     string `json:"Subject" binding:"required"`
	Description string `json:"Description" binding:""`
	MemoDate    string `json:"MemoDate" binding:"required"`
	Lunar       bool   `json:"Lunar" binding:""`
}

type MemoResponse struct {
	Subject          string `json:"Subject" binding:"required"`
	Description      string `json:"Description" binding:""`
	MemoDate         string `json:"MemoDate" binding:"required"`
	Lunar            bool   `json:"Lunar" binding:""`
	Distance         int    `json:"Distance" binding:"required"`
	CreateTime       string `json:"CreateTime,omitempty"`       // - TODO convert to formated datetime
	LastModifiedTime string `json:"LastModifiedTime,omitempty"` //  - TODO convert to formated datetime
}
