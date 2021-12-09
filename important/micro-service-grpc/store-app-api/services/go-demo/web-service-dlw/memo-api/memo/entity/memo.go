package entity

type Memo struct {
	Id               string `json:"Id" binding:""`
	Name             string `json:"Name" binding:"required"`
	Description      string `json:"Description" binding:""`
	UserId           string `json:"UserId" binding:"required"`
	DateTime         string `json:"DateTime" binding:"required"`
	Lunar            bool   `json:"Lunar" binding:"required"` //user care about chinese Lunar only if checked
	CreateTime       string `json:"CreateTime,omitempty"`
	LastModifiedTime string `json:"LastModifyTime,omitempty"`
}

type MemoRequest struct {
	Name        string `json:"Name" binding:"required"`
	Description string `json:"Description" binding:""`
	DateTime    string `json:"DateTime" binding:"required"`
	Lunar       bool   `json:"Lunar" binding:"required"`
}

type MemoResponse struct {
	Name        string `json:"Name" binding:"required"`
	Description string `json:"Description" binding:""`
	DateTime    string `json:"DateTime" binding:"required"`
	Lunar       bool   `json:"Lunar" binding:"required"`
	Distance    int    `json:"Distance" binding:"required"`
}
