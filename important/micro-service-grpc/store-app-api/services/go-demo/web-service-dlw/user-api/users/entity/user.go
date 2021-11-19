package entity

import "fmt"

type Date struct {
	Year  int `json:"year" binding:"required"`
	Month int `json:"month" binding:"required"`
	Day   int `json:"day" binding:"required"`
}

type Address struct {
	Country string `json:"Country" binding:"required"`
	State   string `json:"State" binding:"required"`
	City    string `json:"City" binding:"required"`
	Details string `json:"Details" binding:"required"`
}

type User struct {
	Id         string    `json:"Id" binding:""`
	Name       string    `json:"Name" binding:"required"`
	Email      string    `json:"Email" binding:"required,email"`
	Phone      string    `json:"Phone" binding:"-"`
	Birthday   string    `json:"Birthday" binding:"required"`
	Address    []Address `json:"Address" binding:"required,dive,required"`
	CreateTime string
}

//fake data
var addresses = []Address{
	{Country: "China", State: "Guangdong", City: "Shenzhen", Details: "futian"},
}
var date = "1989-07-11"
var InmemoryUsers = []User{
	{Id: "1", Name: "felix", Email: "felix@example.com", Phone: "+8612345678901", Birthday: date, Address: addresses},
	{Id: "2", Name: "anna", Email: "anna@example.com", Phone: "+8612345678902", Birthday: date, Address: addresses},
}

func (d *Date) String() string {
	return fmt.Sprintf("%04d%02d%02d", d.Year, d.Month, d.Day)
}
