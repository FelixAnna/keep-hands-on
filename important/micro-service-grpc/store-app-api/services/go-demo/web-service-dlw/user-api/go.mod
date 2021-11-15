module github.com/web-service-dlw/user-api

go 1.14

require (
	github.com/aws/aws-sdk-go-v2 v1.11.0 // indirect
	github.com/aws/aws-sdk-go-v2/config v1.10.1 // indirect
	github.com/aws/aws-sdk-go-v2/service/dynamodb v1.8.0 // indirect
	github.com/aws/aws-sdk-go-v2/service/s3 v1.19.0 // indirect
	github.com/gin-gonic/gin v1.7.4
)

//replace github.com/web-service-dlw/user-model => ../user-model
//require github.com/web-service-dlw/user-model v0.0.0-00010101000000-000000000000
