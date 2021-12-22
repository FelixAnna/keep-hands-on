# design for daily life web microservices
# moved to repo: https://github.com/FelixAnna/web-service-dlw
# auth api
auth/register
auth/login
auth/logout

# user api
GET users/

GET  users/:userId
POST users/:userId
DELETE --
PUT    --

type User struct{
    id int
    name string
    email string
    phone string
    birthday time.Time
    home *Address
}

type Address struct {
    Country string
    State string
    City string
    Details string
}
