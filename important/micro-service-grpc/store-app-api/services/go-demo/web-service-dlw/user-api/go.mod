module github.com/web-service-dlw/user-api

go 1.14

require (
	github.com/aws/aws-sdk-go v1.42.8
	github.com/gin-gonic/gin v1.7.4
	github.com/go-oauth2/oauth2/v4 v4.4.2
	github.com/golang-jwt/jwt v3.2.1+incompatible
	github.com/google/go-cmp v0.5.6 // indirect
	golang.org/x/oauth2 v0.0.0-20211104180415-d3ed0bb246c8
)

//replace github.com/web-service-dlw/user-model => ../user-model
//require github.com/web-service-dlw/user-model v0.0.0-00010101000000-000000000000
