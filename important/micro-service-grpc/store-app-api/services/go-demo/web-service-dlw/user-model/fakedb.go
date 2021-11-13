package users

var addresses = []Address{
	{Country: "China", State: "Guangdong", City: "Shenzhen", Details: "futian"},
}
var date = Date{Year: 1989, Month: 4, Day: 22}
var InmemoryUsers = []User{
	{Id: 1, Name: "felix", Email: "felix@example.com", Phone: "+8612345678901", Birthday: date, Address: addresses},
	{Id: 2, Name: "anna", Email: "anna@example.com", Phone: "+8612345678902", Birthday: date, Address: addresses},
}
