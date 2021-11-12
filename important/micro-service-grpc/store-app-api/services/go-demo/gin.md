gin is a web framework build by go

## router
gin use httprouter which is high performance (use perfix tree for router), memory optimized http router, 
support automatic OPTIONS response and CORS, support middleware, authentication and serve static files 
Moreover the router manages a separate tree for every request method.

Exact router > named param router > Catch-all Parameter Router

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
    file, _ := c.FormFile("file") //upload one file
    c.SaveUploadedFile(file, dest)

    form, _ := c.MultipartForm()  //upload multiple files
    files := form.File["upload[]"]

    call example: curl -X POST http://localhost:8080/upload \
            -F "upload[]=@/Users/appleboy/test1.zip" \
            -F "upload[]=@/Users/appleboy/test2.zip" \
            -H "Content-Type: multipart/form-data"

## grouping router
    have group router
    v1 := router.Group("/v1")
	{
		v1.POST("/login", loginEndpoint)
        ...
    }
## middleware
    gin.Default() have gin.Logger() and gin.Recovery() middle, gin.Recovery() can hanle any panics and return 500 to client

    gin.Logger() default write to os.Stdout by gin.DefaultWriter, we can Logging to a file:
    f, _ := os.Create("gin.log")
    gin.DefaultWriter = io.MultiWriter(f)  //gin.DisableConsoleColor() to disable color if write to file
    // gin.DefaultWriter = io.MultiWriter(f, os.Stdout)  //log to both

    //we can define customer logger and Recovery
    router.Use(gin.LoggerWithFormatter(...))
    r.Use(gin.CustomRecovery(...))

    we can have route middleware:
    r.GET("/benchmark", MyBenchLogger(), benchEndpoint)
    we can also ahve per group middleware:
    v1.Use(AuthRequired())

## Model binding and validation
    validation by: go-playground/validator/v10  

    Type Bind
    -  Must bind : the request will abort with c.AbortWithError(400, err).SetType(ErrorBindType) if there is a bind error, they use MustBindWith under the hood
    --  func: Bind, BindJSON, BindXML,  BindYAML, BindQuery, BindHeader, BindUri

    -  ShouldBind : If there is a Bind error, it is the developer's responsibility to handle the request and error apporiately. they use ShouldBindWith under the hood
    --  func: ShoudBind, ShoudBindJSON, ...

    Bind method would use Content-Type for bind to apporiate type (if GET, only Form binding `query` used)

    Custom Validator (FieldLevel)
    -- define validator
    var bookableDate validator.Func = func(fl validator.FieldLevel) bool {}

    -- register validator
    if v, ok := binding.Validator.Engine().(*validator.Validate); ok {
		v.RegisterValidation("bookabledate", bookableDate)
	}

    -- usage
    type Booking struct {
        CheckIn  time.Time `form:"check_in" binding:"required,bookabledate" time_format:"2006-01-02"`
    }
    
    Also support custom StructLevel validator -- check doc if necessary
    Also can bind to html field use c.Bind(&target) or c.ShouldBind(&target)  -- acknowledge only
## Rendering
    Support JSON, XML, YAML, ProtoBuf rending
    ex: c.JSON(status, data), c.ProtoBuf(status, data)

    SecureJSON 
    Add While(1); before the output json, so avoid it be eval(...) by client side js. (client side should be use JSON.parse instead)

    PureJSON : not replace HTML characters
    {"name";"<p>felix</p>"}

    Also support JSONP & AsciiJSON & HTML & DataFromReader -- acknowledge only

    File, FileFromFS  for serving data from File
    ex: c.File("path")

    Redirect : redirect to external or internal url
    c.Redirect(status, "http://www.google.com/")

## Serving static files
    router.Static("/assets", "./assets")
    router.StaticFile("/favicon.ico", "./resources/favicon.ico")

## Customer Middleware

    func FakeToken() gin.HandlerFunc {
        return func(c *gin.Context){
            t := time.Now()

            c.
            c.Set("token", "12345==")
            //before request

            c.Next()

            //after request
            lantency := time.Since(t)
            log.Print(latency)

            // access the status we are sending
            status := c.Writer.Status()
            log.Println(status)
        }
    }

    func main(){
        router := gin.New()
        router.Use(FakeToken())
        r.GET("/test", func(c *gin.Context) {
            token := c.MustGet("token").(string)

            // it would print: "12345=="
            log.Println(token)
        })

        // Listen and serve on 0.0.0.0:8080
        r.Run(":8080")
    }

## Goroutines inside a middleware or handler
    When use goroutines inside a middleware or handler, use a copy of the context

    router.GET("/any", func(c *gin.Context){
        cp := c.Copy()
        go func(){
            time.Sleep(5 * time.Second)

            log.Println(cp.Request.URL.Path)
        }
    })

    router.Run(":8080")
    /*
    http.ListenAndServer(":8080", router)

    or
    
    s := &http.Server{
		Addr:           ":8080",
		Handler:        router,
		ReadTimeout:    10 * time.Second,
		WriteTimeout:   10 * time.Second,
		MaxHeaderBytes: 1 << 20,
	}
	s.ListenAndServe()
    */

## start multiple services
    g.Go(func() error {
		err := server01.ListenAndServe()
		if err != nil && err != http.ErrServerClosed {
			log.Fatal(err)
		}
		return err
	})

    g.Go(func() error {
		err := server02.ListenAndServe()
		if err != nil && err != http.ErrServerClosed {
			log.Fatal(err)
		}
		return err
	})

	if err := g.Wait(); err != nil {
		log.Fatal(err)
	}

## Graceful Shutdown server
    link: https://github.com/gin-gonic/gin#graceful-shutdown-or-restart

    one way is use third-party lib to enable graceful shutdown:
    fvbock/endless :  endless.ListenAndServe(":4242", router)

    another way is:
    1. initial the server in a goroutine so it dont block shutdown process
    2. make a chan of os.Signal
    3. wait for system shutdown behaviour, once have notify the chan
    4. consume the chan
    5. infor the server to stop finish the request in x seconds: context.WithTimeout(...)
    6. defer cancel()
    7. shutdown the context by server.Shutdown(ctx)
    8.done

## http2 server push
    support that -- acknowledge only

## testing
    prefer use: net/http/httptest + testing + github.com/stretchr/testify/assert
    package main

    import (
        "net/http"
        "net/http/httptest"
        "testing"

        "github.com/stretchr/testify/assert"
    )

    func TestPingRoute(t *testing.T) {
        router := setupRouter()

        w := httptest.NewRecorder()
        req, _ := http.NewRequest("GET", "/ping", nil)
        router.ServeHTTP(w, req)

        assert.Equal(t, 200, w.Code)
        assert.Equal(t, "pong", w.Body.String())
    }
