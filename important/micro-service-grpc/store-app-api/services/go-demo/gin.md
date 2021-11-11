gin is a web framework build by go

## router
gin use httprouter which is high performance (use perfix tree for router), memory optimized http router, 
support automatic OPTIONS response and CORS, support middleware, authentication and serve static files 
Moreover the router manages a separate tree for every request method.

Exacct router > named param router > Catch-all Parameter Router

category/desc > category/:id > category/:id/*action
(if dont have category/:id but have category/:id/*action, then category/:id will redirect to category/:id/)

## json replacement
Gin use encoding/json as a default json package, you can change to use high performance josn libary: jsoniter or go-json
```$ go build -tags=jsoniter .```

## reduce binary size
You can disable MsgPack to reduce binary size
```$ go build -tages=nomsgpack .```

## example repository
https://github.com/gin-gonic/examples 

## parameters
    c.Params("name") //path
    c.Query("name") //querystring
    c.DefaultQuery("name", "defaultValue")

    c.PostForm("message") //Multipart/Urlencoded Form

    .... QueryMap , PostFormMap ....

