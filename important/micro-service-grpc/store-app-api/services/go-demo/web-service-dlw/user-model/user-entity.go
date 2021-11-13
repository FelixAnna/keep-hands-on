package users

type Date struct {
	Year  int `json:"year" binding:"required"`
	Month int `json:"month" binding:"required"`
	Day   int `json:"day" binding:"required"`
}

type User struct {
	Id       int       `json:"id" binding:"required"`
	Name     string    `json:"name" binding:"required"`
	Email    string    `json:"email" binding:"required,email"`
	Phone    string    `json:"phone" binding:"-"`
	Birthday Date      `json:"birthday" binding:"required"`
	Address  []Address `json:"address" binding:"required,dive,required"`
}

type Address struct {
	Country string `json:"country" binding:"required"`
	State   string `json:"state" binding:"required"`
	City    string `json:"city" binding:"required"`
	Details string `json:"details" binding:"required"`
}
