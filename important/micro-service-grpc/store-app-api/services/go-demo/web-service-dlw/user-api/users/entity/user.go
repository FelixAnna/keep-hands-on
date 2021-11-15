package entity

type Date struct {
	Year  int `json:"year" binding:"required"`
	Month int `json:"month" binding:"required"`
	Day   int `json:"day" binding:"required"`
}

type Address struct {
	Country string `json:"country" binding:"required"`
	State   string `json:"state" binding:"required"`
	City    string `json:"city" binding:"required"`
	Details string `json:"details" binding:"required"`
}

type User struct {
	Id       int       `json:"id" binding:"required"`
	Name     string    `json:"name" binding:"required"`
	Email    string    `json:"email" binding:"required,email"`
	Phone    string    `json:"phone" binding:"-"`
	Birthday Date      `json:"birthday" binding:"required"`
	Address  []Address `json:"address" binding:"required,dive,required"`
}

//fake data
var addresses = []Address{
	{Country: "China", State: "Guangdong", City: "Shenzhen", Details: "futian"},
}
var date = Date{Year: 1989, Month: 4, Day: 22}
var InmemoryUsers = []User{
	{Id: 1, Name: "felix", Email: "felix@example.com", Phone: "+8612345678901", Birthday: date, Address: addresses},
	{Id: 2, Name: "anna", Email: "anna@example.com", Phone: "+8612345678902", Birthday: date, Address: addresses},
}
